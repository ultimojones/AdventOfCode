using System.Collections;

var initialState = "10010000000110000";
var diskSize = 35651584;

var data = new BitArray(initialState.Select(b => b == '1').ToArray());

while (data.Length < diskSize)
{
    var startLen = data.Length;
    data.Length = data.Length * 2 + 1;
    data[startLen] = false;
    for (int i = startLen - 1, j = startLen + 1; i >= 0; i--, j++)
    {
        data[j] = !data[i];
    }
}
data.Length = diskSize;

do
{
    data = new BitArray(Enumerable.Range(0, data.Length / 2).Select(i => !(data[i * 2] ^ data[i * 2 + 1])).ToArray());
} while (int.IsEvenInteger(data.Length));

Console.WriteLine(string.Concat(data.Cast<bool>().Select(b => b ? 1 : 0)));

