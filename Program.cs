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

            string dataBaseFilePath = "E:\\Ny_backup\\JOBB\\AMO\\Timeplanlegger\\Timeplanlegger\\dataBaseFile.yaml";

            string[] usernames = { };


            // string newLogFile = "E:\\Ny_backup\\JOBB\\AMO\\Timeplanlegger\\Timeplanlegger\\log.txt";

            HandleUserData handleUser = new HandleUserData();
            handleUser.CheckForDuplicateStrings(dataBaseFilePath);
            handleUser.GetUser(usernames);
        }
    }
}
