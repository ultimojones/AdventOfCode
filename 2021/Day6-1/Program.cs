//var input = "3,4,3,1,2";
var input = File.ReadAllText("input.txt");

var fish = input.Split(',').Select(int.Parse).ToList();

Console.WriteLine(string.Join(',', fish));

for (int d = 1; d <= 80; d++)
{
	var spawning = fish.Count(f => f == 0);
	for (int f = 0; f < fish.Count; f++)
	{
		fish[f] = fish[f] == 0 ? 6 : fish[f] - 1;
	}
	fish.AddRange(Enumerable.Repeat(8, spawning));
    //Console.WriteLine($"Day {d}: {string.Join(',', fish)}");
    Console.WriteLine($"Day {d}: {fish.Count}");
}