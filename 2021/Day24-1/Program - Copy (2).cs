var rawCommands = File.ReadAllLines("input.txt");


//for (int i = 1; i < 10; i++)
//{
//    ValidateMonad(string.Concat("1111" + i), rawCommands, new Dictionary<char, long> { { 'w', 0 }, { 'x', 0 }, { 'y', 0 }, { 'z', 0 } });
//}

ValidateMonad("11115", rawCommands, new Dictionary<char, long> { { 'w', 0 }, { 'x', 0 }, { 'y', 0 }, { 'z', 0 } });



long ValidateMonad(string monad, string[] commands, Dictionary<char, long>? regs = null)
{
    var input = new Queue<int>(monad.Select(i => i - '0'));
    if (regs == null) regs = new Dictionary<char, long> { { 'w', 0 }, { 'x', 0 }, { 'y', 0 }, { 'z', 0 } };
    var exit = false;
    int pos = 0;

    foreach (var line in commands)
    {
        pos++;
        switch (line[0..3])
        {
            case "inp":
                Console.WriteLine($"{pos,3} {string.Concat(regs)}");
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

    Console.WriteLine($"{monad}={string.Concat(regs)}");

    return regs['z'];
}