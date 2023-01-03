using System.Linq;
using System.Security.Cryptography;
using System.Text;

var door = Encoding.ASCII.GetBytes("abbhdwsy");

var pin = new Dictionary<int, char>(8);

for (int i = 0; i < 100000000; i++)
{
    var num = Encoding.ASCII.GetBytes(i.ToString());
    var hash = MD5.HashData(door.Concat(num).ToArray());
    if (hash[0] == 0 && hash[1] == 0 && (hash[2] & 0xF0) == 0)
    {
        var pos = hash[2] & 0x0F;

        if (pos < 8 && !pin.ContainsKey(pos))
        {
            var digit = (hash[3] & 0xF0) >> 4;
            pin[pos] = ((char)(digit < 10 ? '0' + digit : 'a' + digit - 10));
            Console.WriteLine($"{i} > {string.Concat(pin.OrderBy(p => p.Key))}");
            if (pin.Count == 8)
                break;
        }
    }
}

Console.WriteLine(string.Concat(pin.OrderBy(p => p.Key).Select(p => p.Value)));