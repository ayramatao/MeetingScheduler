using System;
using System.IO;
using System.Text;
using System.Globalization;

public class HandleUserData {
    bool hasUsername;
    float timeOfDay;
    
    public string[] dateArray = {};

    public void GetUser(string dataBaseFilePath, string[] usernames, string logFile) 
    {
        string existingUsers = File.ReadAllText(dataBaseFilePath);
        DateTime newCalendar = new DateTime(2024, 1, 1, new GregorianCalendar());
        Console.WriteLine("Who is logging in? ");
        var newUser = Console.ReadLine();
        if(existingUsers != "") 
        {
            Console.WriteLine($"{existingUsers} is already logged in!");
            File.AppendAllText(dataBaseFilePath, newUser + "\n");
        }
        else 
        {
            Console.WriteLine($"{newUser} has logged in!");
        }

        try 
        {
            using(StreamWriter writer = new StreamWriter(dataBaseFilePath, true)) 
            {
                foreach(string username in usernames) {
                    writer.WriteLine(username);
                }
            }
            // read the usernames from the db-file
            string[] allUsers = File.ReadAllLines(dataBaseFilePath);

            // Use a foreach loop to iterate over all usernames
            foreach(string users in allUsers) 
            {
                bool isUserAvailable = CheckIfUserIsAvailableInCalendar(users);
               
            }
            Console.WriteLine($"{allUsers} is available for a meeting on {newCalendar.Date}"); 
            Console.WriteLine(newCalendar.ToLocalTime());
        }

        catch(Exception error) 
        {
            using(FileStream IOStream = File.Create(logFile)) {
                Byte[] title = new UTF8Encoding(true).GetBytes("Log file");
                IOStream.Write(title, 0, title.Length);
                byte[] author = new UTF8Encoding(true).GetBytes("System");
                IOStream.Write(author, 0, author.Length);
                byte[] errorMessage = new UTF8Encoding(true).GetBytes("Errors occured");
                IOStream.Write(errorMessage, 0 ,errorMessage.Length);
                Console.WriteLine(error);
            }
        }
    }

    private bool CheckIfUserIsAvailableInCalendar(string username) 
    {
        bool available = true;
        if(available == false) {
            Console.WriteLine($"Not available for a meeting!");
        }
        else {
            Console.WriteLine("Available");
        }
        return true;
    }
}