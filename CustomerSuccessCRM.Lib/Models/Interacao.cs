using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models
{
    [Table("Interacoes")]
    public class Interacao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        [DisplayName("Cliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "O assunto é obrigatório")]
        [StringLength(200, ErrorMessage = "O assunto deve ter no máximo {1} caracteres")]
        [DisplayName("Assunto")]
        public string Assunto { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(2000, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Data da Interação")]
        public DateTime DataInteracao { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Tipo")]
        public TipoInteracao Tipo { get; set; }

        [Required]
        [DisplayName("Prioridade")]
        public PrioridadeInteracao Prioridade { get; set; } = PrioridadeInteracao.Normal;

        [Required]
        [DisplayName("Status")]
        public StatusInteracao Status { get; set; } = StatusInteracao.Aberta;

        [StringLength(100, ErrorMessage = "O nome do responsável deve ter no máximo {1} caracteres")]
        [DisplayName("Responsável")]
        public string? Responsavel { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Data de Conclusão")]
        public DateTime? DataConclusao { get; set; }

        [StringLength(500, ErrorMessage = "As observações devem ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observações")]
        public string? Observacoes { get; set; }

        // Propriedade de navegação
        public virtual Cliente Cliente { get; set; } = null!;
    }

    public enum TipoInteracao
    {
        [Description("Telefone")]
        Telefone,
        [Description("E-mail")]
        Email,
        [Description("Reunião")]
        Reuniao,
        [Description("Visita")]
        Visita,
        [Description("Chat")]
        Chat,
        [Description("Outro")]
        Outro
    }

    public enum PrioridadeInteracao
    {
        [Description("Baixa")]
        Baixa,
        [Description("Normal")]
        Normal,
        [Description("Alta")]
        Alta,
        [Description("Urgente")]
        Urgente
    }

    public enum StatusInteracao
    {
        [Description("Aberta")]
        Aberta,
        [Description("Em Andamento")]
        EmAndamento,
        [Description("Pendente")]
        Pendente,
        [Description("Concluída")]
        Concluida,
        [Description("Cancelada")]
        Cancelada
    }
} 