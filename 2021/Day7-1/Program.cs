//var input = "16,1,2,0,4,2,7,1,2,14";
var input = File.ReadAllText("input.txt");

var crabs = input.Split(',').Select(int.Parse).ToArray();

Console.WriteLine(crabs.Average());

var vals = Enumerable.Range(0, crabs.Max()).Select(p => (p, crabs.Sum(c => int.Abs(c - p)))).ToList();
vals.ForEach(x => Console.WriteLine(x));

var best = Enumerable.Range(0, crabs.Max()).Min(p => crabs.Sum(c => int.Abs(c - p)));
Console.WriteLine(best);

// 341 too low