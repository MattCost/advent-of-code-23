using System.Text;

//62365 -input part 1
Console.WriteLine("Hello, World!");

string? line;
int row = 1;
int col=1;
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

            Nodes.Add( new Node{ Row = nextRow, Col = nextCol});

            row = nextRow;
            col = nextCol;

            line = sr.ReadLine();
        }
    }
    sr.Close();

    Console.WriteLine("Nodes");
    Nodes.ForEach( node => Console.WriteLine(node));


    var volume = CalculateTrenchVolume(Nodes);
    Console.WriteLine(new { length, volume});
}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

long CalculateTrenchVolume(List<Node> nodes)
{
   
    var minRow = nodes.Select(node => node.Row).Min();
    var maxRow = nodes.Select(node => node.Row).Max();
    var minCol = nodes.Select(node => node.Row).Min();
    var maxCol = nodes.Select(node => node.Row).Max();
    
    Console.WriteLine( new { minRow, maxRow, minCol, maxCol});
    long volume = 0;
    List<Edge> edges = new();
    for(int i=0; i<nodes.Count-1 ; i++)
    {
        edges.Add( new Edge( nodes[i], nodes[i+1]));
    }
    edges.ForEach(Console.WriteLine);
    for(int row = minRow ; row <= maxRow ; row++)
    {
        bool isInside = false;
        bool onEdge = false;
        bool upperSet = false;
        var activeEdges = edges.Where(edge => edge.ContainsRow(row)).ToArray();

        Console.WriteLine($"Row {row} Dealing with {activeEdges.Count()} edges");
        var currentEdge = activeEdges[0];
        for(int i=1 ; i<activeEdges.Length ; i++)
        {

            if(currentEdge.IsVertical && activeEdges[i].IsVertical)
            {
                isInside = !isInside;
                if(isInside && !onEdge)
                {
                    volume += Math.Abs(activeEdges[i].Start.Col - currentEdge.Start.Col) - 1;
                }
                currentEdge = activeEdges[i];
                continue;

            }
            else
            {
                var horizontalEdge = currentEdge.IsHorizontal ? currentEdge : activeEdges[i];
                var verticalEdge = currentEdge.IsVertical ? currentEdge : activeEdges[i];
                var vertHighest = Math.Min(verticalEdge.Start.Row, verticalEdge.End.Row);
                if(!onEdge)
                {
                    onEdge = true;
                    if(vertHighest < horizontalEdge.Start.Row)
                    {
                        upperSet = true;
                    }
                    continue;
                }
                else
                {
                    onEdge = false;
                    if( (vertHighest < horizontalEdge.Start.Row) != upperSet)
                    {
                        isInside = !isInside;
                    }
                    if(i<activeEdges.Length-1)
                    {
                        if(isInside)
                        {
                            volume += Math.Abs(verticalEdge.End.Col - activeEdges[i+1].Start.Col) ;
                        }
                    }
                }
            }
        }
    }
/*
#######
#.....#
###...#
..#...#
..#...#
###.###
#...#..
##..###
.#....#
.######

proceess each row
move across row to the right to the next intersection.
if only 1 edge at this intersection 
    toggle isInside
    if(isInside) total += nextIntersection.Col - current.Col
    continue;
    
if 2 edges at horizontal row 
    if(! onEdge)
        set onEdge
        set isAbove if the vertical row is above the hoz row
    else
        reset onEdge
        if iisAbove matches current isAbove - no change - outside row
        else toggle isInside
    end if




*/

    return volume;

}

// bool PointInsidePath(int Py, int Px, List<Node> nodes)
// {
//     // Console.WriteLine($"Entering PointInside Path Row {Py} Col {Px}");
//     var count = 0;
    
//     for(int i=0 ; i<nodes.Count-1 ; i++)
//     {
//         //Edge goes from nodes[i] to nodes[j];
//         var p1x = nodes[i].Col;
//         var p1y = nodes[i].Row;
//         var p2x = nodes[i+1].Col;
//         var p2y = nodes[i+1].Row;

//         // Console.WriteLine($"\tComparing to edge: ({p1y},{p1x}) to ({p2y},{p2x})");
//         //Corners
//         if( (Py ==p1y ) && (Px == p1x) ) return true;
//         if( (Py ==p2y ) && (Px == p2x) ) return true;

//         //Horizontal lines
//         if( (p1y == p2y) &&(Py == p1y) )
//         {
//             if( Px >= Math.Min(p1x, p2x) && Px <= Math.Max(p1x, p2x)) return true;
//         }
//         //Vertical lines
//         if( (p1x == p2x) &&(Px == p1x) )
//         {
//             if( Py >= Math.Min(p1y, p2y) && Py <= Math.Max(p1y, p2y)) return true;
//         }


//         if( (p1y < Py && p2y < Py) || (p1y >= Py && p2y >= Py))
//         {
//             // Console.WriteLine("\tPoint is above or below edge");
//             continue;
//         }
//         //Cheating cus our lines are always 1d
//         var Sx = Math.Min(p1x, p2x);
//         // Console.WriteLine($"\tIntersection Col {Sx}");
//         if(Sx >= Px)
//         {
//             // Console.WriteLine($"\tIntersection Col >= Col. Increasing Count");
//             count++;
//         }
//     }

//     // Console.WriteLine($"Exiting PointInside {new {row=Py, col=Px, count}}");
//     return count % 2 == 1;
// }