using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Web.ViewModels
{
    public class MetaResumo
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public decimal ValorAtingido { get; set; }
        public decimal Progresso { get; set; }
        public string ResponsavelNome { get; set; }
        public DateTime DataFim { get; set; }
        public StatusMeta Status { get; set; }
    }

    public class MetaEstatisticas
    {
        public int TotalMetas { get; set; }
        public int MetasEmAndamento { get; set; }
        public int MetasConcluidas { get; set; }
        public int MetasAtrasadas { get; set; }
        public decimal PercentualAtingimentoGeral { get; set; }
        public decimal MediaProgresso { get; set; }
        public List<MetaPorStatus> MetasPorStatus { get; set; } = new();
    }

    public class MetaPorStatus
    {
        public StatusMeta Status { get; set; }
        public int Quantidade { get; set; }
        public decimal Percentual { get; set; }
    }
} 