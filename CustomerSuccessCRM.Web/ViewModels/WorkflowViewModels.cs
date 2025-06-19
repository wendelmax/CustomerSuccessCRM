namespace CustomerSuccessCRM.Lib.Models.ViewModels
{
    public class WorkflowEstatisticas
    {
        public int TotalExecucoes { get; set; }
        public int ExecucoesSucesso { get; set; }
        public int ExecucoesErro { get; set; }
        public double TempoMedioExecucao { get; set; }
        public int ExecucoesEmAndamento { get; set; }
        public int ExecucoesAgendadas { get; set; }
        public int ExecucoesCanceladas { get; set; }
        public DateTime UltimaExecucao { get; set; }
        public double TaxaSucesso => TotalExecucoes > 0 ? (double)ExecucoesSucesso / TotalExecucoes * 100 : 0;
    }

    public class WorkflowResumo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime UltimaExecucao { get; set; }
        public int TotalExecucoes { get; set; }
        public int ExecucoesSucesso { get; set; }
        public double TaxaSucesso { get; set; }
        public TimeSpan TempoMedioExecucao { get; set; }
    }
} 