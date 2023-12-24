// public class Edge
// {
//     public Node Node { get; set; }
//     public string Color { get; set; }    
//     public Edge(Node node, string color)
//     {
//         Node = node;
//         Color = color;
//     }
// }
// public class Node
// {
//     public int Row { get; set; }
//     public int Col { get; set; }
//     public Node(int row, int col)
//     {
//         Row = row;
//         Col = col;
//     }
//     public List<Edge> Links { get; set; } = new();
// }

public record Node
{
    public int Row { get; init; }
    public int Col { get; init; }

}

public record Edge
{
    public Node Start { get; set; }
    public Node End { get; set; }
    public Edge(Node start, Node end)
    {
        Start = start;
        End = end;
    }
    public bool IsHorizontal { get { return Start.Row == End.Row; } }
    public bool IsVertical { get { return Start.Col == End.Col; } }

    public bool ContainsRow(int row)
    {
        return  row >= Math.Min(Start.Row, End.Row) && row <= Math.Max(Start.Row, End.Row);
    }

    public bool Intersection(Edge other)
    {
        if(this.Start == other.Start) return true;
        if(this.Start == other.End) return true;
        if(this.End == other.Start) return true;
        if(this.End == other.End) return true;
        return false;
    }
}