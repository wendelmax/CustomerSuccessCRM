namespace CustomerSuccessCRM.Lib.Services.Contracts
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string title, string message, NotificationType type = NotificationType.Info);
        Task SendNotificationAsync(IEnumerable<string> userIds, string title, string message, NotificationType type = NotificationType.Info);
        Task SendSystemNotificationAsync(string title, string message, NotificationType type = NotificationType.Info);
        Task SendEmailNotificationAsync(string userId, string title, string message, NotificationType type = NotificationType.Info);
        Task MarkNotificationAsReadAsync(string notificationId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string userId);
        Task<IEnumerable<Notification>> GetNotificationHistoryAsync(string userId, int limit = 50);
        Task DeleteNotificationAsync(string notificationId);
        Task ClearNotificationsAsync(string userId);
    }

    public enum NotificationType
    {
        Info,
        Success,
        Warning,
        Error,
        System
    }

    public class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ReadAt { get; set; }
        public bool IsRead => ReadAt.HasValue;
    }
} 