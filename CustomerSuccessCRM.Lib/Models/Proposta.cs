using System.ComponentModel.DataAnnotations;

namespace CustomerSuccessCRM.Lib.Models
{
    public class Proposta
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public int OportunidadeId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Conteudo { get; set; } = string.Empty;

        public decimal ValorTotal { get; set; }

        public decimal DescontoAplicado { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataAprovacao { get; set; }

        public DateTime DataValidade { get; set; }

        public StatusProposta Status { get; set; } = StatusProposta.Rascunho;

        [StringLength(100)]
        public string? ResponsavelCriacao { get; set; }

        [StringLength(100)]
        public string? ResponsavelAprovacao { get; set; }

        [StringLength(1000)]
        public string? ObservacoesInternas { get; set; }

        // Propriedades de navegação
        public virtual Cliente Cliente { get; set; } = null!;
        public virtual Oportunidade Oportunidade { get; set; } = null!;
        public virtual ICollection<PropostaItem> Itens { get; set; } = new List<PropostaItem>();
        public virtual ICollection<PropostaHistorico> Historico { get; set; } = new List<PropostaHistorico>();
        public virtual ICollection<PropostaAnexo> Anexos { get; set; } = new List<PropostaAnexo>();
    }

    public class PropostaItem
    {
        public int Id { get; set; }
        public int PropostaId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal DescontoItem { get; set; }
        public string? Observacoes { get; set; }
        
        public virtual Proposta Proposta { get; set; } = null!;
        public virtual Produto Produto { get; set; } = null!;
    }

    public class PropostaHistorico
    {
        public int Id { get; set; }
        public int PropostaId { get; set; }
        public DateTime DataModificacao { get; set; }
        public string Modificacao { get; set; } = string.Empty;
        public string ResponsavelModificacao { get; set; } = string.Empty;
        public virtual Proposta Proposta { get; set; } = null!;
    }

    public class PropostaAnexo
    {
        public int Id { get; set; }
        public int PropostaId { get; set; }
        public string NomeArquivo { get; set; } = string.Empty;
        public string CaminhoArquivo { get; set; } = string.Empty;
        public DateTime DataUpload { get; set; }
        public virtual Proposta Proposta { get; set; } = null!;
    }

    public enum StatusProposta
    {
        Rascunho,
        EmAnalise,
        Aprovada,
        Rejeitada,
        Expirada,
        ConvertidaEmContrato
    }
} 