using System.Diagnostics;
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
    return (Pos: (X: x, Y: y), Node: new Node(size, used));
}).ToDictionary(g => g.Pos, g => g.Node);

var start = grid.Where(g => g.Key.Y == 0).MaxBy(g => g.Key.X);
var dataPos = start.Key;
var maxX = grid.Keys.Max(g => g.X);
var maxY = grid.Keys.Max(g => g.Y);

IEnumerable<(int X, int Y)> Neighbours((int X, int Y) pos)
{
    if (pos.Y > 0) yield return (pos.X, pos.Y - 1);
    if (pos.X < maxX) yield return (pos.X + 1, pos.Y);
    if (pos.X > 0) yield return (pos.X - 1, pos.Y);
    if (pos.Y < maxY) yield return (pos.X, pos.Y + 1);
}

var target = (X: dataPos.X - 1, Y: dataPos.Y);
var newMoves = grid.Where(f => f.Value.Used > 0 && f.Key != dataPos)
    .SelectMany(f => Neighbours(f.Key).Where(t => t != dataPos && grid[t].Avail >= f.Value.Used).Select(t => (From: f.Key, To: t)))
    .OrderBy(m => int.Abs(target.X - m.From.X) + int.Abs(target.Y - m.From.Y))
    .AsEnumerable();
if (grid[target].Avail > grid[dataPos].Used)
{
    newMoves = newMoves.Prepend((dataPos, target));
}

var startState = (
    Grid: new Dictionary<(int X, int Y), Node>(grid),
    Data: dataPos,
    Moves: Array.Empty<((int X, int Y) From, (int X, int Y) To)>(),
    Queue: new Queue<((int X, int Y) From, (int X, int Y) To)>(newMoves)
    );
var states = (new[] { startState }).ToDictionary(s => GetGridKey(s.Grid, s.Data));

var dataMoving = false;
var hitCount = 0;

while (true)
{
    var state = states.Where(s => s.Value.Queue.Count > 0).MinBy(s =>
    {
        var n = s.Value.Queue.Peek();
        if (n.From == s.Value.Data)
            return 0;
        return int.Abs(n.From.X - s.Value.Data.X - 1) + int.Abs(n.From.Y - s.Value.Data.Y);
    });
    var move = state.Value.Queue.Dequeue();
    var finished = move == (state.Value.Data, (0, 0));

    dataMoving = dataMoving || move.From == state.Value.Data;
    if (finished || (dataMoving & hitCount++ % 10 == 0))
    {
        for (int y = 0; y < maxY + 1; y++)
        {
            for (int x = 0; x < maxX + 1; x++)
            {
                var pos = (x, y);
                Console.Write(
                    pos == state.Value.Data ? "G"
                    : state.Value.Grid[pos].Used == 0 ? "_"
                    : state.Value.Moves.Any(m => m.From == pos) ? "*"
                    : state.Value.Grid[pos].Used > state.Value.Grid[state.Value.Data].Used ? '^'
                    : ".");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    if (finished)
    {
        Console.WriteLine($"Finish: {state.Value.Moves.Length + 1}");
        return;
    }

    var newGrid = new Dictionary<(int X, int Y), Node>(state.Value.Grid);
    var from = newGrid[move.From];
    var to = newGrid[move.To];
    from.Used = 0;
    to.Used += state.Value.Grid[move.From].Used;
    newGrid[move.From] = from;
    newGrid[move.To] = to;
    var newData = move.From == state.Value.Data ? move.To : state.Value.Data;
    var newKey = GetGridKey(newGrid, newData);
    if (states.ContainsKey(newKey))
    {
        //Debug.Assert(false);
    }
    else
    {
        var newState = (
            Grid: newGrid,
            Data: newData,
            Moves: state.Value.Moves.Append(move).ToArray(),
            Queue: GetNewQueue(newGrid, newData)
            );
        states.Add(newKey, newState);
    }
}

Queue<((int X, int Y) From, (int X, int Y) To)> GetNewQueue(Dictionary<(int X, int Y), Node> grid, (int X, int Y) data)
{
    var target = (X: data.X - 1, Y: data.Y);
    var newMoves = grid.Where(f => f.Value.Used > 0 && f.Key != data)
        .SelectMany(f => Neighbours(f.Key).Where(t => t != data && grid[t].Avail >= f.Value.Used).Select(t => (From: f.Key, To: t)))
        .OrderBy(m => int.Abs(target.X - m.From.X) + int.Abs(target.Y - m.From.Y))
        .AsEnumerable();
    if (grid[target].Avail > grid[data].Used)
    {
        newMoves = newMoves.Prepend((data, target));
    }
    return new Queue<((int X, int Y) From, (int X, int Y) To)>(newMoves);
}

string GetGridKey(Dictionary<(int X, int Y), Node> grid, (int X, int Y) data)
{
    //return Convert.ToBase64String(grid.OrderBy(s => (s.Key.X, s.Key.Y)).Select(s => s.Value.Used).Append(data.X).Append(data.Y).Select(s => (byte)s).ToArray());
    var emptyNode = grid.Single(g => g.Value.Used == 0);
    return string.Join(",", new[] { emptyNode.Key.X, emptyNode.Key.Y, data.X, data.Y });
}

struct Node
{
    public Node(int size, int used)
    {
        Size = size;
        Used = used;
    }

    public int Size { get; set; }
    public int Used { get; set; }
    public int Avail => Size - Used;
}
