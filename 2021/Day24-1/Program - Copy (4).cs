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
var endings = new PriorityQueue<(string monad, int regz), long>();
endings.Enqueue(("", 0), 99999999999999);
var validMonads = new List<string>();
long bestPriority = 99999999999999;

while (endings.TryDequeue(out var ending, out var p))
{
    //if (p < bestPriority)
    //{
    //    Console.WriteLine($"{ending.monad}:{ending.regz}");
    //    bestPriority = p;
    //}
    var startCmd = inputPosn[13 - ending.monad.Length];
    for (int z = 1; z < 50000; z++)
    {
        for (int i = 9; i > 0; i--)
        {
            regs['w'] = regs['x'] = regs['y'] = 0;
            regs['z'] = ending.monad.Length == 13 ? 0 : z;
            ValidateMonad(i.ToString(), regs, startCmd);
            if (regs['z'] == ending.regz)
            {
                var monad = string.Concat(i, ending.monad);
                if (monad.Length == 14)
                {
                    validMonads.Add(monad);
                    Console.WriteLine($"Valid = {monad}");
                }
                else
                {
                    Console.WriteLine($"{monad}:{z} > {ending.monad}:{ending.regz}");
                    endings.Enqueue((monad, z), 14 - monad.Length);
                }
            }
        }
        if (ending.monad.Length == 13)
            break;
    }
}
// [1-9] 9132
// [1-9] 13-21

foreach (var valid in validMonads.Order())
{
    Console.WriteLine(valid);
}

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