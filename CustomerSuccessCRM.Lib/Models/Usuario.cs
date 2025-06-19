using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(200, ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
        [DisplayName("Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [StringLength(150, ErrorMessage = "O e-mail deve ter no máximo {1} caracteres")]
        [DisplayName("E-mail")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Telefone inválido")]
        [StringLength(20, ErrorMessage = "O telefone deve ter no máximo {1} caracteres")]
        [DisplayName("Telefone")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O cargo é obrigatório")]
        [StringLength(100, ErrorMessage = "O cargo deve ter no máximo {1} caracteres")]
        [DisplayName("Cargo")]
        public string Cargo { get; set; } = string.Empty;

        [ForeignKey("Equipe")]
        [DisplayName("Equipe")]
        public int? EquipeId { get; set; }

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

        [StringLength(1000, ErrorMessage = "As observações devem ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observações")]
        public string Observacoes { get; set; } = string.Empty;

        [Required]
        [DisplayName("Tipo")]
        public TipoUsuario Tipo { get; set; }

        [Required]
        [StringLength(2000, ErrorMessage = "As permissões devem ter no máximo {1} caracteres")]
        [DisplayName("Permissões")]
        public string Permissoes { get; set; } = string.Empty;

        public virtual Equipe? Equipe { get; set; }
        public virtual ICollection<Meta> MetasResponsavel { get; set; } = new List<Meta>();
        public virtual ICollection<MetaHistorico> MetasHistorico { get; set; } = new List<MetaHistorico>();
    }

    public enum TipoUsuario
    {
        [Description("Administrador")]
        Administrador,
        [Description("Gerente")]
        Gerente,
        [Description("Vendedor")]
        Vendedor,
        [Description("Suporte")]
        Suporte,
        [Description("Marketing")]
        Marketing,
        [Description("Financeiro")]
        Financeiro,
        [Description("Outro")]
        Outro
    }
} 