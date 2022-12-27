using static System.Net.Mime.MediaTypeNames;

var grid = new HashSet<(int X, int Y)>();

var lines = File.ReadAllLines("input.txt");
for (int i = 0, y = 1; i < lines.Length; i++, y++)
{
    var line = lines[i];
    for (int j = 0, x = 1; j < line.Length; j++, x++)
    {
        if (line[j] == '#')
        {
            grid.Add((x, y));
        }
    }
}

var completed = 0;
for (int i = 0; ; i++)
{
    int moved = 0;
    grid = grid.AsParallel().Select(elf =>
    {
        var near = new[] { NW(elf), N(elf), NE(elf), E(elf), SE(elf), S(elf), SW(elf), W(elf) };
        if (near.Intersect(grid).Any())
        {
            for (int j = 0, check = i % 4; j < 4; j++, check = (check + 1) % 4)
            {
                if (check == 0)
                {
                    if (!near[0..3].Intersect(grid).Any())
                    {
                        return (elf, near[1]);
                    }
                }
                else if (check == 1)
                {
                    if (!near[4..7].Intersect(grid).Any())
                    {
                        return (elf, near[5]);
                    }
                }
                else if (check == 2)
                {
                    if (!new[] { near[0], near[6], near[7] }.Intersect(grid).Any())
                    {
                        return (elf, near[7]);
                    }
                }
                else if (check == 3)
                {
                    if (!near[2..5].Intersect(grid).Any())
                    {
                        return (elf, near[3]);
                    }
                }
            }
        }
        return (elf, elf);
    }).GroupBy(k => k.Item2).SelectMany(g =>
    {
        var first = g.First();
        if (g.Count() == 1)
        {
            if (first.Item1 != first.Item2)
                moved++;
            return new[] { first.Item2 };
        }
        else
        {
            moved += g.Count();
            return g.Select(p => p.Item1);
        }
    }).ToHashSet();

    if (moved == 0)
    {
        completed = i + 1;
        break;
    }
    Console.WriteLine($"{i + 1}={moved}");
}

var minX = grid.Min(g => g.X);
var maxX = grid.Max(g => g.X);
var minY = grid.Min(g => g.Y);
var maxY = grid.Max(g => g.Y);

for (int y = minY; y < maxY + 1; y++)
{
    Console.WriteLine(new string(Enumerable.Range(minX, maxX - minX + 1).Select(x => grid.Contains((x, y)) ? '#' : '.').ToArray()));
}
Console.WriteLine();

//Console.WriteLine((maxX - minX + 1) * (maxY - minY + 1) - grid.Count);
Console.WriteLine(completed);


(int X, int Y) NW((int X, int Y) point) => (point.X - 1, point.Y - 1);
(int X, int Y) N((int X, int Y) point) => (point.X, point.Y - 1);
(int X, int Y) NE((int X, int Y) point) => (point.X + 1, point.Y - 1);
(int X, int Y) E((int X, int Y) point) => (point.X + 1, point.Y);
(int X, int Y) SE((int X, int Y) point) => (point.X + 1, point.Y + 1);
(int X, int Y) S((int X, int Y) point) => (point.X, point.Y + 1);
(int X, int Y) SW((int X, int Y) point) => (point.X - 1, point.Y + 1);
(int X, int Y) W((int X, int Y) point) => (point.X - 1, point.Y);
