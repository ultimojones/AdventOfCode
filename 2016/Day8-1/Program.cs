var grid = new Dictionary<(int X, int Y), bool>();

void PrintGrid()
    => Enumerable.Range(0, 6).ToList().ForEach(
        y => Console.WriteLine(new string(Enumerable.Range(0, 50).Select(x => grid.TryGetValue((x, y), out var p) && p ? '#' : '.').ToArray())));

foreach (var op in File.ReadAllLines("input.txt"))
{
    if (op.StartsWith("rect"))
    {
        var size = op[5..].Split('x').Select(int.Parse).ToArray();
        for (int x = 0; x < size[0]; x++)
        {
            for (int y = 0; y < size[1]; y++)
            {
                grid[(x, y)] = true;
            }
        }
    }
    else if (op.StartsWith("rotate row y="))
    {
        var parms = op[13..].Split(" by ").Select(int.Parse).ToArray();
        var old = grid.Where(g => g.Key.Y == parms[0] && g.Value).ToArray();
        foreach (var item in old)
        {
            grid.Remove(item.Key);
        }
        foreach (var item in old)
        {
            grid[((item.Key.X + parms[1]) % 50, item.Key.Y)] = true;
        }
    }
    else if (op.StartsWith("rotate column x="))
    {
        var parms = op[16..].Split(" by ").Select(int.Parse).ToArray();
        var old = grid.Where(g => g.Key.X == parms[0] && g.Value).ToArray();
        foreach (var item in old)
        {
            grid.Remove(item.Key);
        }
        foreach (var item in old)
        {
            grid[(item.Key.X, (item.Key.Y + parms[1]) % 6)] = true;
        }
    }

    PrintGrid();
    Console.WriteLine();
}
Console.WriteLine(grid.Count(g => g.Value));
