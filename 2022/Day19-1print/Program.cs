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
    return (ID, OrePerOre, OrePerClay, OrePerObsidian, ClayPerObsidian, OrePerGeode, ObsidianPerGeode);
}).ToArray();

PrintBuild(blueprints[5], "OCCCBOBBCG");

void PrintBuild((int ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode) blueprint, string build)
{
    int robotsOre = 1, robotsClay = 0, robotsObsidian = 0, robotsGeode = 0;
    int itemsOre = 0, itemsClay = 0, itemsObsidian = 0, itemsGeode = 0;
    int buildOrder = 0;

    for (int i = 1; i <= 24; i++)
    {
        Console.WriteLine($"== Minute {i} ==");
        var nextBuild = buildOrder < build.Length ? build[buildOrder] : '.';

        int buildOre = 0, buildClay = 0, buildObsidian = 0, buildGeode = 0;
        if (nextBuild == 'O' && itemsOre >= blueprint.OrePerOre)
        {
            buildOre++;
            itemsOre -= blueprint.OrePerOre;
            buildOrder++;
            Console.WriteLine($"Spend {blueprint.OrePerOre} ore to start building an ore-collecting robot.");
        }
        else if (nextBuild == 'C' && itemsOre >= blueprint.OrePerClay)
        {
            buildClay++;
            itemsOre -= blueprint.OrePerClay;
            buildOrder++;
            Console.WriteLine($"Spend {blueprint.OrePerClay} ore to start building a clay-collecting robot.");
        }
        else if (nextBuild == 'B' && itemsOre >= blueprint.OrePerObsidian && itemsClay >= blueprint.ClayPerObsidian)
        {
            buildObsidian++;
            itemsOre -= blueprint.OrePerObsidian;
            itemsClay -= blueprint.ClayPerObsidian;
            buildOrder++;
            Console.WriteLine($"Spend {blueprint.OrePerObsidian} ore and {blueprint.ClayPerObsidian} clay to start building an obsidisn-collecting robot.");
        }
        else if (nextBuild == 'G' && itemsOre >= blueprint.OrePerGeode && itemsObsidian >= blueprint.ObsidianPerGeode)
        {
            buildGeode++;
            itemsOre -= blueprint.OrePerGeode;
            itemsObsidian -= blueprint.ObsidianPerGeode;
            buildOrder++;
            Console.WriteLine($"Spend {blueprint.OrePerGeode} ore and {blueprint.ObsidianPerGeode} obsidian to start building a geode-cracking robot.");
        }

        itemsOre += robotsOre;
        itemsClay += robotsClay;
        itemsObsidian += robotsObsidian;
        itemsGeode += robotsGeode;

        if (robotsOre > 0)
            Console.WriteLine($"{robotsOre} ore-collecting robot collects {robotsOre} ore; you now have {itemsOre} ore.");
        if (robotsClay > 0)
            Console.WriteLine($"{robotsClay} clay-collecting robot collects {robotsClay} clay; you now have {itemsClay} clay.");
        if (robotsObsidian > 0)
            Console.WriteLine($"{robotsObsidian} obsidian-collecting robot collects {robotsObsidian} obsidian; you now have {itemsObsidian} obsidian.");
        if (robotsGeode > 0)
            Console.WriteLine($"{robotsGeode} geode-cracking robot cracks {robotsGeode} geodes; you now have {itemsGeode} geodes.");

        robotsOre += buildOre;
        robotsClay += buildClay;
        robotsObsidian += buildObsidian;
        robotsGeode += buildGeode;

        if (buildOre > 0)
            Console.WriteLine($"The new ore-collecting robot is ready; you now have {robotsOre} of them.");
        if (buildClay > 0)
            Console.WriteLine($"The new clay-collecting robot is ready; you now have {robotsClay} of them.");
        if (buildObsidian > 0)
            Console.WriteLine($"The new obsidian-collecting robot is ready; you now have {robotsObsidian} of them.");
        if (buildGeode > 0)
            Console.WriteLine($"The new geode-cracking robot is ready; you now have {robotsGeode} of them.");
        Console.WriteLine();
    }
}

