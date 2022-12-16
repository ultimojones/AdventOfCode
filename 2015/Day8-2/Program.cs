using System.Text.RegularExpressions;

var codelen = 0;
var memlen = 0;

foreach (var line in File.ReadLines("input.txt"))
{
    codelen += line.Length;
    var mem = '"' + line.Replace(@"\", @"\\").Replace(@"""", @"\""") + '"';
    memlen += mem.Length;
}

Console.WriteLine(memlen - codelen);
