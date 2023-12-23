public enum Direction
{
    None,
    Left,
    Right,
    Up,
    Down
}
public class Edge
{
    public Direction Direction { get; set; }
    public Node Node { get; set; }
    public Edge(Node node, Direction direction)
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
    public List<Edge> Links { get; set; } = new();

    public IEnumerable<Edge> ValidLinks(Direction directionOfTravel, int stepCount)
    {
        var baseOutput = Links.Where(link => link.Direction != Inverse(directionOfTravel));
        
        //More than 3 steps, remove the direction of travel from the valid options
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

public record DirectionalNode
{
    public int Row {get;init;}
    public int Col {get;init;}
    public Direction Direction {get;init;}
    public int StepCount {get;init;}
    
    public DirectionalNode(Node node, Direction direction, int stepCount)
    {
        Row = node.Row;
        Col = node.Col;
        Direction = direction;
        StepCount = stepCount;
    }
}