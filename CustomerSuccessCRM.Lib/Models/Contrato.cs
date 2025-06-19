using System.ComponentModel.DataAnnotations;

namespace CustomerSuccessCRM.Lib.Models
{
    public class Contrato
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Conteudo { get; set; } = string.Empty;

        public decimal ValorTotal { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public DateTime? DataAssinatura { get; set; }

        public DateTime? DataVencimento { get; set; }

        public StatusContrato Status { get; set; } = StatusContrato.Rascunho;

        [StringLength(100)]
        public string? ResponsavelCriacao { get; set; }

        [StringLength(100)]
        public string? ResponsavelAprovacao { get; set; }

        public string? HashAssinatura { get; set; }

        public int? PropostaId { get; set; }

        // Propriedades de navegação
        public virtual Cliente Cliente { get; set; } = null!;
        public virtual Proposta? Proposta { get; set; }
        public virtual ICollection<ContratoHistorico> Historico { get; set; } = new List<ContratoHistorico>();
        public virtual ICollection<ContratoAnexo> Anexos { get; set; } = new List<ContratoAnexo>();
    }

    public class ContratoHistorico
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public DateTime DataModificacao { get; set; }
        public string Modificacao { get; set; } = string.Empty;
        public string ResponsavelModificacao { get; set; } = string.Empty;
        public virtual Contrato Contrato { get; set; } = null!;
    }

    public class ContratoAnexo
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public string NomeArquivo { get; set; } = string.Empty;
        public string CaminhoArquivo { get; set; } = string.Empty;
        public DateTime DataUpload { get; set; }
        public virtual Contrato Contrato { get; set; } = null!;
    }

    public enum StatusContrato
    {
        Rascunho,
        EmAnalise,
        Aprovado,
        Assinado,
        Cancelado,
        Vencido
    }
} 