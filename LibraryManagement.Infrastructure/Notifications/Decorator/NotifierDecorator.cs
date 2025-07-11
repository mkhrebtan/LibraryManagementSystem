using LibraryManagement.Domain.Models;

namespace LibraryManagement.Infrastructure.Notifications.Decorator;

internal abstract class NotifierDecorator : INotifier
{
    protected INotifier _wrappee;

    public NotifierDecorator(INotifier wrappee)
    {
        _wrappee = wrappee;
    }

    public virtual void Notify(Book book, User user)
    {
        _wrappee.Notify(book, user);
    }
}
