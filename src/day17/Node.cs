
using System.Diagnostics;

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
    public Guid Id {get; init;} = Guid.NewGuid();
    public int Cost { get; set; }
    public Node(int cost) { Cost = cost; }
    public HashSet<NodeLink> Links { get; set; } = new();

    public HashSet<NodeLink> ValidLinks(Direction directionOfTravel, int stepCount)
    {
        var baseOutput = Links.Where(link => link.Direction != Inverse(directionOfTravel));
        return (stepCount < 3) ?
            baseOutput.ToHashSet() :
            baseOutput.Where(link => link.Direction != directionOfTravel).ToHashSet();
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
    public Tracker(Node node, Direction directionOfTravel, int stepCount, long score)
    {
        Node = node;
        DirectionOfTravel = directionOfTravel;
        CumulativeScore = score;
        StepCount = stepCount;
    }
}