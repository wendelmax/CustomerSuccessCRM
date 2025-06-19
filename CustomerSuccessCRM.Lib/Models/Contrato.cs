using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models
{
    [Table("Contratos")]
    public class Contrato
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

        [Required(ErrorMessage = "O conteúdo é obrigatório")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Conteúdo")]
        public string Conteudo { get; set; } = string.Empty;

        [Required]
        [Range(0, 9999999.99, ErrorMessage = "O valor total deve estar entre {1} e {2}")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Valor Total")]
        [DataType(DataType.Currency)]
        public decimal ValorTotal { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayName("Data de Assinatura")]
        public DateTime? DataAssinatura { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Data de Vencimento")]
        public DateTime? DataVencimento { get; set; }

        [Required]
        [DisplayName("Status")]
        public StatusContrato Status { get; set; } = StatusContrato.Rascunho;

        [StringLength(100, ErrorMessage = "O nome do responsável deve ter no máximo {1} caracteres")]
        [DisplayName("Responsável pela Criação")]
        public string? ResponsavelCriacao { get; set; }

        [StringLength(100, ErrorMessage = "O nome do responsável deve ter no máximo {1} caracteres")]
        [DisplayName("Responsável pela Aprovação")]
        public string? ResponsavelAprovacao { get; set; }

        [StringLength(500, ErrorMessage = "O hash da assinatura deve ter no máximo {1} caracteres")]
        [DisplayName("Hash da Assinatura")]
        public string? HashAssinatura { get; set; }

        [ForeignKey("Proposta")]
        [DisplayName("Proposta")]
        public int? PropostaId { get; set; }

        // Propriedades de navegação
        public virtual Cliente Cliente { get; set; } = null!;
        public virtual Proposta? Proposta { get; set; }
        public virtual ICollection<ContratoHistorico> Historico { get; set; } = new List<ContratoHistorico>();
        public virtual ICollection<ContratoAnexo> Anexos { get; set; } = new List<ContratoAnexo>();
    }

    public enum StatusContrato
    {
        [Description("Rascunho")]
        Rascunho,
        [Description("Em Análise")]
        EmAnalise,
        [Description("Aprovado")]
        Aprovado,
        [Description("Assinado")]
        Assinado,
        [Description("Cancelado")]
        Cancelado,
        [Description("Vencido")]
        Vencido
    }
} 