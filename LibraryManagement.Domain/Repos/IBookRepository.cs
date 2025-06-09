using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Models;

namespace LibraryManagement.Domain.Repos
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<Book> GetAllAvailable();

    }
}
