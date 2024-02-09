using System;
using System.IO;
using System.Globalization;
using System.Text;

string dataBaseFilePath = "E:\\Ny_backup\\JOBB\\AMO\\Timeplanlegger\\Timeplanlegger\\dataBaseFile.yaml";

string[] usernames = { };

bool isRunning = true;

DateTime currentDatetime = DateTime.Now;


// string newLogFile = "E:\\Ny_backup\\JOBB\\AMO\\Timeplanlegger\\Timeplanlegger\\log.txt";

// while-loop that runs when the program is interactive
HandleUserData handleUser = new HandleUserData();

while (isRunning)
{
    Console.WriteLine("Program is running...\nHere is a list of all the commands you can run in this CLI: ");
    Console.WriteLine($"Current date: {currentDatetime.ToShortDateString()} current time: {currentDatetime.ToShortTimeString()} ");
    Console.WriteLine("1: Log in as existing user");
    Console.WriteLine("2: Check user availability");
    Console.WriteLine("3: Schedule a meeting");
    Console.WriteLine("4: Exit the program!");

    string? userInput = Console.ReadLine();

    switch (userInput)
    {
        case "1":
            Console.Clear();
            handleUser.GetUser(usernames);
            break;
        case "2":
            Console.Clear();
            Console.WriteLine("Enter the name of the user you want to check if available."); string? userNameToCheck = Console.ReadLine();
            handleUser.CheckUserAvailability(userNameToCheck);
            break;
        case "3":
            Console.Clear();
            Console.WriteLine("Enter the name of the user you want to schedule a meeting with: ");
            string? scheduleMeetingWithUser = Console.ReadLine();
            handleUser.ScheduleNewMeeting(scheduleMeetingWithUser);
            break;
        case "4":
            Console.Clear();
            Console.WriteLine("Exiting the program...");
            Environment.Exit(0);
            break;
        default:
            Console.Clear();
            Console.WriteLine("Invalid option selected. Please use numbers [1-4]");
            break;
    }

    Console.WriteLine("Go back to menu");

    if (Console.ReadLine().Contains(ConsoleKey.Escape.ToString()))
    {
        isRunning = false;
        break;
    }
    Console.Clear();
}
