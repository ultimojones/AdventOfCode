using System.Drawing;
using System.Text.RegularExpressions;

var sources = File.ReadAllLines("input.txt").Select(line =>
{
    var match = Regex.Match(line, @"^Sensor at x=(?<sensorX>-?\d+), y=(?<sensorY>-?\d+): closest beacon is at x=(?<beaconX>-?\d+), y=(?<beaconY>-?\d+)$");
    var SensorX = int.Parse(match.Groups["sensorX"].Value);
    var SensorY = int.Parse(match.Groups["sensorY"].Value);
    var BeaconX = int.Parse(match.Groups["beaconX"].Value);
    var BeaconY = int.Parse(match.Groups["beaconY"].Value);
    var Range = Math.Abs(SensorX - BeaconX) + Math.Abs(SensorY - BeaconY);
    var MinX = SensorX - Range;
    var MaxX = SensorX + Range;
    var MinY = SensorY - Range;
    var MaxY = SensorY + Range;
    return new
    {
        SensorX,
        SensorY,
        BeaconX,
        BeaconY,
        Range,
        MinX,
        MaxX,
        MinY,
        MaxY,
    };
});

var locals = sources.Select(s => (s.MinX, s.MaxX, s.MinY, s.MaxY, s.SensorX, s.SensorY, s.Range)).ToArray();
var maxSearch = 4000000;

var grid = new HashSet<(int X, int Y)>();

foreach (var point in locals
        .AsParallel()
        .SelectMany(s => Enumerable.Range(0, s.Range * 2 + 3)
        .SelectMany(r =>
        {
            var x = s.MinX - 1 + r;
            var y = r < s.Range + 2 ? r : s.Range * 2 + 2 - r;
            var points = (y != 0) ? new (int X, int Y)[] { (x, s.SensorY - y), (x, s.SensorY + y) }
                                  : new (int X, int Y)[] { (x, s.SensorY - y) };
            return points;
        }))
        .Where(p => p.X >= 0 && p.X <= maxSearch && p.Y >= 0 && p.Y <= maxSearch
                    && !locals.Any(s => Math.Abs(p.X - s.SensorX) + Math.Abs(p.Y - s.SensorY) <= s.Range))
        )
    grid.Add(point);

foreach (var point in grid)
{
    Console.WriteLine(point);
    var result = point.X * 4000000L + point.Y;
    Console.WriteLine(point.X * 4000000L + point.Y);
}

