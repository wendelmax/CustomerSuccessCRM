using CustomerSuccessCRM.Lib.Services.Contracts;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace CustomerSuccessCRM.Lib.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;
        private readonly NotificationSettings _settings;
        private static readonly ConcurrentDictionary<string, List<Notification>> _notifications = new();

        public NotificationService(
            IEmailService emailService,
            IOptions<NotificationSettings> settings)
        {
            _emailService = emailService;
            _settings = settings.Value;
        }

        public async Task SendNotificationAsync(string userId, string title, string message, NotificationType type = NotificationType.Info)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type
            };

            // Armazenar notificação
            _notifications.AddOrUpdate(
                userId,
                new List<Notification> { notification },
                (key, list) =>
                {
                    list.Add(notification);
                    return list;
                });

            // Enviar email se configurado
            if (_settings.EnableEmailNotifications)
            {
                await SendEmailNotificationAsync(userId, title, message, type);
            }

            // Enviar push notification se configurado
            if (_settings.EnablePushNotifications)
            {
                await SendPushNotificationAsync(notification);
            }
        }

        public async Task SendNotificationAsync(IEnumerable<string> userIds, string title, string message, NotificationType type = NotificationType.Info)
        {
            foreach (var userId in userIds)
            {
                await SendNotificationAsync(userId, title, message, type);
            }
        }

        public async Task SendSystemNotificationAsync(string title, string message, NotificationType type = NotificationType.Info)
        {
            if (_settings.NotificationRecipients != null)
            {
                foreach (var recipient in _settings.NotificationRecipients)
                {
                    await SendNotificationAsync(recipient, title, message, type);
                }
            }
        }

        public async Task SendEmailNotificationAsync(string userId, string title, string message, NotificationType type = NotificationType.Info)
        {
            // TODO: Obter email do usuário a partir do userId
            var userEmail = "user@example.com"; // Substituir pela lógica real

            var emailModel = new
            {
                Title = title,
                Message = message,
                Type = type.ToString(),
                Subject = $"Notificação: {title}"
            };

            await _emailService.SendEmailTemplateAsync(userEmail, "Notification", emailModel);
        }

        public async Task MarkNotificationAsReadAsync(string notificationId)
        {
            foreach (var userNotifications in _notifications)
            {
                var notification = userNotifications.Value.FirstOrDefault(n => n.Id == notificationId);
                if (notification != null)
                {
                    notification.ReadAt = DateTime.Now;
                    break;
                }
            }

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string userId)
        {
            if (_notifications.TryGetValue(userId, out var notifications))
            {
                return await Task.FromResult(notifications.Where(n => !n.IsRead).OrderByDescending(n => n.CreatedAt));
            }

            return await Task.FromResult(Enumerable.Empty<Notification>());
        }

        public async Task<IEnumerable<Notification>> GetNotificationHistoryAsync(string userId, int limit = 50)
        {
            if (_notifications.TryGetValue(userId, out var notifications))
            {
                return await Task.FromResult(notifications.OrderByDescending(n => n.CreatedAt).Take(limit));
            }

            return await Task.FromResult(Enumerable.Empty<Notification>());
        }

        public async Task DeleteNotificationAsync(string notificationId)
        {
            foreach (var userNotifications in _notifications)
            {
                var notification = userNotifications.Value.FirstOrDefault(n => n.Id == notificationId);
                if (notification != null)
                {
                    userNotifications.Value.Remove(notification);
                    break;
                }
            }

            await Task.CompletedTask;
        }

        public async Task ClearNotificationsAsync(string userId)
        {
            _notifications.TryRemove(userId, out _);
            await Task.CompletedTask;
        }

        private async Task SendPushNotificationAsync(Notification notification)
        {
            // TODO: Implementar integração com serviço de push notifications
            // Por exemplo: Firebase Cloud Messaging, SignalR, etc.
            await Task.CompletedTask;
        }
    }
} 