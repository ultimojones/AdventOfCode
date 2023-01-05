using System.Text.RegularExpressions;

var input = File.ReadAllText("input.txt");
int i = 0;

var output = GetOutput().ToArray();

Console.WriteLine(new string(output));
Console.WriteLine(output.Length);

IEnumerable<char> GetOutput()
{
    while (i < input.Length)
    {
        switch (input[i])
        {
            case '(' when Regex.IsMatch(input[i..], @"^\(\d+x\d+\)"):
                var markerEnd = input.IndexOf(')', i);
                var vals = input[(i + 1)..markerEnd].Split('x').Select(int.Parse).ToArray();
                for (int r = 0; r < vals[1]; r++)
                {
                    for (int c = 0, v = markerEnd + 1; c < vals[0]; c++, v++)
                    {
                        yield return input[v];
                    }
                }
                i = markerEnd + vals[0] + 1;
                break;
            default:
                yield return input[i];
                i++;
                break;
        }
    }
}