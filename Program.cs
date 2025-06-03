using LibraryManagementSystem.Models;

User? currentUser = null;

Dictionary<string, Action> userMenuActions = new Dictionary<string, Action>
{
    { "Get All Books", () => Console.WriteLine("Book listing not implemented.") },
    { "Get Books By Genre", () => Console.WriteLine("Book listing by genre not implemented.") },
    { "Reserve Book", () => Console.WriteLine("Book reservation not implemented.") },
    { "Notify Me When Book Is Available", () => Console.WriteLine("Notidications aren't implemented.") },
    { "Return Book", () => Console.WriteLine("Book returning not implemented.") },
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
                // Console.WriteLine("\nPress any key to return back.");
                // Console.ReadKey();
                break;
            case ConsoleKey.Q:
                return;
        }
    }
    while (true);
}

