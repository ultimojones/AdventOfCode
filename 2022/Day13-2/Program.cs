var mark = "[[2]]".AsSpan();
var mark2 = ReadPacket(ref mark).ToArray();
mark = "[[6]]".AsSpan();
var mark6 = ReadPacket(ref mark).ToArray();
var input = File.ReadLines("input.txt").Where(l => l.Length > 0)
    .Select(l => { var s = l.AsSpan(); return ReadPacket(ref s).ToArray(); }).Append(mark2).Append(mark6).ToArray();

Array.Sort(input, new PacketComparer());

input.Select(PrintArray).ToList().ForEach(Console.WriteLine);
Console.WriteLine((Array.IndexOf(input, mark2) + 1) * (Array.IndexOf(input, mark6) + 1));

string PrintArray(object[] arr) => '[' + string.Join(",", arr.Select(a => a is int v ? v.ToString() : PrintArray((object[])a))) + ']';

IEnumerable<object> ReadPacket(ref ReadOnlySpan<char> s)
{
    var pkt = Enumerable.Empty<object>();
    int p = s[0] is '[' ? 1 : 0;
    while (p < s.Length)
    {
        switch (s[p])
        {
            case '[':
                s = s.Slice(p);
                pkt = pkt.Append(ReadPacket(ref s));
                p = 0;
                break;
            case >= '0' and <= '9':
                var len = s[p..].IndexOfAny<char>(new[] { ',', ']' });
                pkt = pkt.Append(int.Parse(s[p..(p + len)]));
                p += len;
                break;
            case ']':
                s = s.Slice(p + 1);
                return pkt.ToArray();
            default:
                p++;
                break;
        }
    }
    return pkt.ToArray();
}

class PacketComparer : System.Collections.IComparer
{
    public int Compare(object? a, object? b)
    {
        var left = a as object[] ?? throw new ArgumentException("a");
        var right = b as object[] ?? throw new ArgumentException("b");

        for (int i = 0; i < int.Min(left.Length, right.Length); i++)
        {
            if (left[i] is int lVal && right[i] is int rVal)
            {
                if (lVal != rVal)
                    return lVal - rVal;
            }
            else
            {
                var lArg = left[i] is object[] l ? l : new[] { left[i] };
                var rArg = right[i] is object[] r ? r : new[] { right[i] };
                var result = Compare(lArg, rArg);
                if (result != 0)
                    return result;
            }
        }
        if (left.Length == right.Length)
            return 0;
        return left.Length - right.Length;
    }
}
