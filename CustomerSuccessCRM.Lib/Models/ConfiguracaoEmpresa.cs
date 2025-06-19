using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerSuccessCRM.Lib.Models
{
    public class ConfiguracaoEmpresa
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string RazaoSocial { get; set; }

        [Required]
        [StringLength(200)]
        public string NomeFantasia { get; set; }

        [Required]
        [StringLength(18)]
        public string CNPJ { get; set; }

        [StringLength(20)]
        public string InscricaoEstadual { get; set; }

        [Required]
        [StringLength(200)]
        public string Endereco { get; set; }

        [Required]
        [StringLength(100)]
        public string Cidade { get; set; }

        [Required]
        [StringLength(2)]
        public string Estado { get; set; }

        [Required]
        [StringLength(10)]
        public string CEP { get; set; }

        [Required]
        [StringLength(20)]
        public string Telefone { get; set; }

        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string EmailPrincipal { get; set; }

        [StringLength(150)]
        [EmailAddress]
        public string EmailFinanceiro { get; set; }

        [StringLength(150)]
        [EmailAddress]
        public string EmailSuporte { get; set; }

        [StringLength(200)]
        public string Website { get; set; }

        [StringLength(500)]
        public string LogoUrl { get; set; }

        public bool UsarLogoPadrao { get; set; } = true;

        [StringLength(7)]
        public string CorPrimaria { get; set; } = "#007bff";

        [StringLength(7)]
        public string CorSecundaria { get; set; } = "#6c757d";

        public bool EnviarEmailBoasVindas { get; set; } = true;

        public bool NotificarNovasOportunidades { get; set; } = true;

        public bool NotificarNovasInteracoes { get; set; } = true;

        public int DiasParaFollowUp { get; set; } = 7;

        public DateTime DataCadastro { get; set; }

        public DateTime? UltimaAtualizacao { get; set; }

        public bool Ativo { get; set; } = true;
    }
} 