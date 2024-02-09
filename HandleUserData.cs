using System;
using System.IO;
using System.Text;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Linq.Expressions;
using YamlDotNet;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Reflection.Metadata;

public class User
{
    public string? Username { get; set; }
    public DateTime MeetingDate { get; set; }
    public string? Availability { get; set; }

    public TimeSpan MeetingDuration { get; set; }
}


public class HandleUserData
{
    string dataBaseFilePath = "E:\\Ny_backup\\JOBB\\AMO\\Timeplanlegger\\Timeplanlegger\\dataBaseFile.yaml";


    public void HandlerUserDataMethod(string dataBaseFilePath)
    {
        this.dataBaseFilePath = dataBaseFilePath;
    }

    public void GetUser(string[] usernames)
    {
        DateTime newCalendar = DateTime.Now;
        Console.WriteLine("Who is logging in? ");
        TimeSpan newRangeTime = new TimeSpan();

        string? newUser = Console.ReadLine();
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
                Availability = "not available",
                MeetingDuration = newRangeTime
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
            string? userInputInternal = Console.ReadLine().ToLower();
            if (userInputInternal == "y")
            {
                ScheduleNewMeeting(newUser);
            }
            else
            {
                Console.WriteLine("Here is a list of the users available for a meeting on this date:\n");

                string? output = File.ReadAllText(dataBaseFilePath);
                Console.WriteLine(output);
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Current meeting date for {username}: {user.MeetingDate.ToShortDateString()}");
                Console.WriteLine("Reschedule another meeting (yyyy-MM-dd): ");
                string? inputDate = Console.ReadLine();

                if (DateTime.TryParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDate))
                {
                    Console.WriteLine("Schedule a meeting within the timespan 1-2 hours, example format: 09:10-11:00: ");
                    string? rangeTime = Console.ReadLine();

                    if (TryParseTimeRange(rangeTime, out TimeSpan startTime, out TimeSpan endTime))
                    {

                        TimeSpan meetingTime = endTime - startTime;
                        Console.WriteLine(meetingTime);

                        if (meetingTime.TotalHours <= 2 && meetingTime.TotalHours > 0)
                        {
                            DateTime newMeetingDateTime = newDate.Date + startTime;

                            user.MeetingDate = newDate.Add(startTime);
                            user.Availability = "available";
                            user.MeetingDuration = meetingTime;
                            Console.WriteLine($"New meeting scheduled with {username} on: {newDate.ToShortDateString()} at: {startTime}-{endTime}");

                            int userIndex = userDatabase.FindIndex(u => u.Username == username);

                            if (userIndex != -1)
                            {
                                userDatabase[userIndex] = user;

                                SaveUserDatabase(userDatabase);
                            }

                            else
                            {
                                Console.WriteLine($"Error: User {username} was not found in the database!");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Meeting duration must be between 1 and 2 hours!");
                        }
                    }
                }

                else
                {
                    Console.WriteLine("Not a valid time format!");
                }
                Console.ResetColor();
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
        DateTime localScopeMeetingDate = DateTime.Now;
        var userDatabase = LoadUserDatabase();
        var user = userDatabase.FirstOrDefault(u => u.Username == username);        // Bug found on Line 145 in if-statement
        if (user != null && user.MeetingDate > localScopeMeetingDate && user.Availability == "available")
        {
            Console.WriteLine($"{username} is available for a meeting on {user.MeetingDate} with an expected duration of {user.MeetingDuration}!");
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

    private bool CheckIfUserIsAvailableInCalendar(string dataBaseFilePathReference, string username)
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
                Console.WriteLine($"Dupe found at {string.Join(", ", readAllUsers)}");
                File.AppendAllText(logFilePath, string.Join("\t\n", readAllUsers) + "\t\n");
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