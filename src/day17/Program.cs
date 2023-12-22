using System.Text;

Console.WriteLine("Hello, World!");

string? line;
List<string> input = new();

try
{
    // StreamReader sr = new StreamReader("sample.txt");
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

    int Rows = input.Count;
    int Cols = input.First().Length;
    Node[,] Maze = new Node[Rows,Cols];

    // Build up the nodes
    for(int row=0; row<Rows ; row++)
    {
        for(int col=0; col<Cols ; col++)
        {
            var node = new Node( row, col, int.Parse($"{input[row][col]}"));
            Maze[row,col] = node;
        }
    }

    // Link up the nodes
    for(int row=0; row<Rows ; row++)
    {
        for(int col=0; col<Cols ; col++)
        {

            if(row > 0)
            {
                Maze[row,col].Links.Add( new NodeLink(Maze[row-1,col], Direction.Up) );
            }
            if(col > 0)
            {
                Maze[row,col].Links.Add( new NodeLink(Maze[row, col-1], Direction.Left));
            }
            if(row < Rows-1)
            {
                Maze[row,col].Links.Add( new NodeLink(Maze[row+1,col], Direction.Down) );
            }
            if(col < Cols - 1)
            {
                Maze[row,col].Links.Add( new NodeLink(Maze[row, col+1], Direction.Right));
            }
        }
    }

    //Search time
    // int stepCount = 0;
    // long score = 0;
    var workingList = new List<Tracker>
    {
        new Tracker(Maze[0, 0], Direction.None, 0, 0, new())
    }; 
    var closedList = new List<Tracker>();
    List<Tracker> TheEnd = new();

    while(workingList.Count>0)
    {
        Console.WriteLine($"WorkingList has {workingList.Count} entries left to process");
        //Find the cheaper node
        var min = workingList.Select( node => node.CumulativeScore).Min();
        var workingStep = workingList.Where( node => node.CumulativeScore == min).First();
        workingList.Remove(workingStep);

        //Generate successors
        var successors = workingStep.Node.ValidLinks(workingStep.DirectionOfTravel, workingStep.StepCount).Select( nextNode => 
            new Tracker(
                nextNode.Node, 
                nextNode.Direction, 
                nextNode.Direction != workingStep.DirectionOfTravel ? 1 : workingStep.StepCount+1, 
                workingStep.CumulativeScore + nextNode.Node.Cost,
                new List<Node>(workingStep.PastNodes){ nextNode.Node}
                ) ).ToList();
        
        //Process successors
        foreach(var successor in successors)
        {
            if(successor.Node == Maze[Rows-1, Cols-1]) 
            {
                Console.WriteLine($"Found the goal with a score of {successor.CumulativeScore}");
                PrintPath(successor, Rows,Cols);
                Environment.Exit(0);
                TheEnd.Add(successor);
            }
            else
            {
                if(workingList.Where( n => n.Node == successor.Node && n.DirectionOfTravel == successor.DirectionOfTravel && n.StepCount == successor.StepCount && n.CumulativeScore < successor.CumulativeScore).Any())
                    continue;

                if(closedList.Where( n => n.Node == successor.Node && n.DirectionOfTravel == successor.DirectionOfTravel && n.StepCount == successor.StepCount && n.CumulativeScore < successor.CumulativeScore).Any())
                    continue;
                
                workingList.Add(successor);
            }
        }

        closedList.Add(workingStep);
    }

    var bestScore = TheEnd.Select( x => x.CumulativeScore).Min();
    var best = TheEnd.Where( x => x.CumulativeScore == bestScore);
    Console.WriteLine($"Found {TheEnd.Count} total paths to the goal. {best.Count()} paths have the best score of {bestScore}.");
    foreach(var bestPath  in best)
    {
        PrintPath(bestPath,Rows,Cols);
    }


}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

void PrintPath(Tracker successor, int rows, int cols)
{
    var output = new List<StringBuilder>();
    for(int row=0; row<rows ; row++)
    {
        var sb = new StringBuilder();
        for(int col = 0 ; col<cols ; col++)
            sb.Append('.');
        output.Add(sb);
    }

    foreach(var node in successor.PastNodes)
    {
        // Console.WriteLine($"{node.Row},{node.Col}");
        output[node.Row][node.Col]='#';
    }
    output.ForEach( x => Console.WriteLine(x.ToString()));
}