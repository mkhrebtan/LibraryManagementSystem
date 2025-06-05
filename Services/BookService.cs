using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class BookService
    {
        private readonly LibraryDbContext _context;

        public BookService(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books.AsNoTracking().ToList();
        }

        public Book? GetById(int id)
        {
            return _context.Books.Find(id);
        }

        public IEnumerable<Book> GetByGenre(string genre)
        {
            return _context.Books.AsNoTracking().Where(b => b.Genre.ToLower().Equals(genre.ToLower())).ToList();
        }

        public IEnumerable<Book> GetAllAvailable()
        {
            return _context.Books.AsNoTracking().Where(b => b.IsAvailable).ToList();
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

            try
            {
                _context.Books.Add(book);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to add book to database. It may already exist.", ex);
            }
        }

        public void Remove(int bookId)
        {
            var bookToDelete = GetById(bookId) ?? throw new ArgumentException("Book not found");

            try
            {
                _context.Books.Remove(bookToDelete);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to remove book from database. It may not exist.", ex);
            }
        }
    }
}
