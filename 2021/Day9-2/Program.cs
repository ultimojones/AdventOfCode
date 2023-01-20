var lines = File.ReadAllLines("input.txt");
var grid = new Dictionary<(int X, int Y), int>();
for (int y = 0; y < lines.Length; y++)
{
	for (int x = 0; x < lines[y].Length; x++)
	{
		grid[(x, y)] = int.Parse(lines[y][x..(x + 1)]);
	}
}
var xMax = grid.Max(g => g.Key.X);
var yMax = grid.Max(g => g.Key.Y);

IEnumerable<(int X, int Y)> GetAdjacent((int X, int Y) location)
{
	if (location.X > 0) yield return (location.X - 1, location.Y);
	if (location.X < xMax) yield return (location.X + 1, location.Y);
    if (location.Y > 0) yield return (location.X, location.Y - 1);
    if (location.Y < yMax) yield return (location.X, location.Y + 1);
}

var points = grid.Where(g => GetAdjacent(g.Key).All(a => grid[a] > g.Value));


var processed = new List<(int X, int Y)>();

IEnumerable<(int X, int Y)> GetBasin((int X, int Y) lowest)
{
	processed.Add(lowest);
	yield return lowest;
	var next = GetAdjacent(lowest).Where(a => grid[a] < 9 && grid[a] > grid[lowest] && !processed.Contains(a));
	foreach (var n in next)
	{
		foreach (var a in GetBasin(n))
		{
			yield return a;
		}
	}
}

//foreach (var p in points)
//{
//	processed.Clear();
//	var basin = GetBasin(p.Key).ToArray();
//	Console.WriteLine($"{p}: {string.Concat(basin)}");

//    for (int y = 0; y < yMax + 1; y++)
//	{
//		Console.WriteLine(string.Concat(Enumerable.Range(0, xMax + 1).Select(x =>
//		{
//			return basin.Contains((x, y)) ? grid[(x, y)].ToString() : ".";
//        })));
//	}
//}

processed.Clear();
Console.WriteLine(string.Join(", ", points.Select(p => GetBasin(p.Key).LongCount())));

processed.Clear();
var result = points.Select(p => GetBasin(p.Key).LongCount()).OrderDescending().Take(3).Aggregate((c, t) => c * t);
Console.WriteLine(result);