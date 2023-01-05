using System.Collections;
using System.Drawing;

var queue = new Queue<((int X, int Y) Point, IEnumerable<(int X, int Y)> Path)>();
queue.Enqueue(((1, 1), new[] { (1, 1) }));
var grid = new Dictionary<(int X, int Y), char>();
(int X, int Y)[] finalRoute = Array.Empty<(int, int)>();

const long favnum = 1362;
(int X, int Y) target = (31, 39);

while (queue.TryDequeue(out var cur))
{
    if (grid.ContainsKey(cur.Point))
        continue;

    var typ = GetPointType(cur.Point);
    grid[cur.Point] = typ;

    if (typ == '#')
        continue;
    
    var neighbours = new List<(int X, int Y)>(4) { (cur.Point.X + 1, cur.Point.Y), (cur.Point.X, cur.Point.Y + 1) };
    if (cur.Point.X > 0) neighbours.Add((cur.Point.X - 1, cur.Point.Y));
    if (cur.Point.Y > 0) neighbours.Add((cur.Point.X, cur.Point.Y - 1));

    var curPath = cur.Path.ToArray();
    if (curPath.Length >= 51)
    {
        PrintGrid(cur.Path);
        continue;
    }

    foreach (var point in neighbours)
    {
        queue.Enqueue((point, curPath.Append(point)));
    }
}

PrintGrid(finalRoute);
Console.WriteLine(grid.Count(g => g.Value == '.'));
// 135 too low


char GetPointType((int X, int Y) point)
{
    long x = point.X, y = point.Y;
    long a = (x * x) + (3 * x) + (2 * x * y) + y + (y * y) + favnum;
    var bits = Convert.ToString(a, 2);
    return int.IsEvenInteger(bits.Count(c => c == '1'))  ? '.' : '#';
}

void PrintGrid(IEnumerable<(int X, int Y)> path)
{
    var printPath = path.ToArray();
    for (int y = 0; y < grid.Max(g => g.Key.Y) + 1; y++)
    {
        Console.WriteLine(new string(Enumerable.Range(0, grid.Max(g => g.Key.X) + 1)
            .Select(x => printPath.Contains((x, y)) ? '+' : grid.TryGetValue((x, y), out var value) ? value : ' ').ToArray()));
    }
    Console.WriteLine($"Steps = {printPath.Length - 1}");
    Console.WriteLine();
}