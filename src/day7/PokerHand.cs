using System.Net.Http.Headers;

public enum HandTypes
{
    HighCard = 1, //5
    OnePair = 2, //4
    TwoPair = 3,  //3
    ThreeOfAKind = 4, //3
    FullHouse = 5, //2
    FourOfAKind = 6, //2
    FiveOfAKind = 7 //1
}

public class PokerHand : IComparable<PokerHand>
{
    public PokerHand(string cards, string wager)
    {
        Wager = int.Parse(wager);
        _cards = cards;
        var chars = cards.ToCharArray().Order().ToArray();

        var uniqueCount = chars.Distinct().Count();


        if (cards.Contains('J') && uniqueCount > 1)
            uniqueCount--;

        int largestCount = 0;
        var lc = chars.Distinct().Where(c => c != 'J').Select(c => chars.Count(c2 => c2 == c));
        if (lc.Any())
        {
            largestCount = lc.Order().Last();
            if (cards.Contains('J'))
            {
                largestCount += chars.Count(c => c == 'J');
            }
        }
        else
        {
            //must be all J
            largestCount = 5;
        }

        switch (uniqueCount)
        {
            case 5:
                Rank = HandTypes.HighCard;
                break;
            case 4:
                Rank = HandTypes.OnePair;
                break;
            case 3:
                //two pair or 3 of a kind
                Rank = largestCount == 2 ? HandTypes.TwoPair : HandTypes.ThreeOfAKind;
                break;

            case 2:
                //full house or 4 of a kind
                Rank = largestCount == 4 ? HandTypes.FourOfAKind : HandTypes.FullHouse;
                break;

            case 1:
                Rank = HandTypes.FiveOfAKind;
                break;
            case 0:
            default:
                throw new Exception();
        }

    }

    public int Wager { get; private init; }

    private string _cards;
    public HandTypes Rank { get; private init; }

    private static int CompareChar(char left, char right)
    {
        var lookup = "J23456789TQKA";
        var leftIndex = lookup.IndexOf(left);
        var rightIndex = lookup.IndexOf(right);
        return (leftIndex == rightIndex) ?
            0 :
            leftIndex < rightIndex ? 1 : -1;
    }

    public bool BeatsHand(PokerHand other)
    {
        if (this.Rank == other.Rank)
        {
            for (int i = 0; i < _cards.Length; i++)
            {
                if (PokerHand.CompareChar(_cards[i], other._cards[i]) == 0)
                {
                    continue;
                }
                return PokerHand.CompareChar(_cards[i], other._cards[i]) == -1 ? true : false;

            }
            return false; //if we got here they are tied?
        }
        else
        {
            return this.Rank > other.Rank;
        }
    }

    public int CompareTo(PokerHand? other)
    {
        if (other == null) return -1;
        return this.BeatsHand(other) ? -1 : 1;
    }

    public override string ToString()
    {
        return $"{_cards} - rank {this.Rank}";
    }
}