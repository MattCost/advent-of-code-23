using System.Text;

Console.WriteLine("Hello, World!");

string? line;
List<string> input = new();

try
{
    // StreamReader sr = new StreamReader("sample2.txt");
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
    Node[,] LavaGrid = new Node[Rows, Cols];

    // Build up the nodes
    for (int row = 0; row < Rows; row++)
    {
        for (int col = 0; col < Cols; col++)
        {
            LavaGrid[row, col] = new Node(row, col, int.Parse($"{input[row][col]}"));
        }
    }

    // Link up the nodes
    for (int row = 0; row < Rows; row++)
    {
        for (int col = 0; col < Cols; col++)
        {
            if (row > 0)
            {
                LavaGrid[row, col].Links.Add(new Edge(LavaGrid[row - 1, col], Direction.Up));
            }
            if (col > 0)
            {
                LavaGrid[row, col].Links.Add(new Edge(LavaGrid[row, col - 1], Direction.Left));
            }
            if (row < Rows - 1)
            {
                LavaGrid[row, col].Links.Add(new Edge(LavaGrid[row + 1, col], Direction.Down));
            }
            if (col < Cols - 1)
            {
                LavaGrid[row, col].Links.Add(new Edge(LavaGrid[row, col + 1], Direction.Right));
            }
        }
    }

    //Search time
    var timer = System.Diagnostics.Stopwatch.StartNew();
    //Part 1 
    // var startingPoint = new DirectionalNode(LavaGrid[0, 0], Direction.None, 0);

    //Part 2
    var firstMoveRight = new DirectionalNode(LavaGrid[0,1],Direction.Right,1);
    var firstMoveDown = new DirectionalNode(LavaGrid[1,0], Direction.Down, 1);

    var queue = new PriorityQueue<DirectionalNode, long>();

    // queue.Enqueue(startingPoint, 0);
    queue.Enqueue(firstMoveRight, LavaGrid[0,1].Cost);
    queue.Enqueue(firstMoveDown, LavaGrid[1,0].Cost);

    var openList = new Dictionary<DirectionalNode, long>();

    // openList[startingPoint] = 0;
    openList[firstMoveRight] = LavaGrid[0,1].Cost;
    openList[firstMoveDown] = LavaGrid[1,0].Cost;

    var closedList = new Dictionary<DirectionalNode, long>();

    //Process the cheaper node
    int cycle = 0;
    while (queue.TryDequeue(out var workingStep, out var workingStepCost))
    {
        if(++cycle % 100 == 0) 
            Console.WriteLine($"Cycle {cycle} Processing [{workingStep.Row},{workingStep.Col}] Dir{workingStep.Direction} Step{workingStep.StepCount} Score:{workingStepCost}");
        
        openList.Remove(workingStep);

        //Generate successors using links from the LavaGrid, but taking the current direction and step count into account
        var successors =
            LavaGrid[workingStep.Row, workingStep.Col].ValidLinks(workingStep.Direction, workingStep.StepCount)
                .Select(nextNode =>
                    new DirectionalNode(
                        nextNode.Node,
                        nextNode.Direction,
                        nextNode.Direction != workingStep.Direction ? 1 : workingStep.StepCount + 1
                    ));

        //Process successors
        foreach (var successor in successors)
        {
            var successorCost = workingStepCost + LavaGrid[successor.Row, successor.Col].Cost;

            //Part 1 had no min step count, part 2 has a 4 step min
            if (successor.Row == Rows - 1 && successor.Col == Cols - 1 && successor.StepCount > 3)
            {
                timer.Stop();
                Console.WriteLine($"Found the goal with a score of {successorCost} in {timer.ElapsedMilliseconds} msec");
                Environment.Exit(0);
            }
            else
            {
                if (openList.ContainsKey(successor) && openList[successor] <= successorCost)
                    continue;

                if (closedList.ContainsKey(successor) && closedList[successor] <= successorCost)
                    continue;

                queue.Enqueue(successor, successorCost);
                openList[successor] = successorCost;
            }
        }
        closedList[workingStep] = workingStepCost;
        // Console.WriteLine($"ClosedList has {closedList.Count} processed nodes. WorkingList has {queue.Count} nodes to process.");    
    }
    Console.WriteLine("Missed the exit");
}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}
