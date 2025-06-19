using System.ComponentModel.DataAnnotations;

namespace CustomerSuccessCRM.Lib.Models
{
    public class Workflow
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public TipoWorkflow Tipo { get; set; }
        public bool Ativo { get; set; } = true;
        public List<WorkflowCondicao> Condicoes { get; set; } = new();
        public List<WorkflowAcao> Acoes { get; set; } = new();
        public string? CronExpressao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? UltimaExecucao { get; set; }
        public DateTime? ProximaExecucao { get; set; }
        public string? ResponsavelId { get; set; }
        public string? Observacoes { get; set; }

        // Propriedades de navegação
        public virtual ICollection<WorkflowExecucao> Execucoes { get; set; } = new List<WorkflowExecucao>();
        public virtual ICollection<WorkflowGatilho> Gatilhos { get; set; } = new List<WorkflowGatilho>();
    }

    public class WorkflowExecucao
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public DateTime DataExecucao { get; set; }
        public StatusExecucao Status { get; set; }
        public string? Resultado { get; set; }
        public string? ErroMensagem { get; set; }
        public virtual Workflow Workflow { get; set; } = null!;
    }

    public class WorkflowGatilho
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public TipoGatilho Tipo { get; set; }
        public string CondicaoJson { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public virtual Workflow Workflow { get; set; } = null!;
    }

    public class WorkflowCondicao
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public string Campo { get; set; } = string.Empty;
        public string Operador { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public string? ValorAdicional { get; set; }
        public bool NecessarioTodas { get; set; }
        public int Ordem { get; set; }
    }

    public class WorkflowAcao
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Parametros { get; set; } = string.Empty;
        public bool Obrigatoria { get; set; }
        public int Ordem { get; set; }
        public string? Descricao { get; set; }
    }

    public enum TipoWorkflow
    {
        Automatico,
        Manual,
        Agendado,
        Condicional
    }

    public enum PrioridadeWorkflow
    {
        Baixa,
        Normal,
        Alta,
        Critica
    }

    public enum StatusExecucao
    {
        Agendada,
        EmExecucao,
        Concluida,
        Erro,
        Cancelada
    }

    public enum TipoGatilho
    {
        CriacaoRegistro,
        AtualizacaoRegistro,
        ExclusaoRegistro,
        MudancaStatus,
        DataHora,
        CondicaoPersonalizada
    }
} 