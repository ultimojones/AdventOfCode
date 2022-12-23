using System.Xml.Linq;

var monkies = File.ReadLines("input.txt").ToDictionary(l => l.Remove(4), l => new Monkey(l));

foreach (var monkey in monkies)
{
    if (monkey.Value.Name1 != null) monkey.Value.Monkey1 = monkies[monkey.Value.Name1];
    if (monkey.Value.Name2 != null) monkey.Value.Monkey2 = monkies[monkey.Value.Name2];
    monkey.Value.ReferencedBy.AddRange(monkies.Values.Where(m => m.Name1 == monkey.Key || m.Name2 == monkey.Key));
}


var root = monkies["root"];
Console.WriteLine(root);


class Monkey
{
    public Monkey(string spec)
    {
        Spec = spec[6..];
        ID = spec.Remove(4);
        if (long.TryParse(Spec, out long shout))
            Shout = shout;
        else
        {
            ExecCalc = spec[11] switch
            {
                '+' => Add,
                '-' => Subtract,
                '*' => Multiply,
                '/' => Divide,
                _ => throw new InvalidOperationException()
            };
            Name1 = spec.Substring(6, 4);
            Name2 = spec.Substring(13, 4);
        }
    }

    public string Spec { get; }
    public string ID { get; }
    public long Shout { get; }
    CalcOp? ExecCalc { get; set; }
    public string? Name1 { get; }
    public string? Name2 { get; }
    public Monkey? Monkey1 { get; set; }
    public Monkey? Monkey2 { get; set; }
    public List<Monkey> ReferencedBy { get; } = new List<Monkey>();

    delegate long CalcOp(long val1, long val2);
    long Add(long val1, long val2) => val1 + val2;
    long Subtract(long val1, long val2) => val1 - val2;
    long Multiply(long val1, long val2) => val1 * val2;
    long Divide(long val1, long val2) => val1 / val2;

    public override string ToString()
    {
        return Spec;
    }
}
