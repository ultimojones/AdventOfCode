using System.Reflection.Emit;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

var blueprints = File.ReadLines("sample.txt").Select(line =>
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

var results = new Dictionary<int, int>();

foreach (var blueprint in blueprints)
{
    var geodes = CaclulateGeodes(blueprint);
    results[blueprint.ID] = geodes;
}

var final = results.Sum(r => r.Key * r.Value);
Console.WriteLine(final);

int CaclulateGeodes(Blueprint blueprint)
{
    var items = new TypeCounts();
    var robots = new TypeCounts(1, 0, 0, 0);

    var result = CalcBuilds(blueprint, 1, robots, items, "");//.MaxBy(t => t.Geodes);

    Console.WriteLine($"{result.Build}={result.Geodes}");
    return result.Geodes;
}

(string Build, int Geodes) CalcBuilds(Blueprint blueprint, int minute, TypeCounts robots, TypeCounts items, string build)
{
    if (minute == 24)
        return (build, items.Geode + robots.Geode);

    var options = new List<string>() { "" };

    if (items.Ore >= blueprint.OrePerGeode && items.Obsidian >= blueprint.ObsidianPerGeode)
        options.Add("G");

    if (minute < 22 && items.Ore >= blueprint.OrePerObsidian && items.Clay >= blueprint.ClayPerObsidian && robots.Obsidian < blueprint.ObsidianPerGeode)
        options.Add("N");

    if (minute < 20 && items.Ore >= blueprint.OrePerClay && robots.Clay < blueprint.ClayPerObsidian)
        options.Add("C");

    if (minute < 22 && items.Ore >= blueprint.OrePerOre
        && (robots.Ore < new[] { blueprint.OrePerOre, blueprint.OrePerClay, blueprint.OrePerObsidian, blueprint.OrePerGeode }.Max()))
        options.Add("O");

    var result = options.Select(o =>
    {
        var optitems = items;
        optitems.Ore += robots.Ore;
        optitems.Clay += robots.Clay;
        optitems.Obsidian += robots.Obsidian;
        optitems.Geode += robots.Geode;

        var optrobots = robots;
        switch (o)
        {
            case "G":
                optrobots.Geode++;
                optitems.Ore -= blueprint.OrePerGeode;
                optitems.Obsidian -= blueprint.ObsidianPerGeode;
                break;
            case "N":
                optrobots.Obsidian++;
                optitems.Ore -= blueprint.OrePerObsidian;
                optitems.Clay -= blueprint.ClayPerObsidian;
                break;
            case "C":
                optrobots.Clay++;
                optitems.Ore -= blueprint.OrePerClay;
                break;
            case "O":
                optrobots.Ore++;
                optitems.Ore -= blueprint.OrePerOre;
                break;
        }

        return CalcBuilds(blueprint, minute + 1, optrobots, optitems, build + o);
    });

    return result.MaxBy(r => r.Geodes);
}

record struct Blueprint (int ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode, string Line);
record struct TypeCounts (int Ore, int Clay, int Obsidian, int Geode);
