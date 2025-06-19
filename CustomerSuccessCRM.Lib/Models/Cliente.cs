using System.ComponentModel.DataAnnotations;

namespace CustomerSuccessCRM.Lib.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Sobrenome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Telefone { get; set; }

        [StringLength(200)]
        public string? Empresa { get; set; }

        [StringLength(100)]
        public string? Cargo { get; set; }

        [StringLength(500)]
        public string? Endereco { get; set; }

        [StringLength(100)]
        public string? Cidade { get; set; }

        [StringLength(50)]
        public string? Estado { get; set; }

        [StringLength(20)]
        public string? CEP { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public DateTime? UltimaAtualizacao { get; set; }

        [StringLength(1000)]
        public string? Observacoes { get; set; }

        public StatusCliente Status { get; set; } = StatusCliente.Ativo;

        // Propriedades de navegação
        public virtual ICollection<Interacao> Interacoes { get; set; } = new List<Interacao>();
        public virtual ICollection<Oportunidade> Oportunidades { get; set; } = new List<Oportunidade>();

        // Propriedade calculada
        public string NomeCompleto => $"{Nome} {Sobrenome}".Trim();
    }

    public enum StatusCliente
    {
        Ativo,
        Inativo,
        Prospecto,
        ClienteVip
    }
} 