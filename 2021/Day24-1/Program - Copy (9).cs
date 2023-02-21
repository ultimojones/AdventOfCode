var inputCommands = File.ReadAllLines("input.txt");

var parameters = new List<(int Digit, long DivZ, long AddX, long AddY)>();
for (int i = 0; i < inputCommands.Length; i += 18)
{
    var divZ = long.Parse(inputCommands[i + 4][6..]);
    var addX = long.Parse(inputCommands[i + 5][6..]);
    var addY = long.Parse(inputCommands[i + 15][6..]);
    parameters.Add((i / 18, divZ, addX, addY));
}

var states = new Dictionary<(int pos, int digit, long zin), long>();
states.Add((-1, 0, 0), 0);

int d = 0;
while (d < 14 && d >= 0)
{
    var maxZ = (long)Math.Pow(26, int.Min(14 - d, 10));
    bool noTest = true;
    bool endValid = false;
    for (int w = 9; w > 0; w--)
    {
        var tests = states.Where(s => s.Key.pos == d - 1).Select(s => (d, w, zin: s.Value)).Except(states.Keys).ToArray();
        if (tests.Length > 0)
        {
            foreach (var test in tests)
            {
                var zout = ParseMonadDigit(d, w, test.zin);
                if (zout < maxZ)
                {
                    noTest = false;
                    states.Add(test, zout);
                    Console.WriteLine($"{test}={zout}");
                    if (d == 13 && zout == 0)
                        endValid = true;
                }
            }
            if (d < 13 && !noTest)
                break;
        }
    }
    if (endValid)
        break;
    if (noTest || d == 13)
        d--;
    else
        d++;
}

Console.WriteLine(states.Count(s => s.Key.pos == 13 && s.Value == 0));

long ParseMonadDigit(int d, int w, long z)
{
    long x = 0, y = 0;

    var parms = parameters[d];

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
