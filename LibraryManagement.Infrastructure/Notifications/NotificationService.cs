using LibraryManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Infrastructure.Notifications
{
    public class NotificationService : IUserNotifier
    {
        public void Update(Book book, User user)
        {
            try
            {
                using (var writer = new StreamWriter(@"C:\Dev\LibraryManagementSystem\LibraryManagement.Infrastructure\Notifications\Logs\notifications.txt", append: true))
                {
                    writer.WriteLine($"{DateTime.Now} | Notification for User '{user.Name}' ({user.Email}): Book '{book.Title}' is now available for loan.");
                }
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
}
