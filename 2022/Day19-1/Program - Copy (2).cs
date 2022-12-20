using System.Text.RegularExpressions;
using System.Xml.Linq;

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


foreach (var blueprint in blueprints)
{
    Console.WriteLine(new
    {
        blueprint.ID,
        blueprint.OrePerOre,
        blueprint.OrePerClay,
        oreCost = (double)blueprint.ObsidianPerGeode + blueprint.ClayPerObsidian / blueprint.OrePerGeode + blueprint.OrePerObsidian + blueprint.OrePerClay
    });

    var testresult = TestBlueprint(blueprint, new[] { Robot.Clay, Robot.Clay, Robot.Clay, Robot.Obsidian, Robot.Clay, Robot.Obsidian, Robot.Geode, Robot.Geode });

    Console.WriteLine($"{blueprint.ID}: {testresult}");
}

(int ore, int clay, int obsidian, int geode) TestBlueprint((string ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode) blueprint, Robot[] robots)
{
    (int ore, int clay, int obsidian, int geode) numrobots = (1, 0, 0, 0);
    (int ore, int clay, int obsidian, int geode) items = (0, 0, 0, 0);
    int robotid = 0;

    bool CanAffordOre() => items.ore >= blueprint.OrePerOre;
    bool CanAffordClay() => items.ore >= blueprint.OrePerClay;
    bool CanAffordObsidian() => items.ore >= blueprint.OrePerObsidian && items.clay >= blueprint.ClayPerObsidian;
    bool CanAffordGeode() => items.ore >= blueprint.OrePerGeode && items.obsidian >= blueprint.ObsidianPerGeode;

    for (int i = 0; i < 24; i++)
    {
        (int ore, int clay, int obsidian, int geode) newrobots = (0, 0, 0, 0);
        if (robotid < robots.Length)
            switch (robots[robotid])
            {
                case Robot.Ore when CanAffordOre():
                    newrobots.ore++;
                    items.ore -= blueprint.OrePerOre;
                    robotid++;
                    break;
                case Robot.Clay when CanAffordClay():
                    newrobots.clay++;
                    items.ore -= blueprint.OrePerClay;
                    robotid++;
                    break;
                case Robot.Obsidian when CanAffordObsidian():
                    newrobots.obsidian++;
                    items.ore -= blueprint.OrePerObsidian;
                    items.clay -= blueprint.ClayPerObsidian;
                    robotid++;
                    break;
                case Robot.Geode when CanAffordGeode():
                    newrobots.geode++;
                    items.ore -= blueprint.OrePerGeode;
                    items.obsidian -= blueprint.ObsidianPerGeode;
                    robotid++;
                    break;
                default:
                    break;
            }

        items.ore += numrobots.ore;
        items.clay += numrobots.clay;
        items.obsidian += numrobots.obsidian;
        items.geode += numrobots.geode;

        numrobots.ore += newrobots.ore;
        numrobots.clay += newrobots.clay;
        numrobots.obsidian += newrobots.obsidian;
        numrobots.geode += newrobots.geode;
    }

    return items;
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

    bool NeedMoreOre() => (double)robots.clay / robots.ore < (double)blueprint.ObsidianPerGeode + blueprint.ClayPerObsidian / blueprint.OrePerGeode + blueprint.OrePerObsidian + blueprint.OrePerClay;
    bool NeedMoreClay() => (double)robots.clay / robots.ore < (double)blueprint.ObsidianPerGeode + blueprint.ClayPerObsidian / blueprint.OrePerGeode + blueprint.OrePerObsidian + blueprint.OrePerClay;

    var oreCost = blueprint.OrePerGeode + blueprint.OrePerObsidian + blueprint.OrePerClay;


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


enum Robot
{
    Ore,
    Clay,
    Obsidian,
    Geode
}
