using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

public class Row
{
    static bool print = false;
    public string InputData { get; set; }
    public int[] BrokenGroupings { get; set; }
    public int PossibleComboCount { get { return PossibleCombos.Count(); } }
    public List<string> PossibleCombos { get; set; } = new();
    public Row(string input)
    {
        var split = input.Split(' ');
        int scale = 5;

        InputData = split[0];
        var numbers = split[1];
        for (int i = 1; i < scale; i++)
        {
            InputData = $"{InputData}?{split[0]}";
            numbers = $"{numbers},{split[1]}";
        }

        BrokenGroupings = numbers.Split(',').Select(x => Convert.ToInt32(x)).ToArray();

        PossibleCombos = CalculateTheCombos(InputData, BrokenGroupings);

        //Validation logic    
        var validCombos = new List<string>();
        Regex validationRegex = new Regex("([#]+)", RegexOptions.Compiled);
        foreach (var combo in PossibleCombos)
        {
            MatchCollection matches = validationRegex.Matches(combo);
            if (matches.Count != BrokenGroupings.Count())
            {
                continue;
                // Console.WriteLine($"!!! Invalid possible combo !!!\nInput:{input}\n{combo}. Wrong amount of #### groups");
                // throw new Exception();
            }
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].Length != BrokenGroupings[i])
                {
                    continue;
                    // Console.WriteLine($"!!! Invalid possible combo !!!\nInput:{input}\n{combo}. Match length did not match grouping");
                    // throw new Exception();
                }
            }
            validCombos.Add(combo);
        }
        PossibleCombos = validCombos;


        if (print) Console.WriteLine($"Pattern: {InputData} Groupings:{string.Join(' ', BrokenGroupings)}");
    }

    // private static int CalculateTheCombos(string input, int[] groupings)
    private static List<string> CalculateTheCombos(string input, int[] groupings)
    {
        if (print) Console.WriteLine($"Entering CalculateTheCombos2. Input:`{input}` Groupings:`{string.Join(',', groupings)}`");

        if (string.IsNullOrEmpty(input))
        {
            if (print) Console.WriteLine("input is empty, returning empty list");
            // return 0; //1 was too high, 0 is still too high
            return new();
        }
        //need to return tuple, bool and list, cus an empty list *could* be valid like the above line
        if (groupings.Sum() + groupings.Count() - 1 > input.Trim('.').Length)
        {
            if (print) Console.WriteLine("Input not long enough to hold all groupings. returning 0");
            // return 0;
            return new();
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
            // return 0;
            return new();
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
                    // return 0;
                    return new();
                }
                var output = $"{input.Substring(0, firstGroupingFirstIndex)}{new string('#', groupings[0])}.";
                var subCombos = CalculateTheCombos(input.Substring(firstGroupingFirstIndex + groupings[0] + 1), groupings[Range.StartAt(1)]);
                return subCombos.Select(subCombo => $"{output}{subCombo}").ToList();
            }
            else
            {
                // if (print) Console.WriteLine("Only 1 possible position, and only 1 grouping. returning 1");
                // return 1;
                var output = $"{input.Substring(0, firstGroupingFirstIndex)}{new string('#', groupings[0])}";
                if (print) Console.WriteLine($"Only 1 possible position, and only 1 grouping. returning\n{output}");
                return new List<string> { output };
            }

        }
        // int combos = 0;
        var combos = new List<string>();
        if (print) Console.WriteLine("More than 1 possible position. Iterating across possible positions");
        for (int i = firstGroupingFirstIndex; i <= firstGroupingLastIndex; i++)
        {
            if (IsValidPosition(input, groupings[0], i, !multipleGroupings))
            {
                if (multipleGroupings)
                {
                    if (i + groupings[0] + 1 < input.Length)
                    {
                        if (print) Console.WriteLine($"checking sub-combos. groupings[0]?: {groupings[0]} input: {input} i: {i} first: {firstGroupingFirstIndex} last:{firstGroupingLastIndex} ");
                        // combos += 1 * CalculateTheCombos(input.Substring(i + groupings[0] + 1), groupings[Range.StartAt(1)]);
                        var subCombos = CalculateTheCombos(input.Substring(i + groupings[0] + 1), groupings[Range.StartAt(1)]);

                        if (print) Console.WriteLine($"we have {subCombos.Count} subCombos to review");
                        if (subCombos.Count > 0)
                        {

                            // var output = $"{input.Substring(0, i)}{new string('#', groupings[0])}{input.Substring(i+groupings[0])}".Replace('?','.');
                            var output = $"{input.Substring(0, i)}{new string('#', groupings[0])}.".Replace('?', '.');
                            combos.AddRange(subCombos.Select(subCombo => $"{output}{subCombo}"));
                            if (print) Console.WriteLine(string.Join('\n', combos));
                        }
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
                    // if (print) Console.WriteLine("Only 1 grouping. adding 1 to combos");
                    // combos++;

                    var output = $"{input.Substring(0, i)}{new string('#', groupings[0])}{input.Substring(i + groupings[0])}".Replace('?', '.');
                    if (print) Console.WriteLine($"Only 1 grouping. adding to combo\n{output}");
                    combos.Add(output);
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

        if (index > 0 && input[index - 1] == '#') return false;

        if (index + groupingSize < input.Length && input[index + groupingSize] == '#') return false;

        bool hasAnchor = includeAllAnchors && input.Contains('#');

        if (hasAnchor)
        {
            //If we want to consume all anchors, and the substring before or after our position have the #, then this position is invalid
            if (input.Substring(0, index).Contains('#')) return false;
            if (input.Substring(index + groupingSize, input.Length - (index + groupingSize)).Contains('#')) return false;
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
            if (IsValidPosition(input, groupingSize, i, includeFirstAnchor))
                return i;
        }
        // Console.WriteLine("No valid position found");
        return -1;
    }

}