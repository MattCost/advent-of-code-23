using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

public class Row
{
    static bool print = false;
    public string InputData { get; set; }
    public int[] BrokenGroupings { get; set; }
    public int PossibleComboCount { get; private set; }

    public Row(string input)
    {
        var split = input.Split(' ');
        InputData = split[0];
        BrokenGroupings = split[1].Split(',').Select(x => Convert.ToInt32(x)).ToArray();


        PossibleComboCount = CalculateTheCombos(InputData, BrokenGroupings);
        if (print) Console.WriteLine($"Pattern: {InputData} Groupings:{string.Join(' ', BrokenGroupings)}");
    }

    private static int CalculateTheCombos(string input, int[] groupings)
    {
        if (print) Console.WriteLine($"Entering CalculateTheCombos2. Input:`{input}` Groupings:`{string.Join(',', groupings)}`");

        if (string.IsNullOrEmpty(input))
        {
            if (print) Console.WriteLine("input is empty, returning 1?");
            return 0; //1 was too high, 0 is still too high
        }
        if (groupings.Sum() + groupings.Count() - 1 > input.Trim('.').Length)
        {
            if (print) Console.WriteLine("Input not long enough to hold all groupings. returning 0");
            return 0;
        }

        // if( input.Select(c=> c == '#').Count() == 2 && groupings.Count() == 1)
        // {
        //     //if we have 2 anchors, but only 1 grouping, our grouping need to cover both.
        //     var first = input.IndexOf('#');
        //     var second = input.LastIndexOf('#');
        //     if(second - first > groupings[0]) return 0;
        // }

        if (print) Console.WriteLine($"Searching for the first index of `{groupings[0]}`");
        int firstGroupingFirstIndex = FindFirstIndexOf(input, groupings[0], groupings.Count() == 1);
        if (firstGroupingFirstIndex == -1)
        {
            if (print) Console.WriteLine("No valid position found, returning 0");
            return 0;
        }
        else
        {
            if (print) Console.WriteLine($"First Index of {groupings[0]} found at {firstGroupingFirstIndex}");
        }

        // string second = input;
        bool multipleGroupings = false;
        if (groupings.Count() > 1)
        {
            multipleGroupings = true;
            // second = input.Substring(0, input.Length -(groupings[Range.StartAt(1)].Count() + groupings[Range.StartAt(1)].Sum() ));
        }

        if (print) Console.WriteLine($"Searching for the last index of `{groupings[0]}`");
        int firstGroupingLastIndex = FindLastIndexOf(input, groupings[0], groupings.Count() == 1);
        if (print) Console.WriteLine($"Last Index of {groupings[0]} found at {firstGroupingLastIndex}");


        if (firstGroupingFirstIndex == firstGroupingLastIndex)
        {
            if (multipleGroupings)
            {
                if (firstGroupingFirstIndex + groupings[0] + 1 > input.Length)
                {
                    if (print) Console.WriteLine("Not enough string left for the rest of the groupings. returning 0");
                    return 0;
                }
                return CalculateTheCombos(input.Substring(firstGroupingFirstIndex + groupings[0] + 1), groupings[Range.StartAt(1)]);
            }
            else
            {
                if (print) Console.WriteLine("Only 1 possible position, and only 1 grouping. returning 1");
                return 1;
            }

        }
        int combos = 0;
        if (print) Console.WriteLine("More than 1 possible position. Iterating across possible positions");
        for (int i = firstGroupingFirstIndex; i <= firstGroupingLastIndex; i++)
        {
            if (IsValidPosition(input, groupings[0], i,!multipleGroupings))
            {
                if (multipleGroupings)
                {
                    if (i + groupings[0] + 1 < input.Length)
                    {
                        if (print) Console.WriteLine($"checking sub-combos. groupings[0]?: {groupings[0]} input: {input} i: {i} first: {firstGroupingFirstIndex} last:{firstGroupingLastIndex} ");
                        combos += 1 * CalculateTheCombos(input.Substring(i + groupings[0] + 1), groupings[Range.StartAt(1)]);
                    }
                    else
                    {
                        //we hit the end before using up all the groupings. So this is invalid, and anything to the right would be invalid, return the current count
                        if (print) Console.WriteLine("At the end of the string. no need to check the rest of the sub combos, retuning combos");
                        return combos;
                    }
                }
                else
                {
                    if (print) Console.WriteLine("Only 1 grouping. adding 1 to combos");
                    combos++;
                }
            }
        }

        return combos;
    }

    private static bool IsValidPosition(string input, int groupingSize, int index, bool includeAllAnchors)
    {
        // Console.WriteLine($"IsValidPosition `{input}` [{groupingSize}] Starting at {index}");
        if (input.Length < groupingSize) return false;
        if (index > (input.Length - groupingSize)) return false;

        var substring = input.Substring(index, groupingSize);

        if (substring.Contains('.')) return false;
        //if index > 0
        if (index > 0 && input[index - 1] == '#') return false;
        if (index + groupingSize < input.Length && input[index + groupingSize] == '#') return false;
        // Console.WriteLine("true");
        bool hasAnchor = includeAllAnchors && input.Contains('#');
        // if (hasAnchor && !substring.Contains('#')) return false;
        if( hasAnchor)
        {
            if(input.Substring(0, index).Contains('#')) return false;
            if(input.Substring(index + groupingSize, input.Length - (index+groupingSize)).Contains('#') )return false;
        }
        return true;
    }

    private static int FindLastIndexOf(string input, int groupingSize, bool includeFirstAnchor)
    {
        // Console.WriteLine($"Entering FindLastIndexOf `{input}` GroupingSize {groupingSize}");
        var reversed = FindFirstIndexOf(new string(input.Reverse().ToArray()), groupingSize, includeFirstAnchor);
        // Console.WriteLine($"FindFirstIndexOf on the reversed string returned:{reversed}");
        return reversed == -1 ? -1 : input.Length - reversed - groupingSize;

    }

    private static int FindFirstIndexOf(string input, int groupingSize, bool includeFirstAnchor)
    {
        // Console.WriteLine($"Entering FirstFirstIndexOf `{input}` [{groupingSize}]");
        if (input.Length < groupingSize)
        {
            // Console.WriteLine("input len < groupingSize, -1");
            return -1;
        }

        if (input.Length == groupingSize)
        {
            // Console.WriteLine("input len == groupingSize, if any dots, returning -1");
            return input.Contains('.') ? -1 : 0;
        }

        for (int i = 0; i <= input.Length - groupingSize; i++)
        {
            if (IsValidPosition(input, groupingSize, i,includeFirstAnchor))
                return i;
        }
        // Console.WriteLine("No valid position found");
        return -1;
    }

}