using System.ComponentModel;

var lava = new HashSet<(int X, int Y, int Z)>();

foreach (var line in File.ReadLines("input.txt"))
{
    var x = line.Split(',').Select(int.Parse).ToArray();
    lava.Add((x[0], x[1], x[2]));
}

var minX = lava.Min(g => g.X) - 1;
var minY = lava.Min(g => g.Y) - 1;
var minZ = lava.Min(g => g.Z) - 1;

var maxX = lava.Max(g => g.X) + 1;
var maxY = lava.Max(g => g.Y) + 1;
var maxZ = lava.Max(g => g.Z) + 1;

var air = new HashSet<(int X, int Y, int Z)>();
var nextGrids = new Queue<(int X, int Y, int Z)>();
nextGrids.Enqueue((minX, minY , minZ));

while (nextGrids.TryDequeue(out var point))
{
    if (air.Contains(point)) continue;

    air.Add(point);
    foreach (var next in
        new (int X, int Y, int Z)[]
        {
            (point.X + 1, point.Y, point.Z),
            (point.X - 1, point.Y, point.Z),
            (point.X, point.Y + 1, point.Z),
            (point.X, point.Y - 1, point.Z),
            (point.X, point.Y, point.Z + 1),
            (point.X, point.Y, point.Z - 1),
        }.Where(p => p.X >= minX && p.X <= maxX && p.Y >= minY && p.Y <= maxY && p.Z >= minZ && p.Z <= maxZ)
            .Except(air).Except(lava))
    {
        nextGrids.Enqueue(next);
    }
}

var total = lava.Sum(g =>
    new[]
    {
        (g.X + 1, g.Y, g.Z),
        (g.X - 1, g.Y, g.Z),
        (g.X, g.Y + 1, g.Z),
        (g.X, g.Y - 1, g.Z),
        (g.X, g.Y, g.Z + 1),
        (g.X, g.Y, g.Z - 1),
    }.Intersect(air).Count()
);

Console.WriteLine(total);

