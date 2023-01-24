var lines = File.ReadAllLines("sample.txt");
var insertions = lines.Skip(2).ToDictionary(x => (x[0], x[1]), x => x[6]);

var polymer = lines[0].ToArray();

for (int i = 0; i < 40; i++)
{
	var output = new char[polymer.LongLength + (polymer.LongLength - 1)];
	output[0] = polymer[0];
	Parallel.For(1L, polymer.LongLength, j =>
	{
		output[j * 2 - 1] = insertions[(polymer[(j - 1)], polymer[j])];
		output[j * 2] = polymer[j];
	});
	polymer = output;
}

var results = polymer.GroupBy(p => p).Select(g => new { Element = g.Key, Number = g.LongCount() }).ToList();

results.ForEach(Console.WriteLine);
Console.WriteLine(results.Max(r => r.Number) - results.Min(r => r.Number));
