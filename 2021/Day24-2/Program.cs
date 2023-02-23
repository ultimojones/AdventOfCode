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

for (int d = 13; d >= 0; d--)
{
    foreach (var zout in states.Where(s => s.pos == d + 1).Select(s => s.zin).ToArray())
    {
        for (long w = 1; w < 10; w++)
        {
            var reverse = ReverseMonadDigit(d, w, zout).ToArray();
            foreach (var zin in reverse)
            {
                var state = (d, w, zin, zout);
                states.Add(state);
            }
        }
    }
}

string bestDigits = null!;
long? bestMonadVal = null!;

ValidMonads("", 0, Enumerable.Empty<(int pos, long digit, long zin, long zout)>());

void ValidMonads(string digits, long zin, IEnumerable<(int pos, long digit, long zin, long zout)> monadStates)
{
    if (digits.Length == 14)
    {
        var monadVal = long.Parse(digits);
        if (bestMonadVal is null || monadVal < bestMonadVal)
        {
            bestDigits = digits;
            bestMonadVal = monadVal;
            foreach (var item in monadStates)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(digits);
        }
    }
    else
    {
        if (bestDigits is null || digits.Length == 0 || long.Parse(digits) <= long.Parse(bestDigits[..digits.Length]))
        {
            foreach (var monadState in states.Where(s => s.pos == digits.Length && s.zin == zin).OrderBy(s => s.digit))
            {
                ValidMonads(string.Concat(digits, monadState.digit), monadState.zout, monadStates.Append(monadState));
            }
        }
    }
}

IEnumerable<long> ReverseMonadDigit(int d, long w, long z)
{
    var parms = parameters![d];

    if (parms.DivZ == 26)
    {
        yield return z * parms.DivZ + w - parms.AddX;

        var zin = z - w - parms.AddY;
        if (zin % 26 == 0)
        {
            foreach (var r in Enumerable.Range((int)zin, 25))
            {
                if (r % 26 + parms.AddX != w)
                    yield return r;
            }
        }
    }
    else
    {
        if (z % 26 + parms.AddX == w)
        {
            yield return z;
        }

        var zin = z - w - parms.AddY;
        if (zin % 26 == 0) 
        {
            yield return zin /= 26;
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
    z *= y;
    y = w;
    y += parms.AddY;
    y *= x;
    z += y;

    return z;
}
