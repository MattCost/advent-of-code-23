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
            var node = new Node( int.Parse($"{input[row][col]}"));
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
        new Tracker(Maze[0, 0], Direction.None, 0, Maze[0,0].Cost)
    }; 
    var closedList = new List<Tracker>();
    var scores = new List<long>();

    while(workingList.Count>0)
    {
        Console.WriteLine($"WorkingList has {workingList.Count} entries left to process");
        //Find the cheaper node
        var min = workingList.Select( node => node.CumulativeScore).Min();
        var workingNode = workingList.Where( node => node.CumulativeScore == min).First();
        workingList.Remove(workingNode);

        //Generate successors
        var successors = workingNode.Node.ValidLinks(workingNode.DirectionOfTravel, workingNode.StepCount).Select( nextNode => 
            new Tracker(nextNode.Node, nextNode.Direction, nextNode.Direction != workingNode.DirectionOfTravel ? 0 : workingNode.StepCount+1, workingNode.CumulativeScore + nextNode.Node.Cost) ).ToList();
        
        //Process successors
        foreach(var successor in successors)
        {
            if(successor.Node.Id == Maze[Rows-1, Cols-1].Id) 
            {
                Console.WriteLine($"Found the goal with a score of {successor.CumulativeScore}");                
                scores.Add(successor.CumulativeScore);
            }
            else
            {
                if(workingList.Where( n => n.Node.Id == successor.Node.Id && n.CumulativeScore < successor.CumulativeScore).Any())
                    continue;

                if(closedList.Where( n => n.Node.Id == successor.Node.Id && n.CumulativeScore < successor.CumulativeScore).Any())
                    continue;
                
                workingList.Add(successor);
            }
        }

        closedList.Add(workingNode);
    }
    Console.WriteLine($"Found {scores.Count} paths to the end. Best score is {scores.Min()}");
    Console.WriteLine("Bye");


}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}
