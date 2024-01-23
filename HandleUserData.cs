using System;
using System.IO;
using System.Text;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Linq.Expressions;
using YamlDotNet;

public class HandleUserData
{
    bool hasUsername;
    float timeOfDay;

    public string[] dateArray = { };

    public async void GetUser(string dataBaseFilePath, string[] usernames, string logFile)
    {
        DateTime newCalendar = DateTime.Now;
        Console.WriteLine("Who is logging in? ");

        var newUser = Console.ReadLine();
        Console.WriteLine($"{newUser} has logged in!");
        File.AppendAllText(dataBaseFilePath, newUser + "\n");

        try
        {
            using (StreamWriter writer = new StreamWriter(dataBaseFilePath, true))
            {
                foreach (string username in usernames)
                {
                    writer.WriteLine(username);
                }
            }
            // read the usernames from the db-file
            string[] allUsers = File.ReadAllLines(dataBaseFilePath);
            // bool isUserAvailable = CheckIfUserIsAvailableInCalendar(allUsers[1]);
            /*
            foreach (string user in allUsers)
            {
                Console.WriteLine("DEBUG: LINE 40: CHECKING SIZE OF DATABASEFILE USING ARRAY LENGHT INDEXING");
                Console.WriteLine(allUsers.Length);
                string formatString = string.Join("", user);
                Console.WriteLine($"{user} is available for a meeting on {newCalendar} DEBUG: LINE 41");
            }
            */
            Console.WriteLine($"{allUsers.AsQueryable()} is available for a meeting on {newCalendar} DEBUG: LINE 45");
            var checkAvailabilty = CheckUserAvailability(dataBaseFilePath, newUser);
            Console.WriteLine(checkAvailabilty);
            var checkCalendar = CheckIfUserIsAvailableInCalendar(dataBaseFilePath, newUser);
            Console.WriteLine(checkCalendar);
            //Console.WriteLine($"{allUsers.Split("")} is available for a meeting on {newCalendar}"); 
            //Console.WriteLine(newCalendar.ToLocalTime());
        }

        catch (Exception error)
        {
            using (FileStream IOStream = File.Create(logFile))
            {
                Byte[] title = new UTF8Encoding(true).GetBytes("Log file");
                IOStream.Write(title, 0, title.Length);
                byte[] author = new UTF8Encoding(true).GetBytes("System");
                IOStream.Write(author, 0, author.Length);
                byte[] errorMessage = new UTF8Encoding(true).GetBytes("Errors occured");
                IOStream.Write(errorMessage, 0, errorMessage.Length);
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

            if (userData != null)
            {
                string[] dataParts = userData.Split("|");

                if (dataParts.Length >= 3 && dataParts[1] == meetingDate.ToString("yyyy-MM-dd") && dataParts[2] == "available")
                {
                    string userDataString = string.Join("", username);
                    Console.WriteLine($"{userDataString} is available for a meeting on: {meetingDate.ToShortDateString()} DEBUG: LINE 81");
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
        catch (Exception error)
        {
            Console.WriteLine(error);
        }
        return true;
    }
    private bool CheckIfUserIsAvailableInCalendar(string dataBaseFilePath, string username)
    {
        bool available = true;

        if (!available)
        {
            Console.WriteLine($"{username} is not available for a meeting!");
        }
        else
        {
            DateTime meetingDate = DateTime.Now;
            Console.WriteLine($"{username} is available for a meeting on: {meetingDate.ToString("yyyy-MM-dd HH:mm:ss")} DEBUG: LINE 114");
        }

        return true;
    }

    public void CheckForDuplicateStrings(string dataBaseFilePath)
    {
        string logFilePath = "E:\\Ny_backup\\JOBB\\AMO\\Timeplanlegger\\Timeplanlegger\\log.txt";

        try
        {
            // read all strings line by line
            string[] readAllUsers = File.ReadAllLines(dataBaseFilePath);
            // use a boolean to check for dupes
            bool hasDuplicate = HasDuplicates(readAllUsers);

            if (hasDuplicate)
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
        catch (Exception error)
        {
            Console.WriteLine(error);
        }
    }
    private bool HasDuplicates(string[] dupeArray)
    {
        // use a hashset to check for dupes!
        var hashSet = new HashSet<string>();

        // loop through the hashset with a foreach loop
        foreach (string element in dupeArray)
        {
            // add a new element to the hashset, if it alrady exists.
            if (!hashSet.Add(element))
            {
                return true;
            }
        }
        // Found no dupes, return false!
        return false;
    }
}