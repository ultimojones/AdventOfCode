using System.Collections.Concurrent;

var grid = new Dictionary<(int X, int Y), (int X, int Y)>();
int n = 0;
foreach (var line in File.ReadLines("input.txt"))
{
    n++;
    for (int x = 0; x < line.Length; x++)
    {
        if (line[x] == '#')
            grid[(x, n)] = default;
    }
}

int moves = 0, rounds = 0;
for (int i = 0; ; i++)
{
    foreach (var elf in grid)
    {
        if (grid.ContainsKey(NW(elf.Key))
         || grid.ContainsKey(N(elf.Key))
         || grid.ContainsKey(NE(elf.Key))
         || grid.ContainsKey(E(elf.Key))
         || grid.ContainsKey(SE(elf.Key))
         || grid.ContainsKey(S(elf.Key))
         || grid.ContainsKey(SW(elf.Key))
         || grid.ContainsKey(W(elf.Key)))
        {
            for (int check = i % 4, test = 0; test < 4; test++, check = (check + 1) % 4)
            {
                if (check == 0)
                {
                    if (!grid.ContainsKey(NW(elf.Key))
                     && !grid.ContainsKey(N(elf.Key))
                     && !grid.ContainsKey(NE(elf.Key)))
                    {
                        grid[elf.Key] = N(elf.Key);
                        break;
                    }
                }
                else if (check == 1)
                {
                    if (!grid.ContainsKey(SW(elf.Key))
                     && !grid.ContainsKey(S(elf.Key))
                     && !grid.ContainsKey(SE(elf.Key)))
                    {
                        grid[elf.Key] = S(elf.Key);
                        break;
                    }
                }
                else if (check == 2)
                {
                    if (!grid.ContainsKey(NW(elf.Key))
                     && !grid.ContainsKey(W(elf.Key))
                     && !grid.ContainsKey(SW(elf.Key)))
                    {
                        grid[elf.Key] = W(elf.Key);
                        break;
                    }
                }
                else if (check == 3)
                {
                    if (!grid.ContainsKey(NE(elf.Key))
                     && !grid.ContainsKey(E(elf.Key))
                     && !grid.ContainsKey(SE(elf.Key)))
                    {
                        grid[elf.Key] = E(elf.Key);
                        break;
                    }
                }
            }
        }
    }

    moves = 0;
    foreach (var elf in grid.ToArray())
    {
        if (grid.Values.Count(v => v == elf.Value) == 1)
        {
            grid[elf.Value] = default;
            grid.Remove(elf.Key);
            moves++;
        }
    }

    Console.WriteLine($"{i}={moves}");

    if (moves == 0)
    {
        rounds = i + 1;
        break;
    }

}

var minX = grid.Min(g => g.Key.X);
var maxX = grid.Max(g => g.Key.X);
var minY = grid.Min(g => g.Key.Y);
var maxY = grid.Max(g => g.Key.Y);

for (int y = minY; y < maxY + 1; y++)
{
    Console.WriteLine(new string(Enumerable.Range(minX, maxX - minX + 1).Select(x => grid.ContainsKey((x, y)) ? '#' : '.').ToArray()));
}
Console.WriteLine();

var endMinX = grid.Min(g => g.Key.X);
var endMaxX = grid.Max(g => g.Key.X);
var endMinY = grid.Min(g => g.Key.Y);
var endMaxY = grid.Max(g => g.Key.Y);

Console.WriteLine((endMaxX - endMinX + 1) * (endMaxY - endMinY + 1) - grid.Count);
Console.WriteLine();
Console.WriteLine($"After {rounds} rounds.");


(int X, int Y) NW((int X, int Y) point) => (point.X - 1, point.Y - 1);
(int X, int Y) N((int X, int Y) point) => (point.X, point.Y - 1);
(int X, int Y) NE((int X, int Y) point) => (point.X + 1, point.Y - 1);
(int X, int Y) E((int X, int Y) point) => (point.X + 1, point.Y);
(int X, int Y) SE((int X, int Y) point) => (point.X + 1, point.Y + 1);
(int X, int Y) S((int X, int Y) point) => (point.X, point.Y + 1);
(int X, int Y) SW((int X, int Y) point) => (point.X - 1, point.Y + 1);
(int X, int Y) W((int X, int Y) point) => (point.X - 1, point.Y);
