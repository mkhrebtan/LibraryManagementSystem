using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models;

public class Book
{
    #region Properties

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int PageCount { get; set; }
    public bool IsAvailable { get; set; } = true;

    public List<BookLoan> BookLoans { get; set; } = new List<BookLoan>();
    public List<BookSubscription> BookSubscriptions { get; set; } = new List<BookSubscription>();

    #endregion

    public override string ToString()
    {
       var asciiArt = @$" 
            _______
           /      /, ""{Title}"" ({Id})
          /      //  By {Author}, {PublicationYear}
         /______//   {PageCount} pages
        (______(/    Genre: {Genre}
        Available: {(IsAvailable ? "Yes" : "No")}";

       return asciiArt;
    }
}