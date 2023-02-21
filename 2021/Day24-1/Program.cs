var inputCommands = File.ReadAllLines("input.txt");

var parameters = new List<(int Digit, long DivZ, long AddX, long AddY)>();
for (int i = 0; i < inputCommands.Length; i += 18)
{
    var divZ = long.Parse(inputCommands[i + 4][6..]);
    var addX = long.Parse(inputCommands[i + 5][6..]);
    var addY = long.Parse(inputCommands[i + 15][6..]);
    parameters.Add((i / 18, divZ, addX, addY));
}

var states = new HashSet<(int pos, long digit, long zin, long zout)>();
states.Add((14, 0, 0, 0));

for (int d = 13; d >= 12; d--)
{
    foreach (var zout in states.Where(s => s.pos == d + 1).Select(s => s.zin).ToArray())
    {
        for (long w = 9; w > 0; w--)
        {
            foreach (var zin in ReverseMonadDigit(d, w, zout))
            {
                if (zout != ParseMonadDigit(d, w, zin))
                    throw new Exception();
                var state = (d, w, zin, zout);
                states.Add(state);
                Console.WriteLine($"{state}");
            }
        }
    }
}

Console.WriteLine(states.Count(s => s.pos == 13 && s.zout == 0));

IEnumerable<long> ReverseMonadDigit(int d, long w, long z)
{
    var parms = parameters![d];

    yield return z * parms.DivZ + w - parms.AddX;

    var zin = (int)(z - w - parms.AddY);
    if (zin >= 0)
    {
        foreach (var r in Enumerable.Range(zin, 25).Where(r => r > 26))
        {
            yield return r;
        }
    }
}

long ParseMonadDigit(int d, long w, long z)
{
    var parms = parameters![d];
    long x = 0, y = 0;

    x = z % 26;
    z /= parms.DivZ;
    x += parms.AddX;
    x = x == w ? 0 : 1;
    y = 25 * x + 1;
    z = z * y;
    y = w;
    y += parms.AddY;
    y *= x;
    z += y;

    return z;
}
