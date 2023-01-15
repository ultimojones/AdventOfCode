using System.Security.Cryptography;
using System.Text.RegularExpressions;

var grid = File.ReadLines("input.txt").Skip(2).Select(l =>
    {
        var r = Regex.Match(l, @"^(?<name>/dev/grid/node-x(?<x>\d+)-y(?<y>\d+))\s+(?<size>\d+)T\s+(?<used>\d+)T\s+(?<avail>\d+)T\s+(?<usage>\d+)%$");
        var name = r.Groups["name"].Value;
        var x = int.Parse(r.Groups["x"].Value);
        var y = int.Parse(r.Groups["y"].Value);
        var size = int.Parse(r.Groups["size"].Value);
        var used = int.Parse(r.Groups["used"].Value);
        var avail = int.Parse(r.Groups["avail"].Value);
        var usage = int.Parse(r.Groups["usage"].Value);
        return (Name: name, Pos: (X: x, Y: y), Size: size, Used: used, Avail: avail, Usage: usage);
    }).ToArray();

var target = grid.Where(g => g.Pos.X == 0).MaxBy(g => g.Pos.Y);

((int X, int Y) From, (int X, int Y) To)[] bestMoves = null!;

CalcBest(Array.Empty<((int X, int Y) From, (int X, int Y) To)>(), 0, target.Pos);

void CalcBest(IEnumerable<((int X, int Y) From, (int X, int Y) To)> moves, int steps, (int X, int Y) targetPos)
{
    if (targetPos == (0,0))
    {
        if (bestMoves is null || steps < bestMoves.Length)
        {
            bestMoves = moves.ToArray();
            Console.WriteLine($"({bestMoves.Length}) {{{string.Concat(bestMoves)}}}");
        }
        return;
    }
    if (bestMoves is not null && steps >= bestMoves.Length) { return; }

    var candidates = grid
        .SelectMany(g => 
            grid.Where(n => g.Pos.X - n.Pos.X is 0 && g.Pos.Y - n.Pos.Y is -1 or 1 && g.Used > 0 && g.Used <= n.Avail && n.Pos != targetPos)
                .OrderBy(g => g.Pos.X + g.Pos.Y + int.Abs(g.Pos.X - targetPos.X) + int.Abs(g.Pos.Y - targetPos.Y))
                .Select(n => (From: g, To: n)));

    //candidates.ForEach(c => Console.WriteLine(c));

    foreach (var move in candidates)
    {
        Console.WriteLine(move);
        var from = Array.IndexOf(grid, move.From);
        var to = Array.IndexOf(grid, move.To);
        grid[from].Used = 0;
        grid[from].Avail = grid[from].Size;
        grid[to].Used += move.From.Used;
        grid[to].Avail -= move.From.Used;
        if (move.From.Pos == targetPos) targetPos = move.To.Pos;

        CalcBest(moves.Append((move.From.Pos, move.To.Pos)), steps + 1, targetPos);

        grid[from].Used = move.From.Used;
        grid[from].Avail = move.From.Avail;
        grid[to].Used = move.To.Used;
        grid[to].Avail = move.To.Avail;
        if (move.To.Pos == targetPos) targetPos = move.From.Pos;
    }

}
