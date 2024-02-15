using System;
using System.IO;


public class TestDynamicIO
{


    public void testMethod()
    {
        Console.WriteLine("Pass a filepath: ");
        string? databaseInputPath = Console.ReadLine();
        Console.WriteLine("Pass a filename: ");
        string? databaseInputFile = Console.ReadLine();

        try
        {
            string fullPath = Path.Combine(databaseInputPath, databaseInputFile);
            using (StreamWriter newWriter = new StreamWriter(fullPath))
            {
                newWriter.WriteLine("L1: Test 1 passed");
                newWriter.WriteLine("L2: Test 2 passed");
            }
            Console.WriteLine("Successfully wrote to given files!");


            string fileContent = File.ReadAllText(databaseInputPath + "\\" + databaseInputFile);
            Console.WriteLine($"Successfully read the filepath and files with output: {fileContent}");
        }

        catch (IOException e)
        {
            Console.WriteLine("L1: Test 1 failed");
            Console.WriteLine("L2: Test 2 failed");
            Console.WriteLine(e);
        }

    }

}

