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

    for(int row = minRow-1 ; row <= maxRow ; row++)
    {
        for( int col = minCol-1 ; col <= maxCol ; col++)
        {
            var isInside = PointInsidePath(row, col, nodes);
            // Console.WriteLine($"{new {row, col, isInside}}");
            if(isInside) volume++;
        }
    }


    return volume;

}

bool PointInsidePath(int Py, int Px, List<Node> nodes)
{
    // Console.WriteLine($"Entering PointInside Path Row {Py} Col {Px}");
    var count = 0;
    
    for(int i=0 ; i<nodes.Count-1 ; i++)
    {
        //Edge goes from nodes[i] to nodes[j];
        var p1x = nodes[i].Col;
        var p1y = nodes[i].Row;
        var p2x = nodes[i+1].Col;
        var p2y = nodes[i+1].Row;

        // Console.WriteLine($"\tComparing to edge: ({p1y},{p1x}) to ({p2y},{p2x})");
        //Corners
        if( (Py ==p1y ) && (Px == p1x) ) return true;
        if( (Py ==p2y ) && (Px == p2x) ) return true;

        //Horizontal lines
        if( (p1y == p2y) &&(Py == p1y) )
        {
            if( Px >= Math.Min(p1x, p2x) && Px <= Math.Max(p1x, p2x)) return true;
        }
        //Vertical lines
        if( (p1x == p2x) &&(Px == p1x) )
        {
            if( Py >= Math.Min(p1y, p2y) && Py <= Math.Max(p1y, p2y)) return true;
        }


        if( (p1y < Py && p2y < Py) || (p1y >= Py && p2y >= Py))
        {
            // Console.WriteLine("\tPoint is above or below edge");
            continue;
        }
        //Cheating cus our lines are always 1d
        var Sx = Math.Min(p1x, p2x);
        // Console.WriteLine($"\tIntersection Col {Sx}");
        if(Sx >= Px)
        {
            // Console.WriteLine($"\tIntersection Col >= Col. Increasing Count");
            count++;
        }
    }

    // Console.WriteLine($"Exiting PointInside {new {row=Py, col=Px, count}}");
    return count % 2 == 1;
}