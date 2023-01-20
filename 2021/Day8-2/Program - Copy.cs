var displays = File.ReadLines("input.txt").Select(l =>
{
    var a = l.Split(" | ");
    var input = a[0].Split(' ');
    var output = a[1].Split(' ');
    return (Input: input, Output: output);
}).ToArray();

//var result = displays.Count(d => d.Input.Concat(d.Output).Count(o => o.Length is 2 or 3 or 4) == 0);
//Console.WriteLine(result);

//var map = new Dictionary<char, char> { { 'a', ' ' }, { 'b', ' ' }, { 'c', ' ' }, { 'd', ' ' }, { 'e', ' ' }, { 'f', ' ' }, { 'g', ' ' } };
var map = Enumerable.Range('a', 7).ToDictionary(c => (char)c, c => ' ');

var d1 = "ab";
var d7 = "dab";
var d4 = "eafb";

map['a'] = d7.Except(d1).Single();
map['c'] = 'a';
map['f'] = 'b';

string c0 = new string(new[] { map['a'], map['b'], map['c'], map['e'], map['f'], map['g'] });
string c1 = new string(new[] { map['c'], map['f'] });
string c2 = new string(new[] { map['a'], map['c'], map['d'], map['e'], map['g'] });
string c3 = new string(new[] { map['a'], map['c'], map['d'], map['f'], map['g'] });
string c4 = new string(new[] { map['b'], map['c'], map['d'], map['f'] });
string c5 = new string(new[] { map['a'], map['b'], map['d'], map['f'], map['g'] });
string c6 = new string(new[] { map['a'], map['b'], map['d'], map['e'], map['f'], map['g'] });
string c7 = new string(new[] { map['a'], map['c'], map['f'] });
string c8 = new string(new[] { map['a'], map['b'], map['c'], map['d'], map['e'], map['f'], map['g'] });
string c9 = new string(new[] { map['a'], map['b'], map['c'], map['d'], map['f'], map['g'] });


IEnumerable<Dictionary<char, char>> CalcMap(IEnumerable<char> map, IEnumerable<char> chars)
{
    int count = 0;
    foreach (var c in chars)
    {
        count++;
        foreach (var m in CalcMap(map.Append(c), chars.Except(new[] { c })))
        {
            yield return m;
        }
    }
    if (count == 0)
    {
        yield return map.Select((c, p) => (p: 'a' + p, c)).ToDictionary(m => (char)m.p, m => m.c);
    }
}

foreach (var m in CalcMap(Enumerable.Empty<char>(), "abcdefg"))
{
    Console.WriteLine(string.Concat(m));
}