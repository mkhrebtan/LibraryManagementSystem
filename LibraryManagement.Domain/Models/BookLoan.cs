using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Domain.Models;

public class BookLoan
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    public int BookId { get; set; }

    public Book Book { get; set; } = null!;

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public DateTime LoanDate { get; set; } = DateTime.Now;

    public DateTime? ReturnDate { get; set; } = null;
}
