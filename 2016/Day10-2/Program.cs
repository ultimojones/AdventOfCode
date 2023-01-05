using System.Text.RegularExpressions;

var vals = new Dictionary<int, Val>();
var bots = new Dictionary<int, Bot>();
var outs = new Dictionary<int, List<int>>();

foreach (var line in File.ReadLines("input.txt"))
{
    if (line.StartsWith("value"))
    {
        var r = Regex.Match(line, @"^value (?<Value>\d+) goes to bot (?<Bot>\d+)$");
        var bot = int.Parse(r.Groups["Bot"].Value);
        var val = int.Parse(r.Groups["Value"].Value);
        if (!vals.ContainsKey(val)) vals.Add(val, new Val());
        vals[val].Spec = line;
        vals[val].Value = val;
        vals[val].ToBot = bot;
        if (!bots.ContainsKey(bot)) bots.Add(bot, new Bot());
        bots[bot].BotID = bot;
        bots[bot].Values.Add(val);
    }
    else if (line.StartsWith("bot"))
    {
        var r = Regex.Match(line, @"^bot (?<Bot>\d+) gives low to (?<LowType>bot|output) (?<Low>.+) and high to (?<HighType>bot|output)(?<High>.+)$");
        var bot = int.Parse(r.Groups["Bot"].Value);
        var lowType = r.Groups["LowType"].Value;
        var low = int.Parse(r.Groups["Low"].Value);
        var hghType = r.Groups["HighType"].Value;
        var hgh = int.Parse(r.Groups["High"].Value);
        if (!bots.ContainsKey(bot)) bots.Add(bot, new Bot());
        bots[bot].Spec = line;
        bots[bot].BotID = bot;
        if (lowType == "bot")
        {
            bots[bot].ToLowBot = low;
            if (!bots.ContainsKey(low)) bots.Add(low, new Bot());
        }
        else
        {
            bots[bot].ToLowOut = low;
        }
        if (hghType == "bot")
        {
            bots[bot].ToHighBot = hgh;
            if (!bots.ContainsKey(hgh)) bots.Add(hgh, new Bot());
        }
        else
        {
            bots[hgh].ToHighOut = hgh;
        }
    }
}

while (bots.Any(b => b.Value.Values.Count < 2))
{
    foreach (var bot in bots.Where(b => b.Value.Values.Count == 2))
    {
        if (bot.Value.ToLowBot >= 0 && !bots[bot.Value.ToLowBot].Values.Contains(bot.Value.Values.Min()))
            bots[bot.Value.ToLowBot].Values.Add(bot.Value.Values.Min());
        if (bot.Value.ToHighBot >= 0 && !bots[bot.Value.ToHighBot].Values.Contains(bot.Value.Values.Max()))
            bots[bot.Value.ToHighBot].Values.Add(bot.Value.Values.Max());
    }
}

foreach (var bot in bots.OrderBy(b => b.Key))
{
    Console.WriteLine($"{new { ID = bot.Value.BotID, Vals = string.Join(',', bot.Value.Values) }}");
}

Console.WriteLine();
foreach (var bot in bots.Where(b => b.Value.Values.Order().SequenceEqual(new[] { 17, 61 })))
{
    Console.WriteLine($"{new { ID = bot.Value.BotID, Vals = string.Join(',', bot.Value.Values) }}");
}

var chips = bots.Where(b => b.Value.ToLowOut == 0 || b.Value.ToLowOut == 1 || b.Value.ToLowOut == 2)
                .Select(b => b.Value.Values.Min())
            .Concat(bots.Where(b => b.Value.ToHighOut == 0 || b.Value.ToHighOut == 1 || b.Value.ToHighOut == 2)
                .Select(b => b.Value.Values.Max())).ToArray();

long total = chips[0];
for (int i = 1; i < chips.Length; i++)
{
    total *= chips[i];
}

Console.WriteLine($"{string.Join(',', chips)} = {total}");

class Node
{
}

class Val : Node
{
    public string Spec { get; set; }
    public int Value { get; set; } = -1;
    public int ToBot { get; set; } = -1;
}

class Bot : Node
{
    public string Spec { get; set; }
    public int BotID { get; set; }
    public int ToLowBot { get; set; } = -1;
    public int ToHighBot { get; set; } = -1;
    public int ToLowOut { get; set; } = -1;
    public int ToHighOut { get; set; } = -1;
    public List<int> Values { get; } = new List<int>();
}
