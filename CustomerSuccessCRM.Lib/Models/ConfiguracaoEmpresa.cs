using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models
{
    [Table("ConfiguracoesEmpresa")]
    public class ConfiguracaoEmpresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "A razão social é obrigatória")]
        [StringLength(200, ErrorMessage = "A razão social deve ter no máximo {1} caracteres")]
        [DisplayName("Razão Social")]
        public string RazaoSocial { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome fantasia é obrigatório")]
        [StringLength(200, ErrorMessage = "O nome fantasia deve ter no máximo {1} caracteres")]
        [DisplayName("Nome Fantasia")]
        public string NomeFantasia { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [StringLength(18, ErrorMessage = "O CNPJ deve ter no máximo {1} caracteres")]
        [RegularExpression(@"^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$", ErrorMessage = "CNPJ inválido")]
        [DisplayName("CNPJ")]
        public string CNPJ { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "A inscrição estadual deve ter no máximo {1} caracteres")]
        [DisplayName("Inscrição Estadual")]
        public string InscricaoEstadual { get; set; } = string.Empty;

        [Required(ErrorMessage = "O endereço é obrigatório")]
        [StringLength(200, ErrorMessage = "O endereço deve ter no máximo {1} caracteres")]
        [DisplayName("Endereço")]
        public string Endereco { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade é obrigatória")]
        [StringLength(100, ErrorMessage = "A cidade deve ter no máximo {1} caracteres")]
        [DisplayName("Cidade")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado é obrigatório")]
        [StringLength(2, ErrorMessage = "O estado deve ter {1} caracteres")]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Estado inválido (use UF em maiúsculas)")]
        [DisplayName("Estado")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [StringLength(10, ErrorMessage = "O CEP deve ter no máximo {1} caracteres")]
        [RegularExpression(@"^\d{5}\-\d{3}$", ErrorMessage = "CEP inválido")]
        [DisplayName("CEP")]
        public string CEP { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [StringLength(20, ErrorMessage = "O telefone deve ter no máximo {1} caracteres")]
        [Phone(ErrorMessage = "Telefone inválido")]
        [DisplayName("Telefone")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail principal é obrigatório")]
        [StringLength(150, ErrorMessage = "O e-mail deve ter no máximo {1} caracteres")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [DisplayName("E-mail Principal")]
        public string EmailPrincipal { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "O e-mail deve ter no máximo {1} caracteres")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [DisplayName("E-mail Financeiro")]
        public string EmailFinanceiro { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "O e-mail deve ter no máximo {1} caracteres")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [DisplayName("E-mail Suporte")]
        public string EmailSuporte { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "O website deve ter no máximo {1} caracteres")]
        [Url(ErrorMessage = "URL inválida")]
        [DisplayName("Website")]
        public string Website { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A URL da logo deve ter no máximo {1} caracteres")]
        [Url(ErrorMessage = "URL inválida")]
        [DisplayName("URL da Logo")]
        public string LogoUrl { get; set; } = string.Empty;

        [Required]
        [DisplayName("Usar Logo Padrão")]
        public bool UsarLogoPadrao { get; set; } = true;

        [Required]
        [StringLength(7, ErrorMessage = "A cor deve ter {1} caracteres")]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "Cor inválida (use formato #RRGGBB)")]
        [DisplayName("Cor Primária")]
        public string CorPrimaria { get; set; } = "#007bff";

        [Required]
        [StringLength(7, ErrorMessage = "A cor deve ter {1} caracteres")]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "Cor inválida (use formato #RRGGBB)")]
        [DisplayName("Cor Secundária")]
        public string CorSecundaria { get; set; } = "#6c757d";

        [Required]
        [DisplayName("Enviar E-mail de Boas-vindas")]
        public bool EnviarEmailBoasVindas { get; set; } = true;

        [Required]
        [DisplayName("Notificar Novas Oportunidades")]
        public bool NotificarNovasOportunidades { get; set; } = true;

        [Required]
        [DisplayName("Notificar Novas Interações")]
        public bool NotificarNovasInteracoes { get; set; } = true;

        [Required]
        [Range(1, 90, ErrorMessage = "O número de dias deve estar entre {1} e {2}")]
        [DisplayName("Dias para Follow-up")]
        public int DiasParaFollowUp { get; set; } = 7;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data de Cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayName("Última Atualização")]
        public DateTime? UltimaAtualizacao { get; set; }

        [Required]
        [DisplayName("Ativo")]
        public bool Ativo { get; set; } = true;
    }
} 