using System.Security.Cryptography;
using System.Text;

var input = "iwrupvqb";

var result = Parallel.For(1, 100000000, (i, state) =>
{
    using var md5 = MD5.Create();
    var inputBytes = Encoding.ASCII.GetBytes(input + i.ToString());
    var hashBytes = md5.ComputeHash(inputBytes);
    if (hashBytes[0] == 0x0 && hashBytes[1] == 0x0 && hashBytes[2] == 0x0)
        state.Break();
});

if (result.LowestBreakIteration.HasValue)
    Console.WriteLine($"\nLowest Break Iteration: {result.LowestBreakIteration}");
else
    Console.WriteLine($"\nNo lowest break iteration.");

