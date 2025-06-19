using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IWorkflowRepository : IRepository<Workflow>
    {
        Task<IEnumerable<Workflow>> GetWorkflowsWithRelationshipsAsync();
        Task<Workflow?> GetWorkflowWithRelationshipsAsync(int id);
        Task<IEnumerable<Workflow>> GetWorkflowsByTriggerTypeAsync(TipoGatilho tipo);
        Task<IEnumerable<WorkflowExecucao>> GetWorkflowExecutionsAsync(int workflowId);
        Task<IEnumerable<WorkflowExecucao>> GetActiveExecutionsAsync();
        Task<IEnumerable<WorkflowExecucao>> GetFailedExecutionsAsync();
        Task<IDictionary<string, int>> GetExecutionStatisticsAsync(int workflowId);
        Task<TimeSpan> GetAverageExecutionTimeAsync(int workflowId);
        Task<IEnumerable<Workflow>> GetInactiveWorkflowsAsync();
    }
} 