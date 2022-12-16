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

var wantedRoutes = routes.Where(r => valves.Any(v => v.Valve == r.Key.To && v.Flow > 0)).ToArray();

var results = wantedRoutes.Where(r => r.Key.From == "AA")
    .SelectMany(r => GetNext(1, "AA", Array.Empty<string>(), 0, r.Value));

Console.WriteLine(results.Max());

int[] GetNext(int time, string location, string[] open, int pressure, string[] route)
{
    var currentPressure = open.Join(valves, o => o, i => i.Valve, (o, i) => i.Flow).Sum();
    Console.WriteLine($"== Minute {time} ==");
    Console.WriteLine(open.Length switch
    {
        0 => "No valves are open.",
        1 => $"Valve {open[0]} is open, releasing {currentPressure} pressure.",
        _ => $"Valves {string.Join(", ", open)} are open, releasing {currentPressure} pressure."
    });

    if (time >= 30)
        return new[] { pressure };

    if (route.Length > 1)
    {
        Console.WriteLine($"You move to valve {route[1]}.");
        return GetNext(time + 1, route[1], open, pressure + currentPressure, route[1..]);
    }
    else
    {
        if (!open.Contains(location))
        {
            Console.WriteLine($"You open valve {location}.");
            return GetNext(time + 1, location, open.Concat(new[] { location }).ToArray(), pressure + currentPressure, route);
        }
        else
        {
            var results = wantedRoutes.Where(r => r.Key.From == location && !open.Contains(r.Key.To))
                .SelectMany(r =>
                {
                    Console.WriteLine($"You move to valve {r.Value[1]}.");

                    return GetNext(time + 1, r.Value[1], open, pressure + currentPressure, r.Value[1..]);
                }).ToArray();

            if (results.Length > 0)
                return results;
            else
                return GetNext(time + 1, location, open, pressure + currentPressure, Array.Empty<string>());
        }
    }
}
