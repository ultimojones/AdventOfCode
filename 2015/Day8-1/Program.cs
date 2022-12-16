using System.Text.RegularExpressions;

var codelen = 0;
var memlen = 0;

foreach (var line in File.ReadLines("input.txt"))
{
    codelen += line.Length;
    var mem = Regex.Unescape(line[1..^1]);
    memlen += mem.Length;
}

Console.WriteLine(codelen - memlen);