using System.ComponentModel.DataAnnotations;

namespace CustomerSuccessCRM.Lib.Models
{
    public class Oportunidade
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Descricao { get; set; }

        public decimal ValorEstimado { get; set; }

        public decimal ValorReal { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataFechamento { get; set; }

        public DateTime? DataProximoContato { get; set; }

        public FaseOportunidade Fase { get; set; } = FaseOportunidade.Prospeccao;

        public ProbabilidadeSucesso Probabilidade { get; set; } = ProbabilidadeSucesso.Baixa;

        [StringLength(100)]
        public string? Responsavel { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }

        // Propriedade de navegação
        public virtual Cliente Cliente { get; set; } = null!;
    }

    public enum FaseOportunidade
    {
        Prospeccao,
        Qualificacao,
        Proposta,
        Negociacao,
        Fechada,
        Perdida
    }

    public enum ProbabilidadeSucesso
    {
        Baixa = 25,
        Media = 50,
        Alta = 75,
        MuitoAlta = 90
    }
} 