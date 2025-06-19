namespace CustomerSuccessCRM.Lib.Services.Contracts
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendEmailAsync(string to, string subject, string body, IEnumerable<EmailAttachment> attachments, bool isHtml = false);
        Task SendEmailTemplateAsync(string to, string templateName, object model);
        Task SendEmailTemplateAsync(string to, string templateName, object model, IEnumerable<EmailAttachment> attachments);
        Task SendBulkEmailAsync(IEnumerable<string> to, string subject, string body, bool isHtml = false);
        Task SendBulkEmailTemplateAsync(IEnumerable<string> to, string templateName, object model);
    }

    public class EmailAttachment
    {
        public string FileName { get; set; } = string.Empty;
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "application/octet-stream";
    }
} 