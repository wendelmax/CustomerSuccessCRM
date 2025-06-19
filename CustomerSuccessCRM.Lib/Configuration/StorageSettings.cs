namespace CustomerSuccessCRM.Lib.Configuration
{
    public class StorageSettings
    {
        public string BasePath { get; set; } = string.Empty;
        public long MaxFileSize { get; set; }
        public string[] AllowedFileTypes { get; set; } = Array.Empty<string>();
        public bool UseCompression { get; set; }
        public bool UseEncryption { get; set; }
        public string EncryptionKey { get; set; } = string.Empty;
        public int RetentionDays { get; set; }
        public bool EnableVersioning { get; set; }
        public int MaxVersions { get; set; }
    }
} 