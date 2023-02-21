var inputCommands = File.ReadAllLines("input.txt");

var parameters = new List<(int Digit, long DivZ, long AddX, long AddY)>();
for (int i = 0; i < inputCommands.Length; i += 18)
{
    var divZ = long.Parse(inputCommands[i + 4][6..]);
    var addX = long.Parse(inputCommands[i + 5][6..]);
    var addY = long.Parse(inputCommands[i + 15][6..]);
    parameters.Add((i / 18, divZ, addX, addY));
}

var states = new List<(int pos, int digit, long zin, long zout)>();
states.Add((-1, 0, 0, 0));

for (int d = 0; d < 14; d++)
{
    foreach (var zin in states.Where(s => s.pos == d - 1).Select(s => s.zout).Order().ToArray())
    {
        for (int w = 1; w < 10; w++)
        {
            var zout = ParseMonadDigit(d, w, zin);
            var state = (d, w, zin, zout);
            if (!states.Contains(state))
            {
                states.Add(state);
                Console.WriteLine(state);
            }
        }
    }
}


long ParseMonadDigit(int d, int w, long z)
{
    long x = 0, y = 0;

    var parms = parameters[d];

    x = z % 25;
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
