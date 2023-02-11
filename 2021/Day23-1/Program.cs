var grid = new Dictionary<(int X, int Y), char>();
var start = new Dictionary<(int X, int Y), char>();
var states = new PriorityQueue<(Dictionary<(int X, int Y), char> Pods, long Score), int>();
var visited = new Dictionary<string, long>();
var border = new List<(int X, int Y)>();
var lines = File.ReadAllLines("input.txt");
for (int y = 0; y < lines.Length; y++)
{
    for (int x = 0; x < lines[y].Length; x++)
    {
        switch (lines[y][x])
        {
            case '.':
                grid.Add((x, y), lines[y][x]);
                break;
            case >= 'A' and <= 'D':
                grid.Add((x, y), '.');
                start.Add((x, y), lines[y][x]);
                break;
            case '#':
                border.Add((x, y));
                break;
        }
    }
}
var DestX = new Dictionary<char, int> { { 'A', 3 }, { 'B', 5 }, { 'C', 7 }, { 'D', 9 } };
var Cost = new Dictionary<char, int> { { 'A', 1 }, { 'B', 10 }, { 'C', 100 }, { 'D', 1000 } };
var StopsX = grid.Where(g => g.Key.Y == 1).Select(g => g.Key.X).Except(DestX.Select(d => d.Value)).ToList();

PrintGrid(start);
states.Enqueue((start, 0), 0);
long? bestScore = null!;

while (states.TryDequeue(out var state, out var priority))
{
    var stateKey = FormatState(state);
    if (visited.TryGetValue(stateKey, out var stateBest) && stateBest <= state.Score)
        continue;
    visited[stateKey] = state.Score;

    foreach (var pod in state.Pods)
    {
        var destX = DestX[pod.Value];
        if (pod.Key.Y == 3 && state.Pods.ContainsKey((pod.Key.X, 2))) continue;
        if (pod.Key.X == destX && (pod.Key.Y == 3 || state.Pods[(destX, 3)] == pod.Value)) continue;
        if (!state.Pods.ContainsKey((destX, 2)) && (!state.Pods.ContainsKey((destX, 3)) || state.Pods[(destX, 3)] == pod.Value) 
            && !state.Pods.Any(p => p.Key.Y == 1 && int.Min(pod.Key.X, destX) < p.Key.X && p.Key.X < int.Max(pod.Key.X, destX)))
        {
            var next = (Pods: new Dictionary<(int X, int Y), char>(state.Pods), Score: state.Score);
            var steps = pod.Key.Y - 1 + int.Abs(pod.Key.X - destX) + 1;
            next.Pods.Remove(pod.Key);

            if (state.Pods.ContainsKey((destX, 3)))
            {
                next.Pods.Add((destX, 2), pod.Value);
                next.Score += Cost[pod.Value] * steps;
                if (next.Pods.All(p => p.Key.X == DestX[p.Value]))
                {
                    if (bestScore is null || next.Score < bestScore)
                    {
                        bestScore = next.Score;
                        Console.WriteLine(new { stateKey, bestScore });
                    }
                }
                else
                {
                    Enqueue(next);
                }
            }
            else
            {
                next.Pods.Add((destX, 3), pod.Value);
                next.Score += Cost[pod.Value] * (steps + 1);
                Enqueue(next);
            }
        }
        else
        {
            var leftpods = state.Pods.Where(p => p.Key.Y == 1 && p.Key.X < pod.Key.X).ToArray();
            var min = leftpods.Length == 0 ? 1 : leftpods.Max(p => p.Key.X) + 1;
            var rightpods = state.Pods.Where(p => p.Key.Y == 1 && p.Key.X > pod.Key.X).ToArray();
            var max = rightpods.Length == 0 ? 11 : rightpods.Min(p => p.Key.X) - 1;
            foreach (var point in Enumerable.Range(min, max - min + 1).Except(DestX.Select(d => d.Value)))
            {
                var next = (Pods: new Dictionary<(int X, int Y), char>(state.Pods), Score: state.Score);
                next.Pods.Remove(pod.Key);
                next.Pods.Add((point, 1), pod.Value);
                var steps = pod.Key.Y - 1 + int.Abs(pod.Key.X - point);
                next.Score += Cost[pod.Value] * steps;
                Enqueue(next);
            }
        }
    }
}




Console.WriteLine(bestScore);

void Enqueue((Dictionary<(int X, int Y), char> Pods, long Score) next)
{
    var priority = next.Pods.Sum(p => int.Abs(p.Key.X - DestX[p.Value]) * Cost[p.Value]);
    if (bestScore is null || next.Score < bestScore)
        states.Enqueue(next, priority);
}


string FormatState((Dictionary<(int X, int Y), char> Pods, long Score) state)
{
    var podVals = string.Concat(state.Pods.OrderBy(s => s.Value).ThenBy(s => s.Key).Select(p => string.Concat(p.Value, p.Key.X, p.Key.Y)));
    return String.Concat(podVals);
}

void PrintGrid(Dictionary<(int X, int Y), char> pods)
{
    for (int y = 0; y < border.Max(b => b.Y) + 1; y++)
    {
        Console.WriteLine(Enumerable.Range(0, border.Max(b => b.X) + 1).Select(x => border.Contains((x, y)) ? '#' : pods.TryGetValue((x, y), out var pod) ? pod : grid.TryGetValue((x, y), out var value) ? value : ' ').ToArray());
    }
    Console.WriteLine();
}