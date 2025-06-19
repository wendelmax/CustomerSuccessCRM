using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Services.Contracts;
using CustomerSuccessCRM.Lib.Services.Strategies;
using CustomerSuccessCRM.Lib.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerSuccessCRM.Lib.Services.Implementations
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IBusinessRuleStrategy _businessRuleStrategy;
        private readonly WorkflowSettings _workflowSettings;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly ICrmService _crmService;
        private readonly Dictionary<string, Func<WorkflowAcao, Task>> _acaoHandlers;

        public WorkflowService(
            IBusinessRuleStrategy businessRuleStrategy,
            IOptions<WorkflowSettings> workflowSettings,
            INotificationService notificationService,
            IEmailService emailService,
            ICrmService crmService)
        {
            _businessRuleStrategy = businessRuleStrategy;
            _workflowSettings = workflowSettings.Value;
            _notificationService = notificationService;
            _emailService = emailService;
            _crmService = crmService;

            // Registrar handlers para cada tipo de ação
            _acaoHandlers = new Dictionary<string, Func<WorkflowAcao, Task>>
            {
                { "EnviarEmail", HandleEnviarEmailAsync },
                { "AtualizarStatus", HandleAtualizarStatusAsync },
                { "CriarTarefa", HandleCriarTarefaAsync },
                { "NotificarResponsavel", HandleNotificarResponsavelAsync }
            };
        }

        public async Task<bool> ValidateWorkflowAsync(Workflow workflow)
        {
            return await _businessRuleStrategy.ValidateWorkflowAsync(workflow);
        }

        public async Task ExecutarWorkflowAsync(Workflow workflow)
        {
            if (!_workflowSettings.EnableAutomaticWorkflows)
                return;

            // Validar workflow
            if (!await ValidateWorkflowAsync(workflow))
                throw new InvalidOperationException("Workflow inválido");

            // Verificar limite de workflows concorrentes
            if (workflow.Acoes.Count() > _workflowSettings.MaxConcurrentWorkflows)
                throw new InvalidOperationException($"Número de ações excede o limite de {_workflowSettings.MaxConcurrentWorkflows} workflows concorrentes");

            try
            {
                // Executar ações do workflow
                foreach (var acao in workflow.Acoes)
                {
                    if (_acaoHandlers.TryGetValue(acao.Tipo, out var handler))
                    {
                        await handler(acao);
                    }
                    else
                    {
                        throw new NotImplementedException($"Tipo de ação não implementado: {acao.Tipo}");
                    }
                }

                // Notificar administradores sobre sucesso
                foreach (var admin in _workflowSettings.WorkflowAdministrators)
                {
                    await _notificationService.EnviarNotificacaoAsync(
                        admin,
                        $"Workflow {workflow.Nome} executado",
                        $"O workflow {workflow.Nome} foi executado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                // Notificar administradores sobre falha
                foreach (var admin in _workflowSettings.WorkflowAdministrators)
                {
                    await _notificationService.EnviarNotificacaoAsync(
                        admin,
                        $"Erro no Workflow {workflow.Nome}",
                        $"Erro ao executar o workflow {workflow.Nome}: {ex.Message}");
                }

                throw; // Re-throw para manter o stack trace
            }
        }

        private async Task HandleEnviarEmailAsync(WorkflowAcao acao)
        {
            var parametros = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(acao.Parametros);
            
            await _emailService.EnviarEmailAsync(
                parametros["destinatario"],
                parametros["assunto"],
                parametros["corpo"]);
        }

        private async Task HandleAtualizarStatusAsync(WorkflowAcao acao)
        {
            var parametros = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(acao.Parametros);
            var clienteId = int.Parse(parametros["clienteId"]);
            var novoStatus = Enum.Parse<StatusCliente>(parametros["novoStatus"]);

            var cliente = await _crmService.GetClienteByIdAsync(clienteId);
            if (cliente != null)
            {
                cliente.Status = novoStatus;
                await _crmService.UpdateClienteAsync(cliente);
            }
        }

        private async Task HandleCriarTarefaAsync(WorkflowAcao acao)
        {
            var parametros = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(acao.Parametros);
            
            var interacao = new Interacao
            {
                ClienteId = int.Parse(parametros["clienteId"]),
                Assunto = parametros["assunto"],
                Descricao = parametros["descricao"],
                Responsavel = parametros["responsavel"],
                TipoInteracao = TipoInteracao.Tarefa,
                Status = StatusInteracao.Pendente,
                DataInteracao = DateTime.Now
            };

            await _crmService.CreateInteracaoAsync(interacao);
        }

        private async Task HandleNotificarResponsavelAsync(WorkflowAcao acao)
        {
            var parametros = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(acao.Parametros);
            
            await _notificationService.EnviarNotificacaoAsync(
                parametros["responsavel"],
                parametros["titulo"],
                parametros["mensagem"]);
        }
    }
} 