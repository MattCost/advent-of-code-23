Console.WriteLine("Hello, World!");
string? line;
long total = 0;
try
{
    StreamReader sr = new StreamReader("sample.txt");
    line = sr.ReadLine();
    while (line != null)
    {
        var row = new Row(line);
        total += row.Combos;
        line = sr.ReadLine();
    }
    sr.Close();
    Console.WriteLine($"There are {total} possible combos");
}
catch(Exception e)
{
    Console.WriteLine("Exception: " + e);
    
}