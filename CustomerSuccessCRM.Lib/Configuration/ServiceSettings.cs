namespace CustomerSuccessCRM.Lib.Configuration
{
    public class StorageSettings
    {
        public string BasePath { get; set; } = "Storage";
        public long MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10MB
        public string[] AllowedFileTypes { get; set; } = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".jpg", ".jpeg", ".png" };
    }

    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 587;
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public bool UseSsl { get; set; } = true;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
    }

    public class NotificationSettings
    {
        public bool EnableEmailNotifications { get; set; } = true;
        public bool EnablePushNotifications { get; set; } = false;
        public string[] NotificationRecipients { get; set; } = Array.Empty<string>();
        public int MaxNotificationsPerUser { get; set; } = 100;
        public int NotificationExpiryDays { get; set; } = 30;
    }

    public class WorkflowSettings
    {
        public bool EnableAutomaticWorkflows { get; set; } = true;
        public int MaxConcurrentWorkflows { get; set; } = 5;
        public int WorkflowTimeoutMinutes { get; set; } = 30;
        public string[] WorkflowAdministrators { get; set; } = Array.Empty<string>();
    }

    public class MetaSettings
    {
        public bool EnableAutomaticNotifications { get; set; } = true;
        public int AlertThresholdPercentage { get; set; } = 80;
        public int WarningThresholdPercentage { get; set; } = 50;
        public string[] MetaAdministrators { get; set; } = Array.Empty<string>();
    }

    public class ProdutoSettings
    {
        public bool EnableAutomaticPricing { get; set; } = false;
        public decimal MaxDiscountPercentage { get; set; } = 30;
        public bool RequireApprovalForDiscounts { get; set; } = true;
        public string[] ProdutoAdministrators { get; set; } = Array.Empty<string>();
    }
} 