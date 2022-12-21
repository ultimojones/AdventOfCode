checked
{
    var monkies = File.ReadLines("input.txt").ToDictionary(l => l.Remove(4),
        l =>
        {
            var element = new Monkey();
            if (long.TryParse(l.AsSpan(6), out long shout))
                element.Shout = shout;
            else
            {
                element.Calc = l[11];
                element.Val1 = l.Substring(6, 4);
                element.Val2 = l.Substring(13, 4);
            }
            return element;
        });

    monkies["humn"].Shout = 301;

    Console.WriteLine(CalcShout(monkies["root"].Val1));
    Console.WriteLine(CalcShout(monkies["root"].Val2));

    long CalcShout(string monkey)
    {
        var element = monkies[monkey];

        if (element.Shout.HasValue)
            return element.Shout.Value;

        var val1 = CalcShout(element.Val1);
        var val2 = CalcShout(element.Val2);
        var calc = element.Calc switch
        {
            '+' => val1 + val2,
            '-' => val1 - val2,
            '*' => val1 * val2,
            '/' => val1 / val2,
            _ => throw new InvalidOperationException()
        };
        return calc;
    }
}

class Monkey
{
    public Monkey()
    { }

    public Monkey(long? Shout, char? Calc, string Val1, string Val2)
    {
        this.Shout = Shout;
        this.Calc = Calc;
        this.Val1 = Val1;
        this.Val2 = Val2;
    }

    public long? Shout { get; set; }
    public char? Calc { get; set; }
    public string Val1 { get; set; } = default!;
    public string Val2 { get; set; } = default!;
}