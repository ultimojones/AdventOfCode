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
    return new Blueprint (ID, OrePerOre, OrePerClay, OrePerObsidian, ClayPerObsidian, OrePerGeode, ObsidianPerGeode, line);
}).ToArray();

var results = new Dictionary<int, int>();
int bestResult = 0;
var bestBuild = "";
Blueprint blueprint;
var maxOre = 0;

foreach (var bp in blueprints)
{
    blueprint = bp;
    maxOre = new[] { blueprint.OrePerOre, blueprint.OrePerClay, blueprint.OrePerObsidian, blueprint.OrePerGeode }.Max();
    bestResult = 0;
    bestBuild = "";

    ProcessMinute(1, "", 1);
    Console.WriteLine($"{blueprint.ID,2}: {bestBuild}={bestResult,2}");

    results[blueprint.ID] = bestResult;
}
var final = results.Sum(r => r.Key * r.Value);
Console.WriteLine(final);

void ProcessMinute(int minute, string build, int robotsOre = 0, int robotsClay = 0, int robotsObsidian = 0, int robotsGeode = 0, int itemsOre = 0, int itemsClay = 0, int itemsObsidian = 0, int itemsGeode = 0)
{
    if (minute == 24)
    {
        var finalGeodes = itemsGeode + robotsGeode;
        if (finalGeodes > bestResult)
        {
            bestResult = finalGeodes;
            bestBuild = build;
        }
        return;
    }
    var totalPossible = itemsGeode + (24 - minute + 1) * robotsGeode + (24 - minute) * (24 - minute + 1) / 2;
    if (totalPossible < bestResult)
        return;

    if (minute < 24 && itemsOre >= blueprint.OrePerGeode && itemsObsidian >= blueprint.ObsidianPerGeode)
        RunOption("G");
    else
    {
        if (minute < 22 && itemsOre >= blueprint.OrePerObsidian && itemsClay >= blueprint.ClayPerObsidian)
            RunOption("B");
        if (minute < 20 && itemsOre >= blueprint.OrePerClay)
            RunOption("C");
        RunOption("");
        if (minute < 22 && itemsOre >= blueprint.OrePerOre && robotsOre < maxOre)
            RunOption("O");
    }

    void RunOption(string option)
    {
        ProcessMinute(
            minute + 1,
            build + option,
            robotsOre + (option == "O" ? 1 : 0), 
            robotsClay + (option == "C" ? 1 : 0), 
            robotsObsidian + (option == "B" ? 1 : 0), 
            robotsGeode + (option == "G" ? 1 : 0),
            itemsOre + robotsOre - (option switch { "O" => blueprint.OrePerOre, "C" => blueprint.OrePerClay, "B" => blueprint.OrePerObsidian, "G" => blueprint.OrePerGeode, _ => 0 }), 
            itemsClay + robotsClay - (option == "B" ? blueprint.ClayPerObsidian : 0), 
            itemsObsidian + robotsObsidian - (option == "G" ? blueprint.ObsidianPerGeode : 0), 
            itemsGeode + robotsGeode
        );
    }
}

record struct Blueprint (int ID, int OrePerOre, int OrePerClay, int OrePerObsidian, int ClayPerObsidian, int OrePerGeode, int ObsidianPerGeode, string Line);
