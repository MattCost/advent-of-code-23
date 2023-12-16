Console.WriteLine("Hello, World!");
string? line;
long total = 0;
try
{
    StreamReader sr = new StreamReader("input.txt");
    line = sr.ReadLine();
    while (line != null)
    {
        var row = new Row(line);
        Console.WriteLine(line);
        Console.WriteLine($"Possible {row.PossibleComboCount}");
        total += row.PossibleComboCount;
        line = sr.ReadLine();
    }
    sr.Close();
    Console.WriteLine($"There are {total} possible combos");
}
catch(Exception e)
{
    Console.WriteLine("Exception: " + e);   
}