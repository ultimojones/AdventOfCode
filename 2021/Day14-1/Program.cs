var lines = File.ReadAllLines("input.txt");
var insertions = lines.Skip(2).ToDictionary(x => x[0..2], x => x[6]);
var polymer = new LinkedList<char>(lines[0]);

Console.WriteLine(polymer.ToArray());

for (int i = 0; i < 10; i++)
{
	var node = polymer.First;
	while (node?.Next is not null)
	{
		node = node.Next;
		if (insertions.TryGetValue(string.Concat(node.Previous.Value, node.Value), out var value))
			polymer.AddBefore(node, value);
	}
}

var results = polymer.GroupBy(p => p).Select(g => new { Element = g.Key, Number = g.Count() }).ToList();

results.ForEach(Console.WriteLine);
Console.WriteLine(polymer.Count);
Console.WriteLine(results.Max(r => r.Number) - results.Min(r => r.Number));
