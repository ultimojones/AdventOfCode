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
        for (long w = 9; w > 0; w--)
        {
            var reverse = ReverseMonadDigit(d, w, zout).ToArray();
            foreach (var zin in reverse)
            {
                //var ztest = ParseMonadDigit(d, w, zin);
                //if (zout != ztest)
                //    throw new Exception();
                var state = (d, w, zin, zout);
                states.Add(state);
                //Console.WriteLine($"{state}");
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
        if (bestMonadVal is null || monadVal > bestMonadVal)
        {
            bestDigits = digits;
            bestMonadVal = monadVal;
            foreach (var item in monadStates)
            {
                Console.WriteLine(item);
                ParseMonadDigit(item.pos, item.digit, item.zin);
            }
            Console.WriteLine(digits);
        }
    }
    else
    {
        if (bestDigits is null || digits.Length == 0 || long.Parse(digits) >= long.Parse(bestDigits[..digits.Length]))
        {
            foreach (var monadState in states.Where(s => s.pos == digits.Length && s.zin == zin).OrderByDescending(s => s.digit))
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

    Console.WriteLine($"inp w = {w}");
    Console.WriteLine($"mul x 0; add x z; mod x 26 = {x = z % 26}");
    Console.WriteLine($"div z {parms.DivZ} = {z /= parms.DivZ}");
    Console.WriteLine($"add x {parms.AddX} = {x += parms.AddX}");
    Console.WriteLine($"eql x w; eql x 0 = {x = (x == w ? 0 : 1)}");
    if (x != 0)
    {
        Console.WriteLine($"mul y 0; add y 25; mul y x; add y 1; mul z y = {z *= (25 * x + 1)}");
        Console.WriteLine($"mul y 0; add y w; add y {parms.AddY}; mul y x; add z y = {z += (w + parms.AddY) * x}");
    }

    return z;
}
