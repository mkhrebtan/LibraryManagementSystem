using LibraryManagement.Domain.BookAvailability;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Models;

namespace LibraryManagement.Application.Interfaces;

public interface INotificationService
{
    void Update(Book book, User user, ICollection<NotificationPreference> notificationPreferences);
}
