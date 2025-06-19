namespace CustomerSuccessCRM.Lib.Configuration
{
    public class MetaSettings
    {
        public bool EnableAutomaticNotifications { get; set; } = true;
        public int AlertThresholdPercentage { get; set; } = 80;
        public int WarningThresholdPercentage { get; set; } = 50;
        public string[] MetaAdministrators { get; set; } = Array.Empty<string>();
    }
} 