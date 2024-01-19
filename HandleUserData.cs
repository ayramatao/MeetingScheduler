using System;
using System.IO;
using System.Text;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Linq.Expressions;



public class HandleUserData {
    bool hasUsername;
    float timeOfDay;
    
    public string[] dateArray = {};

    public void GetUser(string dataBaseFilePath, string[] usernames, string logFile) 
    {
        DateTime newCalendar = DateTime.Now;
        Console.WriteLine("Who is logging in? ");
        
        var newUser = Console.ReadLine();
        Console.WriteLine($"{newUser} has logged in!");

        try 
        {
            using(StreamWriter writer = new StreamWriter(dataBaseFilePath, true)) 
            {
                foreach(string username in usernames) 
                {
                    writer.WriteLine(username);
                }
            }
            // read the usernames from the db-file
            string allUsers = File.ReadAllText(dataBaseFilePath);
            // bool isUserAvailable = CheckIfUserIsAvailableInCalendar(allUsers[1]);
            Console.WriteLine($"{allUsers.Split("")} is available for a meeting on {newCalendar}"); 
            //Console.WriteLine(newCalendar.ToLocalTime());
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

    public bool CheckUserAvailability(string dataBaseFilePath, string username) 
    {
        DateTime meetingDate = DateTime.Now;
        try 
        {
            string[] allUserData = File.ReadAllLines(dataBaseFilePath);
            string userData = Array.Find(allUserData, line => line.StartsWith(username));

            if(userData != null) 
            {
                string[] dataParts = userData.Split("|");

                if(dataParts.Length >= 3 && dataParts[1] == meetingDate.ToString("yyyy-MM-dd") && dataParts[2] == "available") 
                {
                    Console.WriteLine($"{username} is available for a meeting on: {meetingDate.ToShortDateString()}");
                }
                else 
                {
                    Console.WriteLine($"no users available on this date");
                    return false;
                }

            }
            else 
            {
                Console.WriteLine($"{username} is not available!");
                return false;
            }

        }
        catch(Exception error)
        {
            Console.WriteLine(error);
        }
    return true;
}
    private bool CheckIfUserIsAvailableInCalendar(string dataBaseFilePath, string username) 
    {
        bool available = true;

        if(!available) 
        {
            Console.WriteLine($"{username} is not available for a meeting!");
        }
        else 
        {
            DateTime meetingDate = DateTime.Now;
            Console.WriteLine($"{username} is available for a meeting on: {meetingDate.ToString("yyyy-MM-dd HH:mm:ss")}");
        }

        return true;
    }

    public void CheckForDuplicateStrings(string dataBaseFilePath) 
    {
        string logFilePath = "D:\\Ny_backup\\JOBB\\AMO\\Timeplanlegger\\Timeplanlegger\\log.txt";

        try 
        {   
            // read all strings line by line
            string[] readAllUsers = File.ReadAllLines(dataBaseFilePath);
            // use a boolean to check for dupes
            bool hasDuplicate = HasDuplicates(readAllUsers);

            if(hasDuplicate) 
            {
                Console.WriteLine($"Dupe found at {readAllUsers[0]}");
                File.AppendAllText(logFilePath, readAllUsers[0] + "\t");
                Console.WriteLine("Check log!");
            }
            else 
            {
                Console.WriteLine("No dupes found in database file!");
            }
        }
        catch(Exception error) 
        {   
            Console.WriteLine(error);
        }
    }
    private bool HasDuplicates(string[] dupeArray) 
    {
        // use a hashset to check for dupes!
        var hashSet = new HashSet<string>();

        // loop through the hashset with a foreach loop
        foreach(string element in dupeArray) 
        {
            // add a new element to the hashset, if it alrady exists.
            if(!hashSet.Add(element)) 
            {
                return true;
            }
        }
        // Found no dupes, return false!
        return false;
    }
}