using System.Text;

Console.WriteLine("Hello, World!");

string? line;
List<string> input = new();
long total = 0;
int maps = 0;
// int invalidMaps =0;
try
{
    StreamReader sr = new StreamReader("input.txt");

    Console.WriteLine("starting processing");
    while (!sr.EndOfStream)
    {
        maps++;
        line = sr.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            input.Add(line);
            line = sr.ReadLine();
        }

        //Part 1
        // var horiz = FindHorizontalReflection(input);
        // var vert = FindVerticalReflection(input);

        // if(horiz == -1 && vert == -1)
        // {
        //     invalidMaps++;
        // }


        // total +=
        //     (horiz == -1 ? 0 : 100 * horiz) +
        //     (vert == -1 ? 0 : vert);
        //Part 2

        total += Part2(input);
        input.Clear();
    }

    // Console.WriteLine($"Processed {maps} maps, {invalidMaps} of which had no reflections, for a total of {total}");
    Console.WriteLine($"Processed {maps} maps, for a total of {total}");
    sr.Close();

}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

long Part2(List<string> input)
{
    Console.WriteLine("Finding O.G. reflection points");
    var ogH = FindHorizontalReflection(input,-1);
    var ogV = FindVerticalReflection(input,-1);
    Console.WriteLine($"{new {ogH, ogV}}");
    
    for(int row = 0 ; row < input.Count ; row++)
    {
        for(int col = 0 ; col <input[0].Length ; col++)
        {
            var newInput = ToggleBit(input, row, col);
            var newH = FindHorizontalReflection(newInput,ogH);
            var newV = FindVerticalReflection(newInput, ogV);
            
            
            if( newH == -1 && newV == -1 ) continue;
            if( newH == ogH && newV == ogV) continue;
            long output = 0;
            if(newH != -1)
            {
                if(newH != ogH) output += 100*newH;
            }
            if(newV != -1)
            {
                if(newV != ogV) output += newV;
            }

            Console.WriteLine($"Found new reflection. {new {newH, newV}}. Returning {output}");
            return output;

        }
    }
    input.ForEach(Console.WriteLine);
    Console.WriteLine($"{new {ogH, ogV}}");

    throw new Exception("could not find new mirror  point");

}
List<string> ToggleBit(List<string> input, int row, int col)
{
        string[] output = new string[input.Count];
        input.CopyTo(output);
        var mod = new StringBuilder(output[row]);
        mod[col] = mod[col] == '.' ? '#' : '.';
        output[row] = mod.ToString();
        return output.ToList();
}
long FindHorizontalReflection(List<string> input, long ignore)
{
    //if the last 2 rows are equal, that is the best possible mirror. 
    if (input[^1] == input[^2])
    {
        if(input.Count -1 != ignore)
            return input.Count - 1;
    }

    int mirror = -1;

    //if the first row == the second row, we at least have 1 mirror row
    if (input[0] == input[1])
        mirror = 1;


    for (int i = 1; i < input.Count - 2; i++)
    {
        if (input[i] != input[i + 1]) continue;

        int up = i - 1;
        int down = i + 2;
        bool isMirror = false;
        do
        {
            if (input[up] != input[down])
            {
                break;
            }
            up--;
            down++;
            if (up < 0)
            {
                isMirror = true;
                break;
            }
            if (down >= input.Count)
            {
                isMirror = true;
                break;
            }

        } while (!isMirror);

        if (isMirror )
        {  
            if(ignore == -1 || (i+1) != ignore)
            {
                mirror = i + 1;
            }
        }
    }


    return mirror;
}

long FindVerticalReflection(List<string> input, long ignore)
{
    //transpose input
    // input.ForEach(Console.WriteLine);
    List<string> transpose = new();
    for (int i = 0; i < input[0].Length; i++)
    {
        transpose.Add(new string(input.Select(row => row[i]).ToArray()));
    }
    // transpose.ForEach(Console.WriteLine);
    return FindHorizontalReflection(transpose,ignore);
}