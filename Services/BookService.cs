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

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.AsNoTracking().ToList();
        }

        public Book? GetBookById(int id)
        {
            return _context.Books.Find(id);
        }

        public IEnumerable<Book> GetBooksByGenre(string genre)
        {
            return _context.Books.AsNoTracking().Where(b => b.Genre.ToLower().Equals(genre.ToLower())).ToList();
        }

        public void AddBook(Book book)
        {
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

        public void RemoveBook(Book bookToDelete)
        {  
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
