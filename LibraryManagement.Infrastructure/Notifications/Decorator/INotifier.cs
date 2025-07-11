using LibraryManagement.Domain.Models;

namespace LibraryManagement.Infrastructure.Notifications.Decorator;

internal interface INotifier
{
    void Notify(Book book, User user);
}
