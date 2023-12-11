using System.Numerics;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");



try
{
    StreamReader sr = new StreamReader("sample.txt");
    var timeLine = sr.ReadLine();
    if(string.IsNullOrEmpty(timeLine)) throw new Exception("bad input");
    var distanceLine = sr.ReadLine();
    if(string.IsNullOrEmpty(distanceLine)) throw new Exception("bad input");

    var times = timeLine.Split(':')[1].Trim().Split(' ').Where( x => !string.IsNullOrEmpty(x)).Select( int.Parse).ToList();
    var distances = distanceLine.Split(':')[1].Trim().Split(' ').Where( x => !string.IsNullOrEmpty(x)).Select( int.Parse).ToList();
    var winningCombos = new List<int>();
    if(times.Count != distances.Count) throw new Exception("bad input");

    for(int i=0 ; i<times.Count ; i++)
    {
        var winningDistances = Enumerable.Range(0, times[i]).Select( time => CalculateDistance(time, times[i])).Where( distance => distance.Item2 > distances[i]).Select( item => item.Item1);
        winningCombos.Add(winningDistances.Count());
    }
    
    long total = winningCombos[0];
    for(int i=1 ; i <winningCombos.Count ; i++)
    {
        total *= winningCombos[i];
    }
    Console.WriteLine($"Margin of error {total}");
    sr.Close();

    Console.WriteLine("Part 2");

    sr = new StreamReader("input.txt");
    timeLine = sr.ReadLine();
    if(string.IsNullOrEmpty(timeLine)) throw new Exception("bad input");
    distanceLine = sr.ReadLine();
    if(string.IsNullOrEmpty(distanceLine)) throw new Exception("bad input");
    var time = long.Parse(timeLine.Split(':')[1].Replace(" ", string.Empty));
    var distance = long.Parse(distanceLine.Split(':')[1].Replace(" ", string.Empty));
    
    Console.WriteLine($"{ new {time, distance}}");

    //Distance = TimeHeld * (RaceTime - TimeHeld)
    //Distance = TimeHeld*RaceTime - TimeHeld^2;
    //-TimeHeld^2 + RaceTime*TimeHeld - Distance = 0
    //x = (-b +/- Sqrt(b^2 - 4ac)) / 2a
    //TimeHeld = (-RaceTime +/- Sqrt(RaceTime^2 - 4(-1)(-Distance))) / 2(-1)
    var fart = Math.Sqrt( Math.Pow(time, 2) - 4.0*(-1.0)*(-distance));
    var lowEnd = Math.Floor(((-time)+fart) / -2.0)+1;
    var highEnd = Math.Floor(((-time)-fart) / -2.0);
    Console.WriteLine($"{ new {lowEnd, highEnd}} for a total of {highEnd-lowEnd+1}");


}
catch (Exception e)
{
    Console.WriteLine("Exception: " + e);
}



(int,int) CalculateDistance(int timeHeld, int raceTime)
{
    return (timeHeld, timeHeld*(raceTime - timeHeld));

}