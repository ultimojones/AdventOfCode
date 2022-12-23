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
    return (ID, OrePerOre, OrePerClay, OrePerObsidian, ClayPerObsidian, OrePerGeode, ObsidianPerGeode);
}).ToArray();

var results = new Dictionary<int, int>();

foreach (var blueprint in blueprints[1..2])
{
    var geodes = CaclulateGeodes(blueprint);
    results[blueprint.ID] = geodes;
}

var final = results.Sum(r => r.Key * r.Value);
Console.WriteLine(final);

int CaclulateGeodes((int ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode) blueprint)
{
    var results = new Dictionary<int, (string Build, int Geodes)>();

    //for (int o = 0; o < 10; o++)
    //{
    int robotsOre = 1, robotsClay = 0, robotsObsidian = 0, robotsGeode = 0;
    int itemsOre = 0, itemsClay = 0, itemsObsidian = 0, itemsGeode = 0;
    //int oreTest = o;
    string build = "";

    for (int i = 1; i <= 24; i++)
    {
        int buildOre = 0, buildClay = 0, buildObsidian = 0, buildGeode = 0;
        //if (itemsOre >= blueprint.OrePerOre && oreTest > 0)
        //{
        //    buildOre++; oreTest--; build += 'O';
        //    itemsOre -= blueprint.OrePerOre;
        //}
        //else 
        if (i < 24 && itemsOre >= blueprint.OrePerGeode && itemsObsidian >= blueprint.ObsidianPerGeode)
        {
            buildGeode++; build += 'G';
            itemsOre -= blueprint.OrePerGeode;
            itemsObsidian -= blueprint.ObsidianPerGeode;
        }
        else if (i < 22 && itemsOre >= blueprint.OrePerObsidian && itemsClay >= blueprint.ClayPerObsidian
            && (robotsObsidian == 0 || MinutesToGeodeAfterObsidian_() <= MinutesToGeode_()))
        {
            buildObsidian++; build += 'N';
            itemsOre -= blueprint.OrePerObsidian;
            itemsClay -= blueprint.ClayPerObsidian;
        }
        else if (i < 22 && itemsOre >= blueprint.OrePerOre && ChangeToClayBuildWithOre() > 0)
        {
            buildOre++; build += 'O';
            itemsOre -= blueprint.OrePerOre;
        }
        else if (i < 20 && itemsOre >= blueprint.OrePerClay
            && (robotsClay == 0 || MinutesToObsidianAfterClay_() <= MinutesToObsidian_() && MinutesToGeodeAfterClay_() <= MinutesToGeode_()))
        {
            buildClay++; build += 'C';
            itemsOre -= blueprint.OrePerClay;
        }

        int ChangeToClayBuildWithOre()
        {
            var totalClay = robotsClay * (22 - i);
            var totalOre = robotsOre * (22 - i);
            var maxCanBuild = totalOre / blueprint.OrePerClay;
            var clayBuilt = ((22 - i) / maxCanBuild) * ((maxCanBuild * (maxCanBuild + 1)) / 2);
            var clayBuilt2 = (22 - i)  * ((maxCanBuild + 1) / 2);

            if (itemsOre >= blueprint.OrePerClay) return 0;
            int minutesNow = (int)Math.Ceiling((double)(blueprint.OrePerClay - itemsOre) / robotsOre);
            int minutesWithOre = (int)Math.Ceiling((double)(blueprint.OrePerClay + blueprint.OrePerOre - itemsOre) / (robotsOre + 1));
            return i + minutesWithOre < 22 ? minutesNow - minutesWithOre : 0;
        }

        int MinutesToGeode_()
        {
            int result = itemsObsidian >= blueprint.ObsidianPerGeode && itemsOre >= blueprint.OrePerGeode ? 1
                 : (int)Math.Ceiling(Math.Max(((double)blueprint.ObsidianPerGeode - itemsObsidian) / robotsObsidian,
                            ((double)blueprint.OrePerGeode - itemsOre) / robotsOre));
            return result;
        }

        int MinutesToGeodeAfterObsidian_()
        {
            var testOre = itemsOre - blueprint.OrePerObsidian;
            int result = itemsObsidian >= blueprint.ObsidianPerGeode && testOre >= blueprint.OrePerGeode ? 1
                 : (int)Math.Ceiling(Math.Max(((double)blueprint.ObsidianPerGeode - itemsObsidian) / (robotsObsidian + 1),
                            ((double)blueprint.OrePerGeode - testOre) / robotsOre));
            return result;
        }

        int MinutesToGeodeAfterClay_()
        {
            var testOre = itemsOre - blueprint.OrePerClay;
            int result = itemsObsidian >= blueprint.ObsidianPerGeode && testOre >= blueprint.OrePerGeode ? 1
                 : (int)Math.Ceiling(Math.Max(((double)blueprint.ObsidianPerGeode - itemsObsidian) / robotsObsidian,
                            ((double)blueprint.OrePerGeode - testOre) / robotsOre));
            return result;
        }

        int MinutesToObsidian_()
        {
            int result = itemsClay >= blueprint.ClayPerObsidian && itemsOre >= blueprint.OrePerObsidian ? 1
                       : (int)Math.Ceiling(Math.Max(((double)blueprint.ClayPerObsidian - itemsClay) / robotsClay,
                                  ((double)blueprint.OrePerObsidian - itemsOre) / robotsOre));
            return result;
        }

        int MinutesToObsidianAfterClay_()
        {
            var testOre = itemsOre - blueprint.OrePerClay;
            int result = itemsClay >= blueprint.ClayPerObsidian && testOre >= blueprint.OrePerObsidian ? 1
                       : (int)Math.Ceiling(Math.Max(((double)blueprint.ClayPerObsidian - itemsClay) / (robotsClay + 1),
                                  ((double)blueprint.OrePerObsidian - testOre) / robotsOre));
            return result;
        }

        itemsOre += robotsOre;
        itemsClay += robotsClay;
        itemsObsidian += robotsObsidian;
        itemsGeode += robotsGeode;

        robotsOre += buildOre;
        robotsClay += buildClay;
        robotsObsidian += buildObsidian;
        robotsGeode += buildGeode;

        //results[o] = (build, itemsGeode);
    }
    //}

    //var best = results.OrderByDescending(r => r.Value.Geodes).ThenBy(r => r.Key).FirstOrDefault().Value;
    Console.WriteLine($"{build}={itemsGeode}");
    return itemsGeode;
}



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

