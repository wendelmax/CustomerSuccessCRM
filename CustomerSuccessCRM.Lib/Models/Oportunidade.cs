using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models
{
    [Table("Oportunidades")]
    public class Oportunidade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        [DisplayName("Cliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(200, ErrorMessage = "O título deve ter no máximo {1} caracteres")]
        [DisplayName("Título")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descrição")]
        public string? Descricao { get; set; }

        [Required]
        [Range(0, 9999999.99, ErrorMessage = "O valor estimado deve estar entre {1} e {2}")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Valor Estimado")]
        [DataType(DataType.Currency)]
        public decimal ValorEstimado { get; set; }

        [Range(0, 9999999.99, ErrorMessage = "O valor real deve estar entre {1} e {2}")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Valor Real")]
        [DataType(DataType.Currency)]
        public decimal ValorReal { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayName("Data de Fechamento")]
        public DateTime? DataFechamento { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Data do Próximo Contato")]
        public DateTime? DataProximoContato { get; set; }

        [Required]
        [DisplayName("Fase")]
        public FaseOportunidade Fase { get; set; } = FaseOportunidade.Prospeccao;

        [Required]
        [DisplayName("Probabilidade")]
        public ProbabilidadeSucesso Probabilidade { get; set; } = ProbabilidadeSucesso.Baixa;

        [StringLength(100, ErrorMessage = "O nome do responsável deve ter no máximo {1} caracteres")]
        [DisplayName("Responsável")]
        public string? Responsavel { get; set; }

        [StringLength(500, ErrorMessage = "As observações devem ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observações")]
        public string? Observacoes { get; set; }

        // Propriedade de navegação
        public virtual Cliente Cliente { get; set; } = null!;
        
        public virtual ICollection<Produto> Produtos { get; set; } = null!;
    }

    public enum FaseOportunidade
    {
        [Description("Aberta")]
        Aberta,
        [Description("Prospecção")]
        Prospeccao,
        [Description("Qualificação")]
        Qualificacao,
        [Description("Proposta")]
        Proposta,
        [Description("Negociação")]
        Negociacao,
        [Description("Fechada")]
        Fechada,
        [Description("Perdida")]
        Perdida
    }

    public enum ProbabilidadeSucesso
    {
        [Description("Baixa (25%)")]
        Baixa = 25,
        [Description("Média (50%)")]
        Media = 50,
        [Description("Alta (75%)")]
        Alta = 75,
        [Description("Muito Alta (90%)")]
        MuitoAlta = 90
    }
} 