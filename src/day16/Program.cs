using System.Text;

Console.WriteLine("Hello, World!");

string? line;
List<string> input = new();

try
{
    StreamReader sr = new StreamReader("input.txt");

    Console.WriteLine("starting processing");
    while (!sr.EndOfStream)
    {
        line = sr.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            input.Add(line);
            line = sr.ReadLine();
        }
    }

    sr.Close();

    long Rows = input.Count;
    long Cols = input[0].Length;
    var Maze = new Node[Rows, Cols];
    for (int row = 0; row < Rows; row++)
    {
        for (int col = 0; col < Cols; col++)
        {
            Maze[row, col] = new Node(input[row][col], row, col);
        }
    }

    Console.WriteLine(Maze[0, 0].NodeType);
    //Enter the maze at 0,0 from the left
    var journey = Maze[0, 0].Traverse(Direction.Left);
    Console.WriteLine($"We took 1 step and have {journey.Count} paths to explore");
    journey.ForEach(x => Console.WriteLine($"Row: {x.Coord.Row} Col: {x.Coord.Col} EntryPoint: {x.EntryPoint}"));

    long prevTotal = -1;
    long cycle=0;
    long staleCycle=-1;
    while (journey.Count > 0)
    {
        var valid = journey.Where(x => x.Coord.Row >= 0 && x.Coord.Row < Rows && x.Coord.Col >= 0 && x.Coord.Col < Cols).ToList();
        // Console.WriteLine($"We have {valid.Count} paths to explore");
        var nextSteps = new List<Tracer>();
        foreach (var step in valid)
        {
            var nextStep = Maze[step.Coord.Row, step.Coord.Col].Traverse(step.EntryPoint);
            // Console.WriteLine($"Maze[{step.Coord.Row},{step.Coord.Col}] {Maze[step.Coord.Row,step.Coord.Col].NodeType} = from {step.EntryPoint} led to {nextStep.Count()} paths to explore");
            if (nextStep.Any()) nextSteps.AddRange(nextStep);
        }
        journey = nextSteps.Where(x => x.Coord.Row >= 0 && x.Coord.Row < Rows && x.Coord.Col >= 0 && x.Coord.Col < Cols).ToList();

        cycle++;
        if(cycle %25 == 0)
        {
            Console.WriteLine($"Cycle {cycle} complete");
        }
        long curTotal = 0;
        
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Cols; col++)
            {
                if (Maze[row, col].VisitCount > 0) curTotal++;
            }
        }
        
        if (curTotal == prevTotal && staleCycle == -1)
        {
            staleCycle = cycle;
        };
        if(staleCycle != -1 && cycle - staleCycle > 125) break;
        if(staleCycle != -1 && curTotal != prevTotal) staleCycle = -1;
        prevTotal = curTotal;
    }

    long total = 0;
    for (int row = 0; row < Rows; row++)
    {
        for (int col = 0; col < Cols; col++)
        {
            if (Maze[row, col].VisitCount > 0)
            {
                total++;
                Console.Write("#");
            }
            else
            {
                Console.Write('.');
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine(total);

}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}