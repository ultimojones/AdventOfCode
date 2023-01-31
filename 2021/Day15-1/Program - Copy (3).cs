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
var working = new Dictionary<int, List<((int X, int Y)[] Path, int Score)>>();
working[0] = new List<((int X, int Y)[] Path, int Score)> { (new[] { (0, 0) }, 0) };
var bestWorking = new Dictionary<(int X, int Y), int> { { (0, 0), 0 } };
var done = new HashSet<string>();
(int X, int Y) bestPos = (0, 0);

while (working.Sum(w => w.Value.Count) > 0)
{
    var queue = working.Where(w => w.Value.Count > 0).MaxBy(w => w.Key).Value;
    var current = queue.MinBy(c => c.Score);
    queue.Remove(current);

    var pos = current.Path.Last();
    if (pos == end)
    {
        if (bestScore is null || current.Score < bestScore)
        {
            bestScore = current.Score;
            bestPath = current.Path;
            Console.WriteLine($"{pos} = {bestScore} ({working.Sum(x => x.Value.Count)})");
        }
        continue;
    }
    if (bestScore is not null && current.Score >= bestScore) { continue; }
    if (current.Score > bestWorking[pos]) { continue; }

    var key = string.Concat(current.Path);
    if (done.Contains(key)) { continue; }
    done.Add(key);

    if (pos.X + pos.Y > bestPos.X + bestPos.Y) Console.WriteLine(bestPos = pos);

    foreach (var next in Dirs(pos).Except(current.Path))
    {
        var rank = next.X + next.Y;
        var score = current.Score + grid[next];
        if (bestWorking.TryGetValue(next, out var nextBest) && score > nextBest) { continue; }
        bestWorking[next] = score;
        var ranking = working.TryGetValue(rank, out var ranked) ? ranked : working[rank] = new List<((int X, int Y)[] Path, int Score)>();
        ranking.Add((current.Path.Append(next).ToArray(), current.Score + grid[next]));
    }
}

Console.WriteLine(bestScore);