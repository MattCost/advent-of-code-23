using System.Text;

Console.WriteLine("Hello, World!");

string? line;
Dictionary<string, Node> Nodes = new();
int row = 0;
int col = 0;
var startingNode = new Node(row, col);
Nodes[GenerateKey(row, col)] = startingNode;
try
{
    // StreamReader sr = new StreamReader("sample.txt");
    StreamReader sr = new StreamReader("input.txt");

    Console.WriteLine("starting processing");
    while (!sr.EndOfStream)
    {
        line = sr.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            var split = line.Split(' ');
            var nextRow = row + (split[0] == "U" ? -int.Parse(split[1]) : split[0] == "D" ? int.Parse(split[1]) : 0);
            var nextCol = col + (split[0] == "L" ? -int.Parse(split[1]) : split[0] == "R" ? int.Parse(split[1]) : 0);

            var currentNode = Nodes[GenerateKey(row, col)];
            var nextKey = GenerateKey(nextRow, nextCol);
            if (!Nodes.ContainsKey(nextKey))
            {
                Nodes[nextKey] = new Node(nextRow, nextCol);
            }
            var nextNode = Nodes[nextKey];

            //add link from row,col to nextRow,nextCol and vice versa
            currentNode.Links.Add(new Edge(nextNode, split[2].Trim('(').Trim(')')));
            // nextNode.Links.Add( new Edge(currentNode, split[2].Trim('(').Trim(')')));


            row = nextRow;
            col = nextCol;

            line = sr.ReadLine();
        }
    }
    sr.Close();

    int minRow = Nodes.Values.Select(node => node.Row).Min();
    int minCol = Nodes.Values.Select(node => node.Col).Min();
    int maxRow = Nodes.Values.Select(node => node.Row).Max();
    int maxCol = Nodes.Values.Select(node => node.Col).Max();

    Console.WriteLine(new { minRow, minCol, maxRow, maxCol });
    var rowCount = Math.Abs(minRow) + maxRow + 1;
    var colCount = Math.Abs(minCol) + maxCol + 1;

    var rowOffset = -minRow;
    var colOffset = -minCol;

    // char[,] Map = new char[maxRow + 1, maxCol + 1];
    char[,] Map = new char[rowCount, colCount];

    for (row = 0; row < rowCount; row++)
    {
        for (col = 0; col < colCount; col++)
        {
            Map[row, col] = '.';
        }
    }

    //Draw the # into the map
    var startingKey = GenerateKeyN(startingNode);
    var currentNode2 = startingNode;
    var nextNode2 = currentNode2.Links.First().Node;
    do
    {
        if (currentNode2.Row == nextNode2.Row)
        {
            var deltaCol = currentNode2.Col < nextNode2.Col ? 1 : -1;
            // Console.WriteLine($"Drawing line in map. Row {currentNode2.Row}. Col start {currentNode2.Col} Col end {nextNode2.Col} Delta {deltaCol}");
            for (int c = currentNode2.Col; c != nextNode2.Col; c += deltaCol)
            {
                Map[rowOffset + currentNode2.Row, colOffset + c] = '#';
            }
        }
        else
        {
            var deltaRow = currentNode2.Row < nextNode2.Row ? 1 : -1;
            // Console.WriteLine($"Drawing line in map. Col {currentNode2.Col}. Row start {currentNode2.Row} Row end {nextNode2.Row} Delta {deltaRow}");

            for (int r = currentNode2.Row; r != nextNode2.Row; r += deltaRow)
            {
                Map[rowOffset + r, colOffset + currentNode2.Col] = '#';
            }
        }
        currentNode2 = nextNode2;
        nextNode2 = nextNode2.Links.First().Node;
    } while (GenerateKeyN(currentNode2) != startingKey);


    // ok we have a graph
    var length = CalculateTrenchLength(startingNode);
    var volume = CalculateTrenchVolume(Map, rowCount, colCount);
    PrintMap(Map, rowCount, colCount);
    Console.WriteLine(new { length, volume, total = length + volume });
}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

void PrintMap(char[,] map, int rows, int cols)
{
    for (int row = 0; row < rows; row++)
    {
        // var builder = new StringBuilder(cols);
        for (int col = 0; col < cols; col++)
        {
            Console.Write(map[row, col]);
            // builder[col] = map[row,col];
        }
        Console.WriteLine();
    }

}

long CalculateTrenchVolume(char[,] map, int rows, int cols)
{
    long output = 0;

    for (int row = 0; row < rows; row++)
    {
        bool isInside = false;
        bool onEdge = false;
        bool topSet = false;
        // bool isEdge = false;
        for (int col = 0; col < cols - 1; col++)
        {
            //If we cross a wall, toggle the tracking bit
            if (map[row, col] == '#' && map[row, col + 1] == '.' && (col == 0 || map[row, col - 1] == '.' || map[row,col-1]=='*'))
            {
                isInside = !isInside;
                continue;
            }

            //edge start
            if ((map[row, col] == '#') && map[row, col + 1] == '#' && !onEdge)
            {
                onEdge = true;
                topSet = row > 0 && map[row - 1, col] == '#';
                continue;
            }

            //edge end
            if(map[row,col] == '#' && onEdge && map[row,col+1] == '.')
            {
                if(row != 0 && row != rows-1 && ((map[row-1,col] == '#') != topSet))
                {
                    //if the # changed positions, we are on an "inside" edge and need to toggle the tracking bit
                    isInside = !isInside;
                }
                //regardless we reset the onEdge bit
                onEdge = false;
            }

            if (map[row, col] == '.' && isInside)
            {
                output++;
                map[row,col] = '*';
            }
        }
    }

    return output;
}

string GenerateKey(int row, int col)
{
    return $"R{row}C{col}";
}

string GenerateKeyN(Node node)
{
    return GenerateKey(node.Row, node.Col);
}

long CalculateTrenchLength(Node starting)
{
    long total = 0;
    var startingKey = GenerateKeyN(starting);
    var currentNode = starting;
    var nextNode = currentNode.Links.First().Node;
    do
    {
        total += Math.Abs(currentNode.Row - nextNode.Row) + Math.Abs(currentNode.Col - nextNode.Col);
        currentNode = nextNode;
        nextNode = nextNode.Links.First().Node;
    } while (GenerateKeyN(currentNode) != startingKey);
    return total;

}