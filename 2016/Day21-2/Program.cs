using System.Text.RegularExpressions;

var data = "fbgdceah".ToArray();

var swapPos = new Regex(@"^swap position (?<arg1>\w+) with position (?<arg2>\w+)$");
var swapLtr = new Regex(@"^swap letter (?<arg1>\w+) with letter (?<arg2>\w+)$");
var rotLR = new Regex(@"^rotate (?<dir>left|right) (?<num>\w+) steps?$");
var rotBsd = new Regex(@"^rotate based on position of letter (?<ltr>\w+)$");
var revPos = new Regex(@"^reverse positions (?<arg1>\w+) through (?<arg2>\w+)$");
var movPos = new Regex(@"^move position (?<arg1>\w+) to position (?<arg2>\w+)$");
Match m = null!;

foreach (var line in File.ReadAllLines("input.txt").Reverse())
{
    if ((m = swapPos.Match(line)).Success)
    {
        var arg1 = int.Parse(m.Groups["arg1"].Value);
        var arg2 = int.Parse(m.Groups["arg2"].Value);
        var chr1 = data[arg1];
        var chr2 = data[arg2];
        data[arg1] = chr2;
        data[arg2] = chr1;
    }
    else if ((m = swapLtr.Match(line)).Success)
    {
        var arg1 = char.Parse(m.Groups["arg1"].Value);
        var arg2 = char.Parse(m.Groups["arg2"].Value);
        var pos1 = Array.IndexOf(data, arg1);
        var pos2 = Array.IndexOf(data, arg2);
        data[pos1] = arg2;
        data[pos2] = arg1;
    }
    else if ((m = rotLR.Match(line)).Success)
    {
        var dir = m.Groups["dir"].Value;
        var num = char.Parse(m.Groups["num"].Value);
        var offset = data.Length + (num % data.Length) * (dir == "right" ? 1 : -1);
        data = data.Concat(data).Concat(data).Skip(offset).Take(data.Length).ToArray();
    }
    else if ((m = rotBsd.Match(line)).Success)
    {
        Console.WriteLine(line);
        Console.WriteLine(new string(data));
        var ltr = char.Parse(m.Groups["ltr"].Value);
        var idx = Array.IndexOf(data, ltr);
        var offset = idx switch
        {
            0 => 9,
            1 => 1,
            2 => 6,
            3 => 5,
            4 => 7,
            5 => 3,
            6 => 8,
            7 => 4,
            _ => throw new NotImplementedException()
        };
        data = data.Concat(data).Concat(data).Skip(offset).Take(data.Length).ToArray();
        Console.WriteLine(new string(data));
    }
    else if ((m = revPos.Match(line)).Success)
    {
        var arg1 = int.Parse(m.Groups["arg1"].Value);
        var arg2 = int.Parse(m.Groups["arg2"].Value);
        data = data.Take(arg1).Concat(data.Skip(arg1).Take(arg2 - arg1 + 1).Reverse()).Concat(data.Skip(arg2 + 1)).ToArray();
    }
    else if ((m = movPos.Match(line)).Success)
    {
        var arg2 = int.Parse(m.Groups["arg1"].Value);
        var arg1 = int.Parse(m.Groups["arg2"].Value);
        if (arg1 < arg2)
        {
            data = data
                .Take(arg1)
                .Concat(data.Skip(arg1 + 1).Take(arg2 - arg1))
                .Append(data[arg1])
                .Concat(data.Skip(arg2 + 1))
                .ToArray();
        }
        else
        {
            data = data
                .Take(arg2)
                .Append(data[arg1])
                .Concat(data.Skip(arg2).Take(arg1 - arg2))
                .Concat(data.Skip(arg1 + 1))
                .ToArray();
        }
    }
    else
    {
        throw new NotImplementedException();
    }
}

Console.WriteLine(new string(data));