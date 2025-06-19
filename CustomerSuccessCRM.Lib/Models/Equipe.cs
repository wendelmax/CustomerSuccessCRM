using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models
{
    [Table("Equipes")]
    public class Equipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(200, ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
        [DisplayName("Nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Responsavel")]
        [DisplayName("Responsável")]
        public string ResponsavelId { get; set; } = string.Empty;

        [Required]
        [DisplayName("Ativa")]
        public bool Ativa { get; set; } = true;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data de Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayName("Data de Atualização")]
        public DateTime? DataAtualizacao { get; set; }

        [StringLength(1000, ErrorMessage = "As observações devem ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observações")]
        public string Observacoes { get; set; } = string.Empty;

        [Required]
        [DisplayName("Tipo")]
        public TipoEquipe Tipo { get; set; }

        public virtual Usuario Responsavel { get; set; } = null!;
        public virtual ICollection<Usuario> Membros { get; set; } = new List<Usuario>();
        public virtual ICollection<Meta> Metas { get; set; } = new List<Meta>();
    }

    public enum TipoEquipe
    {
        [Description("Vendas")]
        Vendas,
        [Description("Suporte")]
        Suporte,
        [Description("Marketing")]
        Marketing,
        [Description("Desenvolvimento")]
        Desenvolvimento,
        [Description("Financeiro")]
        Financeiro,
        [Description("Administrativo")]
        Administrativo,
        [Description("Outro")]
        Outro
    }
} 