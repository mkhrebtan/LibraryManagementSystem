using LibraryManagement.Domain.Models;

namespace LibraryManagement.Domain.BookAvailability;

public class BookReturnedEventArgs
{
    required public Book Book { get; init; }
}
