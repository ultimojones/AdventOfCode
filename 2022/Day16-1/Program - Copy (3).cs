using System.ComponentModel;
using System.Text.RegularExpressions;

var valves = File.ReadLines("sample.txt").Select(line =>
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

var paths = GetPaths(new[] { "AA" });

int maxFlow = paths.Max(p => CalcFlow(p));

Console.WriteLine(maxFlow);


string[][] GetPaths(string[] path)
{
    var nextDests = destValves.Keys.Except(path).ToArray();
    if (nextDests.Length == 0)
    {
        Console.WriteLine(string.Join('>', path));
        return new[] { path };
    }

    return nextDests.SelectMany(d => GetPaths(path.Append(d).ToArray())).ToArray();
}

int CalcFlow(string[] path)
{
    var time = 0;
    var totalFlow = 0;

    Console.Write(string.Join('>', path));

    for (int i = 0; i < path.Length - 1; i++)
    {
        var routeLen = routes[(path[i], path[i + 1])].Length;
        var valveFlow = destValves[path[i + 1]];
        time += routeLen;
        totalFlow += valveFlow * (30 - time);
    }

    Console.WriteLine($"={totalFlow}");
    return totalFlow;
}