var east = new List<(int X, int Y)>();
var south = new List<(int X, int Y)>();

var input = File.ReadAllLines("input.txt");
for (int y = 0; y < input.Length; y++)
{
	for (int x = 0; x < input[y].Length; x++)
	{
		if (input[y][x] is '>')
			east.Add((x, y));
		else if (input[y][x] is 'v')
			south.Add((x, y));
    }
}
var xMax = input.Max(i => i.Length);
var yMax = input.Length;
PrintGrid();

int turns = 0;
while (true)
{
	var nextEast = east.Select(n => (From: n, To: (X: (n.X + 1) % xMax, n.Y))).ExceptBy(east, n => n.To).ExceptBy(south, n => n.To).ToList();
	nextEast.ForEach(n => { east.Remove(n.From); east.Add(n.To); });
    var nextSouth = south.Select(n => (From: n, To: (n.X, Y: (n.Y + 1) % yMax))).ExceptBy(east, n => n.To).ExceptBy(south, n => n.To).ToList();
    nextSouth.ForEach(n => { south.Remove(n.From); south.Add(n.To); });
	turns++;
	Console.WriteLine(turns);
	if (turns % 100 == 0) { PrintGrid(); }
	if (nextEast.Count == 0 && nextSouth.Count == 0)
		break;
}

PrintGrid();


void PrintGrid()
{
	for (int y = 0; y < yMax; y++)
	{
		Console.WriteLine(Enumerable.Range(0, xMax).Select(x => east.Contains((x, y)) ? '>' : south.Contains((x, y)) ? 'v' : '.').ToArray());
	}
	Console.WriteLine();
}