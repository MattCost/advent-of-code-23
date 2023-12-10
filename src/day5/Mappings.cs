// public enum Material
// {
//     Seed,
//     Soil,
//     Fertilizer,
//     Water,
//     Light,
//     Temperature,
//     Humidity,
//     Location
// }
// public static class DictionaryExtensions
// {
//     public static void AddMapping(this Dictionary<int, int> map, int dest, int source, int count)
//     {
//         for(int i=0 ; i<count ; i++)
//         {
//             map[source+i] = dest +i;
//         }
//     }
//     public static int LookupDestination(this Dictionary<int, int> map, int source)
//     {
//         return map.ContainsKey(source) ? map[source] : source;
//     }
// }

public class MaterialMapping
{
    private class MappingRange
    {
        public long SourceStart {get;set;}
        public long DestinationStart {get;set;}
        public long MappingCount {get;set;}

        public bool IsCovered(long number)
        {
            if(number < SourceStart) return false;
            if(number > SourceStart+MappingCount) return false;
            return true;
        }
        public long MapSource(long number)
        {
            return IsCovered(number) ? DestinationStart + (number-SourceStart) : number;
        }
    }
    
    private List<MappingRange> _mappingRanges = new();

    public void AddMapping(long dest, long source, long count)
    {
        _mappingRanges.Add(new MappingRange{
            SourceStart = source,
            DestinationStart = dest,
            MappingCount = count
        });
    }

    public long LookupDestination(long source)
    {
        var ranges = _mappingRanges.Where( range => range.IsCovered(source));
        return ranges.Any() ? 
            ranges.First().MapSource(source) :
            source;
    }

}