namespace CustomerSuccessCRM.Lib.Configuration
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public bool UseSsl { get; set; }
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public int MaxRetries { get; set; } = 3;
        public int RetryIntervalSeconds { get; set; } = 60;
    }
} 