namespace CustomerSuccessCRM.Lib.Configuration
{
    public class WorkflowSettings
    {
        public bool EnableAutomaticWorkflows { get; set; }
        public int MaxConcurrentWorkflows { get; set; }
        public string[] WorkflowAdministrators { get; set; }
        public int RetryAttempts { get; set; }
        public int RetryDelayInMinutes { get; set; }
        public bool NotifyOnFailure { get; set; }
        public bool NotifyOnSuccess { get; set; }
        public string LogLevel { get; set; }
        public string WorkflowStoragePath { get; set; }
        public int DiasInatividade { get; set; }
        public bool NotificacaoErros { get; set; }
        public string[] AcoesDisponiveis { get; set; }
        public string[] CondicoesDisponiveis { get; set; }
    }
} 