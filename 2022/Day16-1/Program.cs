﻿using System.ComponentModel;
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

var paths = GetPaths(new[] { "AA" }, 0, 0);

var maxFlow = paths.Max(p => p.totalFlow);

Console.WriteLine(maxFlow);

Console.WriteLine($"{string.Join('>', paths.First(p => p.totalFlow == maxFlow).path)}");


(string[] path, long totalFlow)[] GetPaths(string[] path, int time, long totalFlow)
{
    var nextDests = destValves.Keys.Except(path).ToArray();
    var current = path.Last();
    var bestNext = nextDests.Select(n =>
    {
        var routeLen = routes[(current, n)].Length;
        var valveFlow = destValves[n];
        var value = valveFlow * (30 - Math.Min(time + routeLen, 30));
        return (n, routeLen, value);
    }).Where(b => b.value > 0).OrderByDescending(b => b.value).Take(10).ToArray();

    if (bestNext.Length == 0)
    {
        Console.WriteLine($"{string.Join('>', path)}={totalFlow}");
        return new[] { (path, totalFlow) };
    }

    return bestNext.SelectMany(d => GetPaths(path.Append(d.n).ToArray(), time + d.routeLen, totalFlow + d.value)).ToArray();
}
