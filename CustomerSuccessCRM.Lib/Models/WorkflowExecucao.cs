using System;

namespace CustomerSuccessCRM.Lib.Models
{
    public class WorkflowExecucao
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public DateTime? DataAgendada { get; set; }
        public string Parametros { get; set; }
        public string Resultado { get; set; }
        public string ErroMensagem { get; set; }
        public StatusExecucao Status { get; set; }
        public string Mensagem { get; set; }

        public virtual Workflow Workflow { get; set; }
    }

    public enum StatusExecucao
    {
        Agendado,
        EmAndamento,
        Concluido,
        Erro,
        Cancelado
    }
} 