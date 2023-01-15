using System.Collections;
using System.Collections.Immutable;
using static System.Net.Mime.MediaTypeNames;

var target = Enumerable.Repeat(new[] { 0, 1 }, 4).SelectMany(x => x).ToImmutableArray();

for (int i = 1; ; i ++)
{
    var test = GetSignal(i);

    if (test.SequenceEqual(target))
    {
        Console.WriteLine($"{i}: {string.Join(',', test)}");
        break;
    }

    if (i % 1000 == 0)
    {
        Console.WriteLine($"{i}: {string.Join(',', test)}");
    }
}


IEnumerable<int> GetSignal(int a)
{
    var count = 0;
    //checked
    {
        var regs = new Dictionary<char, int> { { 'a', a }, { 'b', 0 }, { 'c', 1 }, { 'd', 0 } };

        var instr = File.ReadAllLines("input.txt").Select(l => (Cmd: l[0..3], Args: l[4..])).ToArray();

        for (int ptr = 0; ptr < instr.Length;)
        {
            var cmd = instr[ptr];
            switch (cmd.Cmd)
            {
                case "cpy":
                    var cpyArgs = cmd.Args.Split(' ');
                    if (cpyArgs[1] is { Length: 1 } dst && regs.ContainsKey(dst[0]))
                    {
                        if (int.TryParse(cpyArgs[0], out var src))
                            regs[dst[0]] = src;
                        else
                            regs[dst[0]] = regs[cpyArgs[0][0]];
                    }
                    ptr++;
                    break;
                case "inc":
                    regs[cmd.Args[0]]++;
                    ptr++;
                    break;
                case "dec":
                    regs[cmd.Args[0]]--;
                    ptr++;
                    break;
                case "jnz":
                    var jnzArgs = cmd.Args.Split(' ');
                    var testVal = int.TryParse(jnzArgs[0], out var testParse) ? testParse : regs[jnzArgs[0][0]];
                    if (testVal != 0)
                    {
                        var jumpVal = int.TryParse(jnzArgs[1], out var jumpParse) ? jumpParse : (int)regs[jnzArgs[1][0]];
                        ptr += jumpVal;
                    }
                    else
                        ptr++;
                    break;
                case "out":
                    var outVal = regs[cmd.Args[0]];
                    yield return outVal;
                    if (++count >= 8) yield break;
                    ptr++;
                    break;
                default:
                    ptr++;
                    break;
            }
        }

        Console.WriteLine(string.Join(" ", regs));

    }
}