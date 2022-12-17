//var jets = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>".ToCharArray();
var jets = File.ReadAllText("input.txt");

Sprite spriteHorizontal = new(new[] { (0, 0), (1, 0), (2, 0), (3, 0) });
Sprite spriteCross = new(new[] { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) });
Sprite spriteCorner = new(new[] { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) });
Sprite spriteVertical = new(new[] { (0, 0), (0, 1), (0, 2), (0, 3) });
Sprite spriteSquare = new(new[] { (0, 0), (1, 0), (0, 1), (1, 1) });

var newSprite = new[] { spriteHorizontal, spriteCross, spriteCorner, spriteVertical, spriteSquare };
int nextId = 0;
int jetId = 0;

var stopped = new List<(int X, int Y)>();

for (int i = 0; i < 2022; i++)
{
    var next = newSprite[nextId++ % 5];
    var bottom = stopped.Count == 0 ? 0 : stopped.Max(d => d.Y);
    var pos = (X: 3, Y: bottom + 4);

    //PrintGridNext(new(pos, next));

    while (true)
    {
        var jet = jets[jetId++ % jets.Length] == '<' ? -1 : 1;

        var blockedJet = next.Rocks.Select(r => (X: pos.X + r.X + jet, Y: pos.Y + r.Y))
            .Any(r => r.X <= 0 || r.X >= 8 || stopped.Contains(r));

        if (!blockedJet)
            pos.X += jet;

        var blockedDown = next.Rocks.Select(r => (X: pos.X + r.X, Y: pos.Y + r.Y - 1))
            .Any(r => r.Y <= 0 || stopped.Contains(r));

        if (blockedDown)
            break;

        pos.Y--;
        //PrintGridNext(new(pos, next));
    }

    //PrintGridNext(new(pos, next));

    stopped.AddRange(next.Rocks.Select(r => (r.X + pos.X, r.Y + pos.Y)));

    //PrintGridStopped();
}

Console.WriteLine(stopped.Max(d => d.Y));

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

void PrintGridStopped()
{
    var startY = stopped.Count == 0 ? 0 : stopped.Max(d => d.Y);
    for (int y = startY; y > 0; y--)
    {
        Console.WriteLine($"|{new string(Enumerable.Range(1, 7).Select(x =>
              stopped.Contains((x, y)) ? '#'
            : '.').ToArray())}|");
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
