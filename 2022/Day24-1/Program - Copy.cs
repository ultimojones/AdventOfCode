using System.Linq;

var border = new List<(int X, int Y)>();
var startBlizzards = new List<((int X, int Y) Point, char Dir)>();
var blizzards = new List<((int X, int Y) Point, char Dir)>();
var lines = File.ReadAllLines("input.txt");
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
				startBlizzards.Add(((x, y), lines[y][x]));
                break;
            default:
				break;
		}
	}
}
var maxX = border.Max(b => b.X);
var maxY = border.Max(b => b.Y);
var fin = (X: maxX - 1, Y: maxY);
(int X, int Y) cur = (1, 0);
var totalDistance = fin.X - cur.X + fin.Y - cur.Y;

var bestRoute = "";
var bestTime = 0;

var deadRoutes = new HashSet<string>();
var visits = new HashSet<((int X, int Y) Point, int Turn)>();

while (true)
{
    cur = (X: 1, Y: 0);
    var route = "";

    List<((int X, int Y) Point, char Dir)> MoveBlizzards(List<((int X, int Y) Point, char Dir)> b) => 
		b.AsParallel().Select(b => b.Dir switch
		{
			'>' => ((b.Point.X == maxX - 1 ? 1 : b.Point.X + 1, b.Point.Y), b.Dir),
			'<' => ((b.Point.X == 1 ? maxX - 1 : b.Point.X - 1, b.Point.Y), b.Dir),
			'^' => ((b.Point.X, b.Point.Y == 1 ? maxY - 1 : b.Point.Y - 1), b.Dir),
			'v' => ((b.Point.X, b.Point.Y == maxY - 1 ? 1 : b.Point.Y + 1), b.Dir),
			_ => throw new NotImplementedException()
		}).ToList();
	var nextBlizzards = MoveBlizzards(startBlizzards);

    for (int i = 0; i < 100; i++)
	{
		blizzards = nextBlizzards;
		nextBlizzards = MoveBlizzards(blizzards);

		var moves = GetNeighbours(cur).AsParallel()
			.Where(m => m.Point == fin
				    || (m.Point.X > 0 && m.Point.X < maxX && m.Point.Y > 0 && m.Point.Y < maxY
						//&& !visits.Contains((m.Point, i))
				   	    && !blizzards.Any(b => b.Point == m.Point)
				   	    && !GetNeighbours(m.Point).AsParallel().All(n => nextBlizzards.Any(b => b.Point == n.Point))
						&& !((m.Point.X - 1 + m.Point.Y) < i / 2)
				   	    && !deadRoutes.Contains(route + m.Dir)));



        if (!moves.Any())
		{
            //PrintGrid();
            deadRoutes.Add(route);
			Console.WriteLine($"Dead [{cur.X - 1 + cur.Y,3}/{i + 1,3}] {cur} = {route}");
			break;
		}

		var top = moves.MinBy(p => fin.X - p.Point.X + fin.Y - p.Point.Y);
		route = route + top.Dir;
		cur = top.Point;
		visits.Add((cur, i));


		if (cur == fin)
		{
			PrintGrid();
			Console.WriteLine($"Turns {i + 1} = {route}");
			break;
		}
	}
    if (cur == fin)
        break;
}

static ((int X, int Y) Point, char Dir)[] GetNeighbours((int X, int Y) point)
{
    return new[]
    {
                (point, '@'),
                ((point.X, point.Y - 1), '^'),
                ((point.X + 1, point.Y), '>'),
                ((point.X, point.Y + 1), 'v'),
                ((point.X - 1, point.Y), '<'),
            };
}


void PrintGrid()
{
	for (int y = 0; y <= maxY; y++)
	{
		var line = Enumerable.Repeat('.', maxX + 1).ToArray();
		for (int x = 0; x < maxX + 1; x++)
		{
			if (border!.Contains((x, y)))
				line[x] = '#';
			var bliz = blizzards!.Where(b => b.Point == (x, y)).ToList();
            if (cur == (x, y))
                line[x] = bliz.Count switch { 1 => '©', 2 => '%', 3 => '§', _ => 'E' };
			else
				if (bliz.Count > 1)
				{
					line[x] = bliz.Count.ToString()[0];
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
	Console.WriteLine();
}
