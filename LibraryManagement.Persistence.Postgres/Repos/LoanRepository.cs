using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Repos
{
    public class LoanRepository : ILoanRepository
    {
        private readonly LibraryDbContext _context;

        public LoanRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public void Add(BookLoan entity)
        {
            try
            {
                _context.BookLoans.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to add book loan to database.", ex);
            }
        }

        public IEnumerable<BookLoan> GetActiveLoansForUser(int userId)
        {
            return _context.BookLoans
                .Where(bl => bl.UserId == userId && bl.ReturnDate == null)
                .Include(bl => bl.Book)
                .AsNoTracking()
                .ToList();
        }

        public BookLoan? GetActiveLoanForUserAndBook(int userId, int bookId)
        {
            return _context.BookLoans
                .Where(bl => bl.UserId == userId && bl.BookId == bookId && bl.ReturnDate == null)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public IEnumerable<BookLoan> GetAll()
        {
            return _context.BookLoans
                .Include(bl => bl.Book)
                .Include(bl => bl.User)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<BookLoan> GetBookHistory(int bookId)
        {
            return _context.BookLoans
                .Where(bl => bl.BookId == bookId)
                .Include(bl => bl.User)
                .AsNoTracking()
                .ToList();
        }

        public BookLoan? GetById(int id)
        {
            return _context.BookLoans
                .Include(bl => bl.Book)
                .Include(bl => bl.User)
                .AsNoTracking()
                .FirstOrDefault(bl => bl.Id == id);
        }

        public IEnumerable<BookLoan> GetUserHistory(int userId)
        {
            return _context.BookLoans
                .Where(bl => bl.UserId == userId)
                .Include(bl => bl.Book)
                .AsNoTracking()
                .ToList();
        }

        public void Remove(BookLoan entity)
        {
            try
            { 
                _context.BookLoans.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to remove book loan from database.", ex);
            }
        }

        public void Update(BookLoan entity)
        {
            try
            {
                _context.BookLoans.Update(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to update book loan in database.", ex);
            }
        }
    }
}
