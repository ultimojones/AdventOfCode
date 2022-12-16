using System.Linq;
using System.Text.RegularExpressions;

var distances = File.ReadLines("sample.txt").Select(line =>
{
    var match = Regex.Match(line, @"^(?<A>\w+) to (?<B>\w+) = (?<D>\d+)$");
    var A = match.Groups["A"].Value;
    var B = match.Groups["B"].Value;
    var D = int.Parse(match.Groups["D"].Value);
    return (A, B, D);
}).ToArray();

var stops = distances.SelectMany(d => new[] { d.A, d.B }).Distinct().ToArray();
var legs = stops.Length - 1;

foreach (var start in stops)
{
    var routes = GetRoutes(start, Array.Empty<(string, string, int)>());

    foreach (var route in routes)
    {
        foreach (var leg in route)
        {
            Console.Write($"{leg.A}-{leg.B}={leg.D}|");
        }
        Console.WriteLine(route.Sum(l => l.D));
    }
}

(string A, string B, int D)[][] GetRoutes(string start, (string A, string B, int D)[] route)
{
    if (route.Length == legs)
        return new[] { route };

    var visited = route.SelectMany(r => new[] { r.A, r.B }).Distinct().ToArray();
    var candidates = distances.Where(d => (d.A == start || d.B == start) && (!visited.Any(v => v == d.A || v == d.B))).ToArray();
    var newRoutes = candidates
        .SelectMany(l =>
        {
            var newLeg = l.A == start ? l : (A: l.B, B: l.A, D: l.D);
            var newRoute = route.Append(newLeg).ToArray();
            var subroutes = GetRoutes(newLeg.B, newRoute);
            return subroutes;
        }).ToArray();

    return newRoutes;
}







class Leg
{
    public string Depart { get; set; }
    public string Arrive { get; set; }
    public int Distance { get; set; }
    public Leg(string depart, string arrive, int distance)
    {
        Depart = depart;
        Arrive = arrive;
        Distance = distance;
    }
}







//IEnumerable<(string, string, int)[]> GetRoutes(string start, (string Depart, string Arrive, int Distance)[] route)
//{
//    var visited = route.Select(r => r.Depart).Concat(route.Select(r => r.Arrive)).Distinct().ToArray();
//    return legs.Where(l => l.Depart == start)
//    //var routes = legs.Where(l => l.Depart == start && !visited.Contains(l.Arrive))
//        .SelectMany(l =>
//        {
//            var newRoute = route.Append(l).ToArray();
//            return GetRoutes(l.Arrive, newRoute);

//        });
//}


//foreach (var start in starts)
//{
//    var stops = ends.Except(new[] { start }).ToList();
//    var routes = GetRoutes(start, stops);
//    routes.ToList().ForEach(r => Console.WriteLine(r));
//}

//IEnumerable<(string, string, int)> GetRoutes(string start, IEnumerable<string> stops)
//{
//    return legs.Where(l => l.Depart == start && stops.Contains(l.Arrive))
//        .SelectMany(l => new[] { l }.Concat(GetRoutes(l.Arrive, stops.Except(new[] { l.Arrive }))));
//}