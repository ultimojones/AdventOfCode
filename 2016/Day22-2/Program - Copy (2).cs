using System.Collections;
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
        return new Node(name, (x, y), size, used, avail, usage);
    }).ToDictionary(g => g.Pos);

var start = grid.Where(g => g.Key.X == 0).MaxBy(g => g.Key.Y).Value;
var maxX = grid.Keys.Max(g => g.X);
var maxY = grid.Keys.Max(g => g.Y);

var checkedMoves = new HashSet<string>();
var checkedStates = new HashSet<string>();
((int X, int Y) From, (int X, int Y) To)[] bestMoves = null!;

CalcMoves(Array.Empty<((int, int), (int, int))>(), 0, start.Pos);

void CalcMoves(IEnumerable<((int X, int Y) From, (int X, int Y) To)> moves, int steps, (int X, int Y) dataPos)
{
    if (dataPos == (0, 0))
    {
        if (bestMoves is null || steps < bestMoves.Length)
        {
            bestMoves = moves.ToArray();
            Console.WriteLine($"({bestMoves.Length}) {{{string.Concat(bestMoves)}}}");
        }
        return;
    }
    if (steps > 1000 || bestMoves is not null && steps >= bestMoves.Length) { return; }

    var candidates = grid
        .Where(g => g.Value.Used > 0)
        .SelectMany(g => GetNeighbours(g.Key).Where(n => n != dataPos && grid[n].Avail >= g.Value.Used).Select(n => (From: g.Key, To: n)))
        .OrderBy(m => int.Abs(dataPos.X - m.From.X) + int.Abs(dataPos.Y - m.From.Y))
        .ToArray();

    foreach (var move in candidates)
    {
        Console.WriteLine(move);
        //var nextMoves = moves.Append(move).ToArray();
        //var checkedKey = string.Concat(nextMoves.Select(n => $"[({n.From.X},{n.From.Y})({n.To.X},{n.To.Y})]"));
        //if (checkedMoves.Contains(checkedKey)) { continue; }
        var amt = grid[move.From].Used;
        grid[move.From].Used -= amt;
        grid[move.From].Avail += amt;
        grid[move.To].Used += amt;
        grid[move.To].Avail -= amt;
        //var check = string.Join(',', grid.Select(g => $"{g.Key}={g.Value.Used}"));
        var gridState = BitConverter.ToString(MD5.HashData(grid.SelectMany(g => new byte[] { (byte)g.Key.X, (byte)g.Key.Y, (byte)g.Value.Used }).ToArray()));
        if (!checkedStates.Contains(gridState)) 
        {
            checkedStates.Add(gridState);
            CalcMoves(moves.Append(move), steps + 1, move.From == dataPos ? move.To : dataPos);
        }
        grid[move.From].Used += amt;
        grid[move.From].Avail -= amt;
        grid[move.To].Used -= amt;
        grid[move.To].Avail += amt;
    }

}

IEnumerable<(int X, int Y)> GetNeighbours((int X, int Y) pos)
{
    if (pos.Y > 0) yield return (pos.X, pos.Y - 1);
    if (pos.X < maxX) yield return (pos.X + 1, pos.Y);
    if (pos.X > 0) yield return (pos.X - 1, pos.Y);
    if (pos.Y < maxY) yield return (pos.X, pos.Y + 1);
}

class Node
{
    public Node(string name, (int X, int Y) pos, int size, int used, int avail, int usage)
    {
        Name = name;
        Pos = pos;
        Size = size;
        Used = used;
        Avail = avail;
        Usage = usage;
    }

    public string Name { get; set; }
    public (int X, int Y) Pos { get; set; }
    public int Size { get; set; }
    public int Used { get; set; }
    public int Avail { get; set; }
    public int Usage { get; set; }
}
