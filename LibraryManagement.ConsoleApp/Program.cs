using LibraryManagement.Domain.Models;
using LibraryManagement.Domain.Repos;
using LibraryManagement.Application.Services;
using LibraryManagement.Persistence.Postgres;
using LibraryManagement.Persistence.Postgres.Repos;
using Microsoft.EntityFrameworkCore;    

// context
LibraryDbContext libraryDbContext = new LibraryDbContext();

// repositories
IBookRepository bookRepository = new BookRepository(libraryDbContext);  
IUserRepository userRepository = new UserRepository(libraryDbContext);
ISubscriptionRepository subscriptionRepository = new SubscriptionRepository(libraryDbContext);
ILoanRepository loanRepository = new LoanRepository(libraryDbContext);

// services
BookService bookService = new BookService(bookRepository);
UserService userService = new UserService(userRepository);
SubscriptionService subscriptionService = new SubscriptionService(subscriptionRepository, bookRepository, userRepository);
LoanService loanService = new LoanService(loanRepository, bookRepository, userRepository, subscriptionService);

User? loggedInUser = null;

Dictionary<string, Action> userMenuActions = new Dictionary<string, Action>
{
    { "Get All Books", () => GetAllBooks() },
    { "Get Books By Genre", () => GetAllBooksByGenre() },
    { "Loan Book", () => LoanBook() },
    { "Return Book", () => ReturnBook() },
    { "Loaned Books", () => GetLoanedBooks() },
    { "Loan History", () => GetLoanHistory() },
    { "Subscribe to Book", () => SubscribeToBookAvailability() },
    { "Unsubscribe to Book", () => UnsubscribeToBookAvailability() }
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
    var loans = loanService.GetUserHistory(loggedInUser!.Id);
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

void SubscribeToBookAvailability()
{
    var notAvailableBooks = bookService.GetAll().Where(b => b.IsAvailable == false);
    if (notAvailableBooks.Count() == 0)
    {
        Console.WriteLine("All books are available. No need to subscribe.");
        return;
    }
    var subscribeBookMenu = GetBookMenu(notAvailableBooks, BookAction.Subscribe);
    DisplayMenu(subscribeBookMenu, "Select Book to Subscribe for Availability");
}

void SubscribeToBookAvailabilityAction(Book book)
{
    try
    {
        subscriptionService.Subscribe(book.Id, loggedInUser!.Id);
        Console.WriteLine($"Subscribed to book availability: {book.Title} by {book.Author}.");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error subscribing to book: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Error subscribing to book: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
    }
}

void UnsubscribeToBookAvailability()
{
    var userSubscriptions = subscriptionService.GetUserSubscriptions(loggedInUser!.Id);
    if (userSubscriptions.Count() == 0)
    {
        Console.WriteLine("No subscriptions found.");
        return;
    }
    var subscribedBooks = userSubscriptions.Select(s => s.Book).ToList();
    var unsubscribeBookMenu = GetBookMenu(subscribedBooks, BookAction.Unsubscribe);
    DisplayMenu(unsubscribeBookMenu, "Select Book to Unsubscribe from Availability");
}

void UnsubscribeToBookAvailabilityAction(Book book)
{
    try
    {
        subscriptionService.Unsubscribe(book.Id, loggedInUser!.Id);
        Console.WriteLine($"Unsubscribed from book availability: {book.Title} by {book.Author}.");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error unsubscribing from book: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Error unsubscribing from book: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected error: {ex.Message}");
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
                case BookAction.Subscribe:
                    SubscribeToBookAvailabilityAction(book);
                    break;
                case BookAction.Unsubscribe:
                    UnsubscribeToBookAvailabilityAction(book);
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
    Return,
    Subscribe,
    Unsubscribe
}