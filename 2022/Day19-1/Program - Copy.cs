using System.Text.RegularExpressions;

var blueprints = File.ReadLines("sample.txt").Select(line =>
{
    var match = Regex.Match(line, @"Blueprint (?<ID>\d+): Each ore robot costs (?<OrePerOre>\d+) ore. Each clay robot costs (?<OrePerClay>\d+) ore. Each obsidian robot costs (?<OrePerObsidian>\d+) ore and (?<ClayPerObsidian>\d+) clay. Each geode robot costs (?<OrePerGeode>\d+) ore and (?<ObsidianPerGeode>\d+) obsidian.");
    var ID = match.Groups["ID"].Value;
    var OrePerOre = int.Parse(match.Groups["OrePerOre"].Value);
    var OrePerClay = int.Parse(match.Groups["OrePerClay"].Value);
    var OrePerObsidian = int.Parse(match.Groups["OrePerObsidian"].Value);
    var ClayPerObsidian = int.Parse(match.Groups["ClayPerObsidian"].Value);
    var OrePerGeode = int.Parse(match.Groups["OrePerGeode"].Value);
    var ObsidianPerGeode = int.Parse(match.Groups["ObsidianPerGeode"].Value);
    return (ID, OrePerOre, OrePerClay, OrePerObsidian, ClayPerObsidian, OrePerGeode, ObsidianPerGeode);
}).ToArray();

foreach (var item in blueprints)
{
    Console.WriteLine(new { item.ID, item.OrePerOre, item.OrePerClay, 
        ClayPerGeode = item.ObsidianPerGeode * item.ClayPerObsidian});

    var result = CalculateGeodes(item, (1, 0, 0, 0), (0, 0, 0, 0));
    Console.WriteLine(result);
}

int CalculateGeodes(
    (string ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode) blueprint,
    (int ore, int clay, int obsidian, int geode) robots,
    (int ore, int clay, int obsidian, int geode) items)
{
    bool CanAffordOre() => items.ore >= blueprint.OrePerOre;
    bool CanAffordClay() => items.ore >= blueprint.OrePerClay;
    bool CanAffordObsidian() => items.ore >= blueprint.OrePerObsidian && items.clay >= blueprint.ClayPerObsidian;
    bool CanAffordGeode() => items.ore >= blueprint.OrePerGeode && items.obsidian >= blueprint.ObsidianPerGeode;

    bool NeedMoreOre() => (robots.clay / (double)robots.ore) > (blueprint.ObsidianPerGeode * blueprint.ClayPerObsidian / (double)blueprint.OrePerGeode);
    bool NeedMoreClay() => (robots.clay / (double)robots.ore) < (blueprint.ObsidianPerGeode / (double)blueprint.OrePerGeode);

    for (int i = 0; i < 24; i++)
    {
        (int ore, int clay, int obsidian, int geode) building = (0, 0, 0, 0);

        if (CanAffordGeode())
        {
            building.geode += 1;
            items.ore -= blueprint.OrePerGeode;
            items.obsidian -= blueprint.ObsidianPerGeode;
        }
        if (CanAffordObsidian())
        {
            building.obsidian += 1;
            items.ore -= blueprint.OrePerGeode;
            items.obsidian -= blueprint.ClayPerObsidian;
        }
        if (CanAffordClay() && NeedMoreClay())
        {
            building.clay += 1;
            items.ore -= blueprint.OrePerClay;
        }
        if (CanAffordOre() && NeedMoreOre())
        {
            building.clay += 1;
            items.ore -= blueprint.OrePerClay;
        }

        items.ore += robots.ore;
        items.clay += robots.clay;
        items.obsidian += robots.obsidian;
        items.geode += robots.geode;

        robots.ore += building.ore;
        robots.clay += building.clay;
        robots.obsidian += building.obsidian;
        robots.geode += building.geode; 
    }

    return items.geode;
}
