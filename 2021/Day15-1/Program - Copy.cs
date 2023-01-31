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
List<(int X, int Y)> bestPath = default!;
var working = new List<(List<(int X, int Y)> Path, int Score)> { (new List<(int X, int Y)> { (0, 0) }, 0) };
var done = new List<string>();

while (working.Count > 0)
{
    var current = working.OrderBy(w => w.Score + (xMax - w.Path.Last().X) + (yMax - w.Path.Last().Y)).First();
    working.Remove(current);
    if (current.Path.Last() == end)
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

    foreach (var next in Dirs(current.Path.Last()).Except(current.Path))
    {
        working.Add((current.Path.Append(next).ToList(), current.Score + grid[next]));
    }
}

Console.WriteLine(bestScore);