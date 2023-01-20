var grid = new Dictionary<(int X, int Y), char>();
var lines = File.ReadAllLines("input.txt");
for (int y = 0; y < lines.Length; y++)
{
	for (int x = 0; x < lines[y].Length; x++)
	{
		grid[(x, y)] = lines[y][x];
	}
}
var xMax = grid.Max(g => g.Key.X);
var yMax = grid.Max(g => g.Key.Y);

void PrintGrid()
{
	for (int y = 0; y < yMax + 1; y++)
	{
		Console.WriteLine(string.Concat(grid.Where(g => g.Key.Y == y)
			.Select(g => g.Value switch
			{
				'0' => '0',
				_ => '.',
			})));
	}
	Console.WriteLine();
}

var total = 0;

for (int i = 0; i < 100; i++)
{
	var flashing = new Queue<(int X, int Y)>();
    var flashed = new List<(int X, int Y)>();
    foreach (var key in grid.Keys)
	{
		if (++grid[key] > '9')
			flashing.Enqueue(key);
	}
	while (flashing.TryDequeue(out var flash))
	{
		if (flashed.Contains(flash))
			continue;
		flashed.Add(flash);
		foreach (var key in Adjacents(flash))
		{
            if (++grid[key] > '9' && !flashed.Contains(key))
                flashing.Enqueue(key);
        }
    }
	flashed.ForEach(f => grid[f] = '0');
	total += flashed.Count;
}

Console.WriteLine(total);

IEnumerable<(int X, int Y)> Adjacents((int X, int Y) point)
{
	if (point.X > 0) yield return (point.X - 1, point.Y);
	if (point.X < xMax) yield return (point.X + 1, point.Y);
	if (point.Y > 0) yield return (point.X, point.Y - 1);
	if (point.Y < yMax) yield return (point.X, point.Y + 1);
	if (point.X > 0 && point.Y > 0) yield return (point.X - 1, point.Y - 1);
    if (point.X < xMax && point.Y > 0) yield return (point.X + 1, point.Y - 1);
    if (point.X < xMax && point.Y < yMax) yield return (point.X + 1, point.Y + 1);
    if (point.X > 0 && point.Y < yMax) yield return (point.X - 1, point.Y + 1);
}