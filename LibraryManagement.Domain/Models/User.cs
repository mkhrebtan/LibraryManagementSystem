using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Domain.Models;

public class User
{
    #region Properties

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public List<BookLoan> BookLoans { get; set; } = new List<BookLoan>();

    public List<BookSubscription> BookSubscriptions { get; set; } = new List<BookSubscription>();

    #endregion

    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Email: {Email}, Phone: {PhoneNumber}, Login: {Login}";
    }
}