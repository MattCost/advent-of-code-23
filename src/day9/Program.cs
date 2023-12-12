
Console.WriteLine("Hello, World!");


string? line;
long total=0;
try
{
    StreamReader sr = new StreamReader("input.txt");
    line = sr.ReadLine();
    while (line != null)
    {
        Console.WriteLine(line);

        var split = line.Split(' ');
        var history = new History( split.Select( x => int.Parse(x)).ToList());
        Console.WriteLine($"Prediction {history.Prediction}");
        total += history.Prediction;
        line = sr.ReadLine();
    }
    sr.Close();
    Console.WriteLine(total);
}
catch(Exception e)
{
    Console.WriteLine("Exception: " + e);
    
}