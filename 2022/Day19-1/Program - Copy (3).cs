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

        var builds = new Dictionary<string, int>();

        foreach (var blueprint in blueprints)
        {
            //var buildkey = "CBG";
            //var result = CaclulateGeodes(blueprint, buildkey);
            //Console.WriteLine(result);
            //Console.WriteLine(CaclulateGeodes(blueprint, "CBBG"));
            //Console.WriteLine(CaclulateGeodes(blueprint, "CCBG"));
            //Console.WriteLine(CaclulateGeodes(blueprint, "CCBCBG"));

            //foreach (var key in CalculateKeys(0, 3, 2, 2))
            //{
            //    Console.WriteLine(key);
            //}

            foreach (var item in GetList("G", 0, 2, 2, 2))
            {
                Console.WriteLine(item);
            }

            break;
        }
    }

    private static string[] CalculateKeys(int maxOre, int maxClay, int maxObsideon, int maxGeode)
    {
        // G must always be last
        // C must always be before B
        var len = maxOre + maxClay + maxObsideon + maxGeode;
        var reversed = "G";

        var list = GetList(reversed, maxOre, maxClay, maxObsideon, maxGeode--);

        for (int i = 0; i < maxOre + maxClay + maxObsideon + maxGeode; i++)
        {
            
        }


        return new[] { "" };
    }

    private static IEnumerable<string> GetList(string reversed, int maxOre, int maxClay, int maxObsideon, int maxGeode)
    {
        if (maxOre <= 0 && maxClay <= 0 && maxObsideon <= 0 && maxGeode <= 0)
            yield return reversed;

        if (maxGeode > 0)
        {
            foreach (var item in GetList(reversed + 'G', maxOre, maxClay, maxObsideon, --maxGeode))
            {
                yield return item;
            }
        }

        if (maxObsideon > 0)
        {
            foreach (var item in GetList(reversed + 'B', maxOre, maxClay, --maxObsideon, maxGeode))
            {
                yield return item;
            }
        }

        if (maxClay > 0 && reversed.Contains('B'))
        {
            foreach (var item in GetList(reversed + 'C', maxOre, --maxClay, maxObsideon, maxGeode))
            {
                yield return item;
            }
        }

        if (maxOre > 0)
        {
            foreach (var item in GetList(reversed + 'O', --maxOre, maxClay, maxObsideon, maxGeode))
            {
                yield return item;
            }
        }
    }

    private static (int, int, int, int) CaclulateGeodes((string ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode) blueprint, string buildkey)
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

        return (items.Ore, items.Clay, items.Obsidian, items.Geode);
    }
}

enum Robot
{
    Ore,
    Clay,
    Obsidian,
    Geode
}
