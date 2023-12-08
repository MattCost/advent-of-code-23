using System.Numerics;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

Regex NumberRegex = new Regex(@"([0-9]+)", RegexOptions.Compiled);
Regex SymbolRegex = new Regex(@"([^0-9.])", RegexOptions.Compiled);

string ? line;
List<EngineSchematicEntry> EngineSchematic = new();
int row=0;
try
{
    StreamReader sr = new StreamReader("input.txt");
    line = sr.ReadLine();
    while (line != null)
    {
        Console.WriteLine(line);

        var entries = ExtractEntries(line,row++);
        EngineSchematic.AddRange(entries);

        
        line = sr.ReadLine();
    }
    sr.Close();

    var numbers = EngineSchematic.Where( entry => entry.IsNumber);
    Console.WriteLine($"We have {numbers.Count()} numbers");

    var symbols = EngineSchematic.Where( entry => entry.IsSymbol);
    Console.WriteLine($"We have {symbols.Count()} symbols");

    var usedNumbers = symbols.SelectMany( symbolEntry => numbers.Where( numberEntry => numberEntry.IsAdjacent(symbolEntry))).Distinct();
    Console.WriteLine($"We have {usedNumbers.Count()} usedNumbers");

    // foreach(var usedNumber in usedNumbers)
    // {
    //     Console.WriteLine($"Used Number:{usedNumber.Number} X:{usedNumber.X} Y:{usedNumber.Y}");
    // }

    var total = usedNumbers.Sum( entry => entry.Number);
    Console.WriteLine($"The code is {total}");

    var gearRatio = 0;
    var possibleGears = EngineSchematic.Where( entry => entry.IsGearSymbol);
    Console.WriteLine($"There are {possibleGears.Count()} possible gears");
    foreach(var possibleGear in possibleGears)
    {
        var adjacentNumbers = numbers.Where( numberEntry => numberEntry.IsAdjacent(possibleGear));
        if(adjacentNumbers.Count() == 2)
        {
            gearRatio += adjacentNumbers.First().Number * adjacentNumbers.Last().Number;
        }
    }
    Console.WriteLine($"The gear ratio is {gearRatio}");
}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}


IEnumerable<EngineSchematicEntry> ExtractEntries(string line, int row)
{
    var output = new List<EngineSchematicEntry>();  
    
    var numberMatches = NumberRegex.Matches(line);
    foreach(Match numberMatch in numberMatches)
    {
        // Console.WriteLine($"Number match. X:{numberMatch.Index} Y:{row} Value:{numberMatch.Value}");
        output.Add( new EngineSchematicEntry
        {
            X = numberMatch.Index,
            Y = row,
            Characters =numberMatch.Value
        });
    }
 
    var symbolMatches = SymbolRegex.Matches(line);
    foreach(Match symbolMatch in symbolMatches)
    {
        Console.WriteLine($"Synbol match. X:{symbolMatch.Index} Y:{row} Value:{symbolMatch.Value}");
        output.Add( new EngineSchematicEntry
        {
            X = symbolMatch.Index,
            Y = row,
            Characters = symbolMatch.Value
        });
    }
    Console.WriteLine($"Added {symbolMatches.Count} symbols");
    // Console.WriteLine($"Adding {output.Count} entries.");
    return output;
}