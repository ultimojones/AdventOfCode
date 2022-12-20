using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Xml.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        var blueprints = File.ReadLines("input.txt").Select(line =>
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

        //Parallel.ForEach(blueprints, blueprint =>
        foreach (var blueprint in blueprints[1..2])
        {
            var allcombos = new Dictionary<string, int>();

            for (int numOre = 0; numOre <= 4; numOre++)
                for (int numClay = 4; numClay <= 12; numClay++)
                    for (int numObsidian = 1; numObsidian <= 4; numObsidian++)
                        for (int numGeode = 1; numGeode <= 2; numGeode++)
                        {
                            var values = GetBuilds("G", numOre, numClay, numObsidian, numGeode - 1)
                                .AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                                .Select(b => new { b, value = CaclulateGeodes(blueprint, b) })
                                .Where(r => r.value.Item2 == 0);
                            var maxValue = values.MaxBy(x => x.value);
                            if (maxValue != null)
                            {
                                allcombos[maxValue.b] = maxValue.value.Item1;
                            }
                        }

            Console.WriteLine($"{blueprint} = {allcombos.MaxBy(c => c.Value)}");
        }
    }

    private static IEnumerable<string> GetBuilds(string build, int numOre, int numClay, int numObsidian, int numGeode)
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

    private static (int, int) CaclulateGeodes((string ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode) blueprint, string buildkey)
    {
        int robotsOre = 1, robotsClay = 0, robotsObsidian = 0, robotsGeode = 0;
        int itemsOre = 0, itemsClay = 0, itemsObsidian = 0, itemsGeode = 0;
        var nextBuild = 0;
        int geodeTurns = 0;

        for (int i = 0; i < 24; i++)
        {
            int buildOre = 0, buildClay = 0, buildObsidian = 0, buildGeode = 0;
            if (nextBuild < buildkey.Length)
                switch (buildkey[nextBuild])
                {
                    case 'O' when itemsOre >= blueprint.OrePerOre:
                        buildOre++;
                        itemsOre -= blueprint.OrePerOre;
                        nextBuild++;
                        break;
                    case 'C' when itemsOre >= blueprint.OrePerClay:
                        buildClay++;
                        itemsOre -= blueprint.OrePerClay;
                        nextBuild++;
                        break;
                    case 'B' when itemsOre >= blueprint.OrePerObsidian && itemsClay >= blueprint.ClayPerObsidian:
                        buildObsidian++;
                        itemsOre -= blueprint.OrePerObsidian;
                        itemsClay -= blueprint.ClayPerObsidian;
                        nextBuild++;
                        break;
                    case 'G' when itemsOre >= blueprint.OrePerGeode && itemsObsidian >= blueprint.ObsidianPerGeode:
                        buildGeode++;
                        itemsOre -= blueprint.OrePerGeode;
                        itemsObsidian -= blueprint.ObsidianPerGeode;
                        geodeTurns += 24 - i;
                        nextBuild++;
                        break;
                    default:
                        break;
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

        return (itemsGeode, buildkey.Length - nextBuild);
    }
}
