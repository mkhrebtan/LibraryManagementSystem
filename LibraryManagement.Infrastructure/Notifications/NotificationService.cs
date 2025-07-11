using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.Notifications.Abstraction;
using LibraryManagement.Infrastructure.Notifications.Notifiers;

namespace LibraryManagement.Infrastructure.Notifications;

public class NotificationService : INotificationService
{
    public void Update(Book book, User user, ICollection<NotificationPreference> notificationPreferences)
    {
        INotifier notifier = new BasicNotifier();
        foreach (var preference in notificationPreferences)
        {
            notifier = preference switch
            {
                NotificationPreference.SMS => new SmsNotifier(notifier, user.PhoneNumber),
                NotificationPreference.Email => new EmailNotifier(notifier, user.Email),
                _ => notifier
            };
            
        }
        notifier.Notify(book, user);
    }
}
