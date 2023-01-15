using System.Text.RegularExpressions;

var grid = File.ReadLines("input.txt").Skip(2).Select(l =>
    {
        var r = Regex.Match(l, @"^/dev/grid/node-x(?<x>\d+)-y(?<y>\d+)\s+(?<size>\d+)T\s+(?<used>\d+)T\s+(?<avail>\d+)T\s+(?<usage>\d+)%$");
        var x = int.Parse(r.Groups["x"].Value);
        var y = int.Parse(r.Groups["y"].Value);
        var size = int.Parse(r.Groups["size"].Value);
        var used = int.Parse(r.Groups["used"].Value);
        var avail = int.Parse(r.Groups["avail"].Value);
        var usage = int.Parse(r.Groups["usage"].Value);
        return (Pos: (X: x, Y: y), Size: size, Used: used, Avail: avail, Usage: usage);
    }).ToArray();

var viable = new HashSet<(int X, int Y)>();

for (int a = 0; a < grid.Length; a++)
{
    for (int b = 0; b < grid.Length; b++)
    {
        if (a == b) continue;
        if (grid[a].Used == 0) continue;
        if (grid[a].Used < grid[b].Avail)
            viable.Add((a, b));
    }
}

Console.WriteLine(viable.Count);