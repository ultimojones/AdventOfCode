var types = new Dictionary<char, (int RoomX, int Cost)>();
types['A'] = (3, 1);
types['B'] = (5, 10);
types['C'] = (7, 100);
types['D'] = (9, 1000);
var start = new Dictionary<(int X, int Y), char>();
var grid = new Dictionary<(int X, int Y), char>();
var lines = File.ReadAllLines("sample.txt");
for (int y = 0; y < lines.Length; y++)
{
    for (int x = 0; x < lines[y].Length; x++)
    {
        if (lines[y][x] is >= 'A' and <= 'D')
        {
            start.Add((x, y), lines[y][x]);
            grid.Add((x, y), '.');
        }
        else
        {
            grid.Add((x, y), lines[y][x]);
        }
    }
}
var states = new PriorityQueue<(Dictionary<(int X, int Y), char> Pods, long Score), int>();
var visited = new Dictionary<string, long>();
long? bestScore = null!;
states.Enqueue((start, 0), 0);

while (states.TryDequeue(out var state, out var priority))
{
    var stateKey = FormatState(state);
    if (visited.TryGetValue(stateKey, out var stateBest) && stateBest <= state.Score)
        continue;
    visited[stateKey] = state.Score;

    foreach (var pod in state.Pods)
    {
        var podType = types[pod.Value];
        if (pod.Key.X == podType.RoomX && state.Pods.Where(p => p.Key.X == pod.Key.X && p.Key.Y >= pod.Key.Y).All(p => p.Value == pod.Value)) continue;
        if (pod.Key.Y >= 3 && state.Pods.Any(p => p.Key.X == pod.Key.X && p.Key.Y < pod.Key.Y)) continue;
        var destRoom = state.Pods.Where(p => p.Key.X == podType.RoomX).ToArray();
        if ((destRoom.Length == 0 || destRoom.All(p => p.Value == pod.Value))
            && !state.Pods.Any(p => p.Key.Y == 1 && p.Key.X > int.Min(pod.Key.X, podType.RoomX) && p.Key.X < int.Max(pod.Key.X, podType.RoomX)))
        {
            var next = (Pods: new Dictionary<(int X, int Y), char>(state.Pods), state.Score);
            var destY = 5 - destRoom.Length;
            var steps = pod.Key.Y - 1 + int.Abs(pod.Key.X - podType.RoomX) + destY - 1;
            next.Score += podType.Cost * steps;
            next.Pods.Remove(pod.Key);
            next.Pods.Add((podType.RoomX, destY), pod.Value);

            if (destY == 2 && next.Pods.All(p => p.Key.X == types[p.Value].RoomX))
            {
                if (bestScore is null || next.Score < bestScore)
                {
                    bestScore = next.Score;
                    Console.WriteLine(new { bestScore });
                }
                continue;
            }
            
            Enqueue(next);
        }
        else
        {
            var leftpods = state.Pods.Where(p => p.Key.Y == 1 && p.Key.X < pod.Key.X).ToArray();
            var min = leftpods.Length == 0 ? 1 : leftpods.Max(p => p.Key.X) + 1;
            var rightpods = state.Pods.Where(p => p.Key.Y == 1 && p.Key.X > pod.Key.X).ToArray();
            var max = rightpods.Length == 0 ? 11 : rightpods.Min(p => p.Key.X) - 1;
            foreach (var point in Enumerable.Range(min, max - min + 1).Except(new[] { 3, 5, 7, 9, pod.Key.X }))
            {
                var next = (Pods: new Dictionary<(int X, int Y), char>(state.Pods), state.Score);
                next.Pods.Remove(pod.Key);
                next.Pods.Add((point, 1), pod.Value);
                var steps = pod.Key.Y - 1 + int.Abs(pod.Key.X - point);
                next.Score += podType.Cost * steps;
                Enqueue(next);
            }
        }
    }
}

Console.WriteLine(bestScore);

// 43225 too low

void Enqueue((Dictionary<(int X, int Y), char> Pods, long Score) next)
{
    if (bestScore is null || next.Score < bestScore)
    {
        var priority = next.Pods.Sum(p => int.Abs(p.Key.X - types[p.Value].RoomX) * types[p.Value].Cost);
        states.Enqueue(next, priority);
    }
}

string FormatState((Dictionary<(int X, int Y), char> Pods, long Score) state) 
    => string.Concat(state.Pods.OrderBy(s => s.Value).ThenBy(s => s.Key).Select(p => string.Concat(p.Value, p.Key.X, p.Key.Y)));

void PrintGrid(Dictionary<(int X, int Y), char> pods)
{
    for (int y = 0; y < grid.Max(b => b.Key.Y) + 1; y++)
    {
        Console.WriteLine(Enumerable.Range(0, grid.Max(b => b.Key.X) + 1)
            .Select(x => pods.TryGetValue((x, y), out var pod) ? pod : grid.TryGetValue((x, y), out var value) ? value : ' ').ToArray());
    }
    Console.WriteLine();
}