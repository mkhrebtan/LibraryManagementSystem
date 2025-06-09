using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.BookAvailability;
using LibraryManagement.Domain.Repos;

namespace LibraryManagement.Application.Services;

public class SubscriptionService : IBookAvailabilityPublisher
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository, IBookRepository bookRepository, IUserRepository userRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _bookRepository = bookRepository;
        _userRepository = userRepository;
    }

    public void Subscribe(int bookId, int userId)
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
            IsNotified = false
        };

        _subscriptionRepository.Add(bookSubscription);
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

        _subscriptionRepository.Remove(existingSubscription);
    }

    public void Unsubscribe(int subscriptionId)
    {
        var subscription = _subscriptionRepository.GetById(subscriptionId) ?? throw new ArgumentException("Subscription not found.");

        _subscriptionRepository.Remove(subscription);
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
            using (var writer = new StreamWriter(@"C:\Dev\LibraryManagementSystem\LibraryManagement.Application\NotificationsLogs\notifications.txt", append: true))
            {
                foreach (var subscription in subscriptions)
                {
                    writer.WriteLine($"{DateTime.Now} | Notification for User '{subscription.User.Name}' ({subscription.User.Email}): Book '{book.Title}' is now available for loan.");
                    subscription.IsNotified = true;
                    _subscriptionRepository.Update(subscription);
                }
            }
        }
        catch (Exception ex) when (
            ex is UnauthorizedAccessException ||
            ex is DirectoryNotFoundException ||
            ex is IOException)
        {
            try
            {
                foreach (var subscription in subscriptions)
                {
                    subscription.IsNotified = true;
                    _subscriptionRepository.Update(subscription);
                }
            }
            catch (Exception dbEx)
            {
                throw new InvalidOperationException("Failed to update subscription status.", dbEx);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to notify subscribers.", ex);
        }
    }

    public IEnumerable<BookSubscription> GetUserSubscriptions(int userId)
    {
        var user = _userRepository.GetById(userId) ?? throw new ArgumentException("User not found");
        return _subscriptionRepository.GetActiveForUser(user.Id);
    }
}
