using System.Numerics;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");


try
{
    StreamReader sr = new StreamReader("input.txt");
    var seedsLine = sr.ReadLine();
    if(string.IsNullOrEmpty(seedsLine)) throw new Exception("bad input file");

    _ = sr.ReadLine(); //the blank after seeds
    
    Console.WriteLine($"Loading {sr.ReadLine()}");
    var seedSoilMap = LoadMap(sr);

    Console.WriteLine($"Loading {sr.ReadLine()}");
    var soilFertMap = LoadMap(sr);

    Console.WriteLine($"Loading {sr.ReadLine()}");
    var fertWaterMap = LoadMap(sr);

    Console.WriteLine($"Loading {sr.ReadLine()}");
    var waterLightMap = LoadMap(sr);

    Console.WriteLine($"Loading {sr.ReadLine()}");
    var lightTempMap = LoadMap(sr);

    Console.WriteLine($"Loading {sr.ReadLine()}");
    var tempHumidMap = LoadMap(sr);

    Console.WriteLine($"Loading {sr.ReadLine()}");
    var HumidLocMap = LoadMap(sr);

    sr.Close();

    var seedPairs = seedsLine.Split(':')[1].Trim().Split(' ');
    var locations = new List<long>();
    for(int i=0 ; i<seedPairs.Length ; i+=2)
    {
        var firstSeed = long.Parse(seedPairs[i]);
        var seedCount = long.Parse(seedPairs[i+1]);

        for(long seed = firstSeed ; seed < firstSeed+seedCount ; seed++)
        {
            var soil = seedSoilMap.LookupDestination(seed);
            var fert = soilFertMap.LookupDestination(soil);
            var water = fertWaterMap.LookupDestination(fert);
            var light = waterLightMap.LookupDestination(water);
            var temp = lightTempMap.LookupDestination(light);
            var humid = tempHumidMap.LookupDestination(temp);
            var location = HumidLocMap.LookupDestination(humid);
            locations.Add(location);
            Console.WriteLine($"{new {seed, soil, fert, water, light, temp, humid, location}}");
        }
    }
    Console.WriteLine($"The closest location is {locations.Min()}");

}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}

MaterialMapping LoadMap(StreamReader sr)
{
    var line = sr.ReadLine();
    var output = new MaterialMapping();
    while(!string.IsNullOrEmpty(line))
    {
        var split = line.Split(' ');
        output.AddMapping( long.Parse(split[0]), long.Parse(split[1]), long.Parse(split[2]));
        line = sr.ReadLine();
    }
    return output;

}