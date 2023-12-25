namespace day19;
public enum ComparisonType
{
    None,
    GreaterThan,
    LessThan
}
public enum AttributeType
{
    X,
    M,
    A,
    S
}
public class SortingRule
{
    public AttributeType Attribute { get; set; }
    public ComparisonType ComparisonType { get; set; }
    public long Threshold { get; set; }
    public string Destination { get; set; }

    public SortingRule(string input)
    {
        //a<2006:qkq
        //rfg
        //A
        if (!input.Contains(':'))
        {
            ComparisonType = ComparisonType.None;
            Destination = input;
        }
        else
        {
            var split = input.Split(':');
            Destination = split[1];
            Attribute = (AttributeType)Enum.Parse(typeof(AttributeType), split[0].Substring(0,1).ToUpper());
            ComparisonType = split[0][1] == '>' ? ComparisonType.GreaterThan : ComparisonType.LessThan; //might have to parse?
            Threshold = long.Parse(split[0].Substring(2));
        }
    }

    public string Eval(Part part)
    {
        switch (ComparisonType)
        {
            case ComparisonType.None:
                return Destination;
            case ComparisonType.GreaterThan:
                if (part.Attributes[Attribute] > Threshold) return Destination;
                break;
            case ComparisonType.LessThan:
                if (part.Attributes[Attribute] < Threshold) return Destination;
                break;
        }
        return "$$$NoMatch$$$";
    }


}
public class Part
{
    public Dictionary<AttributeType, long> Attributes { get; set; } = new();
    public Part(string input)
    {
        var split = input.Trim('{').Trim('}').Split(',');
        foreach (var attribute in split)
        {
            Attributes[(AttributeType)Enum.Parse(typeof(AttributeType), attribute.Substring(0, 1).ToUpper())] = long.Parse(attribute.Substring(2));
        }
    }
}


public class Workflow
{
    public string Name { get; set; }
    public List<SortingRule> SortingRules { get; set; } = new();
    public Workflow(string input)
    {
        //px{a<2006:qkq,m>2090:A,rfg}
        Name = input.Split('{')[0];
        var rules = input.Split('{')[1].Trim('}').Split(',');
        foreach (var rule in rules)
        {
            SortingRules.Add(new SortingRule(rule));
        }
    }
    public string Eval(Part part)
    {
        foreach(var rule in SortingRules)
        {
            var result = rule.Eval(part);
            if(result != "$$$NoMatch$$$") return result;
        }
        return "$$$NoMatch$$$";
    }
}