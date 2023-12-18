using System.Text;

Console.WriteLine("Hello, World!");

string? line;
List<string> input = new();

try
{
    StreamReader sr = new StreamReader("input.txt");

    Console.WriteLine("starting processing");
    while (!sr.EndOfStream)
    {
        line = sr.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            input.Add(line);
            line = sr.ReadLine();
        }
    }

    sr.Close();

    Dictionary<string, int> _cache = new();
    for (int i = 0; i < 1000000000 ; i++) //1000000000
    {
        var starting = System.Text.Json.JsonSerializer.Serialize(input);
        if(_cache.ContainsKey(starting))
        {
            Console.WriteLine($"Cycle Detected. This starting condition was last seen seen on cycle {_cache[starting]}. This is cycle {i}");
            var period = i - _cache[starting];
            var cycles = Math.DivRem(1000000000-_cache[starting], period, out var remainder);
            Console.WriteLine($"There will be {cycles} iterations with a remainder of {remainder}");
            i = 1000000000 - remainder;
            _cache.Clear();
            // break;
        }
        //3 - start
        //4
        //5
        //6
        //7
        //8
        //9
        //10 - start
        //Period = i - start
        //(1000000000 - start) / period
        //remainder is how many more cycles we need to run
        _cache[starting] = i;
        //Tilt North
        var northTemp = Transpose(input);
        BubbleUp(northTemp);
        input = Transpose(northTemp);

        //Tilt West
        BubbleUp(input);

        //Tilt South
        input.Reverse();
        var southTemp = Transpose(input);
        BubbleUp(southTemp);
        input = Transpose(southTemp);
        input.Reverse();
        
        // Tilt East
        var eastTemp1 = Transpose(input);
        eastTemp1.Reverse();
        var eastTemp2 = Transpose(eastTemp1);
        
        BubbleUp(eastTemp2);

        var eastTemp3 = Transpose(eastTemp2);
        eastTemp3.Reverse();
        input = Transpose(eastTemp3);
    
        var ending = System.Text.Json.JsonSerializer.Serialize(input);
        Console.WriteLine($"Cycle {(i+1).ToString("N0")} complete");
        // Console.WriteLine(starting);
        // Console.WriteLine(ending);
        if(starting == ending)
        {
            Console.WriteLine("Stable!");
            break;
        }

        // input.ForEach(Console.WriteLine);
    }

    Console.WriteLine(CalculateLoad(input));

    

}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

long CalculateLoad(List<string> input)
{
    long total = 0;
    for (int i = 0; i < input.Count; i++)
    {
        var count = input[i].Where(c => c == 'O').Count();
        total += count * (input.Count - i);
    
    }
    return total;
}


List<string> Transpose(List<string> input)
{
    //Transpose input
    List<string> transpose = new();
    for (int i = 0; i < input[0].Length; i++)
    {
        transpose.Add(new string(input.Select(row => row[i]).ToArray()));
    }

    // transpose.ForEach(Console.WriteLine);
    return transpose;
}

void BubbleUp(List<string> input)
{
    for (int i = 0; i < input.Count; i++)
    {
        input[i] = BubbleRow(input[i]);
    }
}

string BubbleRow(string input)
{

    var working = new StringBuilder(input);
    bool swapped = false;
    do
    {
        swapped = false;
        for (int i = 1; i < working.Length; i++)
        {
            if (working[i] == '#') continue;
            if (working[i] == 'O' && working[i - 1] == '.')
            {
                working[i - 1] = 'O';
                working[i] = '.';
                swapped = true;
            }
        }
    }
    while (swapped);
    // Console.WriteLine(new { input, bubbled = working.ToString() });
    return working.ToString();
}