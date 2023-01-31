Pair? building = null;
foreach (var line in File.ReadLines("sample.txt"))
{
    var current = ParsePair(line);
    if (building is null)
    {
        building = current;
        continue;
    }
    var working = new Pair { LeftPair = building, RightPair = current };
    while (working.Explode()) { Console.WriteLine(working); }
    while (working.Split()) { Console.WriteLine(working); }
    Console.WriteLine(working);
}

Pair ParsePair(string input)
{
    int pos = 0;
    var levels = new Stack<(Pair, char Side)>();
    Pair current = new Pair();
    char side = 'L';
    Pair? previous = null;

    while (++pos < input.Length)
    {
        switch (input[pos])
        {
            case '[':
                var parent = current;
                levels.Push((current, side));
                current = new Pair();
                if (side == 'L')
                    parent.LeftPair = current;
                else
                    parent.RightPair = current;
                side = 'L';
                break;
            case ',':
                side = 'R';
                break;
            case ']':
                if (levels.TryPop(out var parent2))
                {
                    current = parent2.Item1;
                    side = parent2.Item2;
                }
                break;
            default:
                var next = input.IndexOfAny(new[] { '[', ']', ',' }, pos);
                var num = int.Parse(input[pos..next]);
                if (side == 'L')
                    current.LeftVal = num;
                else
                    current.RightVal = num;
                if (current.Previous is null)
                {
                    if (previous is not null && previous != current)
                    {
                        previous.Following = current;
                        current.Previous = previous;
                    }
                    previous = current;
                }
                pos = next - 1;
                break;
        }
    }

    return current;
}

class Pair
{
    public Pair? LeftPair { get; set; }
    public int? LeftVal { get; set; }
    public Pair? RightPair { get; set; }
    public int? RightVal { get; set; }
    public Pair? Previous { get; set; }
    public Pair? Following { get; set; }

    public bool Explode(int level = 1)
    {
        if (LeftPair is Pair l)
        {
            if (l.LeftPair is not null || l.RightPair is not null)
            {
                if (l.Explode(level + 1))
                    return true;
            }
            else if (level >= 4)
            {
                if (l.Previous is not null)
                {
                    if (l.Previous.RightVal is not null)
                        l.Previous.RightVal += l.LeftVal;
                    else
                        l.Previous.LeftVal += l.LeftVal;
                    l.Previous.Following = this;
                    Previous = l.Previous;
                }
                if (l.Following is not null)
                {
                    if (l.Following.LeftVal is not null)
                        l.Following.LeftVal += l.RightVal;
                    else
                        l.Following.RightVal += l.RightVal;
                    if (l.Following != this)
                    {
                        l.Following.Previous = this;
                        Following = l.Following;
                    }
                }
                LeftPair = null;
                LeftVal = 0;
                return true;
            }
        }
        if (RightPair is Pair r)
        {
            if (r.LeftPair is not null || r.RightPair is not null)
            {
                return r.Explode(level + 1);
            }
            else if (level >= 4)
            {
                if (r.Previous is not null)
                {
                    if (r.Previous.RightVal is not null)
                        r.Previous.RightVal += r.LeftVal;
                    else
                        r.Previous.LeftVal += r.LeftVal;
                    if (r.Previous != this)
                    {
                        r.Previous.Following = this;
                        Previous = r.Previous;
                    }
                }
                if (r.Following is not null)
                {
                    if (r.Following.LeftVal is not null)
                        r.Following.LeftVal += r.RightVal;
                    else
                        r.Following.RightVal += r.RightVal;
                    r.Following.Previous = this;
                    Following = r.Following;
                }
                RightPair = null;
                RightVal = 0;
                return true;
            }
        }
        return false;
    }

    public bool Split()
    {
        if (LeftPair is not null)
        {
            if (LeftPair.Split())
                return true;
        }
        else if (LeftVal >= 10)
        {
            LeftPair = new Pair { LeftVal = LeftVal / 2, RightVal = LeftVal / 2 + LeftVal % 2 };
            if (Previous is not null)
            {
                LeftPair.Previous = Previous;
                Previous.Following = LeftPair;
                Previous = null;
            }
            if (RightVal is int)
            {
                LeftPair.Following = this;
                Previous = LeftPair;
            }
            else
            {
                if (Following is not null)
                {
                    LeftPair.Following = Following;
                    Following.Previous = LeftPair;
                    Following = null;
                }
            }
            LeftVal = null;
            return true;
        }

        if (RightPair is not null)
        {
            return RightPair.Split();
        }
        else if (RightVal >= 10)
        {
            RightPair = new Pair { LeftVal = LeftVal / 2, RightVal = LeftVal / 2 + LeftVal % 2 };
            if (LeftVal is int)
            {
                RightPair.Previous = this;
                Following = RightPair;
            }
            else
            {
                if (Previous is not null)
                {
                    RightPair.Previous = Previous;
                    Previous.Following = LeftPair;
                    Previous = null;
                }
            }
            if (Following is not null)
            {
                RightPair.Following = Following;
                Following.Previous = RightPair;
                Following = null;
            }
            LeftVal = null;
            return true;
        }

        return false;
    }

    public long GetMagnitude()
    {
        return (LeftPair?.GetMagnitude() ?? (long)LeftVal!) * 3
            + (RightPair?.GetMagnitude() ?? (long)RightVal!) * 2;
    }

    public override string ToString()
    {
        return $"[{(LeftVal is int l ? l : LeftPair.ToString())},{(RightVal is int r ? r : RightPair.ToString())}]";
    }
}
