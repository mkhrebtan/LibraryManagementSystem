using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class LibraryService : ILibraryService
    {
        private ILibraryDataService _dataService;
        public LibraryService(ILibraryDataService dataService)
        {
            _dataService = dataService;
        }

        public List<Book> GetAllBooks()
        {
            return _dataService.Books.ToList();
        }

        public Dictionary<string, List<Book>> GetBooksByGenre()
        {
            return _dataService.Books.GroupBy(b => b.Genre)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public void AddBook(Book book)
        {
            throw new NotImplementedException();
        }

        public void RemoveBook(int bookId)
        {
            throw new NotImplementedException();
        }

        public void ReserveBook(int bookId)
        {
            var bookToReserve = _dataService.Books.FirstOrDefault(b => b.Id == bookId);
            bookToReserve?.Reserve();
        }

        public void ReturnBook(int bookId)
        {
            var bookToReturn = _dataService.Books.FirstOrDefault(b => b.Id == bookId);
            if (bookToReturn != null)
            {
                bookToReturn.Return();
            }
        }

        public void NotifyMeWhenBookIsAvailable(int bookId, Book.BookReturnedHandler handler)
        {
            var bookToSubscribe = _dataService.Books.FirstOrDefault(b => b.Id == bookId);
            if (bookToSubscribe != null)
            {
                bookToSubscribe.AddBookReturnedHandler(handler);
            }
        }
    }
}