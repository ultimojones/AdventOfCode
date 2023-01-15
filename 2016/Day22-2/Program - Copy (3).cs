using System.Collections;
using System.ComponentModel.DataAnnotations;
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
        return new Node(name, (x, y), size, used);
    }).ToDictionary(g => g.Pos);

var start = grid.Where(g => g.Key.Y == 0).MaxBy(g => g.Key.X).Value;
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

    var dataNeighbours = GetNeighbours(dataPos).ToArray();
    var target = dataNeighbours.MinBy(d => d.X + d.Y);

    var dataCandidates = dataNeighbours.Where(n => grid[n].Avail >= grid[dataPos].Used).OrderBy(n => n.X + n.Y).Select(c => (From: dataPos, To: c)).ToArray();
    var positiveDataMoves = dataCandidates.Where(c => c.To.X - dataPos.X <= 0 && c.To.Y - dataPos.Y <= 0);
    var negativeDataMoves = dataCandidates.Where(c => c.To.X - dataPos.X >= 0 && c.To.Y - dataPos.Y >= 0);
    var otherMoves = grid.Where(f => f.Value.Used > 0 && f.Key != dataPos)
        .SelectMany(f => GetNeighbours(f.Key).Where(t => t != dataPos && grid[t].Avail >= f.Value.Used).Select(t => (From: f.Key, To: t)))
        .Except(dataCandidates)
        .OrderBy(m => double.Abs(target.X - m.From.X) + double.Abs(target.Y - m.From.Y)).ThenByDescending(m => double.Abs(target.X - m.To.X) + double.Abs(target.Y - m.To.Y));

    var candidates = positiveDataMoves.Concat(otherMoves).Concat(negativeDataMoves).ToArray();

    foreach (var move in candidates)
    {
        Console.WriteLine(move);
        for (int y = 0; y < maxY + 1; y++)
        {
            for (int x = 0; x < maxX + 1; x++)
            {
                var pos = (x, y);
                if (pos == (move.From == dataPos ? move.To : dataPos)) { Console.Write("G"); }
                else if (pos == move.From) { Console.Write("*"); }
                else if (candidates.Any(m => m.From == pos)) { Console.Write("?"); }
                else { Console.Write(grid[pos].Used == 0 ? "_" : "."); }
            }
            Console.WriteLine();
        }
        var amt = grid[move.From].Used;
        grid[move.From].Used -= amt;
        grid[move.To].Used += amt;
        var gridState = BitConverter.ToString(MD5.HashData(grid.SelectMany(g => new byte[] { (byte)g.Key.X, (byte)g.Key.Y, (byte)g.Value.Used }).ToArray()));
        if (!checkedStates.Contains(gridState)) 
        {
            checkedStates.Add(gridState);
            CalcMoves(moves.Append(move), steps + 1, move.From == dataPos ? move.To : dataPos);
        }
        grid[move.From].Used += amt;
        grid[move.To].Used -= amt;
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
    public Node(string name, (int X, int Y) pos, int size, int used)
    {
        Name = name;
        Pos = pos;
        Size = size;
        Used = used;
    }

    public string Name { get; set; }
    public (int X, int Y) Pos { get; set; }
    public int Size { get; set; }
    public int Used { get; set; }
    public int Avail => Size - Used;
}
