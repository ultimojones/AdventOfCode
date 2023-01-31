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
var working = new PriorityQueue<((int X, int Y)[] Path, int Score), double>();
working.Enqueue((new[] { (0, 0) }, 0), 0);
var done = new List<string>();
(int X, int Y) bestPos = (0, 0);

while (working.TryDequeue(out var current, out var priority))
{
    var pos = current.Path.Last();
    if (pos.X + pos.Y > bestPos.X + bestPos.Y) Console.WriteLine(bestPos = pos);
    if (pos == end)
    {
        if (bestScore is null || current.Score < bestScore)
        {
            bestScore = current.Score;
            bestPath = current.Path;
            Console.WriteLine(bestScore);
        }
        continue;
    }
    if (bestScore is not null && current.Score >= bestScore) { continue; }

    var key = string.Concat(current.Path);
    if (done.Contains(key)) { continue; }
    done.Add(key);

    foreach (var next in Dirs(pos).Except(current.Path))
    {
        var adding = (Path: current.Path.Append(next).ToArray(), Score: current.Score + grid[next]);
        working.Enqueue(adding, adding.Score / (double)(next.X + next.Y + 2));
    }
}

Console.WriteLine(bestScore);