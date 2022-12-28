using System.Drawing;
using System.Linq;

var border = new List<(int X, int Y)>();
var startBlizzards = new List<((int X, int Y) Point, char Dir)>();
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

for (int i = 0; i < 6; i++)
{
    PrintGrid(GetBlizzardLocns(i), sta);
}

IEnumerable<(int X, int Y)> GetBlizzardLocns(int turn)
{
    var move = turn % blizMaxX;
    return startBlizzards.Select(b => b.Dir switch
    {
        '>' => ((b.Point.X + move - 1) % blizMaxX + 1, b.Point.Y),
        '<' => ((b.Point.X - move + blizMaxX - 1) % blizMaxX + 1, b.Point.Y),
        'v' => (b.Point.X, (b.Point.Y + move - 1) % blizMaxY + 1),
        '^' => (b.Point.X, (b.Point.Y - move + blizMaxY - 1) % blizMaxY + 1)
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
    Console.WriteLine();
}
