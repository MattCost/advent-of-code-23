public enum NodeType
{
    None = '.',
    HorizontalSplitter = '-',
    VerticalSplitter = '|',
    LeftToUp = '/',
    LeftToDown = '\\'

}


public enum Direction
{
    Left,
    Right,
    Up,
    Down
}

public class Coord
{
    public long Row { get; set; }
    public long Col { get; set; }

    public Coord MoveLeft()
    {
        return new Coord { Row = this.Row, Col = this.Col - 1 };
    }
    public Coord MoveRight()
    {
        return new Coord { Row = this.Row, Col = this.Col + 1 };
    }

    public Coord MoveUp()
    {
        return new Coord { Row = this.Row - 1, Col = this.Col };
    }

    public Coord MoveDown()
    {
        return new Coord { Row = this.Row + 1, Col = this.Col };
    }

}

public class Tracer
{
    public Coord Coord { get; set; } = new();
    public Direction EntryPoint { get; set; }

}
public class Node
{
    public Node(char nodeType, long row, long col)
    {
        NodeType = (NodeType)Convert.ToInt32(nodeType);
        Coord.Row = row;
        Coord.Col = col;
    }
    public NodeType NodeType { get; set; }

    public int VisitCount {get;set;}
    public Coord Coord { get; set; } = new();
    public List<Tracer> Traverse(Direction entryPoint)
    {
        VisitCount++;
        var output = new List<Tracer>();
        switch (NodeType)
        {
            case NodeType.None:
                switch (entryPoint)
                {
                    case Direction.Left:
                        output.Add(new Tracer { Coord = this.Coord.MoveRight(), EntryPoint = Direction.Left });
                        break;
                    case Direction.Right:
                        output.Add(new Tracer { Coord = this.Coord.MoveLeft(), EntryPoint = Direction.Right });
                        break;
                    case Direction.Up:
                        output.Add(new Tracer { Coord = this.Coord.MoveDown(), EntryPoint = Direction.Up });
                        break;
                    case Direction.Down:
                        output.Add(new Tracer { Coord = this.Coord.MoveUp(), EntryPoint = Direction.Down });
                        break;
                }
                break;

            case NodeType.LeftToUp:
                switch (entryPoint)
                {
                    case Direction.Left:
                        output.Add(new Tracer { Coord = this.Coord.MoveUp(), EntryPoint = Direction.Down });
                        break;
                    case Direction.Right:
                        output.Add(new Tracer { Coord = this.Coord.MoveDown(), EntryPoint = Direction.Up });
                        break;
                    case Direction.Up:
                        output.Add(new Tracer { Coord = this.Coord.MoveLeft(), EntryPoint = Direction.Right });
                        break;
                    case Direction.Down:
                        output.Add(new Tracer { Coord = this.Coord.MoveRight(), EntryPoint = Direction.Left });
                        break;
                }
                break;


            case NodeType.LeftToDown:
                switch (entryPoint)
                {
                    case Direction.Left:
                        output.Add(new Tracer { Coord = this.Coord.MoveDown(), EntryPoint = Direction.Up });
                        break;
                    case Direction.Right:
                        output.Add(new Tracer { Coord = this.Coord.MoveUp(), EntryPoint = Direction.Down });
                        break;
                    case Direction.Up:
                        output.Add(new Tracer { Coord = this.Coord.MoveRight(), EntryPoint = Direction.Left });
                        break;
                    case Direction.Down:
                        output.Add(new Tracer { Coord = this.Coord.MoveLeft(), EntryPoint = Direction.Right });
                        break;
                }
                break;


            case NodeType.HorizontalSplitter:
                switch (entryPoint)
                {
                    case Direction.Left:
                        output.Add(new Tracer { Coord = this.Coord.MoveRight(), EntryPoint = Direction.Left });
                        break;
                    case Direction.Right:
                        output.Add(new Tracer { Coord = this.Coord.MoveLeft(), EntryPoint = Direction.Right });
                        break;

                    case Direction.Up:
                    case Direction.Down:
                        output.Add(new Tracer { Coord = this.Coord.MoveLeft(), EntryPoint = Direction.Right });
                        output.Add(new Tracer { Coord = this.Coord.MoveRight(), EntryPoint = Direction.Left });
                        break;
                }
                break;

            case NodeType.VerticalSplitter:
                switch (entryPoint)
                {
                    case Direction.Left:
                    case Direction.Right:
                        output.Add(new Tracer { Coord = this.Coord.MoveUp(), EntryPoint = Direction.Down });
                        output.Add(new Tracer { Coord = this.Coord.MoveDown(), EntryPoint = Direction.Up });
                        break;

                    case Direction.Up:
                        output.Add(new Tracer { Coord = this.Coord.MoveDown(), EntryPoint = Direction.Up });
                        break;
                    case Direction.Down:
                        output.Add(new Tracer { Coord = this.Coord.MoveUp(), EntryPoint = Direction.Down });
                        break;
                }
                break;


        }
        return output;
    }
}