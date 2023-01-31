var grid = File.ReadAllLines("input.txt").SelectMany((l, y) => l.Select((c, x) => new { Key = (X: x, Y: y), Value = c - '0' })).ToDictionary(d => d.Key, d => d.Value);
var xMax = grid.Max(g => g.Key.X);
var yMax = grid.Max(g => g.Key.Y);
var end = (xMax, yMax);

IEnumerable<(int X, int Y)> Dirs((int X, int Y) pos)
{
    if (pos.X > 0) yield return (pos.X - 1, pos.Y);
    if (pos.Y > 0) yield return (pos.X, pos.Y - 1);
    if (pos.X < xMax) yield return (pos.X + 1, pos.Y);
    if (pos.Y < yMax) yield return (pos.X, pos.Y + 1);
}

int? bestScore = default!;
(int X, int Y)[] bestPath = default!;
var working = new PriorityQueue<((int X, int Y)[] Path, int Score), int>();
working.Enqueue((new[] { (0, 0) }, 0), 0);
var bestWorking = new Dictionary<(int X, int Y), int>();
(int X, int Y) bestPos = (0, 0);

while (working.TryDequeue(out var current, out var score))
{
    var pos = current.Path.Last();
    if (pos == end)
    {
        if (bestScore is null || current.Score < bestScore)
        {
            bestScore = current.Score;
            bestPath = current.Path;
            Console.WriteLine($"{pos} = {bestScore} ({working.Count})");
        }
        continue;
    }
    if (bestScore is not null && score >= bestScore) { continue; }
    if (bestWorking.TryGetValue(pos, out var best) && score >= best) { continue; }
    bestWorking[pos] = score;

    if (pos.X + pos.Y > bestPos.X + bestPos.Y) Console.WriteLine(bestPos = pos);

    foreach (var next in Dirs(pos).Except(current.Path))
    {
        var nextScore = current.Score + grid[next];
        working.Enqueue((current.Path.Append(next).ToArray(), nextScore), nextScore);
    }
}

Console.WriteLine(bestScore);