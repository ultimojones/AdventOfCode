//var input = "16,1,2,0,4,2,7,1,2,14";
var input = File.ReadAllText("input.txt");

var crabs = input.Split(',').Select(int.Parse).ToArray();

var best = Enumerable.Range(0, crabs.Max()).Min(p => crabs.Sum(c => { var n = int.Abs(c - p); return n * (n + 1) / 2; }));
Console.WriteLine(best);
