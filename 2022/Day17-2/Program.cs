using System.Runtime.Versioning;

//var jets = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>".ToCharArray();
var jets = File.ReadAllText("input.txt").ToCharArray();

var rockHorizontal = new (int X, int Y)[] { (0, 0), (1, 0), (2, 0), (3, 0) };
var rockCross = new (int X, int Y)[] { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) };
var rockCorner = new (int X, int Y)[] { (0, 0), (1, 0), (2, 0), (2, 1), (2, 2) };
var rockVertical = new (int X, int Y)[] { (0, 0), (0, 1), (0, 2), (0, 3) };
var rockSquare = new (int X, int Y)[] { (0, 0), (1, 0), (0, 1), (1, 1) };

var rocks = new[] { rockHorizontal, rockCross, rockCorner, rockVertical, rockSquare };
int nextId = 0;
int jetId = 0;

const long stupidElephants = 1000000000000;

var chamber = new List<(int X, int Y)>();
var stack = new List<(int X, int OffsetY, int Rock, int MaxY)>();
var prevY = 0;
var patterns = new HashSet<int>();

for (int i = 0; i < 10000; i++)
{
    var rockId = nextId++ % 5;
    var next = rocks[rockId];
    var bottom = chamber.Count == 0 ? 0 : chamber.Max(d => d.Y);
    var pos = (X: 3, Y: bottom + 4);

    //PrintGridNext(new(pos, next));

    while (true)
    {
        var jet = jets[jetId++ % jets.Length] == '<' ? -1 : 1;

        var blockedJet = next.Select(r => (X: pos.X + r.X + jet, Y: pos.Y + r.Y))
            .Any(r => r.X <= 0 || r.X >= 8 || chamber.Contains(r));

        if (!blockedJet)
            pos.X += jet;

        var blockedDown = next.Select(r => (X: pos.X + r.X, Y: pos.Y + r.Y - 1))
            .Any(r => r.Y <= 0 || chamber.Contains(r));

        if (blockedDown)
            break;

        pos.Y--;
        //PrintGridNext(new(pos, next));
    }

    //PrintGridNext(new(pos, next));
    chamber.AddRange(next.Select(r => (r.X + pos.X, r.Y + pos.Y)));

    stack.Add((pos.X, pos.Y - prevY, rockId, chamber.Max(r => r.Y)));
    prevY = pos.Y;

    if (i > 20) // && i % jets.Length == 0)
    {
        var end = stack.Count;
        for (int j = 5; j <= stack.Count / 2; j++)
        {
            var match = true;

            for (int k = end - j, l = end - j * 2; k < end; k++, l++)
            {
                if (stack[k].Rock != stack[l].Rock || stack[k].X != stack[l].X || stack[k].OffsetY != stack[l].OffsetY)
                {
                    match = false; break;
                }
            }
            if (match)
            {
                if (!patterns.Contains(j))
                {
                    patterns.Add(j);
                    Console.WriteLine($"{i}: Length={j} Start={end - j * 2} Size={stack[end - j].MaxY - stack[end - j * 2].MaxY}");
                    var sequenceLen = j;
                    long sequenceHeight = stack[end - j].MaxY - stack[end - j * 2].MaxY;
                    long seqRepeats = (stupidElephants - (i - 2 * j)) / sequenceLen;
                    var lastNonSequence = stupidElephants - seqRepeats * sequenceLen - 1;
                    var height = stack[(int)lastNonSequence].MaxY + seqRepeats * sequenceHeight;
                    Console.WriteLine($"=>{height}");
                }
            }
        }
        //PrintGridStopped(25);
    }
}

Console.WriteLine(chamber.Max(d => d.Y));

//void PrintGridNext(KeyValuePair<(int X, int Y), (int X, int Y)[]> current)
//{
//    var startY = Math.Max(current.Key.Y + current.Value.Height, stopped.Count == 0 ? 0 .: stopped.Max(d => d.Y));
//    for (int y = startY; y > 0; y--)
//    {
//        Console.WriteLine($"|{new string(Enumerable.Range(1, 7).Select(x =>
//              current.Value.Rocks.Select(r => (r.X + current.Key.X, r.Y + current.Key.Y)).Contains((x, y)) ? '@'
//            : stopped.Contains((x, y)) ? '#'
//            : '.').ToArray())}|");
//    }
//    Console.WriteLine("+-------+");
//    Console.WriteLine();
//}

void PrintGridStopped(int lines)
{
    var startY = chamber.Count == 0 ? 0 : chamber.Max(d => d.Y);
    for (int y = startY, l = 0; y > 0 && l < lines; y--, l++)
    {
        Console.WriteLine($"|{new string(Enumerable.Range(1, 7).Select(x =>
              chamber.Contains((x, y)) ? '#'
            : '.').ToArray())}|");
    }
    Console.WriteLine("+-------+");
    Console.WriteLine();
}
