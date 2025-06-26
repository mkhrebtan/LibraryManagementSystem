using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Postgres.Repos
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryDbContext context) : base(context) { }

        public IEnumerable<Book> GetAllAvailable()
        {
            return _context.Books.AsNoTracking().Where(b => b.IsAvailable).ToList();
        }
    }
}
