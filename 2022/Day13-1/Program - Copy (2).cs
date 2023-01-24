var input = File.ReadAllLines("sample.txt");
for (int i = 1, l = 0; l < input.Length; i++, l += 3)
{
    var p = 0;
    var left = ReadPacket(ref input[l], ref p).ToArray();
    var right = ReadPacket(ref input[l], ref p).ToArray();
}

IEnumerable<object> ReadPacket(ref string s, ref int p)
{
    var pkt = Enumerable.Empty<object>();
    p++;
    while (p < s.Length)
    {
        switch (s[p])
        {
            case '[':
                pkt = pkt.Append(ReadPacket(ref s, ref p));
                break;
            case >= '0' and <= '9':
                var end = s.IndexOfAny(new[] { ',', ']' }, p);
                var integer = int.Parse(s[p..end].TakeWhile(char.IsDigit).ToArray());
                pkt = pkt.Append(integer);
                p = end;
                break;
            case ']':
                p++;
                return pkt.ToArray();
            default:
                p++;
                break;
        }
    }
    return pkt.ToArray();
}