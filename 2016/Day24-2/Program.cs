var walls = new Dictionary<(int X, int Y), bool>();
var points = new Dictionary<(int X, int Y), char>();

var lines = File.ReadAllLines("input.txt");
for (int y = 0; y < lines.Length; y++)
{
    for (int x = 0; x < lines[y].Length; x++)
    {
        walls.Add((x, y), lines[y][x] == '#');
        if (char.IsAsciiDigit(lines[y][x]))
            points.Add((x, y), lines[y][x]);
    }
}
var maxX = walls.Max(w => w.Key.X);
var maxY = walls.Max(w => w.Key.Y);

PrintGrid();
Console.WriteLine();

void PrintGrid()
{
    for (int y = 0; y < maxY + 1; y++)
    {
        var line = Enumerable.Range(0, maxX + 1).Select(x =>
        {
            return points.TryGetValue((x, y), out var point) ? point
                 : walls[(x, y)] ? '#' : '.';
        }).ToArray();
        Console.WriteLine(new string(line));
    }
}

var routes = new Dictionary<(char From, char To), (int X, int Y)[]>();

foreach (var point in points.Values)
{
    CalcRoutes(point);
}

void CalcRoutes(char name)
{
    var from = points.First(p => p.Value == name).Key;
    var visited = new HashSet<(int X, int Y)>();
    var nexts = new Queue<((int X, int Y) Pos, (int X, int Y)[] Steps)>();
    nexts.Enqueue((from, Array.Empty<(int, int)>()));

    while (nexts.TryDequeue(out var cur))
    {
        if (visited.Contains(cur.Pos))
            continue;
        visited.Add(cur.Pos);
        var steps = cur.Steps.Append(cur.Pos).ToArray();

        if (cur.Pos != from && points.TryGetValue(cur.Pos, out var point))
        {
            routes[(name, point)] = steps;
            routes[(point, name)] = steps.Reverse().ToArray();
        }

        new (int X, int Y)[]
        {
            (cur.Pos.X, cur.Pos.Y - 1),
            (cur.Pos.X, cur.Pos.Y + 1),
            (cur.Pos.X - 1, cur.Pos.Y),
            (cur.Pos.X + 1, cur.Pos.Y),
        }.Where(n => !walls[n]).Except(visited)
            .Select(n => (n, steps))
                .ToList().ForEach(nexts.Enqueue);
    }
}

var start = new[] { '0' };
var paths = CalcPaths(new[] { '0' }, '0', 0, points.Values.Except(start).ToArray()).ToArray();

IEnumerable<(char[] Route, int Length)> CalcPaths(IEnumerable<char> route, char last, int length, IEnumerable<char> nexts)
{
    var count = 0;
    foreach (var next in nexts)
    {
        foreach (var path in CalcPaths(route.Append(next), next, length + routes[(last, next)].Length - 1, nexts.Except(new[] { next })))
        {
            count++;
            yield return path;
        }
    }

    if (count == 0)
        yield return (route.Append('0').ToArray(), length + routes[(last, '0')].Length - 1);
}

var best = paths.MinBy(p => p.Length);
Console.WriteLine(string.Concat(best.Route));
Console.WriteLine(best.Length);

PrintPaths(best.Route);

void PrintPaths(char[] itinerary)
{
    var marks = new Dictionary<(int X, int Y), char>();
    for (int i = 0; i < itinerary.Length - 1; i++)
    {
        var fromNum = itinerary[i];
        var fromPoint = points.First(p => p.Value == fromNum).Key;
        var toNum = itinerary[i + 1];
        var toPoint = points.First(p => p.Value == toNum).Key;
        var route = routes[(fromNum, toNum)];
        marks[fromPoint] = fromNum;
        for (int j = 1; j < route.Length - 1; j++)
        {
            var dir = (route[j + 1].X - route[j].X, route[j + 1].Y - route[j].Y) switch
            {
                ( > 0, _) => '>',
                ( < 0, _) => '<',
                (_, > 0) => 'v',
                (_, < 0) => '^',
                _ => throw new NotImplementedException(),
            };
            marks[route[j]] = dir;
        }
        marks[toPoint] = toNum;
    }

    for (int y = 0; y < maxY + 1; y++)
    {
        var line = Enumerable.Range(0, maxX + 1).Select(x =>
        {
            return marks.TryGetValue((x, y), out var point) ? point
                 : walls[(x, y)] ? '#' : ' ';
        }).ToArray();
        Console.WriteLine(new string(line));
    }

}