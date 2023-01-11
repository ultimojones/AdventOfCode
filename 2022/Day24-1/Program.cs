var border = new List<(int X, int Y)>();
var startBlizzards = new List<((int X, int Y) Point, char Dir)>();
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
var gridMaxX = border.Max(b => b.X);
var gridMaxY = border.Max(b => b.Y);
var blizMaxX = gridMaxX - 1;
var blizMaxY = gridMaxY - 1;
var sta = (X: 1, Y: 0);
var fin = (X: blizMaxX, Y: gridMaxY);
var result = int.MaxValue;

var visited = new HashSet<((int X, int Y) Point, int Turn)>();
var pending = new HashSet<((int X, int Y) Point, int Turn)>();

var cur = sta;

do
{
    int i = 1;
    if (cur != sta)
    {
        var next = pending.MinBy(m => (fin.X - m.Point.X) + (fin.Y - m.Point.Y));
        cur = next.Point;
        i = next.Turn;
        pending.Remove(next);
    }
    var from = cur;

    for (; i < 500; i++)
    {
        visited.Add((cur, i));
        var blizzards = GetBlizzardLocns(i).ToList();
        var moves = Neighbours(cur).Append(cur).Where(m => !blizzards.Contains(m) && !visited.Contains((m, i + 1))).ToList();

        if (moves.Count > 0)
        {
            cur = moves.MinBy(m => fin.X - m.X + fin.Y - m.Y);
            foreach (var point in moves)
            {
                if (point != cur)
                    pending.Add((point, i + 1));
            }

            if (cur == fin)
            {
                if (i < result)
                    result = i;
                Console.WriteLine($"FROM {from} FINISH: {i}  BEST: {result}");
                break;
            }
        }
        else
        {
            //Console.WriteLine($"{cur} @ {i}");
            break;
        }
    }
} while (pending.Count > 0);
IEnumerable<(int X, int Y)> Neighbours((int X, int Y) cur)
{
    return new (int X, int Y)[] { cur, (cur.X - 1, cur.Y), (cur.X + 1, cur.Y), (cur.X, cur.Y - 1), (cur.X, cur.Y + 1) }
        .Where(p => (p.X >= 1 && p.X <= blizMaxX && p.Y >= 1 && p.Y <= blizMaxY) || p == fin);
}

IEnumerable<(int X, int Y)> GetBlizzardLocns(int turn)
{
    var moveX = turn % blizMaxX;
    var moveY = turn % blizMaxY;
    return startBlizzards.Select(b => b.Dir switch
    {
        '>' => ((b.Point.X + moveX - 1) % blizMaxX + 1, b.Point.Y),
        '<' => ((b.Point.X - moveX + blizMaxX - 1) % blizMaxX + 1, b.Point.Y),
        'v' => (b.Point.X, (b.Point.Y + moveY - 1) % blizMaxY + 1),
        '^' => (b.Point.X, (b.Point.Y - moveY + blizMaxY - 1) % blizMaxY + 1)
    });
}

void PrintGrid(IEnumerable<(int X, int Y)> blizzards, (int X, int Y) cur)
{
    for (int y = 0; y <= gridMaxY; y++)
    {
        var line = Enumerable.Repeat('.', gridMaxX + 1).ToArray();
        for (int x = 0; x < gridMaxX + 1; x++)
        {
            if (border!.Contains((x, y)))
                line[x] = '#';
            var bliz = blizzards!.Where(b => b == (x, y)).ToList();
            if (cur == (x, y))
                line[x] = bliz.Count > 0 ? '%' : 'E';
            else if (bliz.Count == 1)
                line[x] = '*';
            else if (bliz.Count > 1)
                line[x] = bliz.Count.ToString()[0];
        }
        Console.WriteLine(new string(line));
    }
}
