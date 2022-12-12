var lines = File.ReadAllLines("input.txt");
int maxX = lines[0].Length;
int maxY = lines.Length;
var height = Enumerable.Range(0, maxX).SelectMany(x => Enumerable.Range(0, maxY).Select(y => (X: x, Y: y)))
    .ToDictionary(i => i, i => lines[i.Y][i.X]);
var start = height.First(h => h.Value == 'S').Key;
var end = height.First(h => h.Value == 'E').Key;
height[start] = 'a';
height[end] = 'z';

var routes = height.Where(h => h.Value == 'a').Select(s => new { Start = s.Key, Steps = GetSteps(s.Key, end) }).Where(r => r.Steps != int.MaxValue);

foreach (var item in routes)
{
    Console.WriteLine(item);
}

Console.WriteLine(routes.Select(r => r.Steps).Min());

int GetSteps((int X, int Y) start, (int X, int Y) end)
{
    var path = new Dictionary<(int X, int Y), int>();
    path[start] = 0;
    var nextSteps = GetNeighbours(start).ToList();

    for (int i = 1; i < 2000; i++)
    {
        var checking = nextSteps.Distinct().ToArray();
        nextSteps.Clear();

        foreach (var step in checking)
        {
            var neighbours = GetNeighbours(step).ToArray();
            if (neighbours.Contains(end)) return i + 1;
            path[step] = i;
            nextSteps.AddRange(neighbours);
        }
        if (nextSteps.Count == 0) break;
    }

    return int.MaxValue;

    IEnumerable<(int X, int Y)> GetNeighbours((int X, int Y) s) =>
        new (int X, int Y)[] { (s.X - 1, s.Y), (s.X + 1, s.Y), (s.X, s.Y - 1), (s.X, s.Y + 1) }
        .Where(n => n.X >= 0 && n.X < maxX && n.Y >= 0 && n.Y < maxY
                && !path.ContainsKey(n) && height![n] - height[s] <= 1);
}