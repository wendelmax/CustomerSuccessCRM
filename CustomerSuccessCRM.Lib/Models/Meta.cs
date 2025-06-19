using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models
{
    [Table("Metas")]
    public class Meta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(200, ErrorMessage = "O título deve ter no máximo {1} caracteres")]
        [DisplayName("Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
        [DisplayName("Nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [DisplayName("Tipo")]
        public TipoMeta Tipo { get; set; }

        [Required]
        [Range(0, 9999999.99, ErrorMessage = "O valor deve estar entre {1} e {2}")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Valor")]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }

        [Range(0, 9999999.99, ErrorMessage = "O valor atingido deve estar entre {1} e {2}")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Valor Atingido")]
        [DataType(DataType.Currency)]
        public decimal ValorAtingido { get; set; }

        [Range(0, 100, ErrorMessage = "O progresso deve estar entre {1}% e {2}%")]
        [Column(TypeName = "decimal(5,2)")]
        [DisplayName("Progresso (%)")]
        public decimal Progresso { get; set; }

        [Required]
        [ForeignKey("Responsavel")]
        [DisplayName("Responsável")]
        public string ResponsavelId { get; set; } = string.Empty;

        [Required]
        [StringLength(200, ErrorMessage = "O nome do responsável deve ter no máximo {1} caracteres")]
        [DisplayName("Nome do Responsável")]
        public string ResponsavelNome { get; set; } = string.Empty;

        [ForeignKey("Equipe")]
        [DisplayName("Equipe")]
        public int? EquipeId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data de Início")]
        public DateTime DataInicio { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data de Fim")]
        public DateTime DataFim { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Data de Conclusão")]
        public DateTime? DataConclusao { get; set; }

        [Required]
        [DisplayName("Ativo")]
        public bool Ativo { get; set; } = true;

        [Required]
        [DisplayName("Recorrente")]
        public bool Recorrente { get; set; }

        [StringLength(1000, ErrorMessage = "As observações devem ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observações")]
        public string Observacoes { get; set; } = string.Empty;

        [Required]
        [DisplayName("Status")]
        public StatusMeta Status { get; set; }

        public virtual Usuario Responsavel { get; set; } = null!;
        public virtual Equipe? Equipe { get; set; }
        public virtual ICollection<MetaHistorico> Historicos { get; set; } = new List<MetaHistorico>();
    }

    public enum TipoMeta
    {
        [Description("Vendas")]
        Vendas,
        [Description("Faturamento")]
        Faturamento,
        [Description("Clientes")]
        Clientes,
        [Description("NPS")]
        NPS,
        [Description("Suporte")]
        Suporte,
        [Description("Marketing")]
        Marketing,
        [Description("Outro")]
        Outro
    }

    public enum StatusMeta
    {
        [Description("Não Iniciada")]
        NaoIniciada,
        [Description("Em Andamento")]
        EmAndamento,
        [Description("Concluída")]
        Concluida,
        [Description("Atrasada")]
        Atrasada,
        [Description("Cancelada")]
        Cancelada
    }

    [Table("MetaHistoricos")]
    public class MetaHistorico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Meta")]
        [DisplayName("Meta")]
        public int MetaId { get; set; }

        [Required(ErrorMessage = "A observação é obrigatória")]
        [StringLength(1000, ErrorMessage = "A observação deve ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observação")]
        public string Observacao { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Valor Anterior")]
        [DataType(DataType.Currency)]
        public decimal ValorAntigo { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayName("Valor Novo")]
        [DataType(DataType.Currency)]
        public decimal ValorNovo { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        [DisplayName("Usuário")]
        public string UsuarioId { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data da Alteração")]
        public DateTime DataAlteracao { get; set; } = DateTime.Now;

        public virtual Meta Meta { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
    }
} 