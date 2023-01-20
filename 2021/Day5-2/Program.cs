using System.Text.RegularExpressions;

var lines = File.ReadLines("input.txt").Select(l =>
{
    var r = Regex.Match(l, @"^(\d+),(\d+) -> (\d+),(\d+)$");
    if (!r.Success) throw new InvalidDataException();
    var vals = r.Groups.Values.Skip(1).Select(v => int.Parse(v.Value)).ToArray();
    return (X1: vals[0], Y1: vals[1], X2: vals[2], Y2: vals[3]);
}).ToArray();

var points = new List<(int X, int Y)>();

foreach (var l in lines)
{
    for (int x = l.X1, y = l.Y1; ; x += int.Sign(l.X2 - l.X1), y += int.Sign(l.Y2 - l.Y1))
    {
        points.Add((x, y));
        if (x == l.X2 && y == l.Y2) break;
    }
}

var agg = points.GroupBy(p => p).Count(a => a.Count() > 1);
Console.WriteLine(agg);