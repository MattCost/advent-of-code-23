
using day19;

Console.WriteLine("Hello, World!");

string? line;


List<Part> parts = new();
Dictionary<string, Workflow> workflows = new();
try
{
    // StreamReader sr = new StreamReader("sample.txt");
    StreamReader sr = new StreamReader("input.txt");

    Console.WriteLine("starting processing");

    while (!sr.EndOfStream)
    {
        line = sr.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            Console.WriteLine(line);
            var workflow = new Workflow(line);
            workflows[workflow.Name]=workflow;
            line = sr.ReadLine();
        }
        line = sr.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            Console.WriteLine(line);
            
            parts.Add(new Part(line));
            line = sr.ReadLine();
        }

    }
    Console.WriteLine($"We have {parts.Count} parts and {workflows.Count} workflows");
    long total = 0;
    foreach(var part in parts)
    {
        string result = workflows["in"].Eval(part);
        while(!ProcessingComplete(result))
        {
            result = workflows[result].Eval(part);
        }
        if(result == "A")
        {
            total += 
                part.Attributes[AttributeType.X] + 
                part.Attributes[AttributeType.M] + 
                part.Attributes[AttributeType.A] + 
                part.Attributes[AttributeType.S];
        }
    }
    Console.WriteLine($"Total: {total}");
}
catch(Exception e)
{
    Console.WriteLine(e.Message);

}

bool ProcessingComplete(string result)
{
    if(result == "A" ) return true;
    if(result == "R" ) return true;
    return false;
}