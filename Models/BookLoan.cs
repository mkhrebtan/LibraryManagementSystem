using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models;

public class BookLoan
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime LoanDate { get; set; } = DateTime.Now;
    public DateTime? ReturnDate { get; set; } = null;
}
