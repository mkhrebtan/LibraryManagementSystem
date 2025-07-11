using LibraryManagement.Domain.Models;

namespace LibraryManagement.Infrastructure.Notifications.Abstraction;

internal interface INotifier
{
    void Notify(Book book, User user);
}
