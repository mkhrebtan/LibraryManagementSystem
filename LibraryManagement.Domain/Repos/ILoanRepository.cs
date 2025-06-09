using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Models;

namespace LibraryManagement.Domain.Repos
{
    public interface ILoanRepository : IRepository<BookLoan>
    {
        IEnumerable<BookLoan> GetUserHistory(int userId);
        IEnumerable<BookLoan> GetBookHistory(int bookId);
        IEnumerable<BookLoan> GetActiveLoansForUser(int userId);
        BookLoan? GetActiveLoanForUserAndBook(int userId, int bookId);
    }
}
