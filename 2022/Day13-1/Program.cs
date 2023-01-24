var input = File.ReadAllLines("input.txt");
var total = 0;
for (int i = 1, l = 0; l < input.Length; i++, l += 3)
{
    var lineLeft = input[l].AsSpan();
    var lineRight = input[l + 1].AsSpan();
    var left = ReadPacket(ref lineLeft).ToArray();
    var right = ReadPacket(ref lineRight).ToArray();

    var ok = ComparePackets(left, right);
    if (ok.GetValueOrDefault())
    {
        total += i;
    }
}
Console.WriteLine(total);

bool? ComparePackets(object[] left, object[] right)
{
    for (int i = 0; i < int.Min(left.Length, right.Length); i++)
    {
        if (left[i] is int lVal && right[i] is int rVal)
        {
            if (lVal != rVal)
                return lVal < rVal;
        }
        else
        {
            var lArg = left[i] is object[] l ? l : new[] { left[i] };
            var rArg = right[i] is object[] r ? r : new[] { right[i] };
            var result = ComparePackets(lArg, rArg);
            if (result.HasValue)
                return result;
        }
    }
    if (left.Length == right.Length)
        return null;
    return left.Length < right.Length;
}

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