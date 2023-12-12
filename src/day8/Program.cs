
Console.WriteLine("Hello, World!");


string? line;
var network = new Dictionary<string, Node>();
try
{
    StreamReader sr = new StreamReader("input.txt");
    var instructionLine = sr.ReadLine() ?? throw new Exception("bad input");
    _ = sr.ReadLine();
    line = sr.ReadLine();
    while (line != null)
    {
        Console.WriteLine(line);

        var split = line.Split('=');
        var name = split[0].Trim();
        var left = split[1].Replace("(", string.Empty).Replace(")", string.Empty).Split(',')[0].Trim();
        var right = split[1].Replace("(", string.Empty).Replace(")", string.Empty).Split(',')[1].Trim();
        Console.WriteLine($"{new { name, left, right }}");
        var node = new Node
        {
            Name = name,
            Left = left,
            Right = right
        };
        network[name] = node;

        line = sr.ReadLine();
    }
    sr.Close();

    var startingNodeNames = network.Keys.Where(key => key.EndsWith("A")).ToList();
    int moves = 0;
    int totalNodes = startingNodeNames.Count;
    var currentNodes = startingNodeNames.Select(nodeName => network[nodeName]).ToList();
    var zedHops = new Dictionary<int, int>();
    foreach (var current in currentNodes)
    {
        Console.WriteLine(current.Name);
    }

    while (currentNodes.Where(node => node.Name.EndsWith("Z")).Count() != totalNodes)
    {
        for (int moveItr = 0; moveItr < instructionLine.Length; moveItr++)
        {
            if(instructionLine[moveItr]=='R')
            {
                currentNodes = currentNodes.Select( node => network[node.Right]).ToList();
            }
            else
            {
                currentNodes = currentNodes.Select( node => network[node.Left]).ToList();
            }

            moves++;
            for(int i =0 ; i<totalNodes ; i++)
            {
                if(currentNodes[i].Name.EndsWith("Z") && !zedHops.ContainsKey(i))
                {
                    zedHops[i]=moves;
                }
            }
            if(zedHops.Keys.Count==totalNodes) break;
        }
        if(zedHops.Keys.Count==totalNodes) break;
    }
    Console.WriteLine($"zedHops has {totalNodes} entries");
    foreach(var kvp in zedHops)
    {
        Console.WriteLine($"key:{kvp.Key} value:{kvp.Value}");
    }
    var lcm = Node.LCM( zedHops.Values.Select( x => (long)x).ToArray());
    Console.WriteLine($"Made it to zzz in {lcm} moves");
}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

