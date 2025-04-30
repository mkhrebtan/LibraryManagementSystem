using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces
{
    public interface ILibraryService
    {
        void AddBook(Book book);
        void RemoveBook(int bookId);
        void ReserveBook(int bookId);
        void ReturnBook(int bookId);
    }
}