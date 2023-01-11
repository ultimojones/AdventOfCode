var regs = new Dictionary<char, long> { { 'a', 0 }, { 'b', 0 }, { 'c', 1 }, { 'd', 0 } };

var instr = File.ReadAllLines("input.txt").Select(l =>
{
    var cmd = l[0..3];
    char? reg = cmd switch
    {
        "inc" or "dec" => l[4],
        "cpy" => l.Last(),
        _ => null
    };
    var srcParms = l[4..].Split(' ');
    int? srcVal = default!;
    char? srcReg = default!;
    if (cmd is "cpy" or "jnz")
    {
        if (int.TryParse(srcParms[0], out var val))
        {
            srcVal = val;
        }
        else
        {
            srcReg = srcParms[0][0];
        }
    }
    int jmpVal = cmd == "jnz" ? int.Parse(l[(l.LastIndexOf(' ') + 1)..]) : 0;
    return (Cmd: cmd, Reg: reg, SrcVal: srcVal, SrcReg: srcReg, JmpVal: jmpVal);
}).ToArray();

for (int ptr = 0; ptr < instr.Length; )
{
    var cmd = instr[ptr];
    switch (cmd.Cmd)
    {
        case "cpy":
            regs[cmd.Reg!.Value] = cmd.SrcReg is char reg ? regs[reg] : cmd.SrcVal!.Value;
            ptr++;
            break;
        case "inc":
            regs[cmd.Reg!.Value]++;
            ptr++;
            break;
        case "dec":
            regs[cmd.Reg!.Value]--;
            ptr++;
            break;
        case "jnz":
            if ((cmd.SrcReg is char regJnz && regs[regJnz] != 0) || (cmd.SrcVal is int valJnz && valJnz != 0))
                ptr += cmd.JmpVal;
            else
                ptr++;
            break;
        default:
            ptr++;
            break;
    }
    //Console.WriteLine(string.Join(" ", regs.Values));
}

Console.WriteLine(string.Join(" ", regs));
