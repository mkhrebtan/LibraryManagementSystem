using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using LibraryManagementSystem.Services;

LibraryDbContext libraryDbContext = new LibraryDbContext();
BookService bookService = new BookService(libraryDbContext);
UserService userService = new UserService(libraryDbContext);

Dictionary<string, Action> userMenuActions = new Dictionary<string, Action>
{
    { "Get All Books", () => GetAllBooks() },
    { "Get Books By Genre", () => GetAllBooksByGenre() },
    { "Reserve Book", () => Console.WriteLine("Book reservation not implemented.") },
    { "Notify Me When Book Is Available", () => Console.WriteLine("Notidications aren't implemented.") },
    { "Return Book", () => Console.WriteLine("Book returning not implemented.") },
};

Dictionary<string, Action> adminMenuActions = new Dictionary<string, Action>
{
    { "Add book", () => AddBook() },
    { "Remove book", () => DisplayMenu(GetAllBooksForRemove(), "Select Book to Remove") },
    { "Add user", () => AddUser() },
    { "Remove user", () => RemoveUser() },
    { "Get All Books", () => GetAllBooks() },
    { "Get All Users", () => GetAllUsers() },
};

User? loggedInUser = null;
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

#region Admin

void AddBook()
{
    Console.WriteLine("Book adding not implemented");
}

Dictionary<string, Action> GetAllBooksForRemove()
{
    var books = bookService.GetAllBooks();
    Dictionary<string, Action> bookRemovalActions = new Dictionary<string, Action>();
    foreach (var book in books)
    {
        bookRemovalActions.Add(book.ToString(), () =>
        {
            RemoveBook(book);
        });
    }
    return bookRemovalActions;
}

void RemoveBook(Book bookToRemove)
{
    try
    {
        bookService.RemoveBook(bookToRemove);
        Console.WriteLine($"Book removed: {bookToRemove.Title} by {bookToRemove.Author}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error removing book: {ex.Message}");
    }
}

void AddUser()
{
    var users = userService.GetAllUsers();
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
        userService.AddUser(
            new User 
            { 
                Name = $"User{userId}", 
                Email = $"user{userId}@email",
                PhoneNumber = $"123456789{userId}",
                Login = $"user{userId}",
                Password = $"password{userId}"
            }
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
    var user = userService.GetUserById(userId);
    if (user == null)
    {
        Console.WriteLine("User not found.");
        return;
    }

    try
    {
        userService.RemoveUser(user);
        Console.WriteLine($"User removed: {user.Name}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Error removing user: {ex.Message} It may not exist.");
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
    var users = userService.GetAllUsers();
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
    var books = bookService.GetBooksByGenre(genre);
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

#endregion

Dictionary<string, Action> GetAllUsersForLogIn()
{
    var users = userService.GetAllUsers();
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