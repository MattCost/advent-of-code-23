
Console.WriteLine("Hello, World!");


string? line;
try
{
    StreamReader sr = new StreamReader("sample2.txt");
    line = sr.ReadLine() ?? throw new Exception("bad input");
    Maze maze = new(line.Length);
    while (line != null)
    {
        Console.WriteLine(line);
        maze.AddRow(line);
        line = sr.ReadLine();
    }
    sr.Close();
    maze.ChaseAnimal();
}
catch(Exception e)
{
    Console.WriteLine("Exception: " + e);
    
}