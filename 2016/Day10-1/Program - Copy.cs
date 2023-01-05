using System.Text.RegularExpressions;

var bots = new Dictionary<int, (string Spec, HashSet<int> Vals)>();

foreach (var line in File.ReadLines("sample.txt"))
{
    if (line.StartsWith("value"))
    {
        var r = Regex.Match(line, @"^value (?<Value>\d+) goes to bot (?<Bot>\d+)$");
        var bot = int.Parse(r.Groups["Bot"].Value);
        var val = int.Parse(r.Groups["Value"].Value);
        if (bots.TryGetValue(bot, out var value))
            value.Vals.Add(val);
        else
            bots[bot] = (line, new HashSet<int> { val });
    }
}

foreach (var item in bots.OrderBy(b => b.Key))
{
    Console.WriteLine($"{item.Key,-3} [{string.Join(",", item.Value)}]");
}