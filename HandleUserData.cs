using System;
using System.IO;
using System.Text;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Linq.Expressions;
using YamlDotNet;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class User
{
    public string Username { get; set; }
    public DateTime MeetingDate { get; set; }
    public string Availability { get; set; }
}


public class HandleUserData
{
    string dataBaseFilePath = "E:\\Ny_backup\\JOBB\\AMO\\Timeplanlegger\\Timeplanlegger\\dataBaseFile.yaml";


    public void HandlerUserDataMethod(string dataBaseFilePath, string logFile)
    {
        this.dataBaseFilePath = dataBaseFilePath;
    }

    public void GetUser(string[] usernames)
    {
        DateTime newCalendar = DateTime.Now;
        Console.WriteLine("Who is logging in? ");

        var newUser = Console.ReadLine();
        Console.WriteLine($"{newUser} has logged in!");
        // File.AppendAllText(dataBaseFilePath, newUser + "\n");

        var userDatabase = LoadUserDatabase();
        var existingUser = userDatabase.FirstOrDefault(user => user.Username == newUser);

        if (existingUser == null)
        {
            userDatabase.Add(new User
            {
                Username = newUser,
                MeetingDate = newCalendar,
                Availability = "not available"
            });
            CheckIfUserIsAvailableInCalendar(dataBaseFilePath, newUser);
            SaveUserDatabase(userDatabase);
        }

        else
        {

            Console.WriteLine($"Welcome: {newUser}!");

        }

        if (existingUser != null && existingUser.Availability == "available")
        {
            Console.WriteLine($"Would you like to schedule a new meeting with {newUser}? Y/n? ");
            string userInputInternal = Console.ReadLine().ToString().ToLower();
            if (userInputInternal == "y")
            {
                ScheduleNewMeeting(newUser);
            }
            else
            {
                Console.WriteLine("Here is a list of the users available for a meeting on this date:\n");
                Console.WriteLine(LoadUserDatabase().ToString());

            }
        }

    }

    public void ScheduleNewMeeting(string username)
    {
        try
        {
            var userDatabase = LoadUserDatabase();
            var user = userDatabase.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                Console.WriteLine($"Current meeting date for {username}: {user.MeetingDate.ToShortDateString()}");
                Console.WriteLine("Reschedule another meeting (yyyy-MM-dd): ");
                string inputDate = Console.ReadLine();

                if (DateTime.TryParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDate))
                {
                    Console.WriteLine("Schedule a meeting within the timespan 1-2 hours, example format: 09:10-11:00: ");
                    string rangeTime = Console.ReadLine();

                    if (TryParseTimeRange(rangeTime, out TimeSpan startTime, out TimeSpan endTime))
                    {
                        var addTime = startTime - endTime;
                        user.MeetingDate = newDate;
                        user.Availability = "available";
                        Console.WriteLine($"{addTime} L:100");
                        Console.WriteLine($"New meeting scheduled with {username} on: {newDate.ToShortDateString()} at: {startTime}-{endTime}");
                        SaveUserDatabase(userDatabase);
                    }
                }

                else
                {
                    Console.WriteLine("Not a valid time format!");
                }
            }

            else
            {
                Console.WriteLine($"{username} was not found in the user database!");
            }
        }
        catch (Exception error)
        {
            Console.WriteLine(error);
        }
    }

    private bool TryParseTimeRange(string timeRange, out TimeSpan startTime, out TimeSpan endTime)
    {
        startTime = TimeSpan.MinValue;
        endTime = TimeSpan.MinValue;

        string[] timeParts = timeRange.Split("-");

        if (timeParts.Length == 2 && TimeSpan.TryParseExact(timeParts[0], "hh\\:mm", CultureInfo.InvariantCulture, out startTime) && TimeSpan.TryParseExact(timeParts[1], "hh\\:mm", CultureInfo.InvariantCulture, out endTime))
        {
            return true;
        }

        return false;
    }

    public bool CheckUserAvailability(string username)
    {
        DateTime meetingDate = DateTime.Now;
        var userDatabase = LoadUserDatabase();
        var user = userDatabase.FirstOrDefault(u => u.Username == username);

        // Bug found on Line 145 in if-statement
        if (user != null && user.MeetingDate == meetingDate && user.Availability == "available")
        {
            Console.WriteLine($"{username} is available for a meeting on {meetingDate}! DEBUG: LINE 113");
            return true;
        }

        else
        {
            Console.WriteLine($"{username} is not available!");
            return false;
        }

    }

    private List<User> LoadUserDatabase()
    {
        try
        {
            if (File.Exists(dataBaseFilePath))
            {
                var yamlDeserializer = new DeserializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .Build();

                var yamlContent = File.ReadAllText(dataBaseFilePath);
                return yamlDeserializer.Deserialize<List<User>>(yamlContent) ?? new List<User>();
            }

            else
            {
                return new List<User>();
            }
        }

        catch (Exception error)
        {
            Console.WriteLine(error);
            return new List<User>();
        }
    }

    private void SaveUserDatabase(List<User> userDatabase)
    {
        try
        {
            var yamlSerializer = new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

            var yamlContent = yamlSerializer.Serialize(userDatabase);

            File.WriteAllText(dataBaseFilePath, yamlContent);
        }

        catch (Exception error)
        {
            Console.WriteLine(error);
        }
    }

    private bool CheckIfUserIsAvailableInCalendar(string dataBaseFilePath, string username)
    {
        User available = new User();

        if (available.Availability != "available")
        {
            Console.WriteLine($"{username} is not available for a meeting!");
        }

        else
        {
            DateTime meetingDate = DateTime.Now;
            Console.WriteLine($"{username} is available for a meeting on: {meetingDate.ToString("yyyy-MM-dd HH:mm:ss")} DEBUG: LINE 205");
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
                Console.WriteLine($"Dupe found at {readAllUsers.ToString()}");
                File.AppendAllText(logFilePath, readAllUsers.ToString() + "\t\n");
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