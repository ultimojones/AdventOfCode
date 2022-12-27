var border = new List<(int X, int Y)>();
var blizzards = new List<(int X, int Y, char Dir)>();
var lines = File.ReadAllLines("sample.txt");
for (int y = 0; y < lines.Length; y++)
{
	for (int x = 0; x < lines[y].Length; x++)
	{
		switch (lines[y][x])
		{
			case '#':
				border.Add((x, y));
                break;
			case '<' or '>' or '^' or 'v':
				blizzards.Add((x, y, lines[y][x]));
                break;
            default:
				break;
		}
	}
}
var maxX = border.Max(b => b.X);
var maxY = border.Max(b => b.Y);
var cur = (1, 0);

var bestRoute = "";
var bestTime = 0;





blizzards = blizzards.Select(b => b.Dir switch
{
	'>' => (b.X == maxX - 1 ? 1 : b.X + 1, b.Y, b.Dir),
	'<' => (b.X == 1 ? maxX - 1 : b.X - 1, b.Y, b.Dir),
	'^' => (b.X, b.Y == 1 ? maxY - 1 : b.Y - 1, b.Dir),
	'v' => (b.X, b.Y == maxY - 1 ? 1 : b.Y + 1, b.Dir),
	_ => throw new NotImplementedException()
}).ToList();



PrintGrid();

void PrintGrid()
{
	for (int y = 0; y < maxY + 1; y++)
	{
		var line = Enumerable.Repeat('.', maxX + 1).ToArray();
		for (int x = 0; x < maxX + 1; x++)
		{
			if (border!.Contains((x, y)))
				line[x] = '#';
			if (cur == (x, y))
				line[x] = 'E';
			var bliz = blizzards!.Where(b => (b.X, b.Y) == (x, y)).ToList();
			if (bliz.Count > 1)
			{
				line[x] = '2';
			}
			else if (bliz.Count == 1)
			{
				line[x] = bliz.Single().Dir;
			}
		}
		Console.WriteLine(new string(line));

		//Console.WriteLine(new string(Enumerable.Range(0, maxX + 1)
		//	.Select(x =>
		//	  border!.Contains((x, y)) ? '#'
		//	: cur == (x, y) ? 'E'
		//	: blizzards!.Any(b => (b.X, b.Y) == (x, y))
		//		? blizzards!.GroupBy(b => (b.X, b.Y)).Select(b => b.Count() > 1 ? b.Count().ToString()[0] : b.First().Dir).First()
		//		: '.'
		//	).ToArray()));
	}
}

