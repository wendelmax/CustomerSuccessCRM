using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public class WorkflowRepository : Repository<Workflow>, IWorkflowRepository
    {
        public WorkflowRepository(CrmDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Workflow>> GetWorkflowsWithRelationshipsAsync()
        {
            return await _dbSet
                .Include(w => w.Gatilhos)
                .Include(w => w.Acoes)
                .Include(w => w.Condicoes)
                .ToListAsync();
        }

        public async Task<Workflow?> GetWorkflowWithRelationshipsAsync(int id)
        {
            return await _dbSet
                .Include(w => w.Gatilhos)
                .Include(w => w.Acoes)
                .Include(w => w.Condicoes)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<IEnumerable<Workflow>> GetWorkflowsByTriggerTypeAsync(TipoGatilho tipo)
        {
            return await _dbSet
                .Include(w => w.Gatilhos)
                .Where(w => w.Gatilhos.Any(g => g.Tipo == tipo))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowExecucao>> GetWorkflowExecutionsAsync(int workflowId)
        {
            return await _context.WorkflowExecucoes
                .Where(e => e.WorkflowId == workflowId)
                .OrderByDescending(e => e.DataInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowExecucao>> GetActiveExecutionsAsync()
        {
            return await _context.WorkflowExecucoes
                .Where(e => e.Status == StatusExecucao.EmAndamento)
                .Include(e => e.Workflow)
                .OrderByDescending(e => e.DataInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkflowExecucao>> GetFailedExecutionsAsync()
        {
            return await _context.WorkflowExecucoes
                .Where(e => e.Status == StatusExecucao.Erro)
                .Include(e => e.Workflow)
                .OrderByDescending(e => e.DataInicio)
                .ToListAsync();
        }

        public async Task<IDictionary<string, int>> GetExecutionStatisticsAsync(int workflowId)
        {
            var execucoes = await _context.WorkflowExecucoes
                .Where(e => e.WorkflowId == workflowId)
                .ToListAsync();

            return new Dictionary<string, int>
            {
                { "Total", execucoes.Count },
                { "Sucesso", execucoes.Count(e => e.Status == StatusExecucao.Concluido) },
                { "Erro", execucoes.Count(e => e.Status == StatusExecucao.Erro) },
                { "EmAndamento", execucoes.Count(e => e.Status == StatusExecucao.EmAndamento) }
            };
        }

        public async Task<TimeSpan> GetAverageExecutionTimeAsync(int workflowId)
        {
            var execucoes = await _context.WorkflowExecucoes
                .Where(e => e.WorkflowId == workflowId && e.Status == StatusExecucao.Concluido)
                .ToListAsync();

            if (!execucoes.Any())
                return TimeSpan.Zero;

            var tempoTotal = execucoes
                .Where(e => e.DataFim.HasValue)
                .Sum(e => (e.DataFim!.Value - e.DataInicio).TotalMilliseconds);

            return TimeSpan.FromMilliseconds(tempoTotal / execucoes.Count);
        }

        public async Task<IEnumerable<Workflow>> GetInactiveWorkflowsAsync()
        {
            var dataLimite = DateTime.UtcNow.AddDays(-30);
            return await _dbSet
                .Include(w => w.Execucoes)
                .Where(w => !w.Execucoes.Any() || w.Execucoes.Max(e => e.DataInicio) < dataLimite)
                .ToListAsync();
        }
    }
} 