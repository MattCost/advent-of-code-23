public enum Material
{
    Seed,
    Soil,
    Fertilizer,
    Water,
    Light,
    Temperature,
    Humidity,
    Location

}

public class MaterialMapping
{
    public Material SourceMaterial { get; init; }
    public Material DestinationMaterial { get; init; }
    private Dictionary<int, int> _map { get; init; } = new();

    // public MaterialMapping()
    // {
    //     Map = Enumerable.Range(0,100).ToDictionary(x => x, x => x);
    // }

    public void AddMapping(int dest, int source, int count)
    {
        for(int i=0 ; i<count ; i++)
        {
            _map[source+i] = dest +i;
        }
    }

    public int LookupDestination(int source)
    {
        return _map.ContainsKey(source) ? _map[source] : source;
    }

}