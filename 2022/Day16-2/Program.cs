using System.ComponentModel;
using System.Text.RegularExpressions;

var valves = File.ReadLines("input.txt").Select(line =>
{
    var match = Regex.Match(line, @"^Valve (?<valve>\w\w) has flow rate=(?<rate>\d+); tunnels? leads? to valves? (?<paths>.*)$");
    var Valve = match.Groups["valve"].Value;
    var Flow = int.Parse(match.Groups["rate"].Value);
    var Paths = match.Groups["paths"].Value.Split(", ");
    return (Valve, Flow, Paths);
}).ToArray();

var routes = valves.SelectMany(v => v.Paths.Select(p => new[] { v.Valve, p })).ToDictionary(v => (From: v[0], To: v[v.Length - 1]));

for (int i = 2; routes.Count < valves.Length * (valves.Length - 1); i++)
{
    foreach (var route in routes.Where(r => r.Value.Length == i).ToArray())
    {
        foreach (var direct in routes.Where(d => d.Value.Length == 2 && d.Key.From == route.Key.To && d.Key.To != route.Key.From).ToArray())
        {
            var key = (route.Key.From, direct.Key.To);
            if (!routes.ContainsKey(key))
            {
                routes[key] = route.Value.Concat(direct.Value[^1..]).ToArray();
            }
        }
    }
}

var destValves = valves.Where(v => v.Flow > 0).ToDictionary(v => v.Valve, v => v.Flow);

var paths = GetPaths(new[] { "AA" }, destValves.Keys.ToArray(), 0, 0);

var paths2 = paths.Select(p => 
{
    var p2s = GetPaths(new[] { "AA" }, destValves.Keys.Except(p.path).ToArray(), 0, 0);
    var max = p2s.OrderByDescending(p => p.totalFlow).First();
    return new { path1 = p.path, flow1 = p.totalFlow, path2 = max.path, flow2 = max.totalFlow, totalFlow = p.totalFlow + max.totalFlow };
});

var best = paths2.OrderByDescending(p => p.totalFlow).First();

Console.WriteLine($"{best.totalFlow} = {string.Join('>', best.path1)}={best.flow1} + {string.Join('>', best.path2)}={best.flow2}");


(string[] path, long totalFlow)[] GetPaths(string[] path, string[] dests, int time, long totalFlow)
{
    var nextDests = dests.Except(path).ToArray();
    var current = path.Last();
    var bestNext = nextDests.Select(n =>
    {
        var routeLen = routes[(current, n)].Length;
        var valveFlow = destValves[n];
        var value = valveFlow * (26 - Math.Min(time + routeLen, 26));
        return (n, routeLen, value);
    }).Where(b => b.value > 0).OrderByDescending(b => b.value).Take(10).ToArray();

    if (bestNext.Length == 0)
    {
        //Console.WriteLine($"{string.Join('>', path)}={totalFlow}");
        return new[] { (path, totalFlow) };
    }

    return bestNext.SelectMany(d => GetPaths(path.Append(d.n).ToArray(), dests, time + d.routeLen, totalFlow + d.value)).ToArray();
}
