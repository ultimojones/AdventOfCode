var sues = File.ReadLines("input.txt").Select(l =>
{
    var name = l.Remove(l.IndexOf(':'));
    var items = l[(name.Length + 2)..].Split(',').ToDictionary(i => i.Remove(i.IndexOf(':')).Trim(), i => int.Parse(i[(i.IndexOf(":") + 2)..]));
    return (Name: name, Items: items);
});

foreach (var sue in sues.Where(s => 
    (!s.Items.ContainsKey("children") || s.Items["children"] == 3)
    && (!s.Items.ContainsKey("cats") || s.Items["cats"] > 7)
    && (!s.Items.ContainsKey("samoyeds") || s.Items["samoyeds"] == 2)
    && (!s.Items.ContainsKey("pomeranians") || s.Items["pomeranians"] < 3)
    && (!s.Items.ContainsKey("akitas") || s.Items["akitas"] == 0)
    && (!s.Items.ContainsKey("vizslas") || s.Items["vizslas"] == 0)
    && (!s.Items.ContainsKey("goldfish") || s.Items["goldfish"] < 5)
    && (!s.Items.ContainsKey("trees") || s.Items["trees"] > 3)
    && (!s.Items.ContainsKey("cars") || s.Items["cars"] == 2)
    && (!s.Items.ContainsKey("perfumes") || s.Items["perfumes"] == 1)
))
{
    Console.WriteLine($"{sue.Name} {string.Join(",", sue.Items.Select(i => $"{i.Key}={i.Value}"))}");
}