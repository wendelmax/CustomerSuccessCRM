using CustomerSuccessCRM.Lib.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Scriban;

namespace CustomerSuccessCRM.Lib.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            await SendEmailAsync(to, subject, body, Enumerable.Empty<EmailAttachment>(), isHtml);
        }

        public async Task SendEmailAsync(string to, string subject, string body, IEnumerable<EmailAttachment> attachments, bool isHtml = false)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            var builder = new BodyBuilder();
            if (isHtml)
                builder.HtmlBody = body;
            else
                builder.TextBody = body;

            foreach (var attachment in attachments)
            {
                builder.Attachments.Add(attachment.FileName, attachment.Content, ContentType.Parse(attachment.ContentType));
            }

            message.Body = builder.ToMessageBody();

            await SendMessageAsync(message);
        }

        public async Task SendEmailTemplateAsync(string to, string templateName, object model)
        {
            await SendEmailTemplateAsync(to, templateName, model, Enumerable.Empty<EmailAttachment>());
        }

        public async Task SendEmailTemplateAsync(string to, string templateName, object model, IEnumerable<EmailAttachment> attachments)
        {
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates", $"{templateName}.html");
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Template de email não encontrado: {templateName}");
            }

            var templateContent = await File.ReadAllTextAsync(templatePath);
            var template = Template.Parse(templateContent);
            var body = template.Render(model);

            await SendEmailAsync(to, GetSubjectFromModel(model), body, attachments, true);
        }

        public async Task SendBulkEmailAsync(IEnumerable<string> to, string subject, string body, bool isHtml = false)
        {
            foreach (var recipient in to)
            {
                await SendEmailAsync(recipient, subject, body, isHtml);
            }
        }

        public async Task SendBulkEmailTemplateAsync(IEnumerable<string> to, string templateName, object model)
        {
            foreach (var recipient in to)
            {
                await SendEmailTemplateAsync(recipient, templateName, model);
            }
        }

        private async Task SendMessageAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, _settings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls);

                if (!string.IsNullOrEmpty(_settings.SmtpUsername))
                {
                    await client.AuthenticateAsync(_settings.SmtpUsername, _settings.SmtpPassword);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        private string GetSubjectFromModel(object model)
        {
            // Tenta obter o assunto do modelo se ele tiver uma propriedade Subject
            var subjectProperty = model.GetType().GetProperty("Subject");
            return subjectProperty?.GetValue(model)?.ToString() ?? "Notificação do Sistema";
        }
    }
} 