
Console.WriteLine("Hello, World!");


string? line;
List<PokerHand> pokerHands = new();

try
{
    StreamReader sr = new StreamReader("input.txt");
    line = sr.ReadLine();
    while (line != null)
    {
        Console.WriteLine(line);

        pokerHands.Add(new PokerHand(line.Split(' ')[0], line.Split(' ')[1]));

        line = sr.ReadLine();
    }
    sr.Close();

    var sorted = pokerHands.Order().Reverse().ToList();
    Console.WriteLine($"We have {sorted.Count()} hands");
    long total = 0;
    for (int i = 0; i < sorted.Count; i++)
    {
        total += sorted[i].Wager * (i + 1);
        Console.WriteLine($"Hand #{i}: {sorted[i]} : Wager {sorted[i].Wager} Winning {sorted[i].Wager*(i+1)}");
    }
    
    Console.WriteLine($"Total: {total}");
}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

