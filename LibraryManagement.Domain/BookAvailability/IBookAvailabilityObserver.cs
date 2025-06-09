using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Domain.BookAvailability;

/// <summary>
/// Defines a contract for observing and receiving notifications when a book becomes available.
/// </summary>
public interface IBookAvailabilityObserver
{
    void NotifyBookAvailable(int bookId, string bookTitle);
}
