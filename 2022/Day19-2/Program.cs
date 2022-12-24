using System.Text.RegularExpressions;

var blueprints = File.ReadLines("input.txt").Select(line =>
{
    var match = Regex.Match(line, @"Blueprint (?<ID>\d+): Each ore robot costs (?<OrePerOre>\d+) ore. Each clay robot costs (?<OrePerClay>\d+) ore. Each obsidian robot costs (?<OrePerObsidian>\d+) ore and (?<ClayPerObsidian>\d+) clay. Each geode robot costs (?<OrePerGeode>\d+) ore and (?<ObsidianPerGeode>\d+) obsidian.");
    var ID = int.Parse(match.Groups["ID"].Value);
    var OrePerOre = int.Parse(match.Groups["OrePerOre"].Value);
    var OrePerClay = int.Parse(match.Groups["OrePerClay"].Value);
    var OrePerObsidian = int.Parse(match.Groups["OrePerObsidian"].Value);
    var ClayPerObsidian = int.Parse(match.Groups["ClayPerObsidian"].Value);
    var OrePerGeode = int.Parse(match.Groups["OrePerGeode"].Value);
    var ObsidianPerGeode = int.Parse(match.Groups["ObsidianPerGeode"].Value);
    return new Blueprint (ID, OrePerOre, OrePerClay, OrePerObsidian, ClayPerObsidian, OrePerGeode, ObsidianPerGeode, line);
}).ToArray();

var totalMinutes = 32;
Blueprint blueprint = default!;
var maxResult = 0;

var results = new Dictionary<int, int>();
foreach (var blueprintt in blueprints[0..3])
{
    maxResult = 0;
    blueprint = blueprintt;
    var geodes = CaclulateGeodes();
    results[blueprintt.ID] = geodes;
}
Console.WriteLine(results[1] * results[2] * results[3]);
//Console.WriteLine(results.Sum(r => r.Key * r.Value));

int CaclulateGeodes()
{
    var results = ProcessMinute(1, "", new TypeCounts(1, 0, 0, 0), new TypeCounts());

    Console.WriteLine($"{blueprint.ID,2} = {results.Geodes,2} {results.Build}");
    return results.Geodes;
}

(string Build, int Geodes) ProcessMinute(int minute, string build, TypeCounts robots, TypeCounts items)
{
    if (minute == totalMinutes)
    {
        if (items.Geode + robots.Geode > maxResult)
        {
            maxResult = items.Geode + robots.Geode;
            //Console.WriteLine($"{blueprint.ID,2} {build}={items.Geode + robots.Geode}");
        }
        return (build, items.Geode + robots.Geode);
    }

    var totalPossible = items.Geode + (totalMinutes - minute + 1) * robots.Geode + (totalMinutes - minute) * (totalMinutes - minute + 1) / 2;
    if (totalPossible < maxResult)
    {
        //Console.WriteLine($"{blueprint.ID,2} {build}*={items.Geode + robots.Geode}");
        return (build + "*", items.Geode + robots.Geode);
    }

    var options = new List<string>();

    if (minute < totalMinutes && items.Ore >= blueprint.OrePerGeode && items.Obsidian >= blueprint.ObsidianPerGeode)
        options.Add("G");
    else
    {
        if (minute < totalMinutes - 2 && items.Ore >= blueprint.OrePerObsidian && items.Clay >= blueprint.ClayPerObsidian)
            options.Add("B");

        if (minute < totalMinutes - 4 && items.Ore >= blueprint.OrePerClay)
            options.Add("C");

        if (minute < totalMinutes - 2 && items.Ore >= blueprint.OrePerOre)
        {
            var maxOre = new[] { blueprint.OrePerClay, blueprint.OrePerObsidian, blueprint.OrePerGeode }.Max();
            if (robots.Ore * (totalMinutes - minute) + items.Ore < maxOre * (totalMinutes - minute))
                options.Add("O");
        }
        if (items.Ore < new[] { blueprint.OrePerOre, blueprint.OrePerClay, blueprint.OrePerObsidian, blueprint.OrePerGeode }.Max()
            || (robots.Clay > 0 && items.Clay < blueprint.ClayPerObsidian)
            || (robots.Obsidian > 0 && items.Obsidian < blueprint.ObsidianPerGeode)
            )
            options.Add("");
    }

    if (options.Count == 0)
        options.Add("");

    items.Ore += robots.Ore;
    items.Clay += robots.Clay;
    items.Obsidian += robots.Obsidian;
    items.Geode += robots.Geode;
    minute++;

    var results = options.Select(o =>
    {
        var optRobots = new TypeCounts
        {
            Ore = robots.Ore + (o == "O" ? 1 : 0),
            Clay = robots.Clay + (o == "C" ? 1 : 0),
            Obsidian = robots.Obsidian + (o == "B" ? 1 : 0),
            Geode = robots.Geode + (o == "G" ? 1 : 0)
        };
        var optItems = new TypeCounts
        {
            Ore = items.Ore - (o switch { "O" => blueprint.OrePerOre, "C" => blueprint.OrePerClay, "B" => blueprint.OrePerObsidian, "G" => blueprint.OrePerGeode, _ => 0 }),
            Clay = items.Clay - (o == "B" ? blueprint.ClayPerObsidian : 0),
            Obsidian = items.Obsidian - (o == "G" ? blueprint.ObsidianPerGeode : 0),
            Geode = items.Geode
        };
        return ProcessMinute(minute, build + o, optRobots, optItems);
    });

    return results.MaxBy(r => r.Geodes);
}

record struct Blueprint (int ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode, string Line);
record struct TypeCounts (int Ore, int Clay, int Obsidian, int Geode);
