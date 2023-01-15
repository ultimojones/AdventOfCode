checked
{
    var regs = new Dictionary<char, long> { { 'a', 12 }, { 'b', 0 }, { 'c', 1 }, { 'd', 0 } };

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
                var testVal = long.TryParse(jnzArgs[0], out var testParse) ? testParse : regs[jnzArgs[0][0]];
                if (testVal != 0)
                {
                    var jumpVal = int.TryParse(jnzArgs[1], out var jumpParse) ? jumpParse : (int)regs[jnzArgs[1][0]];
                    ptr += jumpVal;
                }
                else
                    ptr++;
                break;
            case "tgl":
                var tglArgs = cmd.Args.Split(' ');
                var tgt = ptr + (int.TryParse(tglArgs[0], out var tglParse) ? tglParse : (int)regs[tglArgs[0][0]]);
                if (tgt >= 0 && tgt < instr.Length)
                {
                    var tgtVal = instr[tgt];
                    tgtVal.Cmd = tgtVal.Cmd switch
                    {
                        "inc" => "dec",
                        "dec" => "inc",
                        "tgl" => "inc",
                        "cpy" => "jnz",
                        "jnz" => "cpy",
                        _ => throw new NotImplementedException(),
                    };
                    instr[tgt] = tgtVal;
                }
                ptr++;
                break;
            default:
                ptr++;
                break;
        }
    }

    Console.WriteLine(string.Join(" ", regs));

}