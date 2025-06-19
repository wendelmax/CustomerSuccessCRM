namespace CustomerSuccessCRM.Lib.Models
{
    public class Interacao
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Assunto { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Responsavel { get; set; } = string.Empty;
        public TipoInteracao TipoInteracao { get; set; }
        public StatusInteracao Status { get; set; } = StatusInteracao.Pendente;
        public DateTime DataInteracao { get; set; } = DateTime.Now;
        public DateTime? DataConclusao { get; set; }
    }

    public enum TipoInteracao
    {
        Telefone,
        Email,
        Reuniao,
        Tarefa
    }

    public enum StatusInteracao
    {
        Pendente,
        EmAndamento,
        Concluida,
        Cancelada
    }
} 