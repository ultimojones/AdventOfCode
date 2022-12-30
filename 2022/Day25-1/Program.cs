long total = 0;

foreach (var line in File.ReadLines("input.txt"))
{
    var val = new Snafu(line);
    var snafu = new Snafu(val.ToLong());
    Console.WriteLine($"{line,24} {val.ToLong(),20} {snafu.ToString(),24}");

    total += val.ToLong();
}
Console.WriteLine($"{total} = {new Snafu(total).ToString()}");

public struct Snafu
{
    long valueLong;
    string valueString;

    public Snafu(string value)
    {
        valueString = value;

        valueLong = 0;
        for (int i = value.Length - 1, p = 0; i >= 0; i--, p++)
        {
            valueLong += (long)Math.Pow(5, p) 
                * value[i] switch { '2' => 2, '1' => 1, '0' => 0, '-' => -1, '=' => -2, 
                    _ => throw new NotImplementedException() };
        }
    }

    public Snafu(long value)
    {
        valueLong = value;

        var workValue = value;
        var digits = new Dictionary<int, long>();
        for (int i = 1; workValue > 0; i++)
        {
            var order = (long)Math.Pow(5, i);
            var digitVal = workValue % order;
            workValue -= digitVal;
            digits[i] = digitVal / (long)Math.Pow(5, i - 1);
        }

        for (int i = 1; i <= digits.Keys.Max(); i++)
        {
            long carry = 0;
            if (digits[i] > 4)
            {
                var divRem = Math.DivRem(digits[i], 5);
                carry = divRem.Quotient;
                digits[i] = divRem.Remainder;
            }
            if (digits[i] == 3 || digits[i] == 4)
            {
                carry += 1;
            }
            if (carry > 0)
            {
                if (digits.TryGetValue(i + 1, out long next))
                {
                    digits[i + 1] = next + carry;
                }
                else
                {
                    digits[i + 1] = carry;
                }
            }
        }

        for (int i = 1; i <= digits.Keys.Max(); i++)
        {
            var item = digits[i];
            valueString = item switch
            {
                0 => '0',
                1 => '1',
                2 => '2',
                3 => '=',
                4 => '-',
                _ => throw new NotImplementedException()
            } + valueString;
        }
    }

    public long ToLong()
    {
        return valueLong;
    }

    public override string ToString()
    {
        return valueString;
    }
}