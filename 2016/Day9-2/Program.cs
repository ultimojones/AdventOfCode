using System.Text.RegularExpressions;

var input = File.ReadAllText("input.txt");
//var input = "X(8x2)(3x3)ABCY";

var output = Decompress(0, input.Length);
var decomp = output.LongCount();
Console.WriteLine(decomp);

IEnumerable<char> Decompress(int start, int len)
{
    for (int i = start; i < start + len; i++)
    {
        if (input[i] != '(' && !char.IsWhiteSpace(input[i]))
        {
            yield return input[i];
            continue;
        }

        var markerEnd = input.IndexOf(')', i);
        var vals = input[(i + 1)..markerEnd].Split('x').Select(int.Parse).ToArray();
        var decomp = Decompress(markerEnd + 1, vals[0]).ToArray();
        for (int r = 0; r < vals[1]; r++)
        {
            foreach (var c in decomp)
            {
                yield return c;
            }
        }
        i = markerEnd + vals[0];
    }
}

