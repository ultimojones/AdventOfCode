var inputCommands = File.ReadAllLines("input.txt");

//var commands = inputCommands.Split(new[] { '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).Select(l =>
//{
//    var op = l[..3] switch { "inp" => 'i', "add" => 'a', "mul" => 't', "div" => 'd', "mod" => 'm', "eql" => 'e', _ => '?' };
//    var rega = l[4];
//    char? regb = l.Length > 5 && l[6] is 'w' or 'x' or 'y' or 'z' ? l[6] : null;
//    int? valb = l.Length > 5 && int.TryParse(l[6..], out var b) ? b : null;
//    return (op, rega, regb, valb, l);
//}).ToArray();
//var inputPosn = commands.Select((c, p) => (c, p)).Where(c => c.c.op == 'i').Select(c => c.p).ToArray();
//var regs = new Dictionary<char, long> { { 'w', 0 }, { 'x', 0 }, { 'y', 0 }, { 'z', 0 } };

var parameters = new List<(long DivZ, long AddX, long AddY)>();
for (int i = 0; i < inputCommands.Length; i++)
{

}

//ValidateMonad("9959993496897", regs, 0, true);
//return;

var states = new List<(int pos, int digit, long zin, long zout)>();
states.Add((-1, 0, 0, 0));

for (int d = 0; d < 14; d++)
{
    Console.WriteLine(d);
    var inputs = states.Where(s => s.pos == d - 1).Select(s => s.zout).Distinct().ToArray();
    foreach (var z in inputs)
    {
        for (int i = 1; i < 10; i++)
        {
            regs['z'] = z;
            ValidateMonad(i.ToString(), regs, inputPosn[d], false);
            var state = (d, i, z, regs['z']);
            if (regs['z'] < 25000000 && !states.Contains(state))
            {
                //if (d == 13 && regs['z'] != 0)
                //    continue;
                states.Add(state);
            }
        }
    }
}

var valids = new List<(string monad, long zin)>();
valids.AddRange(states.Where(s => s.pos == 13 && s.zout == 0).Select(s => (s.digit.ToString(), s.zin)));

for (int i = 12; i >= 0; i--)
{
    var next = valids.GroupJoin(states.Where(s => s.pos == i), o => o.zin, i => i.zout, (o, i) => i.Select(s => (string.Concat(s.digit, o.monad), s.zin))).SelectMany(s => s);
    valids = next.ToList();
}
valids.OrderDescending().Take(40).ToList().ForEach(v => Console.WriteLine(v.monad));
Console.WriteLine();


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