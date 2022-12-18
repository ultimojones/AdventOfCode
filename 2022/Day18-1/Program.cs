var grid = new HashSet<(int X, int Y, int Z)>();

foreach (var line in File.ReadLines("input.txt"))
{
    var x = line.Split(',').Select(int.Parse).ToArray();
    grid.Add((x[0], x[1], x[2]));
}

var total = grid.Sum(g =>
    new[]
    {
        (g.X + 1, g.Y, g.Z),
        (g.X - 1, g.Y, g.Z),
        (g.X, g.Y + 1, g.Z),
        (g.X, g.Y - 1, g.Z),
        (g.X, g.Y, g.Z + 1),
        (g.X, g.Y, g.Z - 1),
    }.Except(grid).Count()
);

Console.WriteLine(total);









