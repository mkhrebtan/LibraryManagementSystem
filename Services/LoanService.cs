using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Services
{
    public class LoanService
    {
        private readonly LibraryDbContext _context;

        public LoanService(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<BookLoan> GetForUser(int userId)
        {
            return _context.BookLoans
                .Where(bl => bl.UserId == userId)
                .Include(bl => bl.Book)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<BookLoan> GetForBook(int bookId)
        {
            return _context.BookLoans
                .Where(bl => bl.BookId == bookId)
                .Include(bl => bl.User)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<Book> GetLoanedBooksForUser(int userId)
        {
            var loans = _context.BookLoans
                .Where(bl => bl.UserId == userId && bl.ReturnDate == null)
                .Include(bl => bl.Book)
                .AsNoTracking();

            return loans.Select(bl => bl.Book).ToList();
        }

        public void StartBookLoan(int userId, int bookId)
        {
            var user = _context.Users.Find(userId) ?? throw new ArgumentException("User not found");
            var book = _context.Books.Find(bookId) ?? throw new ArgumentException("Book not found");

            var bookLoan = new BookLoan
            {
                UserId = userId,
                BookId = bookId,
                LoanDate = DateTime.UtcNow,
                ReturnDate = null
            };

            try
            {
                _context.BookLoans.Add(bookLoan);
                UpdateBookAvailability(bookId, false);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to loan book.", ex);
            }
        }

        public void EndBookLoan(int loanId)
        {
            var bookLoan = _context.BookLoans.Find(loanId)
                ?? throw new ArgumentException("Loan not found");
            if (bookLoan.ReturnDate != null)
            {
                throw new InvalidOperationException("Loan has already been returned.");
            }
            bookLoan.ReturnDate = DateTime.UtcNow;
            try
            {
                _context.BookLoans.Update(bookLoan);
                UpdateBookAvailability(bookLoan.BookId, true);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to end loan. It may not exist.", ex);
            }
        }

        public void EndBookLoan(int bookId, int userId)
        {
            var bookLoan = _context.BookLoans
                .FirstOrDefault(bl => bl.BookId == bookId && bl.UserId == userId && bl.ReturnDate == null)
                ?? throw new ArgumentException("Loan not found for the specified book and user.");
            bookLoan.ReturnDate = DateTime.UtcNow;
            try
            {
                _context.BookLoans.Update(bookLoan);
                UpdateBookAvailability(bookId, true);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to end loan. It may not exist.", ex);
            }
        }

        private void UpdateBookAvailability(int bookId, bool isAvailable)
        {
            var book = _context.Books.Find(bookId);
            book!.IsAvailable = isAvailable;
            _context.Books.Update(book);
            _context.SaveChanges();
        }
    }
}
