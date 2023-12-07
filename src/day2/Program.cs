// See https://aka.ms/new-console-template for more information
using System.Diagnostics.Contracts;

Console.WriteLine("Hello, World!");

var gameRecords = new Dictionary<int,List<Dictionary<Colors, int>>>();

string? line;
Dictionary<Colors, int> MAX = new()
{
    {Colors.Red,12},
    {Colors.Green,13},
    {Colors.Blue,14},
};

int total = 0;
try
{
    StreamReader sr = new StreamReader("input.txt");
    line = sr.ReadLine();
    while (line != null)
    {
        Console.WriteLine(line);

        var gameId = ExtractGameId(line);
        gameRecords[gameId] = ExtractGameRecords(line);
        
        line = sr.ReadLine();
    }
    sr.Close();

    //part 1
    // foreach(var kvp in gameRecords)
    // {
    //     if(GameIsPossible(kvp.Value))
    //     {
    //         total += kvp.Key;
    //     }
    // }

    // Console.WriteLine($"The code is {total}");
    foreach(var kvp in gameRecords)
    {
        var power = CalculateGamePower(kvp.Value);
        total += power;
    }
    Console.WriteLine($"The code is {total}");

}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

int CalculateGamePower(List<Dictionary<Colors, int>> value)
{
    var red = value.Where( round => round.ContainsKey(Colors.Red)).Max( round => round[Colors.Red]);
    var green = value.Where( round => round.ContainsKey(Colors.Green)).Max( round => round[Colors.Green]);
    var blue = value.Where( round => round.ContainsKey(Colors.Blue)).Max( round => round[Colors.Blue]);
    
    return red * green*blue;
}

bool GameIsPossible(List<Dictionary<Colors, int>> gameRounds)
{
    foreach(var round in gameRounds)
    {
        if(round.ContainsKey(Colors.Red) && round[Colors.Red] > MAX[Colors.Red]) return false;
        if(round.ContainsKey(Colors.Green) && round[Colors.Green] > MAX[Colors.Green]) return false;
        if(round.ContainsKey(Colors.Blue) && round[Colors.Blue] > MAX[Colors.Blue]) return false;
    }
    return true;
}

//Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
List<Dictionary<Colors, int>> ExtractGameRecords(string line)
{
    Console.WriteLine($"Entering ExtractGameRecords:{line}");
    var output = new List<Dictionary<Colors, int>>();
    
    var split = line.Split(':');
    var recordsString = split[1].Trim().Split(';');
    foreach(var recordString in recordsString)
    {
        output.Add(ExtractGameRecord(recordString));
    }


    return output;
}

//3 blue, 4 red
Dictionary<Colors, int> ExtractGameRecord(string recordString)
{
    Console.WriteLine($"Entering ExtractGameRecord:{recordString}");
    var output = new Dictionary<Colors, int>();
    var cubes = recordString.Trim().Split(',');
    foreach(var cube in cubes)
    {
         output[ExtractColor(cube)] = ExtractCount(cube);
    }
    return output;
}

//3 blue
int ExtractCount(string cube)
{
    Console.WriteLine($"Entering ExtractCount:{cube}");

    var split = cube.Trim().Split(' ');
    return Convert.ToInt32(split[0]);
}

Colors ExtractColor(string cube)
{
    Console.WriteLine($"Entering ExtractColor:{cube}");

    var split = cube.Trim().Split(' ');
    return (Colors)Enum.Parse(typeof(Colors), split[1], true);
}

//Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
int ExtractGameId(string line)
{
    Console.WriteLine($"Entering ExtractGameId:{line}");

    var split = line.Trim().Split(':');
    return Convert.ToInt32(split[0][5..]);
}