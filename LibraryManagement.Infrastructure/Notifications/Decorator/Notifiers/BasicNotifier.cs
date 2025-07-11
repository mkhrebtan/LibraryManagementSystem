using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.Notifications.Decorator;

namespace LibraryManagement.Infrastructure.Notifications.Decorator.Notifiers;

internal class BasicNotifier : INotifier
{
    public void Notify(Book book, User user)
    {
        try
        {
            using var writer = new StreamWriter(@"C:\\Dev\\LibraryManagementSystem\\LibraryManagement.Infrastructure\\Notifications\\Logs\\notifications.txt", append: true);
            writer.WriteLine($"{DateTime.Now} | Notification for user '{user.Name}': Book '{book.Title}' is now available for loan.");
        }
        catch (Exception ex) when (
            ex is UnauthorizedAccessException ||
            ex is DirectoryNotFoundException ||
            ex is IOException)
        {
            throw new InvalidOperationException("Failed to notify subscribers.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unexpected error while notifying subscribers.", ex);
        }
    }
}