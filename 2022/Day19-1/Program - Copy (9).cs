using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

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

//foreach (var bp in blueprints)
//{
//    Console.WriteLine($"{(bp.OrePerClay * bp.OrePerObsidian * bp.OrePerGeode / Math.Pow(bp.OrePerOre, 3))}");
//}
//return;

// (2, 2, 3, 3, 8, 3, 12)=OOCCCCCBBBBBGBGG=12

PrintTest(blueprints[25], 1);
return;

void PrintTest((int ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode) blueprint, int oreTest)
{
    int robotsOre = 1, robotsClay = 0, robotsObsidian = 0, robotsGeode = 0;
    int itemsOre = 0, itemsClay = 0, itemsObsidian = 0, itemsGeode = 0;
    string build = "";

    for (int i = 1; i <= 24; i++)
    {
        Console.WriteLine($"== Minute {i} ==");

        int buildOre = 0, buildClay = 0, buildObsidian = 0, buildGeode = 0;
        if (itemsOre >= blueprint.OrePerOre && oreTest > 0)
        {
            buildOre++; oreTest--; build += 'O';
            itemsOre -= blueprint.OrePerOre;
            Console.WriteLine($"Spend {blueprint.OrePerOre} ore to start building an ore-collecting robot.");
        }
        else if (i < 24 && itemsOre >= blueprint.OrePerGeode && itemsObsidian >= blueprint.ObsidianPerGeode)
        {
            buildGeode++; build += 'G';
            itemsOre -= blueprint.OrePerGeode;
            itemsObsidian -= blueprint.ObsidianPerGeode;
            Console.WriteLine($"Spend {blueprint.OrePerGeode} ore and {blueprint.ObsidianPerGeode} obsidian to start building a geode-cracking robot.");
        }
        else if (i < 22 && itemsOre >= blueprint.OrePerObsidian && itemsClay >= blueprint.ClayPerObsidian
            && (robotsObsidian == 0 || MinutesToGeodeAfterObsidian() <= MinutesToGeode()))
        {
            buildObsidian++; build += 'N';
            itemsOre -= blueprint.OrePerObsidian;
            itemsClay -= blueprint.ClayPerObsidian;
            Console.WriteLine($"Spend {blueprint.OrePerObsidian} ore and {blueprint.ClayPerObsidian} clay to start building an obsidisn-collecting robot.");
        }
        else if (i < 20 && itemsOre >= blueprint.OrePerClay
            && (robotsClay == 0 || MinutesToObsidianAfterClay() <= MinutesToObsidian()))
        {
            buildClay++; build += 'C';
            itemsOre -= blueprint.OrePerClay;
            Console.WriteLine($"Spend {blueprint.OrePerClay} ore to start building a clay-collecting robot.");
        }

        int MinutesToGeode()
        {
            int result = itemsObsidian >= blueprint.ObsidianPerGeode && itemsOre >= blueprint.OrePerGeode ? 1
                 : (int)Math.Ceiling(Math.Max(((double)blueprint.ObsidianPerGeode - itemsObsidian) / robotsObsidian,
                            ((double)blueprint.OrePerGeode - itemsOre) / robotsOre));
            return result;
        }

        int MinutesToGeodeAfterObsidian()
        {
            var testOre = itemsOre - blueprint.OrePerObsidian;
            int result = itemsObsidian >= blueprint.ObsidianPerGeode && testOre >= blueprint.OrePerGeode ? 1
                 : (int)Math.Ceiling(Math.Max(((double)blueprint.ObsidianPerGeode - itemsObsidian) / (robotsObsidian + 1),
                            ((double)blueprint.OrePerGeode - testOre) / robotsOre));
            return result;
        }

        int MinutesToGeodeAfterClay()
        {
            var testOre = itemsOre - blueprint.OrePerClay;
            int result = itemsObsidian >= blueprint.ObsidianPerGeode && testOre >= blueprint.OrePerGeode ? 1
                 : (int)Math.Ceiling(Math.Max(((double)blueprint.ObsidianPerGeode - itemsObsidian) / robotsObsidian,
                            ((double)blueprint.OrePerGeode - testOre) / robotsOre));
            return result;
        }

        int MinutesToObsidian()
        {
            int result = itemsClay >= blueprint.ClayPerObsidian && itemsOre >= blueprint.OrePerObsidian ? 1
                       : (int)Math.Ceiling(Math.Max(((double)blueprint.ClayPerObsidian - itemsClay) / robotsClay,
                                  ((double)blueprint.OrePerObsidian - itemsOre) / robotsOre));
            return result;
        }

        int MinutesToObsidianAfterClay()
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

    Console.WriteLine($"{build}={itemsGeode}");

}

int CaclulateGeodes((int ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode) blueprint)
{
    var testQueue = new Queue<(int Ore, int Clay, int Obsidian, int Geode)>();
    var runtests = new HashSet<(int Ore, int Clay, int Obsidian, int Geode)>();
    testQueue.Enqueue((0, 1, 1, 1));

    int maxGeodes = 0;
    string maxBuild = string.Empty;

    while (testQueue.TryDequeue(out var test))
    {
        if (runtests.Contains(test))
            continue;
        runtests.Add(test);
        var builds = GetBuilds("G", test.Ore, test.Clay, test.Obsidian, test.Geode - 1);

        var results = builds.AsParallel().Select(build =>
        {
            int robotsOre = 1, robotsClay = 0, robotsObsidian = 0, robotsGeode = 0;
            int itemsOre = 0, itemsClay = 0, itemsObsidian = 0, itemsGeode = 0;
            int buildOrder = 0;
            for (int i = 0; i < 24; i++)
            {
                var nextBuild = buildOrder < build.Length ? build[buildOrder] : '.';

                int buildOre = 0, buildClay = 0, buildObsidian = 0, buildGeode = 0;
                if (nextBuild == 'O' && itemsOre >= blueprint.OrePerOre)
                {
                    buildOre++;
                    itemsOre -= blueprint.OrePerOre;
                    buildOrder++;
                }
                else if (nextBuild == 'C' && itemsOre >= blueprint.OrePerClay)
                {
                    buildClay++;
                    itemsOre -= blueprint.OrePerClay;
                    buildOrder++;
                }
                else if (nextBuild == 'B' && itemsOre >= blueprint.OrePerObsidian && itemsClay >= blueprint.ClayPerObsidian)
                {
                    buildObsidian++;
                    itemsOre -= blueprint.OrePerObsidian;
                    itemsClay -= blueprint.ClayPerObsidian;
                    buildOrder++;
                }
                else if (nextBuild == 'G' && itemsOre >= blueprint.OrePerGeode && itemsObsidian >= blueprint.ObsidianPerGeode)
                {
                    buildGeode++;
                    itemsOre -= blueprint.OrePerGeode;
                    itemsObsidian -= blueprint.ObsidianPerGeode;
                    buildOrder++;
                }

                itemsOre += robotsOre;
                itemsClay += robotsClay;
                itemsObsidian += robotsObsidian;
                itemsGeode += robotsGeode;

                robotsOre += buildOre;
                robotsClay += buildClay;
                robotsObsidian += buildObsidian;
                robotsGeode += buildGeode;
            }

            return (Build: build, Clay: itemsClay, Obsidian: itemsObsidian, Geodes: itemsGeode, Unbuilt: build.Length - buildOrder);
        });

        var result = results.OrderByDescending(r => r.Geodes).ThenBy(r => r.Unbuilt).FirstOrDefault();

        if (result.Geodes > maxGeodes)
        {
            maxGeodes = result.Geodes;
            maxBuild = result.Build;
        }

        if (result.Build.Length > 12)
        {
            // Has more than 1 robot left unbuilt
            if (result.Unbuilt > 1)
                continue;
            // Build is 2 or more robots than best result
            if (result.Build.Length - maxBuild.Length > 2)
                continue;
            // Has lower output of geodes
            if ((double)result.Geodes / maxGeodes < 0.5)
                continue;
        }

        if (result.Obsidian >= blueprint.ObsidianPerGeode)
            testQueue.Enqueue((test.Ore, test.Clay, test.Obsidian, test.Geode + 1));
        if (result.Clay >= blueprint.ClayPerObsidian)
            testQueue.Enqueue((test.Ore, test.Clay, test.Obsidian + 1, test.Geode));
        testQueue.Enqueue((test.Ore, test.Clay + 1, test.Obsidian, test.Geode));
        testQueue.Enqueue((test.Ore + 1, test.Clay, test.Obsidian, test.Geode));
    }

    Console.WriteLine($"{blueprint}={maxBuild}={maxGeodes}");
    //PrintBuild(blueprint, maxBuild);
    return maxGeodes;
}

IEnumerable<string> GetBuilds(string build, int numOre, int numClay, int numObsidian, int numGeode)
{
    if (numOre == 0 && numClay == 0 && numObsidian == 0 && numGeode == 0)
        yield return build;

    if (numGeode > 0 && numObsidian > 0)
        foreach (var item in GetBuilds("G" + build, numOre, numClay, numObsidian, numGeode - 1))
            yield return item;

    if (numObsidian > 0 && numClay > 0)
        foreach (var item in GetBuilds("B" + build, numOre, numClay, numObsidian - 1, numGeode))
            yield return item;

    if (numClay > 0 && build.Contains('B'))
        foreach (var item in GetBuilds("C" + build, numOre, numClay - 1, numObsidian, numGeode))
            yield return item;

    if (numOre > 0 && numClay == 0 && numObsidian == 0 && numGeode == 0)
        yield return new string('O', numOre) + build;

    yield break;
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

