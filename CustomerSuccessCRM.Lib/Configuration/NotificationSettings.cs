namespace CustomerSuccessCRM.Lib.Configuration
{
    public class NotificationSettings
    {
        public bool EnableNotifications { get; set; }
        public string[] NotificationRecipients { get; set; }
        public string NotificationTemplate { get; set; }
        public string NotificationSender { get; set; }
        public bool SendEmailNotifications { get; set; }
        public bool SendPushNotifications { get; set; }
        public bool SendSmsNotifications { get; set; }
        public int NotificationRetentionDays { get; set; }
        public string[] NotificationAdministrators { get; set; }
        public string NotificationStoragePath { get; set; }
    }
} 