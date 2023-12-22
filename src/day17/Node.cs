public enum Direction
{
    None,
    Left,
    Right,
    Up,
    Down
}
public class NodeLink
{
    public Direction Direction { get; set; }
    public Node Node { get; set; }
    public NodeLink(Node node, Direction direction)
    {
        Node = node;
        Direction = direction;
    }
}
public class Node
{
    public int Cost { get; set; }
    public int Row {get;init;}
    public int Col {get;init;}
    public Node(int row, int col, int cost)
    {
        Row = row;
        Col = col;
        Cost = cost; 
    }
    public List<NodeLink> Links { get; set; } = new();

    public IEnumerable<NodeLink> ValidLinks(Direction directionOfTravel, int stepCount)
    {
        var baseOutput = Links.Where(link => link.Direction != Inverse(directionOfTravel));
        
        return (stepCount < 3) ?
            baseOutput : 
            baseOutput.Where(link => link.Direction != directionOfTravel);
    }
    private Direction Inverse(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.None => Direction.None,
            _ => Direction.None
        };
    }
}

public class Tracker
{
    public Node Node { get; set; }
    public int StepCount { get; set; }
    public Direction DirectionOfTravel { get; set; }
    public long CumulativeScore { get; set; }
    public long EstimatedScore {get;set;}
    public List<Node> PastNodes {get;set;}
    public Tracker(Node node, Direction directionOfTravel, int stepCount, long score, List<Node> pastNodes)
    {
        Node = node;
        DirectionOfTravel = directionOfTravel;
        CumulativeScore = score;
        EstimatedScore = CumulativeScore;
        StepCount = stepCount;

        PastNodes = pastNodes;
    
    }
}