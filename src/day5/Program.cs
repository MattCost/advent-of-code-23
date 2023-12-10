using System.Numerics;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");


string? line;
// double total = 0;
MaterialMapping seedSoilMap = new MaterialMapping { SourceMaterial = Material.Seed, DestinationMaterial = Material.Soil };

try
{
    StreamReader sr = new StreamReader("sample.txt");
    var seedsLine = sr.ReadLine();

    _ = sr.ReadLine(); //the blank
    
    line = sr.ReadLine();
    if(line == "seed-to-soil map:")
    {
        Console.WriteLine("Loading seed to soil map");
        line = sr.ReadLine();
        while(!string.IsNullOrEmpty(line))
        {
            Console.WriteLine($"Processing {line}:");
            var split = line.Split(' ');
            seedSoilMap.AddMapping( int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
            line = sr.ReadLine();
        }
        Console.WriteLine("Finished loading seed to soil map");
    }

    sr.Close();

    Console.WriteLine($"SeedToSoil.Map(96):{seedSoilMap.LookupDestination(96)}");
    Console.WriteLine($"SeedToSoil.Map(97):{seedSoilMap.LookupDestination(97)}");
    Console.WriteLine($"SeedToSoil.Map(98):{seedSoilMap.LookupDestination(98)}");
    Console.WriteLine($"SeedToSoil.Map(99):{seedSoilMap.LookupDestination(99)}");
}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}
