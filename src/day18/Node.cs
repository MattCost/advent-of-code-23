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