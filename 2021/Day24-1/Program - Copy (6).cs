var inputCommands = File.ReadAllText("input.txt");

var commands = inputCommands.Split(new[] { '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).Select(l =>
{
    var op = l[..3] switch { "inp" => 'i', "add" => 'a', "mul" => 't', "div" => 'd', "mod" => 'm', "eql" => 'e', _ => '?' };
    var rega = l[4];
    char? regb = l.Length > 5 && l[6] is 'w' or 'x' or 'y' or 'z' ? l[6] : null;
    int? valb = l.Length > 5 && int.TryParse(l[6..], out var b) ? b : null;
    return (op, rega, regb, valb, l);
}).ToArray();
var inputPosn = commands.Select((c, p) => (c, p)).Where(c => c.c.op == 'i').Select(c => c.p).ToArray();
var regs = new Dictionary<char, long> { { 'w', 0 }, { 'x', 0 }, { 'y', 0 }, { 'z', 0 } };

//ValidateMonad("99999999999999", regs, 0, true);
//return;

var states = new List<(int pos, int digit, long zin, long zout)>();
states.Add((14, 0, 0, 0));

for (int d = 13; d >= 0; d--)
{
    var targets = states.Where(s => s.pos == d + 1).Select(s => s.zin).ToArray();
    for (int i = 1; i < 10; i++)
    {
        var monad = i.ToString();
        if (d > 0)
        {
            bool foundState = false;
            long baseZLimit = 100000;
            for (int m = 0; !foundState && m < 2; m++)
            {
                var zLimit = baseZLimit * (long)Math.Pow(10, m);
                var zInner = m == 0 ? 0 : baseZLimit * (long)Math.Pow(10, m - 1);
                if (m > 0) Console.WriteLine(new { m, zLimit, zInner });
                for (long z = zInner; z < zLimit; z = m == 0 || z < -zInner || z >= zInner ? z + 1 : zInner)
                {
                    regs['z'] = z;
                    ValidateMonad(i.ToString(), regs, inputPosn[d], false);
                    if (targets.Contains(regs['z']))
                    {
                        states.Add((d, i, z, regs['z']));
                        foundState = true;
                        Console.WriteLine($"{monad} @ {d} + {z,6} = {regs['z'],6}");
                    }
                }
            }
        }
        else
        {
            regs['z'] = 0;
            ValidateMonad(i.ToString(), regs, inputPosn[d], false);
            if (targets.Contains(regs['z']))
            {
                states.Add((d, i, 0, regs['z']));
                Console.WriteLine($"{monad} @ {d} + {0,6} = {regs['z'],6}");
            }
        }
    }
}
//int digitPosn = 7;
//long targetZ = 378532;
//for (int i = 1; i < 10; i++)
//{
//    var monad = i.ToString();
//    for (int z = 0; z < 1500000; z++)
//    {
//        regs['w'] = regs['x'] = regs['y'] = 0;
//        regs['z'] = z;
//        ValidateMonad(i.ToString(), regs, inputPosn[digitPosn], false);
//        if (regs['z'] == targetZ)
//        {
//            regs['w'] = regs['x'] = regs['y'] = 0;
//            regs['z'] = z;
//            Console.WriteLine($"{monad}@{digitPosn}+{z}");
//            ValidateMonad(i.ToString(), regs, inputPosn[digitPosn], true);
//            Console.WriteLine();
//            break;
//        }
//    }
//}

void ValidateMonad(string monad, Dictionary<char, long> regs, int startCmd, bool log)
{
    var input = new Queue<int>(monad.Select(i => i - '0'));
    var exit = false;
    var pos = startCmd + 1;

    foreach (var line in commands[startCmd..])
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
        if (log) Console.WriteLine($"{pos,3}: {line.l,-9} {string.Concat(regs)}");
        pos++;
    }
}