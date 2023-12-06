// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

string? line;
double total = 0;

try
{
    StreamReader sr = new StreamReader("../../inputs/input.txt");
    line = sr.ReadLine();
    while (line != null)
    {
        Console.WriteLine(line);

        var firstDigit = GetFirstDigit(line);
        var lastDigit = GetLastDigit(line);
        string numberStr = $"{firstDigit}{lastDigit}";
        double number = Convert.ToDouble(numberStr);
        total += number;
        // Console.WriteLine($"First: {firstDigit} Last: {lastDigit}. NumberStr: {numberStr} Number: {number} Updated Total: {total}");

        line = sr.ReadLine();
    }
    sr.Close();
    Console.WriteLine($"The code is {total}");
}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

int GetFirstDigit(string line)
{
    // Valid digits include 0-9 or [one-nine]
    // if first char is a digit, return that
    if (Char.IsAsciiDigit(line[0]))
    {
        return Convert.ToInt32($"{line[0]}");
    }
    
    var numberRegex = new Regex(@"([1-9])", RegexOptions.Compiled);
    var numberMatches = numberRegex.Matches(line);
    if(numberMatches.Count >0)
    {
        Console.WriteLine($"numberMatches found Index:{numberMatches.First().Index} Value:{numberMatches.First().Value}");
    }

    string[] words = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    var wordMatches = words.Select( word => new { word, index=line.IndexOf(word)}).Where(match => match.index != -1);
    var firstWordMatch = wordMatches.Where( match => match.index == wordMatches.Min( match => match.index));
    if (numberMatches.Count == 0)
    {
        return ConvertToInt(firstWordMatch.First().word);
    }
    if (!firstWordMatch.Any())
    {
        return Convert.ToInt32(numberMatches.First().Value);
    }
    if (firstWordMatch.First().index <  numberMatches.First().Index)
    {
        return ConvertToInt(firstWordMatch.First().word);
    }
    else
    {
        return Convert.ToInt32(numberMatches.First().Value);
    }
 }

int GetLastDigit(string line)
{
    
    var numberRegex = new Regex(@"([1-9])", RegexOptions.Compiled);
    var numberMatches = numberRegex.Matches(line);

    string[] words = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    var wordMatches = words.Select( word => new { word, index=line.LastIndexOf(word)}).Where(match => match.index != -1);
    var lastWordMatch = wordMatches.Where( match => match.index == wordMatches.Max( match => match.index));


    if (numberMatches.Count == 0)
    {
        return ConvertToInt(lastWordMatch.First().word);
    }
    if (!lastWordMatch.Any())
    {
        return Convert.ToInt32(numberMatches.Last().Value);
    }
    if (lastWordMatch.First().index > numberMatches.Last().Index)
    {
        return ConvertToInt(lastWordMatch.First().word);
    }
    else
    {
        return Convert.ToInt32(numberMatches.Last().Value);
    }
}

int ConvertToInt(string word)
{
    switch (word)
    {
        case "one":
            return 1;
        case "two":
            return 2;
        case "three":
            return 3;
        case "four":
            return 4;
        case "five":
            return 5;
        case "six":
            return 6;
        case "seven":
            return 7;
        case "eight":
            return 8;
        case "nine":
            return 9;
        default:
            return 0;
    }
}