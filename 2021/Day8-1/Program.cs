var displays = File.ReadLines("input.txt").Select(l =>
{
    var a = l.Split(" | ");
    var input = a[0].Split(' ');
    var output = a[1].Split(' ');
    return (Input: input, Output: output);
}).ToArray();

var result = displays.Sum(d => d.Output.Count(o => o.Length is 2 or 3 or 4 or 7));

Console.WriteLine(result);