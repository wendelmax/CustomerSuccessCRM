namespace CustomerSuccessCRM.Lib.Models
{
    public class Oportunidade
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string VendedorId { get; set; } = string.Empty;
        public StatusOportunidade Status { get; set; } = StatusOportunidade.Prospeccao;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataFechamento { get; set; }
    }

    public enum StatusOportunidade
    {
        Prospeccao,
        Qualificacao,
        Proposta,
        Negociacao,
        Fechada,
        Perdida
    }
} 