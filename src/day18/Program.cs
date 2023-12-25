using System.Text;

//62365 -input part 1
//61257 

Console.WriteLine("Hello, World!");

string? line;
int row = 1;
int col = 1;
List<Node> Nodes = new()
{
    new Node { Row = row, Col = col }
};

try
{
    // StreamReader sr = new StreamReader("sample.txt");
    StreamReader sr = new StreamReader("input.txt");

    Console.WriteLine("starting processing");

    long length = 0;
    while (!sr.EndOfStream)
    {
        line = sr.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            var split = line.Split(' ');

            //Part 2
            // var hex = split[2].Trim('(').Trim(')').Trim('#');
            // var directionCode = hex[5];
            // var difference = int.Parse(hex.Substring(0, 5), System.Globalization.NumberStyles.HexNumber);

            // length += difference;
            // Console.WriteLine($"#{hex} = {directionCode} {difference}");

            //Part 1
            var nextRow = row + (split[0] == "U" ? -int.Parse(split[1]) : split[0] == "D" ? int.Parse(split[1]) : 0);
            var nextCol = col + (split[0] == "L" ? -int.Parse(split[1]) : split[0] == "R" ? int.Parse(split[1]) : 0);
            length += int.Parse(split[1]);
            //Part 2
            // var nextRow = row + directionCode == '3' ? -difference : directionCode == '1' ? difference : 0;
            // var nextCol = col + directionCode == '2' ? -difference : directionCode == '0' ? difference : 0;

            Nodes.Add(new Node { Row = nextRow, Col = nextCol });

            row = nextRow;
            col = nextCol;

            line = sr.ReadLine();
        }
    }
    sr.Close();

    // Console.WriteLine("Nodes");
    // Nodes.ForEach(node => Console.WriteLine(node));


    var volume = CalculateTrenchVolume(Nodes);
    Console.WriteLine(new { length, volume, total = length + volume });

}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

long CalculateTrenchVolume(List<Node> nodes)
{
    var minRow = nodes.Select(node => node.Row).Min();
    var maxRow = nodes.Select(node => node.Row).Max();
    var minCol = nodes.Select(node => node.Col).Min();
    var maxCol = nodes.Select(node => node.Col).Max();

    //draw map
    var rowCount = maxRow - minRow + 1;
    var colCount = maxCol - minCol + 1;
    char[,] Map = new char[rowCount, colCount];

    for (row = 0; row < rowCount; row++)
    {
        for (col = 0; col < colCount; col++)
        {
            Map[row, col] = '.';
        }
    }

    Console.WriteLine(new { minRow, maxRow, minCol, maxCol });

    long volume = 0;

    //Generate edges
    List<Edge> edges = new();
    for (int i = 0; i < nodes.Count - 1; i++)
    {
        edges.Add(new Edge(nodes[i], nodes[i + 1]));
    }
    // edges.ForEach(Console.WriteLine);

    //draw map
    for (int i = 0; i < edges.Count; i++)
    {
        // Console.WriteLine(edges[i]);
        if (edges[i].IsHorizontal)
        {
            var delta = edges[i].End.Col > edges[i].Start.Col ? 1 : -1;
            for (int j = edges[i].Start.Col; j != edges[i].End.Col; j += delta)
            {
                // Console.WriteLine($"i {i} start {edges[i].Start.Row - minRow} col {j-minCol}");
                Map[edges[i].Start.Row - minRow, j - minCol] = '#';
            }
        }
        else
        {
            var delta = edges[i].End.Row > edges[i].Start.Row ? 1 : -1;
            for (int j = edges[i].Start.Row; j != edges[i].End.Row; j += delta)
            {
                Map[j - minRow, edges[i].Start.Col - minCol] = '#';
            }
        }
    }

    // PrintMap(Map, rowCount, colCount);

    for (int row = minRow; row <= maxRow; row++)
    {
        bool printLog = true;//row == -189;//minRow + 19;
        long rowVolume = 0;
        bool isInside = false;
        bool onEdge = false;
        bool upperSet = false;
        var activeEdges = edges.Where(edge => edge.ContainsRow(row)).OrderBy(edge => Math.Min(edge.Start.Col, edge.End.Col)).ThenBy(edge => Math.Min(edge.Start.Row, edge.End.Row)).ThenBy(edge => !edge.IsVertical).ToArray();

        Console.WriteLine($"Row {row} Dealing with {activeEdges.Count()} edges");
        activeEdges.ToList().ForEach(Console.WriteLine);
        var currentEdge = activeEdges[0];
        for (int i = 1; i < activeEdges.Length; i++)
        {
            if (printLog) Console.WriteLine($"{new { currentEdge }} - {new { activeEdge = activeEdges[i] }}");
            var intersecting = activeEdges.Where(edge => edge != currentEdge && currentEdge.Intersection(edge));
            if (!intersecting.Any())
            {
                if (printLog) Console.WriteLine("No intersection, toggling isInside");
                isInside = !isInside;

                // if(printLog) Console.WriteLine("No Intersection");
                // if(onEdge)
                // {
                //     if(printLog) Console.WriteLine("resetting onEdge");
                //     onEdge = false;
                // }
                // else
                // {
                //     if(printLog) Console.WriteLine("no onEdge, toggling isInside");
                //     isInside = !isInside;
                // }

                if (isInside)
                {
                    FillMap(Map, row - minRow, currentEdge.Start.Col - minCol + 1, Math.Min(activeEdges[i].Start.Col, activeEdges[i].End.Col) - minCol);

                    var delta = Math.Abs(Math.Min(activeEdges[i].Start.Col, activeEdges[i].End.Col) - currentEdge.Start.Col) - 1;
                    if (printLog) Console.WriteLine($"Adding delta {delta} to rowVolume");
                    rowVolume += delta;
                }
                currentEdge = activeEdges[i];
                continue;
            }
            else
            {
                if (printLog) Console.WriteLine("Dealing with intersection");
                var first = currentEdge;
                var second = activeEdges[i];
                var third = activeEdges[i + 1];

                if (!first.IsVertical) throw new Exception("assumption failed");
                if (!second.IsHorizontal)
                {
                    Console.WriteLine($"Second Assumption failed. i {i}. {new { first, second, third }}");
                    throw new Exception("second assumption failed");
                }
                if (!third.IsVertical) throw new Exception("third assumption failed");

                var firstHighest = Math.Min(first.Start.Row, first.End.Row) < second.Start.Row;
                var thirdHighest = Math.Min(third.Start.Row, third.End.Row) < second.Start.Row;

                if (firstHighest != thirdHighest)
                {
                    isInside = !isInside;
                }

                if (i + 2 < activeEdges.Length)
                {
                    if (isInside)
                    {
                        var fourth = activeEdges[i + 2];
                        var delta = Math.Abs(third.Start.Col - Math.Min(fourth.Start.Col, fourth.End.Col)) - 1;
                        FillMap(Map, row - minRow, third.Start.Col - minCol + 1, Math.Min(fourth.Start.Col, fourth.End.Col) - minCol);
                        if (printLog) Console.WriteLine($"Adding delta {delta} to rowVolume");
                        rowVolume += delta;
                    }
                    currentEdge = activeEdges[i + 2];
                }
                i += 2;
            }

            //     if(printLog) Console.WriteLine("Dealing with intersection");
            //     var horizontalEdge = currentEdge.IsHorizontal ? currentEdge : activeEdges[i];
            //     var verticalEdge = currentEdge.IsVertical ? currentEdge : activeEdges[i];
            //     // if(printLog) Console.WriteLine($"{ new { horizontalEdge, verticalEdge}}");

            //     var vertHighest = Math.Min(verticalEdge.Start.Row, verticalEdge.End.Row);
            //     if (!onEdge)
            //     {
            //         if(printLog) Console.WriteLine("Setting on edge and saving upperSet");
            //         onEdge = true;
            //         if (vertHighest < horizontalEdge.Start.Row)
            //         {
            //             upperSet = true;
            //         }
            //     }
            //     else
            //     {
            //         if(printLog) Console.WriteLine("Leaving edge. Checking upperSet");
            //         // onEdge = false;
            //         if ((vertHighest < horizontalEdge.Start.Row) != upperSet)
            //         {
            //             if(printLog) Console.WriteLine("toggling isInside");
            //             isInside = !isInside;
            //         }
            //         // if (i < activeEdges.Length - 1)
            //         // {
            //         //     if (isInside)
            //         //     {
            //         //         volume += Math.Abs(verticalEdge.End.Col - activeEdges[i + 1].Start.Col);
            //         //     }
            //         // }
            //     }
            // }
        }
        Console.WriteLine(rowVolume);
        volume += rowVolume;

        // if (printLog)
        //     PrintMap(Map, rowCount, colCount);
    }
    PrintMap(Map, rowCount, colCount);

    return volume;

}

void FillMap(char[,] map, int row, int startCol, int endCol)
{
    Console.WriteLine($" filling row {row} from {startCol} to {endCol}");
    for (int col = startCol; col < endCol; col++)
    {
        map[row, col] = '*';
    }
}

void PrintMap(char[,] map, int rows, int cols)
{
    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            Console.Write(map[row, col]);
        }
        Console.WriteLine();
    }
}
