using LibraryManagement.Infrastructure.Notifications.Decorator;
using LibraryManagement.Infrastructure.Notifications.Decorator.Notifiers;

namespace LibraryManagement.Infrastructure.Notifications.Factory.Factories;

internal abstract class NotifierFactory
{
    public abstract INotifier GetNotifier(INotifier notifier);
}
