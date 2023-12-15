Console.WriteLine("Hello, World!");

string? line;
List<string> input = new();
List<long> emptyRows = new();
List<long> emptyCols = new();

try
{
    StreamReader sr = new StreamReader("input.txt");
    line = sr.ReadLine() ?? throw new Exception("bad input");
    long row = 0;
    while (line != null)
    {
        input.Add(line);
        if (!line.Contains('#')) emptyRows.Add(row);
        row++;
        line = sr.ReadLine();
    }
    sr.Close();
    Console.WriteLine("Empty Rows");
    emptyRows.ForEach(Console.WriteLine);


    for (int i = 0; i < input[0].Length; i++)
    {
        var col = new string(input.Select(line => line[i]).ToArray());

        if (col.Contains('#')) continue;

        emptyCols.Add(i);
    }

    // input.ForEach(Console.WriteLine);
    Console.WriteLine("Empty cols");
    emptyCols.ForEach(Console.WriteLine);

    var galaxies = new List<(int, int)>();
    for (int y = 0; y < input.Count; y++)
    {
        if (!input[y].Contains('#')) continue;
        int x = input[y].IndexOf('#');
        while (x != -1)
        {
            galaxies.Add((x, y));
            x = input[y].IndexOf('#', x + 1);
        }
    }

    double totalDistance = 0;
    for (int i = 1; i < galaxies.Count; i++)
    {
        var current = galaxies[i - 1];
        var targets = galaxies.GetRange(i, galaxies.Count - i);
        totalDistance += targets.Select(target => DistanceCalc2(current.Item1, current.Item2, target.Item1, target.Item2, emptyCols, emptyRows)).Sum();
    }

    Console.WriteLine($"Sum {totalDistance}");
}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

long DistanceCalc2(long x1, long y1, long x2, long y2, List<long> emptyX, List<long> emptyY)
{
    long scale =999999;
    long startX = Math.Min(x1, x2);
    long endX = Math.Max(x1, x2);
    long startY = Math.Min(y1, y2);
    long endY = Math.Max(y1, y2);
    var emptyXs = emptyX.Where( x=> (x > startX) && (x <endX));
    var emptyYs = emptyY.Where( y=> (y>startY) && (y < endY));
    
    var output =
        (endX - startX) + (emptyXs.Count()*scale) +
        (endY - startY) + (emptyYs.Count()*scale);
        
    return output;
}