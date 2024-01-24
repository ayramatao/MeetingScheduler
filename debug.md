```cs
    private string GetNewUsernames(string[] newUser) 
    {
        Console.WriteLine("Who is logging in? ");
        var loggedIn = Console.ReadLine();
        Console.WriteLine($"{loggedIn} has logged in!");
        return loggedIn;
    }

```


# DynamicDatePlanner scrapped class

````cs
using System;
using System.Windows.Forms;
using System.Globalization;
using System.Reflection.Emit;

public class DynamicDatePlanner : Form 
{
    public DatePlanner() 
    {
        InitalizeComponent();
    }

    private void GenerateCalendar(int year, int month) 
    {
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        int daysInMonth = DateTime.DaysInMonth(year, month);

        tableLayoutPanel.Controls.Clear();
        tableLayoutPanel.RowStyles.Clear();

        for(int i = 0; i < (int)firstDayOfMonth.DayOfWeek; i++) 
        {
            tableLayoutPanel.Controls.Add(new Label(), i, 0);
        }

        int currentRow = 0;

        for(int day = 1; day <= daysInMonth; day++) 
        {
            Label label = new Label 
            {
                Text = day.ToString(),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            tableLayoutPanel.Controls.Add(label, (currentRow + (int)firstDayOfMonth.DayOfWeek) % 7, currentRow / 7);
            currentRow++;

            if(currentRow % 7 == 0) 
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }


        }
    }

    private void monthCalendarDateChanged(object sender, DateRangeEventArgs e) 
    {
        GenerateCalendar(e.Start.Year, e.Start.Month);
    }

    [STAThread]
    static void Main() 
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new DynamicDatePlanner());
    }
}


````

````cs
        string existingUsers = File.ReadAllText(dataBaseFilePath);

        if(existingUsers != "") 
        {
            Console.WriteLine($"{existingUsers} is already logged in!");
        }
        else 
        {
            Console.WriteLine($"{newUser} has logged in!");
            File.AppendAllText(dataBaseFilePath, newUser + "\n");
        }

````
# GetUser method
````cs
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

````