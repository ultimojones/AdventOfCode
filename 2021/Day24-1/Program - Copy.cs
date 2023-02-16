var regs = new Dictionary<char, long> { { 'w', 0 }, { 'x', 0 }, { 'y', 0 }, { 'z', 0 } };
var input = new Queue<int>("13579246899999".Select(i => i - '0'));

foreach (var line in File.ReadLines("input.txt"))
{
    switch (line[0..3])
    {
        case "inp":
            regs[line[4]] = input.Dequeue();
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

    long GetArg2() => int.TryParse(line[6..], out int num) ? num : regs[line[6]];
}

Console.WriteLine(regs['z']);