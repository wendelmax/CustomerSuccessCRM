using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models
{
    [Table("Propostas")]
    public class Proposta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        [DisplayName("Cliente")]
        public int ClienteId { get; set; }

        [Required]
        [ForeignKey("Oportunidade")]
        [DisplayName("Oportunidade")]
        public int OportunidadeId { get; set; }

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

        [Range(0, 100, ErrorMessage = "O desconto deve estar entre {1}% e {2}%")]
        [Column(TypeName = "decimal(5,2)")]
        [DisplayName("Desconto Aplicado (%)")]
        public decimal DescontoAplicado { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayName("Data de Aprovação")]
        public DateTime? DataAprovacao { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data de Validade")]
        public DateTime DataValidade { get; set; }

        [Required]
        [DisplayName("Status")]
        public StatusProposta Status { get; set; } = StatusProposta.Rascunho;

        [StringLength(100, ErrorMessage = "O nome do responsável deve ter no máximo {1} caracteres")]
        [DisplayName("Responsável pela Criação")]
        public string? ResponsavelCriacao { get; set; }

        [StringLength(100, ErrorMessage = "O nome do responsável deve ter no máximo {1} caracteres")]
        [DisplayName("Responsável pela Aprovação")]
        public string? ResponsavelAprovacao { get; set; }

        [StringLength(1000, ErrorMessage = "As observações devem ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observações Internas")]
        public string? ObservacoesInternas { get; set; }

        // Propriedades de navegação
        public virtual Cliente Cliente { get; set; } = null!;
        public virtual Oportunidade Oportunidade { get; set; } = null!;
        public virtual ICollection<PropostaItem> Itens { get; set; } = new List<PropostaItem>();
        public virtual ICollection<PropostaHistorico> Historico { get; set; } = new List<PropostaHistorico>();
        public virtual ICollection<PropostaAnexo> Anexos { get; set; } = new List<PropostaAnexo>();
    }

    [Table("PropostaItens")]
    public class PropostaItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Proposta")]
        public int PropostaId { get; set; }

        [Required]
        [ForeignKey("Produto")]
        [DisplayName("Produto")]
        public int ProdutoId { get; set; }

        [Required]
        [Range(1, 999, ErrorMessage = "A quantidade deve estar entre {1} e {2}")]
        [DisplayName("Quantidade")]
        public int Quantidade { get; set; }

        [Required]
        [Range(0, 9999999.99, ErrorMessage = "O valor unitário deve estar entre {1} e {2}")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Valor Unitário")]
        [DataType(DataType.Currency)]
        public decimal ValorUnitario { get; set; }

        [Range(0, 100, ErrorMessage = "O desconto deve estar entre {1}% e {2}%")]
        [Column(TypeName = "decimal(5,2)")]
        [DisplayName("Desconto do Item (%)")]
        public decimal DescontoItem { get; set; }

        [StringLength(500, ErrorMessage = "As observações devem ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observações")]
        public string? Observacoes { get; set; }
        
        public virtual Proposta Proposta { get; set; } = null!;
        public virtual Produto Produto { get; set; } = null!;
    }

    [Table("PropostaHistoricos")]
    public class PropostaHistorico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Proposta")]
        public int PropostaId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data da Modificação")]
        public DateTime DataModificacao { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A descrição da modificação é obrigatória")]
        [StringLength(1000, ErrorMessage = "A descrição da modificação deve ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Modificação")]
        public string Modificacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O responsável pela modificação é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome do responsável deve ter no máximo {1} caracteres")]
        [DisplayName("Responsável")]
        public string ResponsavelModificacao { get; set; } = string.Empty;

        public virtual Proposta Proposta { get; set; } = null!;
    }

    [Table("PropostaAnexos")]
    public class PropostaAnexo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Proposta")]
        public int PropostaId { get; set; }

        [Required(ErrorMessage = "O nome do arquivo é obrigatório")]
        [StringLength(255, ErrorMessage = "O nome do arquivo deve ter no máximo {1} caracteres")]
        [DisplayName("Nome do Arquivo")]
        public string NomeArquivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O caminho do arquivo é obrigatório")]
        [StringLength(1000, ErrorMessage = "O caminho do arquivo deve ter no máximo {1} caracteres")]
        [DisplayName("Caminho do Arquivo")]
        public string CaminhoArquivo { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data de Upload")]
        public DateTime DataUpload { get; set; } = DateTime.Now;

        public virtual Proposta Proposta { get; set; } = null!;
    }

    public enum StatusProposta
    {
        [Description("Rascunho")]
        Rascunho,
        [Description("Em Análise")]
        EmAnalise,
        [Description("Aprovada")]
        Aprovada,
        [Description("Rejeitada")]
        Rejeitada,
        [Description("Expirada")]
        Expirada,
        [Description("Convertida em Contrato")]
        ConvertidaEmContrato
    }
} 