using System.Security.Cryptography.X509Certificates;

public class Maze
{
    public int Size { get; init; }
    private int lastRow = 0;
    public int StartCol { get; private set; }
    public int StartRow { get; private set; }

    private Node[,] _nodes;
    private int[,] _chased;
    public Maze(int size)
    {
        Size = size;
        _nodes = new Node[size, size];
        _chased = new int[size, size];
    }
    public void AddRow(string row)
    {
        if (row.Length != Size) throw new Exception("not correct size");

        if (row.Contains('S')) StartRow = lastRow;

        for (int col = 0; col < Size; col++)
        {
            if (row[col] == 'S') StartCol = col;
            _nodes[lastRow, col] = new Node(col, lastRow, row[col]);
        }

        lastRow++;
    }

    public void ChaseAnimal()
    {
        //Find start X/Y
        var start = _nodes[StartRow,StartCol];
        List<(Node, string)> paths = new();

        if (StartCol > 0 && _nodes[StartRow,StartCol - 1].East)
        {
            paths.Add((_nodes[StartCol - 1, StartRow], "East"));
        }
        if (StartCol < Size - 1 && _nodes[StartRow,StartCol + 1].West)
        {
            paths.Add((_nodes[StartRow,StartCol + 1], "West"));
        }

        if (StartRow > 0 && _nodes[StartRow - 1,StartCol].South)
        {
            paths.Add((_nodes[ StartRow - 1,StartCol], "South"));
        }

        if (StartRow < Size - 1 && _nodes[StartRow + 1,StartCol].North)
        {
            paths.Add((_nodes[StartRow + 1,StartCol], "North"));
        }
        if (paths.Count() != 2) throw new Exception("unable to find 2 paths");

        var path1 = paths[0];
        var path2 = paths[1];


        Console.WriteLine($"The chase will start from {StartCol},{StartRow}");
        Console.WriteLine($"Path 1 : {path1.Item1.Col},{path1.Item1.Row} and will head {path1.Item2}");
        Console.WriteLine($"Path 2 : {path2.Item1.Col},{path2.Item1.Row} and will head {path2.Item2}");
        int moves = 2;
        var path1_next = path1.Item1.Traverse(path1.Item2);
        var path2_next = path2.Item1.Traverse(path2.Item2);

        _chased[StartRow, StartCol] = 0;
        _chased[path1.Item1.Row, path1.Item1.Col] = 1;
        _chased[path2.Item1.Row, path2.Item1.Col] = 1;
        _chased[path1_next.Item2, path1_next.Item1] = 2;
        _chased[path2_next.Item2, path2_next.Item1] = 2;
        while ((path1_next.Item1 != path2_next.Item1) || (path1_next.Item2 != path2_next.Item2))
        {
            path1_next = _nodes[path1_next.Item2, path1_next.Item1].Traverse(path1_next.Item3);
            path2_next = _nodes[path2_next.Item2, path2_next.Item1].Traverse(path2_next.Item3);

            moves++;
            _chased[path1_next.Item2, path1_next.Item1] = moves;
            _chased[path2_next.Item2, path2_next.Item1] = moves;
        }
        Console.WriteLine($"Caught after {moves} moves");
        Console.WriteLine($"Path1 : {path1_next.Item2},{path1_next.Item1}");
        Console.WriteLine($"Path2 : {path2_next.Item2},{path2_next.Item1}");
        
        Console.WriteLine();
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if(i==StartCol && j==StartRow) Console.Write('0'); 
                else if (_chased[i, j] == default) Console.Write('.');
                else Console.Write(_chased[i, j]);
            }
            Console.WriteLine();
        }
    }
}