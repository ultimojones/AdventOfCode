using System.Net.WebSockets;

var displays = File.ReadLines("input.txt").Select(l =>
{
    var a = l.Split(" | ");
    var input = a[0].Split(' ');
    var output = a[1].Split(' ');
    return (Input: input, Output: output);
}).ToArray();

IEnumerable<Dictionary<char, char>> CalcMap(IEnumerable<char> map, IEnumerable<char> chars)
{
    int count = 0;
    foreach (var c in chars)
    {
        count++;
        foreach (var m in CalcMap(map.Append(c), chars.Except(new[] { c })))
            yield return m;
    }
    if (count == 0)
        yield return map.Select((c, p) => (p: 'a' + p, c)).ToDictionary(m => (char)m.p, m => m.c);
}

string Calc0(Dictionary<char, char> m) => new string(new[] { m['a'], m['b'], m['c'], m['e'], m['f'], m['g'] }.Order().ToArray());
string Calc1(Dictionary<char, char> m) =>  new string(new[] { m['c'], m['f'] }.Order().ToArray());
string Calc2(Dictionary<char, char> m) =>  new string(new[] { m['a'], m['c'], m['d'], m['e'], m['g'] }.Order().ToArray());
string Calc3(Dictionary<char, char> m) =>  new string(new[] { m['a'], m['c'], m['d'], m['f'], m['g'] }.Order().ToArray());
string Calc4(Dictionary<char, char> m) =>  new string(new[] { m['b'], m['c'], m['d'], m['f'] }.Order().ToArray());
string Calc5(Dictionary<char, char> m) =>  new string(new[] { m['a'], m['b'], m['d'], m['f'], m['g'] }.Order().ToArray());
string Calc6(Dictionary<char, char> m) =>  new string(new[] { m['a'], m['b'], m['d'], m['e'], m['f'], m['g'] }.Order().ToArray());
string Calc7(Dictionary<char, char> m) =>  new string(new[] { m['a'], m['c'], m['f'] }.Order().ToArray());
string Calc8(Dictionary<char, char> m) =>  new string(new[] { m['a'], m['b'], m['c'], m['d'], m['e'], m['f'], m['g'] }.Order().ToArray());
string Calc9(Dictionary<char, char> m) => new string(new[] { m['a'], m['b'], m['c'], m['d'], m['f'], m['g'] }.Order().ToArray());

IEnumerable<(int Digit, string Segments)[]> CalcDigits()
{
    foreach (var m in CalcMap(Enumerable.Empty<char>(), "abcdefg"))
    {
        yield return new[] 
        { 
            (0, Calc0(m)), 
            (1, Calc1(m)), 
            (2, Calc2(m)), 
            (3, Calc3(m)), 
            (4, Calc4(m)), 
            (5, Calc5(m)), 
            (6, Calc6(m)), 
            (7, Calc7(m)), 
            (8, Calc8(m)), 
            (9, Calc9(m)) 
        };
    }
}

var digits = CalcDigits().ToList();
double total = 0;

foreach (var disp in displays.Select(d => 
    (Input: d.Input.Select(i => string.Concat(i.Order())), 
     Output: d.Output.Select(i => string.Concat(i.Order())))))
{
    var match = digits.Single(d => disp.Input.Concat(disp.Output).Distinct()
        .All(a => d.Any(s => s.Segments == a)));
    total += disp.Output.Reverse().Select((d, p) => match.First(m => m.Segments == d).Digit * Math.Pow(10, p)).Sum();
}

Console.WriteLine(total);