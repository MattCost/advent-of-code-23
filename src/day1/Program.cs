// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string? line;
int total = 0;

try
{
    //Pass the file path and file name to the StreamReader constructor
    StreamReader sr = new StreamReader("input.txt");
    //Read the first line of text
    line = sr.ReadLine();
    //Continue to read until you reach end of file
    while (line != null)
    {
        //write the line to console window
        Console.WriteLine(line);
        var firstDigit = line.FirstOrDefault( c => Char.IsAsciiDigit(c) );
        var lastDigit = line.Reverse().FirstOrDefault( c => Char.IsAsciiDigit(c) );
        string number = $"{firstDigit}{lastDigit}";
        total += Convert.ToInt32(number);
        //Read the next line
        line = sr.ReadLine();
    }
    //close the file
    sr.Close();
    Console.WriteLine($"The code is {total}");
}
catch(Exception e)
{
    Console.WriteLine("Exception: " + e.Message);
}
finally
{
    Console.WriteLine("Executing finally block.");
}