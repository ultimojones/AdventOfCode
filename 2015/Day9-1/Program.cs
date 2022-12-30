using System.Text.RegularExpressions;

var cities = new Dictionary<string, Dictionary<string, int>>();

foreach (var line in File.ReadLines("input.txt"))
{
    var match = Regex.Match(line, @"^(?<A>\w+) to (?<B>\w+) = (?<D>\d+)$");
    var A = match.Groups["A"].Value;
    var B = match.Groups["B"].Value;
    var D = int.Parse(match.Groups["D"].Value);
    if (cities.TryGetValue(A, out var cityA)) 
        cityA.Add(B, D); 
    else
        cities[A] = new Dictionary<string, int> { { B, D } };
    if (cities.TryGetValue(B, out var cityB))
        cityB.Add(A, D);
    else
        cities[B] = new Dictionary<string, int> { { A, D } };
}

foreach (var city in cities)
{
    Console.WriteLine($"{city.Key}: {string.Join(' ', city.Value)}");
}

List<string> bestRoute;
int bestDist = 0;

foreach (var city in cities.Keys)
{
    FindShortest(city, 0, Array.Empty<string>(), cities.Keys.Except(new[] { city }));
}

void FindShortest(string cur, int dist, IEnumerable<string> route, IEnumerable<string> dests)
{
    var curRoute = route.Append(cur).ToList();

    if (!dests.Any())
    {
        if (bestDist == 0 || dist > bestDist)
        {
            bestDist = dist;
            bestRoute = curRoute;
            Console.WriteLine($"{string.Join("->", curRoute)} = {dist}");
        }
    }
    else
    {
        foreach (var dest in dests)
        {
            FindShortest(dest, dist + cities![cur][dest], curRoute, dests.Except(new[] { dest }));
        }
    }
}
