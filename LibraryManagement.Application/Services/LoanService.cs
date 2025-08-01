﻿using LibraryManagement.Domain.BookAvailability;
using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;
using LibraryManagement.Domain.UnitOfWork;

namespace LibraryManagement.Application.Services
{
    public class LoanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public event EventHandler<BookReturnedEventArgs>? BookReturned;

        public LoanService(
            ILoanRepository loanRepository, 
            IBookRepository bookRepository, 
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<BookLoan> GetUserHistory(int userId)
        {
            User user = _userRepository.GetById(userId)
                ?? throw new ArgumentException("User not found");
            return _loanRepository.GetUserHistory(user.Id);
        }

        public IEnumerable<BookLoan> GetBookHistory(int bookId)
        {
            Book book = _bookRepository.GetById(bookId)
                ?? throw new ArgumentException("Book not found");
            return _loanRepository.GetBookHistory(book.Id);
        }

        public IEnumerable<Book> GetLoanedBooksForUser(int userId)
        {
            User user = _userRepository.GetById(userId)
                ?? throw new ArgumentException("User not found");

            var loans = _loanRepository.GetActiveLoansForUser(user.Id);

            return loans.Select(bl => bl.Book).ToList();
        }

        public void StartBookLoan(int userId, int bookId)
        {
            var user = _userRepository.GetById(userId)
                ?? throw new ArgumentException("User not found");
            var book = _bookRepository.GetById(bookId)
                ?? throw new ArgumentException("Book not found");

            if (!book.IsAvailable)
            {
                throw new InvalidOperationException("Book is not available for loan.");
            }

            var bookLoan = new BookLoan
            {
                UserId = userId,
                BookId = bookId,
                LoanDate = DateTime.UtcNow,
                ReturnDate = null
            };

            try
            {
                _unitOfWork.CreateTransaction();

                _loanRepository.Add(bookLoan);
                book.IsAvailable = false;
                _bookRepository.Update(book);

                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
            }
            catch (InvalidOperationException)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public void EndBookLoan(int loanId)
        {
            var bookLoan = _loanRepository.GetById(loanId)
                ?? throw new ArgumentException("Loan not found");
            if (bookLoan.ReturnDate != null)
            {
                throw new InvalidOperationException("Loan has already been returned.");
            }

            bookLoan.ReturnDate = DateTime.UtcNow;
            bookLoan.Book.IsAvailable = true;

            try
            {
                _unitOfWork.CreateTransaction();
                _loanRepository.Update(bookLoan);
                _bookRepository.Update(bookLoan.Book);
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
            }
            catch (InvalidOperationException)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }

            OnBookReturned(new BookReturnedEventArgs { Book = bookLoan.Book });
        }

        public void EndBookLoan(int bookId, int userId)
        {
            var book = _bookRepository.GetById(bookId)
                ?? throw new ArgumentException("Book not found");
            var user = _userRepository.GetById(userId)
                ?? throw new ArgumentException("User not found");

            var bookLoan = _loanRepository.GetActiveLoanForUserAndBook(user.Id, book.Id)
                ?? throw new InvalidOperationException("Active loan not found for user and book.");
            if (bookLoan.ReturnDate != null)
            {
                throw new InvalidOperationException("Loan has already been returned.");
            }

            bookLoan.ReturnDate = DateTime.UtcNow;
            book.IsAvailable = true;

            try
            {
                _unitOfWork.CreateTransaction();
                _loanRepository.Update(bookLoan);
                _bookRepository.Update(book);
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
            }
            catch (InvalidOperationException)
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }

            OnBookReturned(new BookReturnedEventArgs { Book = book });
        }

        protected virtual void OnBookReturned(BookReturnedEventArgs args)
        {
            BookReturned?.Invoke(this, args);
        }
    }
}
