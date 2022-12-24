using System.Collections.Concurrent;
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
    return new Blueprint(ID, OrePerOre, OrePerClay, OrePerObsidian, ClayPerObsidian, OrePerGeode, ObsidianPerGeode, line);
}).ToArray();

const int totalMinutes = 32;
var results = new Dictionary<int, int>();

var bestResult = 0;
string bestBuild = null!;
Blueprint blueprint = default!;
var queue = new ConcurrentQueue<(int Minute, string Build, TypeCounts Robots, TypeCounts Items)>();

foreach (var bp in blueprints[0..3])
{
    bestResult = 0;
    bestBuild = default!;
    queue.Clear();
    blueprint = bp;
    bool finished = false;
    int active = 0;

    queue.Enqueue((1, string.Empty, new(1, 0, 0, 0), default));
    var tasks = Enumerable.Range(0, 20).Select(i => Task.Run(async () =>
    {
        while (!finished)
        {
            Interlocked.Increment(ref active);
            if (queue.TryDequeue(out var input))
            {
                Caclulation(input);
            }
            Interlocked.Decrement(ref active);
            await Task.Yield();
            if (queue.Count == 0 && active == 0)
                finished = true;
        }
    }));

    await Task.WhenAll(tasks.ToArray());
    Console.WriteLine($"{bp.ID}:{bestBuild}={bestResult}");

    results[bp.ID] = bestResult;
}

void Caclulation((int Minute, string Build, TypeCounts Robots, TypeCounts Items) input)
{
    if (input.Minute == totalMinutes)
    {
        var totalGeodes = input.Items.Geode + input.Robots.Geode;
        while (bestResult < totalGeodes)
            Interlocked.CompareExchange(ref bestResult, totalGeodes, bestResult);
        if (bestResult == totalGeodes)
            bestBuild = input.Build;
        return;
    }
    var totalPossible = input.Items.Geode + (totalMinutes - input.Minute + 1) * input.Robots.Geode + (totalMinutes - input.Minute) * (totalMinutes - input.Minute + 1) / 2;
    if (totalPossible < bestResult)
    {
        return;
    }
    var options = new List<string>();
    if (input.Minute < totalMinutes && input.Items.Ore >= blueprint.OrePerGeode && input.Items.Obsidian >= blueprint.ObsidianPerGeode)
        options.Add("G");
    else
    {
        if (input.Minute < totalMinutes - 2 && input.Items.Ore >= blueprint.OrePerObsidian && input.Items.Clay >= blueprint.ClayPerObsidian)
            options.Add("B");

        if (input.Minute < totalMinutes - 4 && input.Items.Ore >= blueprint.OrePerClay)
            options.Add("C");

        if (input.Minute < totalMinutes - 2 && input.Items.Ore >= blueprint.OrePerOre)
        {
            var maxOre = new[] { blueprint.OrePerClay, blueprint.OrePerObsidian, blueprint.OrePerGeode }.Max();
            if (input.Robots.Ore * (totalMinutes - input.Minute) + input.Items.Ore < maxOre * (totalMinutes - input.Minute))
                options.Add("O");
        }
        if (input.Items.Ore < new[] { blueprint.OrePerOre, blueprint.OrePerClay, blueprint.OrePerObsidian, blueprint.OrePerGeode }.Max()
            || (input.Robots.Clay > 0 && input.Items.Clay < blueprint.ClayPerObsidian)
            || (input.Robots.Obsidian > 0 && input.Items.Obsidian < blueprint.ObsidianPerGeode)
            )
            options.Add("");
    }

    if (options.Count == 0)
        options.Add("");

    input.Items.Ore += input.Robots.Ore;
    input.Items.Clay += input.Robots.Clay;
    input.Items.Obsidian += input.Robots.Obsidian;
    input.Items.Geode += input.Robots.Geode;
    input.Minute++;

    foreach (var o in options)
    {
        var optRobots = new TypeCounts
        {
            Ore = input.Robots.Ore + (o == "O" ? 1 : 0),
            Clay = input.Robots.Clay + (o == "C" ? 1 : 0),
            Obsidian = input.Robots.Obsidian + (o == "B" ? 1 : 0),
            Geode = input.Robots.Geode + (o == "G" ? 1 : 0)
        };
        var optItems = new TypeCounts
        {
            Ore = input.Items.Ore - (o switch { "O" => blueprint.OrePerOre, "C" => blueprint.OrePerClay, "B" => blueprint.OrePerObsidian, "G" => blueprint.OrePerGeode, _ => 0 }),
            Clay = input.Items.Clay - (o == "B" ? blueprint.ClayPerObsidian : 0),
            Obsidian = input.Items.Obsidian - (o == "G" ? blueprint.ObsidianPerGeode : 0),
            Geode = input.Items.Geode
        };
        queue.Enqueue((input.Minute, input.Build + o, optRobots, optItems));
    }
}

record struct Blueprint(int ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode, string Line);
record struct TypeCounts(int Ore, int Clay, int Obsidian, int Geode);
