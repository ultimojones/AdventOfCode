using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Xml.Linq;
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
    var testQueue = new ConcurrentQueue<(int Ore, int Clay, int Obsidian, int Geode)>();
    var runtests = new ConcurrentBag<(int Ore, int Clay, int Obsidian, int Geode)>();
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

        if (result.Build.Length > 14)
        {
            //// Has more than 1 robot left unbuilt
            if (result.Unbuilt > 1)
                continue;
            // Build is 2 more robots than best result
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
