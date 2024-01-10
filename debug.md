```cs
    private string GetNewUsernames(string[] newUser) 
    {
        Console.WriteLine("Who is logging in? ");
        var loggedIn = Console.ReadLine();
        Console.WriteLine($"{loggedIn} has logged in!");
        return loggedIn;
    }

```