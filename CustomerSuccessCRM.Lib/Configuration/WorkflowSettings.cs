namespace CustomerSuccessCRM.Lib.Configuration
{
    public class WorkflowSettings
    {
        public bool EnableAutomaticWorkflows { get; set; } = true;
        public int MaxConcurrentWorkflows { get; set; } = 5;
        public int WorkflowTimeoutMinutes { get; set; } = 30;
        public string[] WorkflowAdministrators { get; set; } = Array.Empty<string>();
    }
} 