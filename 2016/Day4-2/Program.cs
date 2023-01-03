using System.Text.RegularExpressions;

checked
{
    var input = File.ReadLines("input.txt").Select(l =>
    {
        var r = Regex.Match(l, @"^(?<Name>[a-z\-]+)\-(?<Sector>\d+)\[(?<Checksum>[a-z]+)\]$");
        var cs = new string(r.Groups["Name"].Value.Where(c => c != '-').GroupBy(c => c).Select(g => (g.Key, Count: g.Count())).OrderByDescending(c => c.Count).ThenBy(c => c).Take(5).Select(c => c.Key).ToArray());
        return (Name: r.Groups["Name"].Value, Sector: int.Parse(r.Groups["Sector"].Value), Valid: r.Groups["Checksum"].Value == cs);
    }).Where(i => i.Valid).ToArray();

    foreach (var item in input)
    {
        var name = new string(item.Name.Select(c => c == '-' ? '-' : (char)((c - 'a' + item.Sector) % 26 + 'a')).ToArray());
        if (name.Contains("pole"))
            Console.WriteLine((name, item.Sector));
    }
}