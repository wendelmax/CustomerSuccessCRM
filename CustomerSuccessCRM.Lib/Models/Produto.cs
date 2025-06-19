using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models
{
    [Table("Produtos")]
    public sealed class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
        [DisplayName("Nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço base é obrigatório")]
        [Range(0, 999999.99, ErrorMessage = "O preço base deve estar entre {1} e {2}")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Preço Base")]
        [DataType(DataType.Currency)]
        public decimal PrecoBase { get; set; }

        [Required(ErrorMessage = "O código é obrigatório")]
        [StringLength(50, ErrorMessage = "O código deve ter no máximo {1} caracteres")]
        [DisplayName("Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [DisplayName("Categoria")]
        public CategoriaProduto Categoria { get; set; }

        [Required]
        [DisplayName("Tipo de Precificação")]
        public TipoPrecificacao TipoPrecificacao { get; set; }

        [StringLength(1000, ErrorMessage = "As observações devem ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observações")]
        public string Observacoes { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "O desconto máximo deve estar entre {1}% e {2}%")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Desconto Máximo (%)")]
        public decimal DescontoMaximo { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A quantidade em estoque não pode ser negativa")]
        [DisplayName("Quantidade em Estoque")]
        public int QuantidadeEstoque { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A quantidade vendida não pode ser negativa")]
        [DisplayName("Quantidade Vendida")]
        public int QuantidadeVendida { get; set; }

        [StringLength(200, ErrorMessage = "O nome do fornecedor deve ter no máximo {1} caracteres")]
        [DisplayName("Fornecedor")]
        public string Fornecedor { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "O percentual de desconto deve estar entre {1}% e {2}%")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Percentual de Desconto (%)")]
        public decimal PercentualDesconto { get; set; }

        [Required]
        [DisplayName("Ativo")]
        public bool Ativo { get; set; } = true;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data de Cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayName("Data de Atualização")]
        public DateTime? DataAtualizacao { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "A tabela de preço deve ter no máximo {1} caracteres")]
        [DisplayName("Tabela de Preço")]
        public string TabelaPreco { get; set; } = string.Empty;

        // Propriedades de navegação
        public ICollection<ProdutoVariacao> Variacoes { get; set; } = new List<ProdutoVariacao>();
        public ICollection<ProdutoPreco> Precos { get; set; } = new List<ProdutoPreco>();
        public ICollection<ProdutoBundle> Bundles { get; set; } = new List<ProdutoBundle>();
        public ICollection<ProdutoBundle> BundlesRelacionados { get; set; } = new List<ProdutoBundle>();
    }

    [Table("ProdutoPrecos")]
    public sealed class ProdutoPreco
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "A tabela de preço é obrigatória")]
        [StringLength(50, ErrorMessage = "A tabela de preço deve ter no máximo {1} caracteres")]
        [DisplayName("Tabela de Preço")]
        public string TabelaPreco { get; set; } = string.Empty;

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Range(0, 999999.99, ErrorMessage = "O valor deve estar entre {1} e {2}")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Valor")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data de Início")]
        public DateTime DataInicio { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayName("Data de Fim")]
        public DateTime? DataFim { get; set; }

        [Required]
        [DisplayName("Ativo")]
        public bool Ativo { get; set; } = true;

        public Produto Produto { get; set; } = null!;
    }

    [Table("ProdutoVariacoes")]
    public class ProdutoVariacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
        [DisplayName("Nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ajuste de preço é obrigatório")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Ajuste de Preço")]
        [DataType(DataType.Currency)]
        public decimal AjustePreco { get; set; }

        [Required]
        [DisplayName("Ativo")]
        public bool Ativo { get; set; } = true;

        public virtual Produto Produto { get; set; } = null!;
    }

    [Table("ProdutoBundles")]
    public class ProdutoBundle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
        [DisplayName("Nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O percentual de desconto é obrigatório")]
        [Range(0, 100, ErrorMessage = "O percentual de desconto deve estar entre {1}% e {2}%")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Percentual de Desconto (%)")]
        public decimal PercentualDesconto { get; set; }

        [Required]
        [DisplayName("Ativo")]
        public bool Ativo { get; set; } = true;

        public virtual Produto Produto { get; set; } = null!;
        public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }

    public enum CategoriaProduto
    {
        [Description("Software")]
        Software,
        [Description("Hardware")]
        Hardware,
        [Description("Serviço")]
        Servico,
        [Description("Consultoria")]
        Consultoria,
        [Description("Treinamento")]
        Treinamento,
        [Description("Suporte")]
        Suporte,
        [Description("Outro")]
        Outro
    }

    public enum TipoPrecificacao
    {
        [Description("Fixo")]
        Fixo,
        [Description("Variável")]
        Variavel,
        [Description("Tabela")]
        Tabela,
        [Description("Bundle")]
        Bundle,
        [Description("Personalizado")]
        Personalizado
    }
} 