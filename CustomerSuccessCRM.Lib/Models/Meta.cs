using System.ComponentModel.DataAnnotations;

namespace CustomerSuccessCRM.Lib.Models
{
    public class Meta
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public TipoMeta Tipo { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorAtingido { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string ResponsavelId { get; set; } = string.Empty;
        public string? ResponsavelNome { get; set; }
        public StatusMeta Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string? Observacoes { get; set; }

        // Propriedades calculadas
        public decimal PercentualAtingido => Valor > 0 ? (ValorAtingido / Valor) * 100 : 0;
        public bool MetaAtingida => PercentualAtingido >= 100;

        // Propriedades de navegação
        public virtual ICollection<MetaHistorico> Historico { get; set; } = new List<MetaHistorico>();
    }

    public class MetaHistorico
    {
        public int Id { get; set; }
        public int MetaId { get; set; }
        public DateTime DataModificacao { get; set; }
        public decimal ValorAnterior { get; set; }
        public decimal ValorNovo { get; set; }
        public string Responsavel { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public virtual Meta Meta { get; set; } = null!;
    }

    public enum TipoMeta
    {
        Vendas,
        NPS,
        Churn,
        Receita,
        Leads,
        Tickets,
        Custom
    }

    public enum StatusMeta
    {
        EmAndamento,
        Concluida,
        NaoAtingida,
        Cancelada
    }

    public enum PeriodoMeta
    {
        Diario,
        Semanal,
        Mensal,
        Trimestral,
        Semestral,
        Anual
    }
} 