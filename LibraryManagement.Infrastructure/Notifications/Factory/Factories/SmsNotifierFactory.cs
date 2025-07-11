using LibraryManagement.Infrastructure.Notifications.Decorator.Notifiers;
using LibraryManagement.Infrastructure.Notifications.Decorator;

namespace LibraryManagement.Infrastructure.Notifications.Factory.Factories;

internal class SmsNotifierFactory : NotifierFactory
{
    public override INotifier GetNotifier(INotifier notifier)
    {
        return new SmsNotifier(notifier);
    }
}
