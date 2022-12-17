var jets = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>".ToCharArray();

Sprite spriteHorizontal() => new Sprite(new[] { (0, 0), (1, 0), (2, 0), (3, 0) });
Sprite spriteCross() => new Sprite(new[] { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) });
Sprite spriteCorner() => new Sprite(new[] { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) });
Sprite spriteVertical() => new Sprite(new[] { (0, 0), (0, 1), (0, 2), (0, 3) });
Sprite spriteSquare() => new Sprite(new[] { (0, 0), (1, 0), (0, 1), (1, 1) });

var newSprite = new[] { spriteHorizontal, spriteCross, spriteCorner, spriteVertical, spriteSquare };
int nextId = 0;

//var displayed = new Dictionary<(int X, int Y), Sprite> { { (2, 3), spriteHorizontal() } };
//PrintGrid(displayed);

var displayed = new Dictionary<(int X, int Y), Sprite>();

for (int i = 0; i < 2; i++)
{
    var next = newSprite[nextId++ % 5]();
    var bottom = displayed.Count == 0 ? 0 : displayed.Max(d => d.Key.Y + d.Value.Height);
    displayed[(2, bottom + 4)] = next;
    PrintGrid(displayed);
}


void PrintGrid(Dictionary<(int X, int Y), Sprite> displayed)
{
    var maxY = displayed.Max(d => d.Key.Y + d.Value.Height);
    for (int y = maxY; y >= 0; y--)
    {
        var rocks = displayed.Where(d => y >= d.Key.Y && y <= d.Key.Y + d.Value.Height).SelectMany(d => d.Value.Rocks.Where(r => d.Key.Y + r.Y == y).Select(r => d.Key.X + r.X)).ToHashSet();
        Console.WriteLine($"|{new string(Enumerable.Range(0, 7).Select(r => rocks.Contains(r) ? '#' : '.').ToArray())}|");
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
