using Xunit;
using Moq;
using Microsoft.Extensions.Options;
using CustomerSuccessCRM.Lib.Services.Implementations;
using CustomerSuccessCRM.Lib.Services.Contracts;
using CustomerSuccessCRM.Lib.Services.Strategies;
using CustomerSuccessCRM.Lib.Configuration;
using CustomerSuccessCRM.Lib.Models;
using System.Text.Json;

namespace CustomerSuccessCRM.Tests.Services
{
    public class WorkflowServiceTests
    {
        private readonly Mock<IBusinessRuleStrategy> _mockBusinessRuleStrategy;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<ICrmService> _mockCrmService;
        private readonly WorkflowSettings _workflowSettings;
        private readonly WorkflowService _service;

        public WorkflowServiceTests()
        {
            _mockBusinessRuleStrategy = new Mock<IBusinessRuleStrategy>();
            _mockNotificationService = new Mock<INotificationService>();
            _mockEmailService = new Mock<IEmailService>();
            _mockCrmService = new Mock<ICrmService>();

            _workflowSettings = new WorkflowSettings
            {
                EnableAutomaticWorkflows = true,
                MaxConcurrentWorkflows = 5,
                WorkflowAdministrators = new[] { "admin@example.com" }
            };

            var mockOptions = new Mock<IOptions<WorkflowSettings>>();
            mockOptions.Setup(x => x.Value).Returns(_workflowSettings);

            _service = new WorkflowService(
                _mockBusinessRuleStrategy.Object,
                mockOptions.Object,
                _mockNotificationService.Object,
                _mockEmailService.Object,
                _mockCrmService.Object);
        }

        [Fact]
        public async Task ValidateWorkflowAsync_CallsStrategy()
        {
            // Arrange
            var workflow = new Workflow { Nome = "Test Workflow" };
            _mockBusinessRuleStrategy.Setup(x => x.ValidateWorkflowAsync(workflow))
                .ReturnsAsync(true);

            // Act
            var result = await _service.ValidateWorkflowAsync(workflow);

            // Assert
            Assert.True(result);
            _mockBusinessRuleStrategy.Verify(x => x.ValidateWorkflowAsync(workflow), Times.Once);
        }

        [Fact]
        public async Task ExecutarWorkflowAsync_DisabledWorkflows_DoesNothing()
        {
            // Arrange
            var workflow = new Workflow { Nome = "Test Workflow" };
            var settings = new WorkflowSettings { EnableAutomaticWorkflows = false };
            var mockOptions = new Mock<IOptions<WorkflowSettings>>();
            mockOptions.Setup(x => x.Value).Returns(settings);

            var service = new WorkflowService(
                _mockBusinessRuleStrategy.Object,
                mockOptions.Object,
                _mockNotificationService.Object,
                _mockEmailService.Object,
                _mockCrmService.Object);

            // Act
            await service.ExecutarWorkflowAsync(workflow);

            // Assert
            _mockBusinessRuleStrategy.Verify(x => x.ValidateWorkflowAsync(It.IsAny<Workflow>()), Times.Never);
        }

        [Fact]
        public async Task ExecutarWorkflowAsync_InvalidWorkflow_ThrowsException()
        {
            // Arrange
            var workflow = new Workflow { Nome = "Test Workflow" };
            _mockBusinessRuleStrategy.Setup(x => x.ValidateWorkflowAsync(workflow))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.ExecutarWorkflowAsync(workflow));
        }

        [Fact]
        public async Task ExecutarWorkflowAsync_TooManyActions_ThrowsException()
        {
            // Arrange
            var workflow = new Workflow
            {
                Nome = "Test Workflow",
                Acoes = Enumerable.Range(0, 6).Select(i => new WorkflowAcao()).ToList()
            };
            _mockBusinessRuleStrategy.Setup(x => x.ValidateWorkflowAsync(workflow))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.ExecutarWorkflowAsync(workflow));
        }

        [Fact]
        public async Task ExecutarWorkflowAsync_EnviarEmail_ExecutesCorrectly()
        {
            // Arrange
            var parametros = new Dictionary<string, string>
            {
                { "destinatario", "test@example.com" },
                { "assunto", "Test Subject" },
                { "corpo", "Test Body" }
            };

            var workflow = new Workflow
            {
                Nome = "Email Workflow",
                Acoes = new List<WorkflowAcao>
                {
                    new WorkflowAcao
                    {
                        Tipo = "EnviarEmail",
                        Parametros = JsonSerializer.Serialize(parametros)
                    }
                }
            };

            _mockBusinessRuleStrategy.Setup(x => x.ValidateWorkflowAsync(workflow))
                .ReturnsAsync(true);

            // Act
            await _service.ExecutarWorkflowAsync(workflow);

            // Assert
            _mockEmailService.Verify(x => x.EnviarEmailAsync(
                parametros["destinatario"],
                parametros["assunto"],
                parametros["corpo"]), Times.Once);
        }

        [Fact]
        public async Task ExecutarWorkflowAsync_AtualizarStatus_ExecutesCorrectly()
        {
            // Arrange
            var clienteId = 1;
            var cliente = new Cliente { Id = clienteId, Nome = "Test Client" };
            var parametros = new Dictionary<string, string>
            {
                { "clienteId", clienteId.ToString() },
                { "novoStatus", StatusCliente.Ativo.ToString() }
            };

            var workflow = new Workflow
            {
                Nome = "Status Workflow",
                Acoes = new List<WorkflowAcao>
                {
                    new WorkflowAcao
                    {
                        Tipo = "AtualizarStatus",
                        Parametros = JsonSerializer.Serialize(parametros)
                    }
                }
            };

            _mockBusinessRuleStrategy.Setup(x => x.ValidateWorkflowAsync(workflow))
                .ReturnsAsync(true);
            _mockCrmService.Setup(x => x.GetClienteByIdAsync(clienteId))
                .ReturnsAsync(cliente);

            // Act
            await _service.ExecutarWorkflowAsync(workflow);

            // Assert
            _mockCrmService.Verify(x => x.UpdateClienteAsync(It.Is<Cliente>(c => 
                c.Id == clienteId && c.Status == StatusCliente.Ativo)), Times.Once);
        }

        [Fact]
        public async Task ExecutarWorkflowAsync_CriarTarefa_ExecutesCorrectly()
        {
            // Arrange
            var parametros = new Dictionary<string, string>
            {
                { "clienteId", "1" },
                { "assunto", "Test Task" },
                { "descricao", "Test Description" },
                { "responsavel", "user@example.com" }
            };

            var workflow = new Workflow
            {
                Nome = "Task Workflow",
                Acoes = new List<WorkflowAcao>
                {
                    new WorkflowAcao
                    {
                        Tipo = "CriarTarefa",
                        Parametros = JsonSerializer.Serialize(parametros)
                    }
                }
            };

            _mockBusinessRuleStrategy.Setup(x => x.ValidateWorkflowAsync(workflow))
                .ReturnsAsync(true);

            // Act
            await _service.ExecutarWorkflowAsync(workflow);

            // Assert
            _mockCrmService.Verify(x => x.CreateInteracaoAsync(It.Is<Interacao>(i =>
                i.ClienteId == 1 &&
                i.Assunto == parametros["assunto"] &&
                i.Descricao == parametros["descricao"] &&
                i.Responsavel == parametros["responsavel"] &&
                i.TipoInteracao == TipoInteracao.Tarefa &&
                i.Status == StatusInteracao.Pendente)), Times.Once);
        }

        [Fact]
        public async Task ExecutarWorkflowAsync_NotificarResponsavel_ExecutesCorrectly()
        {
            // Arrange
            var parametros = new Dictionary<string, string>
            {
                { "responsavel", "user@example.com" },
                { "titulo", "Test Notification" },
                { "mensagem", "Test Message" }
            };

            var workflow = new Workflow
            {
                Nome = "Notification Workflow",
                Acoes = new List<WorkflowAcao>
                {
                    new WorkflowAcao
                    {
                        Tipo = "NotificarResponsavel",
                        Parametros = JsonSerializer.Serialize(parametros)
                    }
                }
            };

            _mockBusinessRuleStrategy.Setup(x => x.ValidateWorkflowAsync(workflow))
                .ReturnsAsync(true);

            // Act
            await _service.ExecutarWorkflowAsync(workflow);

            // Assert
            _mockNotificationService.Verify(x => x.EnviarNotificacaoAsync(
                parametros["responsavel"],
                parametros["titulo"],
                parametros["mensagem"]), Times.Once);
        }

        [Fact]
        public async Task ExecutarWorkflowAsync_InvalidActionType_ThrowsException()
        {
            // Arrange
            var workflow = new Workflow
            {
                Nome = "Invalid Workflow",
                Acoes = new List<WorkflowAcao>
                {
                    new WorkflowAcao { Tipo = "InvalidType" }
                }
            };

            _mockBusinessRuleStrategy.Setup(x => x.ValidateWorkflowAsync(workflow))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<NotImplementedException>(
                () => _service.ExecutarWorkflowAsync(workflow));
        }

        [Fact]
        public async Task ExecutarWorkflowAsync_ActionError_NotifiesAdmins()
        {
            // Arrange
            var workflow = new Workflow
            {
                Nome = "Error Workflow",
                Acoes = new List<WorkflowAcao>
                {
                    new WorkflowAcao
                    {
                        Tipo = "EnviarEmail",
                        Parametros = JsonSerializer.Serialize(new Dictionary<string, string>())
                    }
                }
            };

            _mockBusinessRuleStrategy.Setup(x => x.ValidateWorkflowAsync(workflow))
                .ReturnsAsync(true);
            _mockEmailService.Setup(x => x.EnviarEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _service.ExecutarWorkflowAsync(workflow));

            _mockNotificationService.Verify(x => x.EnviarNotificacaoAsync(
                _workflowSettings.WorkflowAdministrators[0],
                $"Erro no Workflow {workflow.Nome}",
                It.Is<string>(msg => msg.Contains("Test error"))), Times.Once);
        }
    }
} 