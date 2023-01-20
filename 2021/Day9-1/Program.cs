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

var points = grid.Where(g => GetAdjacent(g.Key).All(a => grid[a] > g.Value)).Sum(g => g.Value + 1);
Console.WriteLine(points);
