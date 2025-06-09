using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;

namespace LibraryManagement.Application.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IEnumerable<Book> GetAll()
        {
            return _bookRepository.GetAll();
        }

        public Book? GetById(int id)
        {
            return _bookRepository.GetById(id);
        }

        public IEnumerable<Book> GetByGenre(string genre)
        {
            var allBooks = _bookRepository.GetAll();
            return allBooks.Where(b => string.Equals(b.Genre, genre, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public IEnumerable<Book> GetAllAvailable()
        {
            return _bookRepository.GetAllAvailable();
        }

        public void Add(string title, string genre, string author, int publicationYear, int pageCount)
        {
            var book = new Book
            {
                Title = title,
                Genre = genre,
                Author = author,
                PublicationYear = publicationYear,
                PageCount = pageCount
            };

            _bookRepository.Add(book);
        }

        public void Remove(int bookId)
        {
            Book bookToRemove = _bookRepository.GetById(bookId) ?? throw new ArgumentException("Book not found.");
            _bookRepository.Remove(bookToRemove);
        }
    }
}
