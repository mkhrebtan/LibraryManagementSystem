using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Models;

namespace LibraryManagement.Domain.BookAvailability;

/// <summary>
/// Defines a contract for managing subscriptions and notifying users about the availability of books.
/// </summary>
public interface IBookAvailabilityPublisher
{
    void Subscribe(int bookId, int userId, ICollection<NotificationPreference> notificationPreferences);
    void Unsubscribe(int bookId, int userId);
    void NotifySubscribers(Book book);
}
