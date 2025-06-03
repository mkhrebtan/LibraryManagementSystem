using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using LibraryManagementSystem.Services;

LibraryDbContext libraryDbContext = new LibraryDbContext();
BookService bookService = new BookService(libraryDbContext);

List<Book> books = new List<Book>
{
    new Book { Title = "1984", Genre = "Dystopian", Author = "George Orwell", PublicationYear = 1985, PageCount = 328 },
    new Book { Title = "To Kill a Mockingbird", Genre = "Fiction", Author = "Harper Lee", PublicationYear = 1960, PageCount = 281 },
    new Book { Title = "The Great Gatsby", Genre = "Fiction", Author = "F. Scott Fitzgerald", PublicationYear = 1925, PageCount = 180 },
    new Book { Title = "The Catcher in the Rye", Genre = "Fiction", Author = "J.D. Salinger", PublicationYear = 1951, PageCount = 277 },
    new Book { Title = "Brave New World", Genre = "Dystopian", Author = "Aldous Huxley", PublicationYear = 1932, PageCount = 311 },
};


Dictionary<string, Action> userMenuActions = new Dictionary<string, Action>
{
    { "Get All Books", () => GetAllBooks() },
    { "Get Books By Genre", () => Console.WriteLine("Book listing by genre not implemented.") },
    { "Reserve Book", () => Console.WriteLine("Book reservation not implemented.") },
    { "Notify Me When Book Is Available", () => Console.WriteLine("Notidications aren't implemented.") },
    { "Return Book", () => Console.WriteLine("Book returning not implemented.") },
};

Dictionary<string, Action> adminMenuActions = new Dictionary<string, Action>
{
    { "Add book", () => AddBook() },
    { "Remove book", () => RemoveBook() },
    { "Get All Books", () => GetAllBooks() },
};

Dictionary<string, Action> loginMenuActions = new Dictionary<string, Action>
{
    { 
        "Login as User", 
        () => DisplayMenu(userMenuActions, "User Menu")
    },
    { 
        "Login as Admin", 
        () => DisplayMenu(adminMenuActions, "Admin Menu") 
    },
};

DisplayMenu(loginMenuActions, "Login Menu");

void DisplayMenu(Dictionary<string, Action> menuActions, string title)
{
    int selectedIndex = 0;

    do
    {
        Console.Clear(); Console.WriteLine("\x1b[3J");
        Console.WriteLine(title + "\n");
        for (int i = 0; i < menuActions.Count; i++)
        {
            if (i == selectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(menuActions.Keys.ElementAt(i));
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(menuActions.Keys.ElementAt(i));
            }
        }

        var key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.UpArrow:
                selectedIndex = (selectedIndex - 1 + menuActions.Count) % menuActions.Count;
                break;
            case ConsoleKey.DownArrow:
                selectedIndex = (selectedIndex + 1) % menuActions.Count;
                break;
            case ConsoleKey.Enter:
                var action = menuActions.Values.ElementAt(selectedIndex);
                Console.Clear(); Console.WriteLine("\x1b[3J");
                action.Invoke();
                break;
            case ConsoleKey.Q:
                return;
        }
    }
    while (true);
}

#region Admin

void AddBook()
{
    Console.WriteLine("Book adding not implemented");
    ConfirmAction();
}

void RemoveBook()
{
    Console.Write("Enter book ID: ");
    if (!int.TryParse(Console.ReadLine(), out int bookId))
    {
        Console.WriteLine("Invalid book ID.");
        return;
    }

    var book = bookService.GetBookById(bookId);
    if (book == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    try
    {
        bookService.RemoveBook(book);
        Console.WriteLine($"Book removed: {book.Title} by {book.Author}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error removing book: {ex.Message}");
    }

    ConfirmAction();
}

#endregion

#region User

void GetAllBooks()
{
    var books = bookService.GetAllBooks();
    if (books.Count() > 0)
    {
        foreach(var book in books)
        {
            Console.WriteLine(book);
        }
    }
    else
    {
        Console.WriteLine("No books found.");
    }
    
    ConfirmAction();
}

#endregion

void ConfirmAction()
{
    Console.WriteLine("\nPress any key to return back.");
    Console.ReadKey();
}