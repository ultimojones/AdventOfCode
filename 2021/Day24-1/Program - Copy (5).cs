using System.Text;

var rawCommands = File.ReadAllLines("input.txt").Select(l =>
{
    var op = l[..3] switch { "inp" => 'i', "add" => 'a', "mul" => 't', "div" => 'd', "mod" => 'm', "eql" => 'e', _ => '?' };
    var rega = l[4];
    char? regb = l.Length > 5 && l[6] is 'w' or 'x' or 'y' or 'z' ? l[6] : null;
    int? valb = l.Length > 5 && int.TryParse(l[6..], out var b) ? b : null;
    return (op, rega, regb, valb, l);
}).ToArray();
var inputPosn = rawCommands.Select((c, p) => (c, p)).Where(c => c.c.op == 'i').Select(c => c.p).ToArray();

var regs = new Dictionary<char, long> { { 'w', 0 }, { 'x', 0 }, { 'y', 0 }, { 'z', 0 } };
var validStates = new List<(int Pos, int Digit, long ZIn, long ZOut)> { (14, 0, 0, 0) };

for (int p = 13; p >= 0; p--)
{
    Console.WriteLine(p);
    var startCmd = inputPosn[p];
    var targets = validStates.Where(s => s.Pos == p + 1).Select(s => s.ZIn).Distinct().ToArray();
    for (int z = -5000; z < 5000; z++)
    {
        for (int i = 9; i > 0; i--)
        {
            regs['w'] = regs['x'] = regs['y'] = 0;
            regs['z'] = p == 0 ? 0 : z;
            ValidateMonad(i.ToString(), regs, startCmd);
            if (targets.Contains(regs['z']))
            {
                var state = (p, i, z, regs['z']);
                validStates.Add(state);
                //if (p < 5)
                    Console.WriteLine(state);
            }
        }
        if (p == 0)
            break;
    }
}

long inputZ = 0;
for (int i = 2; i < 14; i++)
{
    var digit = validStates.Where(s => s.Pos == i && s.ZIn == inputZ).MaxBy(s => s.Digit);
    Console.WriteLine(digit);
    inputZ = digit.ZOut;
}

//foreach (var valid in validStates.Order())
//{
//    Console.WriteLine(valid);
//}

void ValidateMonad(string monad, Dictionary<char, long> regs, int startCmd)
{
    var input = new Queue<int>(monad.Select(i => i - '0'));
    var exit = false;

    foreach (var line in rawCommands[startCmd..])
    {
        switch (line.op)
        {
            case 'i':
                if (input.Count == 0)
                    exit = true;
                else
                    regs[line.rega] = input.Dequeue();
                break;
            case 'a':
                regs[line.rega] += line.valb ?? regs[line.regb.Value];
                break;
            case 't':
                regs[line.rega] *= line.valb ?? regs[line.regb.Value];
                break;
            case 'd':
                regs[line.rega] /= line.valb ?? regs[line.regb.Value];
                break;
            case 'm':
                regs[line.rega] %= line.valb ?? regs[line.regb.Value];
                break;
            case 'e':
                regs[line.rega] = regs[line.rega] == (line.valb ?? regs[line.regb.Value]) ? 1 : 0;
                break;
            default:
                break;
        }

        if (exit) break;
    }
}