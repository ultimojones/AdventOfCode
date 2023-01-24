var nodes = new Dictionary<string, List<string>>();

foreach (var line in File.ReadLines("input.txt"))
{
    var names = line.Split('-');
    if (!nodes.ContainsKey(names[0])) nodes[names[0]] = new List<string>();
    if (!nodes.ContainsKey(names[1])) nodes[names[1]] = new List<string>();
    nodes[names[0]].Add(names[1]);
    nodes[names[1]].Add(names[0]);
}

var paths = FindPaths(new[] { "start" }, false).ToArray();
foreach (var path in paths)
{
    Console.WriteLine(string.Join(',', path));
}
Console.WriteLine(paths.Length);

IEnumerable<string[]> FindPaths(IEnumerable<string> starting, bool smallDone)
{
    var path = starting.ToArray();
    var last = starting.Last();
    if (last == "end")
    {
        yield return path;
        yield break;
    }

    foreach (var node in nodes[last].Except(new[] { "start" }))
    {
        bool doingSmall = smallDone;

        if (node.All(char.IsLower) && path.Contains(node))
        {
            if (smallDone) { continue; }
            doingSmall = true;
        }

        foreach (var next in FindPaths(path.Append(node), doingSmall))
        {
            yield return next;
        }
    }
}