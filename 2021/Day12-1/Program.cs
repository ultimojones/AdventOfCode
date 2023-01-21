using System.Collections.Immutable;

var nodes = new Dictionary<string, List<string>>();

foreach (var line in File.ReadLines("input.txt"))
{
    var names = line.Split('-');
    if (!nodes.ContainsKey(names[0])) nodes[names[0]] = new List<string>();
    if (!nodes.ContainsKey(names[1])) nodes[names[1]] = new List<string>();
    nodes[names[0]].Add(names[1]);
    nodes[names[1]].Add(names[0]);
}

var paths = FindPaths(new[] { "start" }).ToArray();
foreach (var path in paths)
{
    Console.WriteLine(string.Join(',', path));
}
Console.WriteLine(paths.Length);

IEnumerable<string[]> FindPaths(IEnumerable<string> starting)
{
    var path = starting.ToArray();
    var last = starting.Last();
    if (last == "end")
    {
        yield return path;
        yield break;
    }

    var smalls = path.Where(n => n.All(char.IsLower));
    foreach (var node in nodes[last].Except(smalls))
    {
        foreach (var next in FindPaths(path.Append(node)))
        {
            yield return next;
        }
    }
}