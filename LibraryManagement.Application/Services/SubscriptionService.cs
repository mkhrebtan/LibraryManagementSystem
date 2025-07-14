using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.BookAvailability;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;
using LibraryManagement.Domain.UnitOfWork;

namespace LibraryManagement.Application.Services;

public class SubscriptionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;
    private readonly INotificationService _notificationService;

    public SubscriptionService(
        ISubscriptionRepository subscriptionRepository, 
        IBookRepository bookRepository, 
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService,
        LoanService loanService)
    {
        _subscriptionRepository = subscriptionRepository;
        _bookRepository = bookRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        loanService.BookReturned += (sender, args) => NotifySubscribers(args.Book);
    }

    public void Subscribe(int bookId, int userId, ICollection<NotificationPreference> notificationPreferences)
    {
        var user = _userRepository.GetById(userId) ?? throw new ArgumentException("User not found");
        var book = _bookRepository.GetById(bookId) ?? throw new ArgumentException("Book not found");

        if (book.IsAvailable)
        {
            throw new InvalidOperationException("Book is currently available, no need to subscribe.");
        }

        var existingSubscription = _subscriptionRepository.GetActiveByUserAndBook(user.Id, book.Id);

        if (existingSubscription != null)
        {
            throw new InvalidOperationException("User is already subscribed to this book.");
        }

        var bookSubscription = new BookSubscription
        {
            BookId = book.Id,
            UserId = user.Id,
            SubscribedAt = DateTime.UtcNow,
            IsNotified = false,
            NotificationPreferences = notificationPreferences
        };

        try
        {
            _unitOfWork.CreateTransaction();
            _subscriptionRepository.Add(bookSubscription);
            _unitOfWork.SaveChanges();
            _unitOfWork.CommitTransaction();
        }
        catch (InvalidOperationException)
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public void Unsubscribe(int bookId, int userId)
    {
        var user = _userRepository.GetById(userId) ?? throw new ArgumentException("User not found");
        var book = _bookRepository.GetById(bookId) ?? throw new ArgumentException("Book not found");

        var existingSubscription = _subscriptionRepository.GetActiveByUserAndBook(user.Id, book.Id);

        if (existingSubscription == null)
        {
            throw new InvalidOperationException("User is not subscribed to this book.");
        }

        try
        {
            _unitOfWork.CreateTransaction();
            _subscriptionRepository.Remove(existingSubscription);
            _unitOfWork.SaveChanges();
            _unitOfWork.CommitTransaction();
        }
        catch (InvalidOperationException)
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public void Unsubscribe(int subscriptionId)
    {
        var subscription = _subscriptionRepository.GetById(subscriptionId) ?? throw new ArgumentException("Subscription not found.");

        try
        {
            _unitOfWork.CreateTransaction();
            _subscriptionRepository.Remove(subscription);
            _unitOfWork.SaveChanges();
            _unitOfWork.CommitTransaction();
        }
        catch (InvalidOperationException)
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }
    }

    public void NotifySubscribers(Book book)
    {
        var subscriptions = _subscriptionRepository.GetActiveForBook(book.Id).ToList();

        if (!subscriptions.Any())
        {
            return;
        }

        try
        {
            _unitOfWork.CreateTransaction();
            foreach (var subscription in subscriptions)
            {
                subscription.IsNotified = true;
            }
            _unitOfWork.SaveChanges();
            _unitOfWork.CommitTransaction();
        }
        catch (InvalidOperationException)
        {
            _unitOfWork.RollbackTransaction();
            throw;
        }

        foreach (var subscription in subscriptions)
        {
            _notificationService.Update(book, subscription.User, subscription.NotificationPreferences);
        }
    }

    public IEnumerable<BookSubscription> GetUserSubscriptions(int userId)
    {
        var user = _userRepository.GetById(userId) ?? throw new ArgumentException("User not found");
        return _subscriptionRepository.GetActiveForUser(user.Id);
    }
}
