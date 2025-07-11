using LibraryManagement.Infrastructure.Notifications.Decorator.Notifiers;
using LibraryManagement.Infrastructure.Notifications.Decorator;

namespace LibraryManagement.Infrastructure.Notifications.Factory.Factories;

internal class EmailNotifierFactory : NotifierFactory
{
    public override INotifier GetNotifier(INotifier notifier)
    {
        return new EmailNotifier(notifier);
    }
}
