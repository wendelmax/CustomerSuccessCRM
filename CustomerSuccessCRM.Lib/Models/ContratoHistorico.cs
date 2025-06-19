using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models;

[Table("ContratoHistoricos")]
public class ContratoHistorico
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Contrato")]
    [DisplayName("Contrato")]
    public int ContratoId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [DisplayName("Data da Modificação")]
    public DateTime DataModificacao { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "A descrição da modificação é obrigatória")]
    [StringLength(1000, ErrorMessage = "A descrição da modificação deve ter no máximo {1} caracteres")]
    [DataType(DataType.MultilineText)]
    [DisplayName("Modificação")]
    public string Modificacao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O responsável pela modificação é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome do responsável deve ter no máximo {1} caracteres")]
    [DisplayName("Responsável")]
    public string ResponsavelModificacao { get; set; } = string.Empty;

    public virtual Contrato Contrato { get; set; } = null!;
}