using System.Text.RegularExpressions;

var reindeer = File.ReadLines("input.txt").Select(line =>
{
    var r = Regex.Match(line, @"^(?<Name>\w+) can fly (?<Speed>\d+) km/s for (?<FlyingTime>\d+) seconds, but then must rest for (?<Rest>\d+) seconds.$");
    return (Name: r.Groups["Name"].Value, Speed: int.Parse(r.Groups["Speed"].Value), FlyingTime: int.Parse(r.Groups["FlyingTime"].Value), RestTime: int.Parse(r.Groups["Rest"].Value));
}).ToList();

const int finish = 2503;

var results = reindeer.ToDictionary(r => r.Name, r => 0);

for (int i = 1; i <= finish; i++)
{
    var distances = reindeer.Select(r =>
    {
        var div = Math.DivRem(i, r.FlyingTime + r.RestTime);
        var distance = r.Speed * r.FlyingTime * div.Quotient;
        if (div.Remainder <= r.FlyingTime)
            distance += r.Speed * div.Remainder;
        else
            distance += r.Speed * r.FlyingTime;
        return (Name: r.Name, Distance: distance);
    });
    foreach (var leader in distances.Where(d => d.Distance == distances.Max(r => r.Distance)))
    {
        results[leader.Name]++;
    }
}


foreach (var item in results)
{
    Console.WriteLine(item);
}
