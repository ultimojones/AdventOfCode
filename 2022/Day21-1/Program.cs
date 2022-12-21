checked
{
    var monkies = File.ReadLines("input.txt").ToDictionary(l => l.Remove(4),
        l =>
        {
            (long? Shout, char? Calc, string Val1, string Val2) element = default;
            if (long.TryParse(l.Substring(6), out long shout))
                element.Shout = shout;
            else
            {
                element.Calc = l[11];
                element.Val1 = l.Substring(6, 4);
                element.Val2 = l.Substring(13, 4);
            }
            return element;
        });

    var result = CalcShout("root");
    Console.WriteLine(result);

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