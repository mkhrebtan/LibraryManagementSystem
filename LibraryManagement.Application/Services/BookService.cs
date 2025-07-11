using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;
using LibraryManagement.Domain.UnitOfWork;

namespace LibraryManagement.Application.Services;

public class BookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
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

        try
        {
            _unitOfWork.CreateTransaction();
            _bookRepository.Add(book);
            _unitOfWork.SaveChanges();
            _unitOfWork.CommitTransaction();
        }
        catch (InvalidOperationException)
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public void Remove(int bookId)
    {
        Book bookToRemove = _bookRepository.GetById(bookId) ?? throw new ArgumentException("Book not found.");

        try
        {
            _unitOfWork.CreateTransaction();
            _bookRepository.Remove(bookToRemove);
            _unitOfWork.SaveChanges();
            _unitOfWork.CommitTransaction();
        }
        catch (InvalidOperationException)
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }
    }
}
