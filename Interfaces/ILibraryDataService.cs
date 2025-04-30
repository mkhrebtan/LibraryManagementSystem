using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces
{
    public interface ILibraryDataService
    {
        public ICollection<Book> Books { get; }
    }
}