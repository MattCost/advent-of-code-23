using System.Collections.Concurrent;

Console.WriteLine("Hello, World!");
string? line;
long total = 0;
List<string> lines = new();
try
{
    StreamReader sr = new StreamReader("input.txt");
    line = sr.ReadLine();
    while (line != null)
    {
        lines.Add(line);
        line = sr.ReadLine();
    }
    sr.Close();

    var counts = new ConcurrentBag<int>();
    Parallel.ForEach( lines,  line => {
        var row = new Row(line);
         Console.WriteLine($"Possible {row.PossibleComboCount}");
        counts.Add(row.PossibleComboCount);
    });
        // Console.WriteLine(line);
        // Console.WriteLine( string.Join('\n', row.PossibleCombos));
    total = counts.ToList().Sum();
    Console.WriteLine($"There are {total} possible combos");
}
catch(Exception e)
{
    Console.WriteLine("Exception: " + e);   
}