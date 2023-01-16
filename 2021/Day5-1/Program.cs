using System.Text.RegularExpressions;

var lines = File.ReadLines("input.txt").Select(l =>
{
    var r = Regex.Match(l, @"^(\d+),(\d+) -> (\d+),(\d+)$");
    if (!r.Success) throw new InvalidDataException();
    var vals = r.Groups.Values.Skip(1).Select(v => int.Parse(v.Value)).ToArray();
    return (From: (X: vals[0], Y: vals[1]), To: (X: vals[2], Y: vals[3]));
}).Where(l => l.From.X == l.To.X || l.From.Y == l.To.Y).ToArray();

if (lines.Min(l => int.Max(l.From.X, l.To.X)) < 0) throw new NotImplementedException();
if (lines.Min(l => int.Max(l.From.Y, l.To.Y)) < 0) throw new NotImplementedException();
var maxX = lines.Max(l => int.Max(l.From.X, l.To.X));
var maxY = lines.Max(l => int.Max(l.From.Y, l.To.Y));

var count = 0;

for (int x = 0; x < maxX + 1; x++)
{
    for (int y = 0; y < maxY + 1; y++)
    {
        var match = lines.Count(l =>
        {
            var horz = l.From.X == x && l.From.X == l.To.X && y >= int.Min(l.From.Y, l.To.Y) && y <= int.Max(l.From.Y, l.To.Y);
            var vert = l.From.Y == y && l.From.Y == l.To.Y && x >= int.Min(l.From.X, l.To.X) && x <= int.Max(l.From.X, l.To.X);
            return horz || vert;
        });
        if (match > 1) { count++; }
    }
}

Console.WriteLine(count);