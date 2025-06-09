using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;

namespace LibraryManagement.Persistence.Postgres.Repos
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly LibraryDbContext _context;

        public SubscriptionRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(BookSubscription entity)
        {
            try
            {
                _context.BookSubscriptions.Add(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to add book subscription to database.", ex);
            }
        }

        public IEnumerable<BookSubscription> GetActiveForBook(int bookId)
        {
            return _context.BookSubscriptions
                .Where(bs => bs.BookId == bookId && !bs.IsNotified)
                .Include(bs => bs.User)
                .AsNoTracking()
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

        public IEnumerable<BookSubscription> GetAll()
        {
            return _context.BookSubscriptions
                .Include(bs => bs.User)
                .Include(bs => bs.Book)
                .AsNoTracking()
                .ToList();
        }

        public BookSubscription? GetById(int id)
        {
            return _context.BookSubscriptions
                .Include(bs => bs.User)
                .Include(bs => bs.Book)
                .AsNoTracking()
                .FirstOrDefault(bs => bs.Id == id);
        }

        public BookSubscription? GetActiveByUserAndBook(int userId, int bookId)
        {
            return _context.BookSubscriptions
                .FirstOrDefault(bs => bs.UserId == userId && bs.BookId == bookId && !bs.IsNotified);
        }

        public void Remove(BookSubscription entity)
        {
            try
            {
                _context.BookSubscriptions.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to remove book subscription from database.", ex);
            }
        }

        public void Update(BookSubscription entity)
        {
            try
            {
                _context.BookSubscriptions.Update(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to update book subscription in database.", ex);
            }
        }
    }

}
