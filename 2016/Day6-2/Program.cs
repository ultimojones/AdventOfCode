var msg = File.ReadAllLines("input.txt");

var chars = Enumerable.Range(0, msg[0].Length).Select(i =>
{
    var agg = msg.Select(m => m[i]).GroupBy(g => g).Select(g => (g.Key, Count: g.Count())).MinBy(g => g.Count).Key; 
    return agg;
}).ToArray();

Console.WriteLine(new string(chars));