public class Row
{
    public string Data {get;set;}
    public List<int> Broken {get;set;} = new();

    public Row(string input)
    {
        var split = input.Split(' ');
        Data = split[0];
        Broken = split[1].Split(',').Select( x => Convert.ToInt32(x)).ToList();
    }

    public int Combos {get;}
}