var lines = File.ReadAllLines("input.txt");
var grid = new List<(int X, int Y)>();
var split = Array.IndexOf(lines, string.Empty);
foreach (var line in lines[..split])
{
    var vals = line.Split(',').Select(int.Parse).ToArray();
    grid.Add((vals[0], vals[1]));
}

void PrintGrid()
{
    var xMax = grid.Max(x => x.X);
    var yMax = grid.Max(y => y.Y);
    for (int y = 0; y < yMax + 1; y++)
    {
        Console.WriteLine(Enumerable.Range(0, xMax + 1).Select(x => grid.Contains((x, y)) ? '#' : '.').ToArray());
    }
    Console.WriteLine();
}

PrintGrid();

foreach (var fold in lines[(split + 1)..])
{
    var dir = fold[11];
    var pos = int.Parse(fold[13..]);

    var from = grid.Where(g => (dir == 'y' && g.Y > pos) || (dir == 'x' && g.X > pos)).ToArray();
    foreach (var f in from)
    {
        var to = dir switch
        {
            'y' => (f.X, f.Y - 2 * (f.Y - pos)),
            'x' => (f.X - 2 * (f.X - pos), f.Y),
        };
        if (!grid.Contains(to)) { grid.Add(to); }
        grid.Remove(f);
    }

    PrintGrid();

    Console.WriteLine(grid.Count);
    break;
}