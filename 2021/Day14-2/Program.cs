using System.IO.Compression;
using System.Text;

var lines = File.ReadAllLines("input.txt");
var insertions = lines.Skip(2).ToDictionary(x => (x[0..2]), x => x[6]);

var polymer = Enumerable.Range(0, lines[0].Length - 1).Select(i => lines[0][i..(i + 2)]).GroupBy(p => p).ToDictionary(x => x.Key, p => p.LongCount());

for (int i = 0; i < 40; i++)
{
    polymer = polymer.SelectMany(p =>
    {
        if (insertions.TryGetValue(p.Key, out var ch))
        {
            return new KeyValuePair<string, long>[] { new(string.Concat(p.Key[0], ch), p.Value), new(string.Concat(ch, p.Key[1]), p.Value) };
        }
        else
        {
            return new[] { p };
        }
    }).GroupBy(i => i.Key).ToDictionary(g => g.Key, g => g.Sum(v => v.Value));
    Console.WriteLine(polymer.Sum(p => p.Value));
}

polymer[$" {lines[0][0]}"] = 1;
var counts = polymer.GroupBy(p => p.Key[1]).Select(g => new { Element = g.Key, Number = g.Sum(v => v.Value) }).OrderBy(g => g.Element).ToList();
counts.ForEach(Console.WriteLine);
Console.WriteLine(counts.Max(c => c.Number) - counts.Min(c => c.Number));

// var length = 20L; for (int i = 0; i < 40; i++) { length = length * 2 - 1; } Console.WriteLine(length);
// 20,890,720,927,745  return;
