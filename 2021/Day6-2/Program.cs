//var input = "3,4,3,1,2";
var input = File.ReadAllText("input.txt");
var fish = input.Split(',').Select(int.Parse).ToList();
var batches = Enumerable.Range(0, 9).Select(i => fish.LongCount(f => f == i)).ToArray();

Console.WriteLine(string.Join(',', batches));

for (int d = 1; d <= 256; d++)
{
	var spawning = batches[0];
	Array.ConstrainedCopy(batches, 1, batches, 0, 8);
	batches[8] = spawning;
	batches[6] += spawning;
    Console.WriteLine($"Day {d}: {batches.Sum()}");
}