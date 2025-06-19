using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models;

[Table("ContratoAnexos")]
public class ContratoAnexo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Contrato")]
    [DisplayName("Contrato")]
    public int ContratoId { get; set; }

    [Required(ErrorMessage = "O nome do arquivo é obrigatório")]
    [StringLength(255, ErrorMessage = "O nome do arquivo deve ter no máximo {1} caracteres")]
    [DisplayName("Nome do Arquivo")]
    public string NomeArquivo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O caminho do arquivo é obrigatório")]
    [StringLength(1000, ErrorMessage = "O caminho do arquivo deve ter no máximo {1} caracteres")]
    [DisplayName("Caminho do Arquivo")]
    public string CaminhoArquivo { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    [DisplayName("Data de Upload")]
    public DateTime DataUpload { get; set; } = DateTime.Now;

    public virtual Contrato Contrato { get; set; } = null!;
}