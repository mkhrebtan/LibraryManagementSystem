using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Repos
{
    public class LoanRepository : GenericRepository<BookLoan>, ILoanRepository
    {
        public LoanRepository(LibraryDbContext context) : base(context) { } 

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
                .FirstOrDefault();
        }

        public IEnumerable<BookLoan> GetBookHistory(int bookId)
        {
            return _context.BookLoans
                .Where(bl => bl.BookId == bookId)
                .Include(bl => bl.User)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<BookLoan> GetUserHistory(int userId)
        {
            return _context.BookLoans
                .Where(bl => bl.UserId == userId)
                .Include(bl => bl.Book)
                .AsNoTracking()
                .ToList();
        }
    }
}
