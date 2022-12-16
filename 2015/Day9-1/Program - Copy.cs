using System.Linq;
using System.Text.RegularExpressions;

var legs = File.ReadLines("input.txt").Select(line =>
{
    var match = Regex.Match(line, @"^(?<Depart>\w+) to (?<Arrive>\w+) = (?<Distance>\d+)$");
    var Depart = match.Groups["Depart"].Value;
    var Arrive = match.Groups["Arrive"].Value;
    var Distance = int.Parse(match.Groups["Distance"].Value);
    return new Leg(Depart, Arrive, Distance);
}).ToList();

var starts = legs.Select(l => l.Depart).Distinct().ToArray();
var ends = legs.Select(l => l.Arrive).Distinct().ToArray();
var legCount = starts.Concat(ends).Distinct().Count() - 1;

foreach (var start in starts)
{
    var routes = GetRoutes(start, Array.Empty<Leg>());

    foreach (var route in routes)
    {
        foreach (var leg in route)
        {
            Console.Write($"{leg.Depart}-{leg.Arrive}={leg.Distance}|");
        }
        Console.WriteLine(route.Sum(l => l.Distance));
    }
}

Leg[][] GetRoutes(string start, Leg[] route)
{
    if (route.Length == legCount)
        return new[] { route };

    var visited = route.SelectMany(r => new[] { r.Depart, r.Arrive }).Distinct().ToArray();
    var candidates = legs.Where(l => l.Depart == start && !visited.Contains(l.Arrive)).ToArray();
    var newRoutes = candidates
        .SelectMany(l =>
        {
            var newRoute = route.Append(l).ToArray();
            var subroutes = GetRoutes(l.Arrive, newRoute);
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