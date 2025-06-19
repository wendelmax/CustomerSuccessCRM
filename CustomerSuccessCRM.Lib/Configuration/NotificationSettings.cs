namespace CustomerSuccessCRM.Lib.Configuration
{
    public class NotificationSettings
    {
        public bool EnableEmailNotifications { get; set; } = true;
        public bool EnablePushNotifications { get; set; } = false;
        public string[] NotificationRecipients { get; set; } = Array.Empty<string>();
        public int MaxNotificationsPerUser { get; set; } = 100;
        public int NotificationExpiryDays { get; set; } = 30;
    }
} 