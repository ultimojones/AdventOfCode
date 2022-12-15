using System.Text.RegularExpressions;

var sources = File.ReadAllLines("input.txt").Select(line =>
{
    var match = Regex.Match(line, @"^Sensor at x=(?<sensorX>-?\d+), y=(?<sensorY>-?\d+): closest beacon is at x=(?<beaconX>-?\d+), y=(?<beaconY>-?\d+)$");
    var SensorX = int.Parse(match.Groups["sensorX"].Value);
    var SensorY = int.Parse(match.Groups["sensorY"].Value);
    var BeaconX = int.Parse(match.Groups["beaconX"].Value);
    var BeaconY = int.Parse(match.Groups["beaconY"].Value);
    return new
    {
        SensorX,
        SensorY,
        BeaconX,
        BeaconY,
        Range = Math.Abs(SensorX - BeaconX) + Math.Abs(SensorY - BeaconY)
    };
}).ToList();

sources.ForEach(Console.WriteLine);

var minX = sources.Min(s => s.SensorX);
var maxX = sources.Max(s => s.SensorX);
var maxR = sources.Max(s => s.Range);
var y = 2000000;
var count = 0;

for (int x = minX - maxR; x < maxX + maxR; x++)
{
    if (!sources.Exists(s => s.BeaconX == x && s.BeaconY == y))
        if (sources.Any(s => Math.Abs(x - s.SensorX) + Math.Abs(y - s.SensorY) <= s.Range))
            count++;
}

Console.WriteLine(count);