using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Services.Contracts
{
    public interface IWorkflowService
    {
        // Operações básicas
        Task<IEnumerable<Workflow>> GetAllWorkflowsAsync();
        Task<Workflow?> GetWorkflowByIdAsync(int id);
        Task<Workflow> CreateWorkflowAsync(Workflow workflow);
        Task<Workflow> UpdateWorkflowAsync(Workflow workflow);
        Task DeleteWorkflowAsync(int id);

        // Operações de Execução
        Task<WorkflowExecucao> ExecutarWorkflowAsync(int workflowId, IDictionary<string, object> parametros);
        Task<WorkflowExecucao> AgendarWorkflowAsync(int workflowId, DateTime dataExecucao, IDictionary<string, object> parametros);
        Task CancelarExecucaoAsync(int execucaoId);
        Task<IEnumerable<WorkflowExecucao>> GetExecucoesWorkflowAsync(int workflowId);
        Task<IEnumerable<WorkflowExecucao>> GetExecucoesEmAndamentoAsync();

        // Operações de Gatilhos
        Task<WorkflowGatilho> AddGatilhoAsync(int workflowId, WorkflowGatilho gatilho);
        Task<WorkflowGatilho> UpdateGatilhoAsync(WorkflowGatilho gatilho);
        Task RemoveGatilhoAsync(int gatilhoId);
        Task<IEnumerable<WorkflowGatilho>> GetGatilhosWorkflowAsync(int workflowId);
        Task<IEnumerable<Workflow>> GetWorkflowsPorTipoGatilhoAsync(TipoGatilho tipo);

        // Operações de Condições e Ações
        Task<bool> ValidarCondicoesAsync(int workflowId, IDictionary<string, object> contexto);
        Task<bool> ExecutarAcoesAsync(int workflowId, IDictionary<string, object> contexto);
        Task<IEnumerable<string>> GetAcoesDisponiveisAsync();
        Task<IEnumerable<string>> GetCondicoesDisponiveisAsync();

        // Monitoramento e Análise
        Task<IEnumerable<WorkflowExecucao>> GetExecucoesComErroAsync();
        Task<IDictionary<string, int>> GetEstatisticasExecucaoAsync(int workflowId);
        Task<TimeSpan> GetTempoMedioExecucaoAsync(int workflowId);
        Task<IEnumerable<Workflow>> GetWorkflowsInativosAsync();

        // Notificações
        Task NotificarErroExecucaoAsync(int execucaoId);
        Task NotificarConclusaoAsync(int execucaoId);
    }
} 