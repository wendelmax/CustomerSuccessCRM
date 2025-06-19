using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
        [DisplayName("Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O sobrenome é obrigatório")]
        [StringLength(100, ErrorMessage = "O sobrenome deve ter no máximo {1} caracteres")]
        [DisplayName("Sobrenome")]
        public string Sobrenome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [StringLength(150, ErrorMessage = "O e-mail deve ter no máximo {1} caracteres")]
        [DisplayName("E-mail")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Telefone inválido")]
        [StringLength(20, ErrorMessage = "O telefone deve ter no máximo {1} caracteres")]
        [DisplayName("Telefone")]
        public string? Telefone { get; set; }

        [StringLength(200, ErrorMessage = "O nome da empresa deve ter no máximo {1} caracteres")]
        [DisplayName("Empresa")]
        public string? Empresa { get; set; }

        [StringLength(100, ErrorMessage = "O cargo deve ter no máximo {1} caracteres")]
        [DisplayName("Cargo")]
        public string? Cargo { get; set; }

        [StringLength(500, ErrorMessage = "O endereço deve ter no máximo {1} caracteres")]
        [DisplayName("Endereço")]
        public string? Endereco { get; set; }

        [StringLength(100, ErrorMessage = "A cidade deve ter no máximo {1} caracteres")]
        [DisplayName("Cidade")]
        public string? Cidade { get; set; }

        [StringLength(50, ErrorMessage = "O estado deve ter no máximo {1} caracteres")]
        [DisplayName("Estado")]
        public string? Estado { get; set; }

        [StringLength(20, ErrorMessage = "O CEP deve ter no máximo {1} caracteres")]
        [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "CEP inválido")]
        [DisplayName("CEP")]
        public string? CEP { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data de Cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayName("Última Atualização")]
        public DateTime? UltimaAtualizacao { get; set; }

        [StringLength(1000, ErrorMessage = "As observações devem ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observações")]
        public string? Observacoes { get; set; }

        [Required]
        [DisplayName("Status")]
        public StatusCliente Status { get; set; } = StatusCliente.Ativo;

        // Propriedades de navegação
        public virtual ICollection<Interacao> Interacoes { get; set; } = new List<Interacao>();
        public virtual ICollection<Oportunidade> Oportunidades { get; set; } = new List<Oportunidade>();
        public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();

        [NotMapped]
        [DisplayName("Nome Completo")]
        public string NomeCompleto => $"{Nome} {Sobrenome}".Trim();
    }

    public enum StatusCliente
    {
        [Description("Ativo")]
        Ativo,
        [Description("Inativo")]
        Inativo,
        [Description("Prospecto")]
        Prospecto,
        [Description("Cliente VIP")]
        ClienteVip
    }
} 