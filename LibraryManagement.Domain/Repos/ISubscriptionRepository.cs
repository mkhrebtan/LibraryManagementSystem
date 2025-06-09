using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Models;

namespace LibraryManagement.Domain.Repos
{
    public interface ISubscriptionRepository : IRepository<BookSubscription>
    {
        BookSubscription? GetActiveByUserAndBook(int userId, int bookId);
        IEnumerable<BookSubscription> GetActiveForUser(int userId);
        IEnumerable<BookSubscription> GetActiveForBook(int bookId);
    }
}
