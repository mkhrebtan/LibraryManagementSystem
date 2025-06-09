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
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books.AsNoTracking().ToList();
        }

        public IEnumerable<Book> GetAllAvailable()
        {
            return _context.Books.AsNoTracking().Where(b => b.IsAvailable).ToList();
        }

        public Book? GetById(int id)
        {
            return _context.Books.Find(id);
        }

        public void Add(Book entity)
        {
            try
            {
                _context.Books.Add(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to add book to database.", ex);
            }
        }

        public void Remove(Book entity)
        {
            try
            {
                _context.Books.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to remove book from database.", ex);
            }
        }

        public void Update(Book entity)
        {
            try
            {
                _context.Books.Update(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to update book in database.", ex);
            }
        }
    }

}
