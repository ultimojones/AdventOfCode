using System.Collections;

var input = File.ReadAllText("input.txt");
var bits = input.SelectMany(c =>
{
    var v = byte.Parse(new[] { c }, System.Globalization.NumberStyles.HexNumber);
    var b = new BitArray(new[] { v }).Cast<bool>().Reverse().TakeLast(4).ToArray();
    return b;
}).ToArray();

// 343989586 too low

int pos = 0;

var parents = new Stack<Packet>();

while (pos < bits.Length)
{
    var current = bits[pos..];
    if (current.All(b => !b)) break;

    var pkt = new Packet { Pos = pos, Type = BitsToNum(current[3..6]) };
    var len = 6;

    if (pkt.Type == 4)
    {
        var binary = new List<bool>();
        for (int i = 6; ; i += 5)
        {
            binary.AddRange(current[(i + 1)..(i + 5)]);
            len += 5;
            if (!current[i]) break;
        }
        pkt.Value = BitsToNum(binary);
    }
    else
    {
        if (!current[6])
        {
            pkt.SubLength = BitsToNum(current[7..22]);
            len += 15;
        }
        else
        {
            pkt.SubCount = BitsToNum(current[7..18]);
            len += 11;
        }
        len++;
    }

    pkt.Length = len;
    pos += len;

    if (parents.TryPop(out var parent))
    {
        parent.AddSubPacket(pkt);
        if (pkt.Type != 4)
        {
            parents.Push(parent);
            parents.Push(pkt);
        }
        else
        {
            while ((parent.SubCount is long c && parent.SubPackets.Count == c)
                || (parent.SubLength is long l && pos - parent.Pos - parent.Length == l))
            {
                switch (parent.Type)
                {
                    case 0:
                        parent.Value = parent.SubPackets.Sum(p => p.Value);
                        break;
                    case 1:
                        parent.Value = parent.SubPackets.Aggregate(1L, (a, p) => a * p.Value!.Value);
                        break;
                    case 2:
                        parent.Value = parent.SubPackets.Min(p => p.Value);
                        break;
                    case 3:
                        parent.Value = parent.SubPackets.Max(p => p.Value);
                        break;
                    case 5:
                        parent.Value = parent.SubPackets[0].Value > parent.SubPackets[1].Value ? 1 : 0;
                        break;
                    case 6:
                        parent.Value = parent.SubPackets[0].Value < parent.SubPackets[1].Value ? 1 : 0;
                        break;
                    case 7:
                        parent.Value = parent.SubPackets[0].Value == parent.SubPackets[1].Value ? 1 : 0;
                        break;
                    default:
                        break;
                }
                Console.WriteLine($"Type: {parent.Type}, Input: {string.Join(',', parent.SubPackets.Select(p => p.Value))}, Value={parent.Value}");
                if (parents.Count == 0)
                    break;
                parent = parents.Pop();
            }
            parents.Push(parent!);
        }
    }
    else
    {
        parents.Push(pkt);
    }
}

Console.WriteLine();
Console.WriteLine(parents.Peek().Value);

long BitsToNum(IEnumerable<bool> bits)
{
    long value = 0;
    foreach (var bit in bits)
    {
        value = value * 2 + (bit ? 1 : 0);
    }
    return value;
}

class Packet
{
    public long Type { get; set; }
    public int Pos { get; set; }
    public int Length { get; set; }
    public long? SubLength { get; set; }
    public long? SubCount { get; set; }
    public long? Value { get; set; }
    public List<Packet> SubPackets { get; } = new List<Packet>();
    public Packet Parent { get; set; }
    public void AddSubPacket(Packet packet)
    {
        SubPackets.Add(packet);
        packet.Parent = this;
    }
}