using System.Text.RegularExpressions;

var grid = Enumerable.Range(0, 1000).SelectMany(x => Enumerable.Range(0, 1000).Select(y => (X: x, Y: y))).ToDictionary(p => p, _ => 0);

foreach (var line in File.ReadLines("input.txt"))
{
    var match = Regex.Match(line, @"^(?<Action>turn on|turn off|toggle) (?<FromX>\d+),(?<FromY>\d+) through (?<ToX>\d+),(?<ToY>\d+)");
    var action = match.Groups["Action"].Value;
    var fromX = int.Parse(match.Groups["FromX"].Value);
    var fromY = int.Parse(match.Groups["FromY"].Value);
    var toX = int.Parse(match.Groups["ToX"].Value);
    var toY = int.Parse(match.Groups["ToY"].Value);

    for (int x = fromX; x <= toX; x++)
        for (int y = fromY; y <= toY; y++)
        {
            grid[(x, y)] += action switch
            {
                "turn on" => 1,
                "turn off" => grid[(x, y)] > 0 ? -1 : 0,
                "toggle" => 2,
                _ => 0
            };
        }
}

Console.WriteLine(grid.Sum(g => g.Value));