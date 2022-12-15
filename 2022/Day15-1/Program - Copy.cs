using System.Text.RegularExpressions;

var sources = File.ReadAllLines("input.txt").Select(line =>
{
    var match = Regex.Match(line, @"^Sensor at x=(?<sensorX>-?\d+), y=(?<sensorY>-?\d+): closest beacon is at x=(?<beaconX>-?\d+), y=(?<beaconY>-?\d+)$");
    return new
    {
        SensorX = int.Parse(match.Groups["sensorX"].Value),
        SensorY = int.Parse(match.Groups["sensorY"].Value),
        BeaconX = int.Parse(match.Groups["beaconX"].Value),
        BeaconY = int.Parse(match.Groups["beaconY"].Value),
    };
}).ToList();

sources.ForEach(Console.WriteLine);

var grid = new HashSet<(int X, int Y)>();

foreach (var source in sources)
{
    var range = Math.Abs(source.SensorX - source.BeaconX) + Math.Abs(source.SensorY - source.BeaconY);

    for (int x = source.SensorX - range, y = 0; x < source.SensorX + range + 1; x++, y += x <= source.SensorX ? 1 : -1)
    {
        foreach (var point in Enumerable.Range(source.SensorY - y, y * 2 + 1).Select(y => (x, y))) 
            grid.Add(point);
    }
}

var minX = grid.Min(p => p.X);
var maxX = grid.Max(p => p.X);
for (int y = grid.Min(p => p.Y); y < grid.Max(p => p.Y); y++)
{
    var s = new string(Enumerable.Range(minX, maxX - minX + 1)
        .Select(x => sources.Exists(s => s.SensorX == x && s.SensorY == y) ? 'S'
                   : sources.Exists(s => s.BeaconX == x && s.BeaconY == y) ? 'B'
                   : grid.Contains((x, y)) ? '#'
                   : '.').ToArray());
    Console.WriteLine(s);
}
