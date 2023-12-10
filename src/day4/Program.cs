using System.Numerics;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");


string? line;
double total = 0;
int maxValidCard = 0;
Dictionary<int, int> CardCounts = new();

try
{
    StreamReader sr = new StreamReader("input.txt");
    line = sr.ReadLine();
    while (line != null)
    {
        Console.WriteLine(line);
        var cardId = ExtractCardId(line);
        var winningNumbers = ExtractWinningNumbers(line);
        var myNumbers = ExtractPlayerNumbers(line);

        total += CalculateCardScore(winningNumbers, myNumbers);

        var match = myNumbers.Intersect(winningNumbers);
        var winningCount = match.Count();

        // Count the current card
        CardCounts[cardId] = CardCounts.ContainsKey(cardId) ? CardCounts[cardId] + 1 : 1;

        for (int i = cardId + 1; i <= cardId+winningCount; i++)
        {
            Console.WriteLine($"Adding 1 to card {i}");
            CardCounts[i] = CardCounts.ContainsKey(i) ? CardCounts[i] + CardCounts[cardId] : CardCounts[cardId];
        }
        maxValidCard = cardId;
        line = sr.ReadLine();
    }
    sr.Close();

    Console.WriteLine($"The total score is {total}");
    foreach(var kvp in CardCounts)
    {
        Console.WriteLine($"Card {kvp.Key} : Count {kvp.Value}");
    }
    var totalCards =CardCounts.Where(kvp => kvp.Key<=maxValidCard).Sum( kvp => kvp.Value);
    Console.WriteLine($"You have {totalCards} cards");

}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

double CalculateCardScore(List<int> winningNumbers, List<int> myNumbers)
{
    var match = myNumbers.Intersect(winningNumbers);
    Console.WriteLine($"We have {match.Count()} winning numbers this round");
    return match.Count() > 0 ? Math.Pow(2, match.Count() - 1) : 0;
}

List<int> ExtractPlayerNumbers(string line)
{
    var numbersOnly = line.Split(':');
    var numbersSplit = numbersOnly[1].Trim().Split("|");
    var playerNumbers = numbersSplit[1].Trim().Split(' ');
    return playerNumbers.Where(number => !string.IsNullOrEmpty(number)).Select(number => int.Parse(number.Trim())).ToList();
}

List<int> ExtractWinningNumbers(string line)
{
    var numbersOnly = line.Split(':');
    var numbersSplit = numbersOnly[1].Trim().Split("|");
    var winningNumbers = numbersSplit[0].Trim().Split(' ');
    return winningNumbers.Where(number => !string.IsNullOrEmpty(number)).Select(number => int.Parse(number)).ToList();
}

int ExtractCardId(string line)
{
    var split = line.Split(':');
    return int.Parse(split[0].Substring(4));
}

