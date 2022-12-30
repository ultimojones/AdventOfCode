using System.Text.RegularExpressions;

var people = new Dictionary<string, Dictionary<string, int>>();

foreach (var line in File.ReadLines("input.txt"))
{
    var match = Regex.Match(line, @"^(?<person>\w+) would (?<effect>gain|lose) (?<points>\d+) happiness units by sitting next to (?<other>\w+).$");
    var effectPoints = int.Parse(match.Groups["points"].Value) * (match.Groups["effect"].Value == "gain" ? 1 : -1);
    if (people.TryGetValue(match.Groups["person"].Value, out var points))
    {
        points[match.Groups["other"].Value] = effectPoints;
    }
    else
    {
        people[match.Groups["person"].Value] = new Dictionary<string, int> { { match.Groups["other"].Value, effectPoints } };
    }
}

var top = CalcSeating(Array.Empty<string>(), people.Keys).Select(o =>
{
    var happiness = 0;
    var list = o.ToList();
    for (int i = 0, prev = list.Count - 1, next = 1; i < list.Count; i++, prev = (prev + 1) % list.Count, next = (next + 1) % list.Count)
    {
        happiness += people[list[i]].Where(p => p.Key == list[prev] || p.Key == list[next]).Sum(p => p.Value);
    }
    return (Seating: string.Join(",", list), Happiness: happiness);
}).MaxBy(o => o.Happiness);

Console.WriteLine(top);


//var options = CalcSeating(Array.Empty<string>(), people.Keys);
//foreach (var option in options)
//{
//    var happiness = 0;
//    var list = option.ToList();
//    for (int i = 0, prev = list.Count - 1, next = 1; i < list.Count; i++, prev = (prev + 1) % list.Count, next = (next + 1) % list.Count)
//    {
//        happiness += people[list[i]].Where(p => p.Key == list[prev] || p.Key == list[next]).Sum(p => p.Value);
//    }

//    Console.WriteLine($"{string.Join(",", option)} = {happiness}");
//}


IEnumerable<IEnumerable<string>> CalcSeating(IEnumerable<string> seated, IEnumerable<string> unseated)
{
    if (!unseated.Any())
        yield return seated;

    foreach (var next in unseated)
    {
        foreach (var seating in CalcSeating(seated.Append(next), unseated.Except(new[] { next })))
        {
            yield return seating;
        }
    }
}