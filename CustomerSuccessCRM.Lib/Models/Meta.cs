namespace CustomerSuccessCRM.Lib.Models
{
    public class Meta
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal Progresso { get; set; }
        public string ResponsavelId { get; set; } = string.Empty;
        public string EquipeId { get; set; } = string.Empty;
        public StatusMeta Status { get; set; } = StatusMeta.EmAndamento;
        public DateTime DataInicio { get; set; } = DateTime.Now;
        public DateTime DataFim { get; set; }
        public DateTime? DataConclusao { get; set; }
    }

    public enum StatusMeta
    {
        EmAndamento,
        Concluida,
        Atrasada,
        Cancelada
    }
} 