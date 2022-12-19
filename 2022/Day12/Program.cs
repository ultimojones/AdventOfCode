using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

var grid = File.ReadLines("input.txt").SelectMany((line, y) => line.Select((letter, x) =>
    new { x, y, letter })).ToDictionary(g => (X: g.x, Y: g.y),
    g => (Letter: g.letter, Height: (int)g.letter switch { 'S' => 'a', 'E' => 'z', _ => g.letter }));

var start = grid.First(g => g.Value.Letter == 'S');
var end = grid.First(g => g.Value.Letter == 'E');
var gridMinX = grid.Min(g => g.Key.X);
var gridMinY = grid.Min(g => g.Key.Y);
var gridMaxX = grid.Max(g => g.Key.X);
var gridMaxY = grid.Max(g => g.Key.Y);

var selectedRoutes = new ConcurrentDictionary<(int X, int Y), (int X, int Y)[]>();
var routes = new List<(int X, int Y)[]>();
routes.Add(new[] { start.Key });

while (routes.Count > 0 && !selectedRoutes.ContainsKey(end.Key))
{
    var nextRoutes = routes.AsParallel().SelectMany(r =>
    {
        var current = r.Last();
        var nexts = new (int X, int Y)[]
        {
            (current.X - 1, current.Y),
            (current.X + 1, current.Y),
            (current.X, current.Y - 1),
            (current.X, current.Y + 1)
        };

        return nexts.Where(n => n.X >= gridMinX && n.X <= gridMaxX && n.Y >= gridMinY && n.Y <= gridMaxY
            && grid[n].Height - grid[current].Height <= 1)
        .Select(n => new { n, r = r.Append(n).ToArray() })
        .Where(r => selectedRoutes.TryAdd(r.n, r.r))
        .Select(r => r.r);
    });
    routes = nextRoutes.ToList();
}

var final = selectedRoutes[end.Key];

PrintRoute(final);
Console.WriteLine(
    );
void PrintRoute((int X, int Y)[] final)
{
    for (int y = 0; y <= gridMaxY; y++)
    {
        var line = new string(Enumerable.Range(gridMinX, gridMaxX - gridMinX + 1)
            .Select(x =>
            {
                var pos = (x, y);
                if (end.Key == pos)
                {
                    return 'E';
                }
                else if (final.Contains(pos))
                {
                    var index = Array.FindIndex(final, f => f == pos);
                    if (index == final.Length - 1)
                    {
                        return 'X';
                    }
                    else
                    {
                        var nextPos = final[index + 1];
                        switch (nextPos.X - pos.x, nextPos.Y - pos.y)
                        {
                            case (0, -1):
                                return '^';
                                break;
                            case (0, 1):
                                return 'v';
                                break;
                            case (-1, 0):
                                return '<';
                                break;
                            case (1, 0):
                                return '>';
                                break;
                            default:
                                break;
                        }
                    }
                }
                return '.';
            }).ToArray());
        Console.WriteLine(line);
    }
}

while (true)
{
    var rng = new Random();
    var randRoute = selectedRoutes.OrderBy(x => rng.Next()).First();
    PrintRoute(randRoute.Value);
    Console.WriteLine("Press any key...");
    Console.ReadKey(true);
}