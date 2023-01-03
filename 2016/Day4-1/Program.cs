using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

checked
{
    var input = File.ReadLines("input.txt").Select(l =>
    {
        var r = Regex.Match(l, @"^(?<Name>([a-z]+\-)+)(?<Sector>\d+)\[(?<Checksum>[a-z]+)\]$");
        var nam = r.Groups["Name"].Value;
        var chk = nam.Where(c => c != '-').GroupBy(c => c).Select(g => (g.Key, Count: g.Count()));
        var top = new string(chk.OrderByDescending(c => c.Count).ThenBy(c => c).Take(5).Select(c => c.Key).ToArray());
        return (Name: r.Groups["Name"].Value, Sector: int.Parse(r.Groups["Sector"].Value), ChecksumGiven: r.Groups["Checksum"].Value, ChecksumCalc: top, Valid: r.Groups["Checksum"].Value == top);
    }).ToArray();

    foreach (var item in input)
    {
        Console.WriteLine(item);
    }

    Console.WriteLine(input.Where(i => i.Valid).Sum(i => i.Sector));
}