using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.Notifications.Decorator;
using LibraryManagement.Infrastructure.Notifications.Decorator.Notifiers;
using LibraryManagement.Infrastructure.Notifications.Factory;
using LibraryManagement.Infrastructure.Notifications.Factory.Factories;

namespace LibraryManagement.Infrastructure.Notifications;

public class NotificationService : INotificationService
{
    public NotificationService()
    {
        NotifierFactoryRegistry.RegisterFactory(NotificationPreference.SMS, new SmsNotifierFactory());
        NotifierFactoryRegistry.RegisterFactory(NotificationPreference.Email, new EmailNotifierFactory());
    }

    public void Update(Book book, User user, ICollection<NotificationPreference> notificationPreferences)
    {
        INotifier notifier = new BasicNotifier();
        foreach (var preference in notificationPreferences)
        {
            NotifierFactory factory = NotifierFactoryRegistry.GetFactory(preference);
            notifier = factory.GetNotifier(notifier);
        }
        notifier.Notify(book, user);
    }
}
