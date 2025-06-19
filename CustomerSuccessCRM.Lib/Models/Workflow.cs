using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CustomerSuccessCRM.Lib.Models
{
    [Table("Workflows")]
    public class Workflow
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
        [DisplayName("Tipo")]
        public TipoWorkflow Tipo { get; set; }

        [StringLength(100, ErrorMessage = "A expressão CRON deve ter no máximo {1} caracteres")]
        [DisplayName("Expressão CRON")]
        public string CronExpressao { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Responsavel")]
        [DisplayName("Responsável")]
        public string ResponsavelId { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "As observações devem ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Observações")]
        public string Observacoes { get; set; } = string.Empty;

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

        [DataType(DataType.DateTime)]
        [DisplayName("Última Execução")]
        public DateTime? UltimaExecucao { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Próxima Execução")]
        public DateTime? ProximaExecucao { get; set; }

        [StringLength(2000, ErrorMessage = "Os parâmetros devem ter no máximo {1} caracteres")]
        [DisplayName("Parâmetros")]
        public string Parametros { get; set; } = string.Empty;

        public virtual ICollection<WorkflowExecucao> Execucoes { get; set; } = new List<WorkflowExecucao>();
        public virtual ICollection<WorkflowGatilho> Gatilhos { get; set; } = new List<WorkflowGatilho>();
        public virtual ICollection<WorkflowCondicao> Condicoes { get; set; } = new List<WorkflowCondicao>();
        public virtual ICollection<WorkflowAcao> Acoes { get; set; } = new List<WorkflowAcao>();
    }

    [Table("WorkflowGatilhos")]
    public class WorkflowGatilho
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Workflow")]
        public int WorkflowId { get; set; }

        [Required]
        [DisplayName("Tipo")]
        public TipoGatilho Tipo { get; set; }

        [StringLength(2000, ErrorMessage = "Os parâmetros devem ter no máximo {1} caracteres")]
        [DisplayName("Parâmetros")]
        public string Parametros { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [DisplayName("Ativo")]
        public bool Ativo { get; set; } = true;

        public virtual Workflow Workflow { get; set; } = null!;
    }

    [Table("WorkflowCondicoes")]
    public class WorkflowCondicao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Workflow")]
        public int WorkflowId { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo deve ter no máximo {1} caracteres")]
        [DisplayName("Campo")]
        public string Campo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O operador é obrigatório")]
        [StringLength(50, ErrorMessage = "O operador deve ter no máximo {1} caracteres")]
        [DisplayName("Operador")]
        public string Operador { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "O valor deve ter no máximo {1} caracteres")]
        [DisplayName("Valor")]
        public string Valor { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "O valor adicional deve ter no máximo {1} caracteres")]
        [DisplayName("Valor Adicional")]
        public string ValorAdicional { get; set; } = string.Empty;

        [Required]
        [DisplayName("Ativo")]
        public bool Ativo { get; set; } = true;

        public virtual Workflow Workflow { get; set; } = null!;
    }

    [Table("WorkflowAcoes")]
    public class WorkflowAcao
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Workflow")]
        public int WorkflowId { get; set; }

        [Required]
        [DisplayName("Tipo")]
        public TipoAcao Tipo { get; set; }

        [StringLength(2000, ErrorMessage = "Os parâmetros devem ter no máximo {1} caracteres")]
        [DisplayName("Parâmetros")]
        public string Parametros { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [DisplayName("Ativo")]
        public bool Ativo { get; set; } = true;

        public virtual Workflow Workflow { get; set; } = null!;
    }

    public enum TipoWorkflow
    {
        [Description("Agendado")]
        Agendado,
        [Description("Gatilho")]
        Gatilho,
        [Description("Manual")]
        Manual,
        [Description("Automático")]
        Automatico
    }

    public enum TipoGatilho
    {
        [Description("Novo Cliente")]
        NovoCliente,
        [Description("Nova Oportunidade")]
        NovaOportunidade,
        [Description("Nova Interação")]
        NovaInteracao,
        [Description("Alteração de Status")]
        AlteracaoStatus,
        [Description("Data de Aniversário")]
        DataAniversario,
        [Description("Data de Renovação")]
        DataRenovacao,
        [Description("Outro")]
        Outro
    }

    public enum TipoAcao
    {
        [Description("Enviar E-mail")]
        EnviarEmail,
        [Description("Enviar Notificação")]
        EnviarNotificacao,
        [Description("Criar Tarefa")]
        CriarTarefa,
        [Description("Atualizar Status")]
        AtualizarStatus,
        [Description("Executar Integração")]
        ExecutarIntegracao,
        [Description("Outro")]
        Outro
    }
} 