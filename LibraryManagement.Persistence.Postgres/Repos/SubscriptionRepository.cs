using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;

namespace LibraryManagement.Persistence.Postgres.Repos
{
    public class SubscriptionRepository : GenericRepository<BookSubscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(LibraryDbContext context) : base(context) { }

        public IEnumerable<BookSubscription> GetActiveForBook(int bookId)
        {
            return _context.BookSubscriptions
                .Where(bs => bs.BookId == bookId && !bs.IsNotified)
                .Include(bs => bs.User)
                .ToList();
        }

        public IEnumerable<BookSubscription> GetActiveForUser(int userId)
        {
            return _context.BookSubscriptions
                .Where(bs => bs.UserId == userId && !bs.IsNotified)
                .Include(bs => bs.Book)
                .AsNoTracking()
                .ToList();
        }

        public BookSubscription? GetActiveByUserAndBook(int userId, int bookId)
        {
            return _context.BookSubscriptions
                .FirstOrDefault(bs => bs.UserId == userId && bs.BookId == bookId && !bs.IsNotified);
        }
    }

}
