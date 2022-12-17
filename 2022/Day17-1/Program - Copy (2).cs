var jets = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>".ToCharArray();

Sprite spriteHorizontal() => new(new[] { (0, 0), (1, 0), (2, 0), (3, 0) });
Sprite spriteCross() => new(new[] { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) });
Sprite spriteCorner() => new(new[] { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) });
Sprite spriteVertical() => new(new[] { (0, 0), (0, 1), (0, 2), (0, 3) });
Sprite spriteSquare() => new(new[] { (0, 0), (1, 0), (0, 1), (1, 1) });

var newSprite = new[] { spriteHorizontal, spriteCross, spriteCorner, spriteVertical, spriteSquare };
int nextId = 0;
int jetId = 0;

//var displayed = new Dictionary<(int X, int Y), Sprite> { { (2, 3), spriteHorizontal() } };
//PrintGrid(displayed);

var displayed = new Dictionary<(int X, int Y), Sprite>();
var stopped = new List<(int X, int Y)>();

for (int i = 0; i < 2; i++)
{
    var next = newSprite[nextId++ % 5]();
    var bottom = displayed.Count == 0 ? 0 : displayed.Max(d => d.Key.Y + d.Value.Height);
    var pos = (X: 3, Y: bottom + 4);

    PrintGridNext(new(pos, next));

    while (true)
    {
        var jet = jets[jetId++ % jets.Length] == '<' ? -1 : 1;

        var blockedJet = next.Rocks.Select(r => (X: pos.X + r.X + jet, Y: pos.Y + r.Y))
            .Any(r => r.X <= 0 || r.X >= 8 || displayed.Any(d => d.Value.Rocks.Contains(r)));

        if (!blockedJet)
            pos.X += jet;

        var blockedDown = next.Rocks.Select(r => (X: pos.X + r.X, Y: pos.Y + r.Y - 1))
            .Any(r => r.Y <= 0 || displayed.Any(d => d.Value.Rocks.Contains(r)));

        if (blockedDown)
            break;

        pos.Y--;
        PrintGridNext(new(pos, next));
    }

    displayed[pos] = next;
    stopped.AddRange(next.Rocks.Select(r => (r.X + pos.X, r.Y + pos.Y)));

    PrintGridNext(new(pos, next));
    //PrintGrid();
}

Console.WriteLine("end");

void PrintGridNext(KeyValuePair<(int X, int Y), Sprite> current)
{
    var startY = Math.Max(current.Key.Y + current.Value.Height, stopped.Count == 0 ? 0 : stopped.Max(d => d.Y));
    for (int y = startY; y > 0; y--)
    {
        Console.WriteLine($"|{new string(Enumerable.Range(1, 7).Select(x =>
              current.Value.Rocks.Select(r => (r.X + current.Key.X, r.Y + current.Key.Y)).Contains((x, y)) ? '@'
            : stopped.Contains((x, y)) ? '#'
            : '.').ToArray())}|");
    }
    Console.WriteLine("+-------+");
    Console.WriteLine();
}

void PrintGrid()
{
    var maxY = displayed.Max(d => d.Key.Y + d.Value.Height);
    for (int y = maxY; y > 0; y--)
    {
        var rocks = displayed.Where(d => y >= d.Key.Y && y <= d.Key.Y + d.Value.Height).SelectMany(d => d.Value.Rocks.Where(r => d.Key.Y + r.Y == y).Select(r => d.Key.X + r.X)).ToHashSet();
        Console.WriteLine($"|{new string(Enumerable.Range(1, 7).Select(r => rocks.Contains(r) ? '#' : '.').ToArray())}|");
    }
    Console.WriteLine("+-------+");
    Console.WriteLine();
}

class Sprite
{
    public HashSet<(int X, int Y)> Rocks { get; set; }
    public int Height { get; }
    public int Width { get; }

    public Sprite(IEnumerable<(int X, int Y)> rocks)
    {
        Rocks = rocks.ToHashSet();
        Height = rocks.Max(r => r.Y) - rocks.Min(r => r.Y);
        Width = rocks.Max(r => r.X) - rocks.Min(r => r.X);
    }
}
