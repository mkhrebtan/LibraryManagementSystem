using LibraryManagementSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.IO;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class SubscriptionService : IBookAvailabilityPublisher
    {
        private readonly LibraryDbContext _context;

        public SubscriptionService(LibraryDbContext context)
        {
            _context = context;
        }

        public void Subscribe(int bookId, int userId)
        {
            var user = _context.Users.Find(userId) ?? throw new ArgumentException("User not found");
            var book = _context.Books.Find(bookId) ?? throw new ArgumentException("Book not found");

            if (book.IsAvailable)
            {
                throw new InvalidOperationException("Book is currently available, no need to subscribe.");
            }

            var existingSubscription = _context.BookSubscriptions
                .FirstOrDefault(bs => bs.BookId == bookId && bs.UserId == userId && bs.IsNotified == false);

            if (existingSubscription != null)
            {
                throw new InvalidOperationException("User is already subscribed to this book.");
            }

            var bookSubscription = new BookSubscription
            {
                BookId = bookId,
                UserId = userId,
                SubscribedAt = DateTime.UtcNow,
                IsNotified = false
            };

            try
            {
                _context.BookSubscriptions.Add(bookSubscription);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to subscribe to book.", ex);
            }
        }

        public void Unsubscribe(int bookId, int userId)
        {
            var user = _context.Users.Find(userId) ?? throw new ArgumentException("User not found");
            var book = _context.Books.Find(bookId) ?? throw new ArgumentException("Book not found");

            var subscription = _context.BookSubscriptions
                .FirstOrDefault(bs => bs.BookId == bookId && bs.UserId == userId && bs.IsNotified == false);

            if (subscription == null)
            {
                throw new InvalidOperationException("User is not subscribed to this book.");
            }

            try
            {
                _context.BookSubscriptions.Remove(subscription);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to unsubscribe from book.", ex);
            }
        }

        public void Unsubscribe(int subscriptionId)
        {
            var subscription = _context.BookSubscriptions
                .Find(subscriptionId);

            if (subscription == null)
            {
                throw new ArgumentException("Subscription not found.");
            }

            try
            {
                _context.BookSubscriptions.Remove(subscription);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to unsubscribe from book.", ex);
            }
        }

        public void NotifySubscribers(int bookId)
        {
            var book = _context.Books.Find(bookId)
                ?? throw new ArgumentException("Book not found");

            var subscriptions = _context.BookSubscriptions
                .Where(bs => bs.BookId == bookId && bs.IsNotified == false)
                .Include(bs => bs.User)
                .ToList();

            if (!subscriptions.Any())
            {
                return;
            }

            try
            {
                using (var writer = new StreamWriter(@"C:\Dev\LibraryManagementSystem\notifications.txt", append: true))
                {
                    foreach (var subscription in subscriptions)
                    {
                        writer.WriteLine($"{DateTime.Now} | Notification for User '{subscription.User.Name}' ({subscription.User.Email}): Book '{book.Title}' is now available for loan.");
                        subscription.IsNotified = true;
                    }
                }
                _context.SaveChanges();
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
                    }
                    _context.SaveChanges();
                }
                catch (DbUpdateException dbEx)
                {
                    throw new InvalidOperationException("Failed to update subscription status.", dbEx);
                }
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to notify subscribers.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<BookSubscription> GetUserSubscriptions(int userId)
        {
            return _context.BookSubscriptions
                .Where(bs => bs.UserId == userId && bs.IsNotified == false)
                .Include(bs => bs.Book)
                .AsNoTracking()
                .ToList();
        }
    }
}
