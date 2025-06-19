using System.ComponentModel.DataAnnotations;

namespace CustomerSuccessCRM.Lib.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Descricao { get; set; }

        public decimal PrecoBase { get; set; }

        [StringLength(50)]
        public string? Codigo { get; set; }

        public CategoriaProduto Categoria { get; set; } = CategoriaProduto.Outro;

        public TipoPrecificacao TipoPrecificacao { get; set; } = TipoPrecificacao.Fixo;

        public bool Ativo { get; set; } = true;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public DateTime? UltimaAtualizacao { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }

        public bool PermiteDesconto { get; set; }

        public decimal? DescontoMaximo { get; set; }

        public bool RequerAprovacao { get; set; }

        public string? CondicoesEspeciaisJson { get; set; }

        // Propriedades de navegação
        public virtual ICollection<ProdutoVariacao> Variacoes { get; set; } = new List<ProdutoVariacao>();
        public virtual ICollection<ProdutoPreco> TabelaPrecos { get; set; } = new List<ProdutoPreco>();
        public virtual ICollection<ProdutoBundle> Bundles { get; set; } = new List<ProdutoBundle>();
        public virtual ICollection<ProdutoBundle> BundlesRelacionados { get; set; } = new List<ProdutoBundle>();
    }

    public class ProdutoVariacao
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal AjustePreco { get; set; }
        public bool Ativo { get; set; } = true;
        public string? Especificacoes { get; set; }
        public virtual Produto Produto { get; set; } = null!;
    }

    public class ProdutoPreco
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? CondicoesJson { get; set; }
        public bool Ativo { get; set; } = true;
        public virtual Produto Produto { get; set; } = null!;
    }

    public class ProdutoBundle
    {
        public int Id { get; set; }
        public int ProdutoPrincipalId { get; set; }
        public int ProdutoRelacionadoId { get; set; }
        public int Quantidade { get; set; }
        public decimal DescontoBundle { get; set; }
        public bool Obrigatorio { get; set; }
        public virtual Produto ProdutoPrincipal { get; set; } = null!;
        public virtual Produto ProdutoRelacionado { get; set; } = null!;
    }

    public enum CategoriaProduto
    {
        Software,
        Hardware,
        Servico,
        Consultoria,
        Treinamento,
        Suporte,
        Licenca,
        Assinatura,
        Bundle,
        Outro
    }

    public enum TipoPrecificacao
    {
        Fixo,
        Variavel,
        PorVolume,
        PorUsuario,
        Personalizado
    }
} 