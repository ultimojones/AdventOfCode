using System.Security.Cryptography;
using System.Text;

var input = "iwrupvqb";

using var md5 = MD5.Create();


for (int i = 1; i < 100000000; i++)
{
    var inputBytes = Encoding.ASCII.GetBytes(input + i.ToString());
    var hashBytes = md5.ComputeHash(inputBytes);
    if (hashBytes[0] == 0x0 && hashBytes[1] == 0x0 && hashBytes[2] < 0x10)
    {
        Console.WriteLine(i);
        break;
    }
}
