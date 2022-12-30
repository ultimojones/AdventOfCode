var grid = new Dictionary<(int X, int Y), bool>();

int y = 0;
foreach (var line in File.ReadLines("input.txt"))
{
	y++;
	for (int i = 0, x = 1; i < line.Length; i++, x++)
	{
		grid[(x, y)] = line[i] == '#';
	}
}

var maxX = grid.Keys.Max(k => k.X);
var maxY = grid.Keys.Max(k => k.Y);

PrintGrid();
Console.WriteLine();

for (int i = 0; i < 100; i++)
{
	grid = grid.AsParallel().Select(g =>
	{
		var neighbourKeys = new (int X, int Y)[] {
			(g.Key.X - 1, g.Key.Y - 1),
			(g.Key.X, g.Key.Y - 1),
			(g.Key.X + 1, g.Key.Y - 1),
			(g.Key.X + 1, g.Key.Y),
			(g.Key.X + 1, g.Key.Y + 1),
			(g.Key.X, g.Key.Y + 1),
			(g.Key.X - 1, g.Key.Y + 1),
			(g.Key.X - 1, g.Key.Y)
		}.Where(k => k.X >= 1 && k.X <= maxX && k.Y >= 0 && k.Y <= maxY);
		var neighbours = grid.Join(neighbourKeys, o => o.Key, i => i, (o, i) => o);
		var numLit = neighbours.Count(n => n.Value);
		var result = g.Value ? numLit == 2 || numLit == 3 ? true : false
							 : numLit == 3 ? true : false;
		return (g.Key, Value: result);
	}).ToDictionary(g => g.Key, g => g.Value);

	PrintGrid();
	Console.WriteLine();
}

Console.WriteLine(grid.Count(g => g.Value));

void PrintGrid()
{
	for (int i = 1; i <= grid.Keys.Max(k => k.Y); i++)
	{
		Console.WriteLine(new string(grid.Where(g => g.Key.Y == i).OrderBy(g => g.Key.X).Select(g => g.Value ? '#' : '.').ToArray()));
	}
}