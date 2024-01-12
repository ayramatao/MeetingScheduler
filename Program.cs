using System;
using System.IO;
using System.Globalization;
using System.Text;


namespace Timeplanlegger
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string dataBaseFilePath = "D:\\Ny_backup\\JOBB\\AMO\\Timeplanlegger\\Timeplanlegger\\dataBaseFile.txt";
            
            string[] usernames = {};
            
            string newLogFile = "D:\\Ny_backup\\JOBB\\AMO\\Timeplanlegger\\Timeplanlegger\\log.txt";

            HandleUserData handleUser = new HandleUserData();
            handleUser.GetUser(dataBaseFilePath, usernames, newLogFile);

        }
    }
}


