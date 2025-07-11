using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.BookAvailability;

/// <summary>
/// Defines a contract for observing and receiving notifications when a book becomes available.
/// </summary>
public interface IBookAvailabilityObserver
{
    void Update(Book book, User user, ICollection<NotificationPreference> notificationPreferences);
}
