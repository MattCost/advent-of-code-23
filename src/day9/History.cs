using System.Diagnostics.Contracts;

public class History
{
    public List<int> Readings { get; init; }

    public int Prediction {get { return Readings.First() -_deltas[0].First();}}
    private List<List<int>> _deltas = new();

    public History(List<int> readings)
    {
        Readings = readings;

        var current = readings;
        List<int> deltas;
        do
        {
            deltas = new List<int>();
            for (int i = 1; i < current.Count; i++)
            {
                deltas.Add(current[i] - current[i - 1]);
            }
            _deltas.Add(deltas);
            current = deltas;

        } 
        while (deltas.Where(x => x != 0).Count() > 0);

        Console.WriteLine($"_deltas has {_deltas.Count} rows");

        for(int row = _deltas.Count-2 ; row >= 0 ; row--)
        {
            // Part 1 - predict at end
            // var last = _deltas[row].Last();
            // var prevLast = _deltas[row+1].Last();
            // _deltas[row].Add(last+prevLast);
                    
            var first = _deltas[row].First();
            var prevFirst = _deltas[row+1].First();
            _deltas[row].Insert(0,first-prevFirst);
        }

    }

}