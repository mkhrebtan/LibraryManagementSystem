using LibraryManagement.Domain.Models;
using LibraryManagement.Infrastructure.Notifications.Abstraction;

namespace LibraryManagement.Infrastructure.Notifications.Notifiers;

internal class EmailNotifier : NotifierDecorator
{
    private readonly string _email;

    public EmailNotifier(INotifier wrappee, string email) : base(wrappee)
    {
        _email = email;
    }

    public override void Notify(Book book, User user)
    {
        base.Notify(book, user);
        
        try
        {
            using var writer = new StreamWriter(@"C:\\Dev\\LibraryManagementSystem\\LibraryManagement.Infrastructure\\Notifications\\Logs\\email_notifications.txt", append: true);
            writer.WriteLine($"{DateTime.Now} | Notification for user '{user.Name}' | Email '{_email}': Book '{book.Title}' is now available for loan.");
        }
        catch (Exception ex) when (
            ex is UnauthorizedAccessException ||
            ex is DirectoryNotFoundException ||
            ex is IOException)
        {
            throw new InvalidOperationException("Failed to send email notification.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unexpected error while sending email notification.", ex);
        }
    }
}
