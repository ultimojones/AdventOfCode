using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Xml.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
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
            var allcombos = new Dictionary<string, int>();

            for (int numOre = 0; numOre <= 2; numOre++)
                for (int numClay = 2; numClay <= 6; numClay++)
                    for (int numObsidian = 2; numObsidian <= 6; numObsidian++)
                        for (int numGeode = 2; numGeode <= 4; numGeode++)
                        {
                            var item = GetBuilds("G", numOre, numClay, numObsidian, numGeode - 1)
                                .AsParallel()
                                 .Select(b => new { b, value = CaclulateGeodes(blueprint, b) })
                                 .Where(r => r.value.Item2 == 0).MaxBy(r => r.value.Item1);
                            if (item != null)
                            {
                                allcombos[item.b] = item.value.Item1;
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
        var robots = (Ore: 1, Clay: 0, Obsidian: 0, Geode: 0);
        var items = (Ore: 0, Clay: 0, Obsidian: 0, Geode: 0);
        var nextBuild = 0;
        int geodeTurns = 0;

        for (int i = 0; i < 24; i++)
        {
            var building = (Ore: 0, Clay: 0, Obsidian: 0, Geode: 0);
            if (nextBuild < buildkey.Length)
                switch (buildkey[nextBuild])
                {
                    case 'O' when items.Ore >= blueprint.OrePerOre:
                        building.Ore++;
                        items.Ore -= blueprint.OrePerOre;
                        nextBuild++;
                        break;
                    case 'C' when items.Ore >= blueprint.OrePerClay:
                        building.Clay++;
                        items.Ore -= blueprint.OrePerClay;
                        nextBuild++;
                        break;
                    case 'B' when items.Ore >= blueprint.OrePerObsidian && items.Clay >= blueprint.ClayPerObsidian:
                        building.Obsidian++;
                        items.Ore -= blueprint.OrePerObsidian;
                        items.Clay -= blueprint.ClayPerObsidian;
                        nextBuild++;
                        break;
                    case 'G' when items.Ore >= blueprint.OrePerGeode && items.Obsidian >= blueprint.ObsidianPerGeode:
                        building.Geode++;
                        items.Ore -= blueprint.OrePerGeode;
                        items.Obsidian -= blueprint.ObsidianPerGeode;
                        geodeTurns += 24 - i;
                        nextBuild++;
                        break;
                    default:
                        break;
                }

            items.Ore += robots.Ore;
            items.Clay += robots.Clay;
            items.Obsidian += robots.Obsidian;
            items.Geode += robots.Geode;

            robots.Ore += building.Ore;
            robots.Clay += building.Clay;
            robots.Obsidian += building.Obsidian;
            robots.Geode += building.Geode;
        }

        return (items.Geode, buildkey.Length - nextBuild);
    }
}
