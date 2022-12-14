var cave = new HashSet<(int X, int Y)>();
var sand = new HashSet<(int X, int Y)>();

foreach (var line in
    //new[] { "498,4 -> 498,6 -> 496,6", "503,4 -> 502,4 -> 502,9 -> 494,9" }
    File.ReadLines("input.txt")
)
{
    var points = line.Split(" -> ").Select(p => p.Split(',')).Select(p => (X: int.Parse(p[0]), Y: int.Parse(p[1]))).ToArray();
    for (int i = 0; i < points.Length - 1; i++)
    {
        var dirX = Math.Sign(points[i + 1].X - points[i].X);
        var dirY = Math.Sign(points[i + 1].Y - points[i].Y);
        var from = points[i];
        cave.Add(from);
        while (true)
        {
            var next = (from.X + dirX, from.Y + dirY);
            cave.Add(next);
            if (next == points[i + 1])
                break;
            from = next;
        }
    }
}

var minX = cave.Min(x => x.X);
var maxX = cave.Max(x => x.X);
var minY = cave.Min(x => x.Y);
var maxY = cave.Max(x => x.Y);

int units = 0;
while (true)
{
    (int X, int Y) from, next;
    from = (500, 0);

    while (from.Y <= maxY)
    {
        var dests = new[] { (from.X, from.Y + 1), (from.X - 1, from.Y + 1), (from.X + 1, from.Y + 1), (from.X, from.Y) };
        next = dests.First(d => !cave.Contains(d));
        if (next == from) break;
        from = next;
    }
    cave.Add(from);
    sand.Add(from);

    if (from.Y > maxY)
        break;

    units++;
}

for (int y = minY; y <= maxY; y++)
{
    var s = new string(Enumerable.Range(minX, maxX - minX + 1)
        .Select(x => sand.Contains((x, y)) ? 'o' : cave.Contains((x, y)) ? '#' : '.').ToArray());
    Console.WriteLine(s);
}

Console.WriteLine(units);