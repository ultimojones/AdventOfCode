var cmds = File.ReadAllLines("input.txt");

int ptr = 0;

var reg = new Dictionary<char, int> { { 'a', 1 }, { 'b', 0 } };

while (ptr < cmds.Length)
{
    Console.Write($"[{ptr,3}] {cmds[ptr],-12} => ");
    switch (cmds[ptr][0..3])
	{
        case "hlf":
            reg[cmds[ptr][4]] /= 2;
            ptr++;
            break;
        case "tpl":
            reg[cmds[ptr][4]] *= 3;
            ptr++;
            break;
        case "inc":
            reg[cmds[ptr][4]]++;
            ptr++;
            break;
        case "jmp":
            ptr += int.Parse(cmds[ptr][4..]);
            break;
        case "jie":
            if (int.IsEvenInteger(reg[cmds[ptr][4]]))
                ptr += int.Parse(cmds[ptr][7..]);
            else
                ptr++;
            break;
        case "jio":
            if (reg[cmds[ptr][4]] == 1)
                ptr += int.Parse(cmds[ptr][7..]);
            else
                ptr++;
            break;
		default:
			throw new InvalidOperationException();
	}
    Console.WriteLine($"{string.Join(",", reg)}");
}