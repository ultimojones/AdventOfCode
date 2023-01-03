using System.Linq;
using System.Security.Cryptography;
using System.Text;

var door = Encoding.ASCII.GetBytes("abbhdwsy");

var pin = new List<char>();

for (int i = 0; i < 100000000; i++)
{
    var num = Encoding.ASCII.GetBytes(i.ToString());
    var hash = MD5.HashData(door.Concat(num).ToArray());
    if (hash[0] == 0 && hash[1] == 0 && (hash[2] & 0xF0) == 0)
    {
        var digit = hash[2] & 0x0F;
        pin.Add((char)(digit < 10 ? '0' + digit : 'a' + digit - 10));
        Console.WriteLine($"{i} > {new string(pin.ToArray())}");
        if (pin.Count == 8)
            break;
    }
}
