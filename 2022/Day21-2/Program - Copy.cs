var monkies = File.ReadLines("input.txt").ToDictionary(l => l.Remove(4),
    l =>
    {
        var element = new Monkey();
        if (long.TryParse(l.AsSpan(6), out long shout))
            element.Shout = shout;
        else
        {
            element.CalcSym = l[11];
            element.Name1 = l.Substring(6, 4);
            element.Name2 = l.Substring(13, 4);
        }
        return element;
    });

foreach (var monkey in monkies.Values)
{
    if (monkey.Name1 != null)
        monkey.Monkey1 = monkies[monkey.Name1];
    if (monkey.Name2 != null)
        monkey.Monkey2 = monkies[monkey.Name2];
}


var root = monkies["root"].GetValue();
Console.WriteLine(root);

class Monkey
{
    public Monkey()
    { }

    long? value;
    char? calcSym;

    public long GetValue()
    {
        if (value.HasValue)
            return value.Value;

        if (Shout.HasValue)
            return (value = Shout).Value;

        checked
        {
            value = ExecCalc(Monkey1.GetValue(), Monkey2.GetValue());
            return value.Value;
        }
    }

    public long? Shout { get; set; }
    public char? CalcSym
    {
        get => calcSym;
        set
        {
            calcSym = value;
            ExecCalc = value switch
            {
                '+' => Add,
                '-' => Subtract,
                '*' => Multiply,
                '/' => Divide,
                _ => throw new InvalidOperationException()
            };
        }
    }
    public string Name1 { get; set; } = default!;
    public string Name2 { get; set; } = default!;
    public Monkey? Monkey1 { get; set; }
    public Monkey? Monkey2 { get; set; }

    delegate long CalcOp(long val1, long val2);
    CalcOp? ExecCalc { get; set; }
    long Add(long val1, long val2) => val1 + val2;
    long Subtract(long val1, long val2) => val1 - val2;
    long Multiply(long val1, long val2) => val1 * val2;
    long Divide(long val1, long val2) => val1 / val2;
}
