using System.ComponentModel.DataAnnotations;

namespace CustomerSuccessCRM.Lib.Models
{
    public class Interacao
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        [Required]
        [StringLength(200)]
        public string Assunto { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Descricao { get; set; } = string.Empty;

        public DateTime DataInteracao { get; set; } = DateTime.Now;

        public TipoInteracao Tipo { get; set; }

        public PrioridadeInteracao Prioridade { get; set; } = PrioridadeInteracao.Normal;

        public StatusInteracao Status { get; set; } = StatusInteracao.Aberta;

        [StringLength(100)]
        public string? Responsavel { get; set; }

        public DateTime? DataConclusao { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }

        // Propriedade de navegação
        public virtual Cliente Cliente { get; set; } = null!;
    }

    public enum TipoInteracao
    {
        Telefone,
        Email,
        Reuniao,
        Visita,
        Chat,
        Outro
    }

    public enum PrioridadeInteracao
    {
        Baixa,
        Normal,
        Alta,
        Urgente
    }

    public enum StatusInteracao
    {
        Aberta,
        EmAndamento,
        Concluida,
        Cancelada
    }
} 