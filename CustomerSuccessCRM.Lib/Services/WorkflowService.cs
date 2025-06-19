using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Services.Strategies;
using CustomerSuccessCRM.Lib.Configuration;
using CustomerSuccessCRM.Lib.Repositories;
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
        private readonly IWorkflowRepository _repository;
        private readonly Dictionary<string, Func<WorkflowAcao, Task>> _acaoHandlers;

        public WorkflowService(
            IBusinessRuleStrategy businessRuleStrategy,
            IOptions<WorkflowSettings> workflowSettings,
            INotificationService notificationService,
            IEmailService emailService,
            ICrmService crmService,
            IWorkflowRepository repository)
        {
            _businessRuleStrategy = businessRuleStrategy;
            _workflowSettings = workflowSettings.Value;
            _notificationService = notificationService;
            _emailService = emailService;
            _crmService = crmService;
            _repository = repository;

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

        public async Task<IEnumerable<Workflow>> GetAllWorkflowsAsync()
        {
            return await _repository.GetWorkflowsWithRelationshipsAsync();
        }

        public async Task<Workflow?> GetWorkflowByIdAsync(int id)
        {
            return await _repository.GetWorkflowWithRelationshipsAsync(id);
        }

        public async Task<Workflow> CreateWorkflowAsync(Workflow workflow)
        {
            return await _repository.AddAsync(workflow);
        }

        public async Task<Workflow> UpdateWorkflowAsync(Workflow workflow)
        {
            return await _repository.UpdateAsync(workflow);
        }

        public async Task DeleteWorkflowAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<WorkflowExecucao> AgendarWorkflowAsync(int workflowId, DateTime dataExecucao, IDictionary<string, object> parametros)
        {
            var workflow = await GetWorkflowByIdAsync(workflowId);
            if (workflow == null)
                throw new InvalidOperationException("Workflow não encontrado");

            var execucao = new WorkflowExecucao
            {
                WorkflowId = workflowId,
                DataAgendada = dataExecucao,
                Status = StatusExecucao.Agendado,
                Parametros = System.Text.Json.JsonSerializer.Serialize(parametros)
            };

            await _repository.AddAsync(execucao);
            return execucao;
        }

        public async Task CancelarExecucaoAsync(int execucaoId)
        {
            var execucao = await _repository.GetByIdAsync(execucaoId);
            if (execucao != null)
            {
                execucao.Status = StatusExecucao.Cancelado;
                await _repository.UpdateAsync(execucao);
            }
        }

        public async Task<IEnumerable<WorkflowExecucao>> GetExecucoesWorkflowAsync(int workflowId)
        {
            return await _repository.GetWorkflowExecutionsAsync(workflowId);
        }

        public async Task<IEnumerable<WorkflowExecucao>> GetExecucoesEmAndamentoAsync()
        {
            return await _repository.GetActiveExecutionsAsync();
        }

        public async Task<IEnumerable<WorkflowExecucao>> GetExecucoesComErroAsync()
        {
            return await _repository.GetFailedExecutionsAsync();
        }

        public async Task<IDictionary<string, int>> GetEstatisticasExecucaoAsync(int workflowId)
        {
            return await _repository.GetExecutionStatisticsAsync(workflowId);
        }

        public async Task<TimeSpan> GetTempoMedioExecucaoAsync(int workflowId)
        {
            return await _repository.GetAverageExecutionTimeAsync(workflowId);
        }

        public async Task<IEnumerable<Workflow>> GetWorkflowsInativosAsync()
        {
            return await _repository.GetInactiveWorkflowsAsync();
        }

        public async Task NotificarErroExecucaoAsync(int workflowId)
        {
            var workflow = await GetWorkflowByIdAsync(workflowId);
            if (workflow == null) return;

            foreach (var admin in _workflowSettings.WorkflowAdministrators)
            {
                await _notificationService.EnviarNotificacaoAsync(
                    admin,
                    $"Erro em Workflow",
                    $"O workflow {workflow.Nome} apresentou erro na execução.");
            }
        }

        public async Task NotificarConclusaoAsync(int workflowId)
        {
            var workflow = await GetWorkflowByIdAsync(workflowId);
            if (workflow == null) return;

            foreach (var admin in _workflowSettings.WorkflowAdministrators)
            {
                await _notificationService.EnviarNotificacaoAsync(
                    admin,
                    $"Workflow Concluído",
                    $"O workflow {workflow.Nome} foi concluído com sucesso.");
            }
        }
    }
} 