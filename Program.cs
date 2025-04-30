using LibraryManagementSystem.Services;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Interfaces;

ILibraryDataService jsonDataService = new JsonDataService(@"Data\books.json");
LibraryService libraryService = new LibraryService(jsonDataService);

List<User> users = new List<User>
{
    new User
    {
        Id = 1,
        Name = "John Doe",
        Email = @"Data\user1@email.txt",
        PhoneNumber = @"Data\user1Phone.txt",
        Login = "user1",
        Password = "user1",
    },
    new User
    {
        Id = 2,
        Name = "Jane Doe",
        Email = @"Data\user2@email.txt",
        PhoneNumber = @"Data\user2Phone.txt",
        Login = "user2",
        Password = "user2",
    }
};

User? currentUser = null;

Dictionary<string, Action> userMenuActions = new Dictionary<string, Action>
{
    { "Get All Books", ListAllBooks },
    { "Get Books By Genre", ListBooksByGenre },
    { "Reserve Book", ReserveBook },
    { "Notify Me When Book Is Available", NotifyMe },
    { "Return Book", ReturnBook },
};

Dictionary<string, Action> adminMenuActions = new Dictionary<string, Action>
{
    { "Add book", () => Console.WriteLine("Add book not implemented.") },
    { "Remove book", () => Console.WriteLine("Remove book not implemented.")}
};

Dictionary<string, Action> loginMenuActions = new Dictionary<string, Action>
{
    { 
        "Login as User", 
        () => LogUser()
    },
    { 
        "Login as Admin", 
        () => DisplayMenu(adminMenuActions, "Admin Menu") 
    },
};

void LogUser()
{
    Console.WriteLine("Enter your login:");
    string? login = Console.ReadLine();
    Console.WriteLine("Enter your password:");
    string? password = Console.ReadLine();

    var user = users.FirstOrDefault(u => u.Login == login && u.Password == password);
    if (user != null)
    {
        currentUser = user;
        DisplayMenu(userMenuActions, "User Menu" + $"Welcome, {user.Name}!");
    }
    else
    {
        Console.WriteLine("Invalid login or password.");
        Console.ReadKey();
    }
}

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
                // Console.WriteLine("\nPress any key to return back.");
                // Console.ReadKey();
                break;
            case ConsoleKey.Q:
                return;
        }
    }
    while (true);
}

void ListAllBooks()
{
    foreach (var book in libraryService.GetAllBooks())
    {
        Console.WriteLine(book.ToString());
    }
    Console.ReadKey();
}

void ListBooksByGenre()
{
    foreach (var genre in libraryService.GetBooksByGenre())
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine($"Genre: {genre.Key}");
        Console.ResetColor();
        foreach (var book in genre.Value)
        {
            Console.WriteLine(book.ToString());
        }
    }
    Console.ReadKey();
}

void ReserveBook()
{
    Console.WriteLine("Enter the ID of the book you want to reserve:");
    string? input = Console.ReadLine();
    if (int.TryParse(input, out int bookId))
    {
        try
        {
            libraryService.ReserveBook(bookId);
            Console.WriteLine("Book reserved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }
    Console.ReadKey();
}

void ReturnBook()
{
    Console.WriteLine("Enter the ID of the book you want to return:");
    string? input = Console.ReadLine();
    if (int.TryParse(input, out int bookId))
    {
        try
        {
            libraryService.ReturnBook(bookId);
            Console.WriteLine("Book returned successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }
    Console.ReadKey();
}

void NotifyMe()
{
    if (currentUser == null) { return; }
    
    Console.WriteLine("Enter the ID of the book you want to be notified about:");
    string? input = Console.ReadLine();
    if (int.TryParse(input, out int bookId))
    {
        try
        {
            Console.WriteLine("Email, SMS, or both? (E/S/B):");
            string? choice = Console.ReadLine()?.ToUpper();
            switch (choice)
            {
                case "E":
                    libraryService.NotifyMeWhenBookIsAvailable(bookId, currentUser.NotifyViaEmail);
                    break;
                case "S":
                    libraryService.NotifyMeWhenBookIsAvailable(bookId, currentUser.NotifyViaPhone);
                    break;
                case "B":
                    libraryService.NotifyMeWhenBookIsAvailable(bookId, currentUser.NotifyViaEmail);
                    libraryService.NotifyMeWhenBookIsAvailable(bookId, currentUser.NotifyViaPhone);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter E, S, or B.");
                    return;
            }
            Console.WriteLine("You will be notified when the book is available.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }
    Console.ReadKey();
}