namespace CustomerSuccessCRM.Lib.Configuration
{
    public class StorageSettings
    {
        public string BasePath { get; set; } = "Storage";
        public long MaxFileSize { get; set; } = 10 * 1024 * 1024; // 10MB
        public string[] AllowedFileTypes { get; set; } = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".jpg", ".jpeg", ".png" };
    }
} 