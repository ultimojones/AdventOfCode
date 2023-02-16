var rawCommands = File.ReadAllLines("input.txt");
var inputPosn = rawCommands.Select((c, p) => (c, p)).Where(c => c.c == "inp w").Select(c => c.p).Append(rawCommands.Length - 1).ToArray();

var results = new PriorityQueue<(string MONAD, Dictionary<char, long> Regs), (long Len, long Z)>();
results.Enqueue(("", new Dictionary<char, long> { { 'w', 0 }, { 'x', 0 }, { 'y', 0 }, { 'z', 0 } }), (0, 0));
long bestMonad = 0;
int furthest = 0;

while (results.TryDequeue(out var result, out var p))
{
    if (result.MONAD.Length > furthest)
    {
        Console.WriteLine($"{result.MONAD} = {result.Regs['z']}");
        furthest= result.MONAD.Length;
    }
    var startCmd = inputPosn[result.MONAD.Length];
    var stopCmd = inputPosn[result.MONAD.Length + 1] + 1;
    for (int i = 1; i < 10; i++)
    {
        var monad = string.Concat(result.MONAD, i);
        var next = ValidateMonad(monad, rawCommands[startCmd..stopCmd], result.Regs);
        if (monad.Length < 14)
        {
            results.Enqueue((monad, next), (next['z'] % 26, 14 - monad.Length));
        }
        else
        {
            Console.WriteLine($"{monad} = {next['z']}");
            if (next['z'] == 0)
            {
                var number = long.Parse(monad);
                if (number > bestMonad)
                {
                    bestMonad = number;
                }
            }
        }
    }
}

Console.WriteLine($"{bestMonad} = 0");


Dictionary<char, long> ValidateMonad(string monad, string[] commands, Dictionary<char, long> startRegs)
{
    var input = new Queue<int>(monad.Select(i => i - '0'));
    var regs = new Dictionary<char, long>(startRegs);
    var exit = false;
    int pos = 0;

    foreach (var line in commands)
    {
        pos++;
        switch (line[0..3])
        {
            case "inp":
                if (input.Count == 0)
                {
                    exit = true;
                }
                else
                {
                    regs[line[4]] = input.Dequeue();
                }
                break;
            case "add":
                regs[line[4]] += GetArg2();
                break;
            case "mul":
                regs[line[4]] *= GetArg2();
                break;
            case "div":
                regs[line[4]] /= GetArg2();
                break;
            case "mod":
                regs[line[4]] %= GetArg2();
                break;
            case "eql":
                regs[line[4]] = regs[line[4]] == GetArg2() ? 1 : 0;
                break;
            default:
                break;
        }

        if (exit) break;
        long GetArg2() => int.TryParse(line[6..], out int num) ? num : regs[line[6]];
    }

    return regs;
}