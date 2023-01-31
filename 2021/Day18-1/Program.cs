Pair previous = null!;

foreach (var line in File.ReadLines("input.txt"))
{
    int pos = 0;
    char side = 'L';
    var current = new Pair();
    while (++pos < line.Length)
    {
        switch (line[pos])
        {
            case '[':
                {
                    var child = new Pair { Parent = current, ParentSide = side };
                    if (side == 'L')
                        current.Left = child;
                    else
                        current.Right = child;
                    current = child;
                    side = 'L';
                    break;
                }
            case ',':
                side = 'R';
                break;
            case ']':
                if (current.Parent is not null)
                {
                    side = current.ParentSide;
                    current = current.Parent;
                }
                break;
            default:
                var end = line.IndexOfAny(new[] { '[', ',', ']' }, pos);
                var number = int.Parse(line[pos..end]);
                if (side == 'L')
                    current.Left = number;
                else
                    current.Right = number;
                pos = end - 1;
                break;
        }
    }
    if (previous is not null)
    {
        var working = new Pair { Left = previous, Right = current };
        previous.Parent = working;
        previous.ParentSide = 'L';
        current.Parent = working;
        current.ParentSide = 'R';

        Console.WriteLine($"  {previous}");
        Console.WriteLine($"+ {current}");

        while (working.Explode() || working.Split()) { } //Console.WriteLine(working); }

        Console.WriteLine($"= {working}");
        Console.WriteLine();

        previous = working;
    }
    else
    {
        previous = current;
    }
}

Console.WriteLine(previous.CalcMagnitude());


class Pair
{
    public Pair? Parent { get; set; }
    public char ParentSide { get; set; } = '?';
    public object? Left { get; set; }
    public object? Right { get; set; }

    public override string ToString()
    {
        return $"[{Left},{Right}]";
    }

    public long CalcMagnitude()
    {
        return (Left is int lVal ? lVal : ((Pair)Left).CalcMagnitude()) * 3L
             + (Right is int rVal ? rVal : ((Pair)Right).CalcMagnitude()) * 2L;
    }

    public bool Explode(int level = 1)
    {
        switch (Left, Right)
        {
            case (Pair lPr, int):
                return lPr.Explode(level + 1);
            case (int, Pair rPr):
                return rPr.Explode(level + 1);
            case (Pair lPr, Pair rPr):
                return lPr.Explode(level + 1) || rPr.Explode(level + 1);
            case (int lVal, int rVal) when level >= 5:
                Parent!.AddValue(ParentSide, 'L', lVal);
                Parent.AddValue(ParentSide, 'R', rVal);
                if (ParentSide == 'L')
                    Parent.Left = 0;
                else
                    Parent.Right = 0;
                return true;
            default:
                return false;
        }
    }

    public bool Split()
    {
        if (Left is Pair lPr)
        {
            if (lPr.Split())
                return true;
        }
        else if (Left is int lVal && lVal >= 10)
        { 
            var split = int.DivRem(lVal, 2);
            Left = new Pair { Left = split.Quotient, Right = split.Quotient + split.Remainder, Parent = this, ParentSide = 'L' };
            return true;
        }
        if (Right is Pair rPr)
        {
            return rPr.Split();
        }
        else if (Right is int rVal && rVal >= 10)
        {
            var split = int.DivRem(rVal, 2);
            Right = new Pair { Left = split.Quotient, Right = split.Quotient + split.Remainder, Parent = this, ParentSide = 'R' };
            return true;
        }

        return false;
    }

    public void AddValue(char side, char dir, int value)
    {
        switch (side, dir)
        {
            case ('L', 'L'):
            case ('R', 'R'):
                if (Parent is not null)
                    Parent.AddValue(ParentSide, dir, value);
                break;
            case ('P', 'L'):
            case ('L', 'R'):
                if (Right is int r)
                    Right = r + value;
                else
                    (Right as Pair)!.AddValue('P', dir, value);
                break;
            case ('P', 'R'):
            case ('R', 'L'):
                if (Left is int l)
                    Left = l + value;
                else
                    (Left as Pair)!.AddValue('P', dir, value);
                break;
            default:
                break;
        }
    }
}