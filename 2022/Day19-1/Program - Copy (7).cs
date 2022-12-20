using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

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
    var bestBuild = CaclulateGeodes(blueprint);
    Console.WriteLine(bestBuild);
}

(string, int) CaclulateGeodes((string ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode) blueprint)
{
    var tested = new Dictionary<(int, int, int, int), (string, int)>();
    var testQueue = new Queue<(int Ore, int Clay, int Obsidian, int Geode)>();
    testQueue.Enqueue((0, 1, 1, 1));
    int maxTest = 0, maxTestLen = 0;

    while (testQueue.TryDequeue(out var test))
    {
        if (tested.ContainsKey(test))
            continue;

        var builds = GetBuilds("G", test.Ore, test.Clay, test.Obsidian, test.Geode - 1);
        var results = new Dictionary<string, (int Ore, int Clay, int Obsidian, int Geode, int Unbuilt)>();
        foreach (var build in builds)
        {
            int robotsOre = 1, robotsClay = 0, robotsObsidian = 0, robotsGeode = 0;
            int itemsOre = 0, itemsClay = 0, itemsObsidian = 0, itemsGeode = 0;

            int order = 0;
            for (int i = 0; i < 24; i++)
            {
                var nextBuild = order < build.Length ? build[order] : '.';

                int buildOre = 0, buildClay = 0, buildObsidian = 0, buildGeode = 0;
                if (nextBuild == 'O' && itemsOre >= blueprint.OrePerOre)
                {
                    buildOre++;
                    itemsOre -= blueprint.OrePerOre;
                    order++;
                }
                else if (nextBuild == 'C' && itemsOre >= blueprint.OrePerClay)
                {
                    buildClay++;
                    itemsOre -= blueprint.OrePerClay;
                    order++;
                }
                else if (nextBuild == 'B' && itemsOre >= blueprint.OrePerObsidian && itemsClay >= blueprint.ClayPerObsidian)
                {
                    buildObsidian++;
                    itemsOre -= blueprint.OrePerObsidian;
                    itemsClay -= blueprint.ClayPerObsidian;
                    order++;
                }
                else if (nextBuild == 'G' && itemsOre >= blueprint.OrePerGeode && itemsObsidian >= blueprint.ObsidianPerGeode)
                {
                    buildGeode++;
                    itemsOre -= blueprint.OrePerGeode;
                    itemsObsidian -= blueprint.ObsidianPerGeode;
                    order++;
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

            results[build] = (itemsOre, itemsClay, itemsObsidian, itemsGeode, build.Length - order);
        }
        var max = results.OrderByDescending(r => r.Value.Geode).ThenBy(r => r.Value.Unbuilt).FirstOrDefault();
        tested[test] = (max.Key, max.Value.Geode);
        if (max.Value.Geode > maxTest)
        {
            maxTest = max.Value.Geode;
            maxTestLen = max.Key.Length;
        }

        if (max.Key.Length > 6 && (max.Key.Length - maxTestLen > 2 || max.Value.Unbuilt > 0))
            continue;
        //if (max.Key.Length > 14 && max.Key.Length > maxTestLen)
        //    continue;
        if (max.Key.Length > 13) 
            if ((max.Key.Length <= maxTestLen && max.Value.Geode < maxTest) || (max.Key.Length > maxTestLen && max.Value.Geode <= maxTest))
                continue;
        //if (max.Value.Geode == 0 && maxTest > 0)
        //    continue;
        //if (maxTest == 0 && max.Key.Length > 6)
        //    continue;
        if (max.Value.Obsidian > blueprint.ObsidianPerGeode)
            testQueue.Enqueue((test.Ore, test.Clay, test.Obsidian, test.Geode + 1));
        if (max.Value.Clay > blueprint.ClayPerObsidian)
            testQueue.Enqueue((test.Ore, test.Clay, test.Obsidian + 1, test.Geode));
        testQueue.Enqueue((test.Ore, test.Clay + 1, test.Obsidian, test.Geode));
        testQueue.Enqueue((test.Ore + 1, test.Clay, test.Obsidian, test.Geode));
    }

    return tested.MaxBy(t => t.Value.Item2).Value;
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

    if (numOre > 0)
        foreach (var item in GetBuilds("O" + build, numOre - 1, numClay, numObsidian, numGeode))
            yield return item;

    yield break;
}
