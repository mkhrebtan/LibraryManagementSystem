using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using LibraryManagementSystem.Services;

LibraryDbContext libraryDbContext = new LibraryDbContext();
BookService bookService = new BookService(libraryDbContext);
UserService userService = new UserService(libraryDbContext);
LoanService loanService = new LoanService(libraryDbContext);

User? loggedInUser = null;

Dictionary<string, Action> userMenuActions = new Dictionary<string, Action>
{
    { "Get All Books", () => GetAllBooks() },
    { "Get Books By Genre", () => GetAllBooksByGenre() },
    { "Loan Book", () => LoanBook() },
    { "Return Book", () => ReturnBook() },
    { "Loaned Books", () => GetLoanedBooks() },
    { "Loan History", () => GetLoanHistory() }
};

Dictionary<string, Action> adminMenuActions = new Dictionary<string, Action>
{
    { "Add book", () => AddBook() },
    { "Remove book", () => RemoveBook() },
    { "Add user", () => AddUser() },
    { "Remove user", () => RemoveUser() },
    { "Get All Books", () => GetAllBooks() },
    { "Get All Users", () => GetAllUsers() },
};

Dictionary<string, Action> loginMenuActions = new Dictionary<string, Action>
{
    { 
        "Login as User", 
        () => DisplayMenu(GetAllUsersForLogIn(), "Select User")
    },
    { 
        "Login as Admin", 
        () => DisplayMenu(adminMenuActions, "Admin Menu") 
    },
};

DisplayMenu(loginMenuActions, "Login Menu");

#region Admin

void AddBook()
{
    Console.WriteLine("Book adding not implemented");
}

void RemoveBook()
{
    var allBooks = bookService.GetAll();
    if (allBooks.Count() == 0)
    {
        Console.WriteLine("No books available to remove.");
        return;
    }
    var removeBookMenu = GetBookMenu(allBooks, BookAction.Remove);
    DisplayMenu(removeBookMenu, "Select Book to Remove");
}

void RemoveBookAction(Book bookToRemove)
{
    try
    {
        bookService.Remove(bookToRemove.Id);
        Console.WriteLine($"Book removed: {bookToRemove.Title} by {bookToRemove.Author}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error removing book: {ex.Message}");
    }
}

void AddUser()
{
    var users = userService.GetAll();
    int userId = 0;
    if (users.Count() > 0)
    {
        userId = users.Max(u => u.Id) + 1;
    }
    else 
    {
        userId = 1;
    }

    try
    {
        userService.Add(
            name: $"User{userId}", 
            email: $"user{userId}@email", 
            phoneNumber: $"123456789{userId}", 
            login: $"user{userId}", 
            password: $"password{userId}"
        );
        Console.WriteLine($"User added: User{userId} with email user{userId}@email");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error adding user: {ex.Message}");
        return;
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Error adding user: {ex.Message}");
        return;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
        return;
    }
}

void RemoveUser()
{
    Console.Write("Enter user ID: ");
    if (!int.TryParse(Console.ReadLine(), out int userId))
    {
        Console.WriteLine("Invalid user ID.");
        return;
    }

    try
    {
        userService.Remove(userId);
        Console.WriteLine($"User removed");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Error removing user: {ex.Message} It may not exist.");
        return;
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error removing user: {ex.Message}");
        return;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
        return;
    }
}

void GetAllUsers()
{
    var users = userService.GetAll();
    if (users.Count() > 0)
    {
        foreach (var user in users)
        {
            Console.WriteLine(user);
        }
    }
    else
    {
        Console.WriteLine("No users found.");
    }
}

#endregion

#region User

void GetAllBooks()
{
    var books = bookService.GetAll();
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
}

void GetAllBooksByGenre()
{
    Console.Write("Enter genre: ");
    string genre = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(genre))
    {
        Console.WriteLine("Genre cannot be empty.");
        return;
    }
    var books = bookService.GetByGenre(genre);
    if (books.Count() > 0)
    {
        foreach (var book in books)
        {
            Console.WriteLine(book);
        }
    }
    else
    {
        Console.WriteLine($"No books found for genre: {genre}");
    }
}

void LoanBook()
{
    var allAvailableBooks = bookService.GetAllAvailable();
    if (allAvailableBooks.Count() == 0)
    {
        Console.WriteLine("No available books to loan.");
        return;
    }
    var loanBookMenu = GetBookMenu(allAvailableBooks, BookAction.Loan);
    DisplayMenu(loanBookMenu, "Select Book to Loan");
}

void LoanBookAction(Book book)
{
    try
    {
        loanService.StartBookLoan(loggedInUser!.Id, book.Id);
        Console.WriteLine($"Book loaned: {book.Title} by {book.Author}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error loaning book: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Error loaning book: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}

void ReturnBook()
{
    var loanedBooks = loanService.GetLoanedBooksForUser(loggedInUser!.Id);
    if (loanedBooks.Count() == 0)
    {
        Console.WriteLine("No books to return.");
        return;
    }
    var returnBookMenu = GetBookMenu(loanedBooks, BookAction.Return);
    DisplayMenu(returnBookMenu, "Select Book to Return");
}

void ReturnBookAction(Book book)
{
    try
    {
        loanService.EndBookLoan(book.Id, loggedInUser!.Id);
        Console.WriteLine($"Book returned: {book.Title} by {book.Author}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error returning book: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Error returning book: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}

void GetLoanedBooks()
{
    var books = loanService.GetLoanedBooksForUser(loggedInUser!.Id);
    if (books.Count() > 0)
    {
        foreach (var book in books)
        {
            Console.WriteLine(book);
        }
    }
    else
    {
        Console.WriteLine("No loaned books found.");
    }
}

void GetLoanHistory()
{
    var loans = loanService.GetForUser(loggedInUser!.Id);
    if (loans.Count() > 0)
    {
        foreach (var loan in loans)
        {
            string returnDate = loan.ReturnDate.HasValue ? loan.ReturnDate.Value.ToString() : "Not returned yet";
            Console.WriteLine($"{loan.Book.Title} by {loan.Book.Author} - Loaned on: {loan.LoanDate}, Returned on: {returnDate}");
        }
    }
    else
    {
        Console.WriteLine("No loan history found.");
    }
}

#endregion

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
                ConfirmAction();
                break;
            case ConsoleKey.Q:
                return;
        }
    }
    while (true);
}

Dictionary<string, Action> GetAllUsersForLogIn()
{
    var users = userService.GetAll();
    Dictionary<string, Action> userLoginActions = new Dictionary<string, Action>();
    foreach (var user in users)
    {
        userLoginActions.Add(user.Name, () =>
        {
            LogInUser(user);
        });
    }
    return userLoginActions;
}

Dictionary<string, Action> GetBookMenu(IEnumerable<Book> booksCollection, BookAction bookAction)
{
    Dictionary<string, Action> menu = new Dictionary<string, Action>();
    foreach (var book in booksCollection)
    {
        menu.Add(book.ToString(), () =>
        {
            switch (bookAction)
            {
                case BookAction.Add:
                    AddBook();
                    break;
                case BookAction.Remove:
                    RemoveBookAction(book);
                    break;
                case BookAction.Loan:
                    LoanBookAction(book);
                    break;
                case BookAction.Return:
                    ReturnBookAction(book);
                    break;
            }
        });
    }

    return menu;
}

void LogInUser(User user)
{
    loggedInUser = user;
    DisplayMenu(userMenuActions, $"{loggedInUser?.Name} Menu");
}

void ConfirmAction()
{
    Console.WriteLine("\nPress any key to return back.");
    Console.ReadKey();
}

enum BookAction
{
    Add,
    Remove,
    Loan,
    Return
}