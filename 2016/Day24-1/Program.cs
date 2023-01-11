using System.Diagnostics;

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

var routes = new Dictionary<(char From, char To), int>();

foreach (var point in points.Values)
{
    CalcRoutes(point);
}

void CalcRoutes(char name)
{
	var from = points.First(p => p.Value == name).Key;
	var visited = new HashSet<(int X, int Y)>();
	var nexts = new Queue<((int X, int Y) Pos, int Dist)>();
	nexts.Enqueue((from, 0));

	while (nexts.TryDequeue(out var cur))
	{
		if (visited.Contains(cur.Pos))
			continue;
		visited.Add(cur.Pos);

		if (cur.Dist > 0 && points.TryGetValue(cur.Pos, out var point))
		{
			var routeAB = (name, point);
			var routeBA = (point, name);
			if (routes.TryGetValue(routeAB, out var oldAB) && oldAB != cur.Dist) { Debug.Assert(false); }
			if (routes.TryGetValue(routeBA, out var oldBA) && oldBA != cur.Dist) { Debug.Assert(false); }
			routes[routeAB] = cur.Dist;
			routes[routeBA] = cur.Dist;
		}

		new (int X, int Y)[]
		{
			(cur.Pos.X, cur.Pos.Y - 1),
			(cur.Pos.X, cur.Pos.Y + 1),
			(cur.Pos.X - 1, cur.Pos.Y),
			(cur.Pos.X + 1, cur.Pos.Y),
		}.Where(n => !walls[n]).Except(visited)
			.Select(n => (n, cur.Dist + 1))
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
		foreach (var path in CalcPaths(route.Append(next), next, length + routes[(last, next)], nexts.Except(new[] { next })))
		{
			count++;
			yield return path;
		}
	}

	if (count == 0)
		yield return (route.ToArray(), length);
}

var best = paths.MinBy(p => p.Length);
Console.WriteLine(string.Concat(best.Route));
Console.WriteLine(best.Length);
