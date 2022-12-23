using System.Xml.Linq;

var monkies = File.ReadLines("input.txt").ToDictionary(l => l.Remove(4), l => new Monkey(l));

foreach (var monkey in monkies)
{
    if (monkey.Value.Spec.Length == 11)
    {
        monkey.Value.Monkey1 = monkies[monkey.Value.Spec[0..4]];
        monkey.Value.Monkey2 = monkies[monkey.Value.Spec[7..11]];
    }
    monkey.Value.ReferencedBy.AddRange(monkies.Values.Where(m => m.Spec.Length == 11 && (m.Spec[0..4] == monkey.Key || m.Spec[7..11] == monkey.Key)));
}

var root = monkies["root"];
var humn = monkies["humn"];
long test1 = 5300000000000;
long test2 = 5400000000000;
var target = root.Monkey2.Result();

while (true)
{
    var result1 = Test(test1);
    var result2 = Test(test2);
    Console.WriteLine($"{test1,15} > {test2,-15} {result1,15} > {result2,-15} Target={target}");

    if (result1 == target)
    {
        Console.WriteLine($"Match {test1}");
        break;
    }

    if (result2 == target)
    {
        Console.WriteLine($"Match {test2}");
        break;
    }

    var range = test2 - test1;
    if (result1 > target && result2 > target)
    {
        test1 += range;
        test2 += range;
        continue;
    }
    else if (result1 < target && result2 < target)
    {
        test1 -= range;
        test2 -= range;
        continue;
    }
    else
    {
        test1 += range / 2;
        continue;
    }

    break;
}

long Test(long test)
{
    humn.Result = () => test;
    return root.Monkey1.Result();
}

class Monkey
{
    public Monkey(string spec)
    {
        Spec = spec[6..];
        ID = spec.Remove(4);
        if (long.TryParse(Spec, out long shout))
            Result = () => shout;
        else
        {
            Result = spec[11] switch
            {
                '+' => () => Monkey1.Result() + Monkey2.Result(),
                '-' => () => Monkey1.Result() - Monkey2.Result(),
                '*' => () => Monkey1.Result() * Monkey2.Result(),
                '/' => () => Monkey1.Result() / Monkey2.Result(),
                _ => throw new InvalidOperationException()
            };
        }
    }

    public Func<long> Result;

    public string Spec { get; }
    public string ID { get; }
    public Monkey? Monkey1 { get; set; }
    public Monkey? Monkey2 { get; set; }
    public List<Monkey> ReferencedBy { get; } = new List<Monkey>();

    public override string ToString()
    {
        return Spec;
    }
}
