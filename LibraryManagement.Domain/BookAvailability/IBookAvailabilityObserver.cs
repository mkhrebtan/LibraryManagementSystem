using LibraryManagement.Domain.Models;
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
    void Update(Book book, User user);
}
