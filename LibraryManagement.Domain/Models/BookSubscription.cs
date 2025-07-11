using LibraryManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Domain.Models;

public class BookSubscription
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int BookId { get; set; }

    public int UserId { get; set; }

    public DateTime SubscribedAt { get; set; }

    public bool IsNotified { get; set; } = false;

    public ICollection<NotificationPreference> NotificationPreferences { get; set; } = new List<NotificationPreference>();

    public Book Book { get; set; } = null!;

    public User User { get; set; } = null!;
}
