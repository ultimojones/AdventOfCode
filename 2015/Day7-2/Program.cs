using System.Text.RegularExpressions;

var ops = new List<Operation>();
var matchWire = new Regex(@"^(?<command>.*) -> (?<wire>[a-z]+)$");
var matchSignal = new Regex(@"^((?<signal>\d+)|(?<wire>[a-z]+))$");
var matchAnd = new Regex(@"^((?<signal>\d+)|(?<left>[a-z]+)) AND (?<right>[a-z]+)$");
var matchOr = new Regex(@"^(?<left>[a-z]+) OR (?<right>[a-z]+)$");
var matchLshift = new Regex(@"^(?<left>[a-z]+) LSHIFT (?<right>\d+)$");
var matchRshift = new Regex(@"^(?<left>[a-z]+) RSHIFT (?<right>\d+)$");
var matchNot = new Regex(@"^NOT (?<left>[a-z]+)$");

foreach (var line in File.ReadLines("input.txt"))
{
    var match = matchWire.Match(line);
    var output = match.Groups["wire"].Value;
    var command = match.Groups["command"].Value;
    var op = new Operation { OutWire = output };

    match = matchSignal.Match(command);
    if (match.Success)
    {
        op.OpType = "SIG";
        if (match.Groups["signal"].Success)
            op.OutSignal = op.SigL = ushort.Parse(match.Groups["signal"].Value);
        else
            op.WireL = match.Groups["wire"].Value;
        ops.Add(op);
        continue;
    }

    match = matchAnd.Match(command);
    if (match.Success)
    {
        op.OpType = "AND";
        if (match.Groups["signal"].Success)
            op.SigL = ushort.Parse(match.Groups["signal"].Value);
        else
            op.WireL = match.Groups["left"].Value;
        op.WireR = match.Groups["right"].Value;
        ops.Add(op);
        continue;
    }

    match = matchOr.Match(command);
    if (match.Success)
    {
        op.OpType = "OR";
        op.WireL = match.Groups["left"].Value;
        op.WireR = match.Groups["right"].Value;
        ops.Add(op);
        continue;
    }

    match = matchLshift.Match(command);
    if (match.Success)
    {
        op.OpType = "LSHIFT";
        op.WireL = match.Groups["left"].Value;
        op.SigR = ushort.Parse(match.Groups["right"].Value);
        ops.Add(op);
        continue;
    }

    match = matchRshift.Match(command);
    if (match.Success)
    {
        op.OpType = "RSHIFT";
        op.WireL = match.Groups["left"].Value;
        op.SigR = ushort.Parse(match.Groups["right"].Value);
        ops.Add(op);
        continue;
    }

    match = matchNot.Match(command);
    if (match.Success)
    {
        op.OpType = "NOT";
        op.WireL = match.Groups["left"].Value;
        ops.Add(op);
        continue;
    }

    throw new InvalidOperationException();
}

while (ops.Any(o => !o.OutSignal.HasValue))
{
    foreach (var op in ops.Where(o => !o.OutSignal.HasValue))
    {
        if (!op.SigL.HasValue)
            op.SigL = ops.First(o => o.OutWire == op.WireL).OutSignal;
        if ((op.OpType == "AND" || op.OpType == "OR") && !op.SigR.HasValue)
            op.SigR = ops.First(o => o.OutWire == op.WireR).OutSignal;

        switch (op.OpType)
        {
            case "SIG":
                if (op.SigL.HasValue)
                    op.OutSignal = op.SigL;
                break;
            case "AND":
                if (op.SigL.HasValue && op.SigR.HasValue)
                    op.OutSignal = (ushort)(op.SigL & op.SigR);
                break;
            case "OR":
                if (op.SigL.HasValue && op.SigR.HasValue)
                    op.OutSignal = (ushort)(op.SigL | op.SigR);
                break;
            case "LSHIFT":
                if (op.SigL.HasValue && op.SigR.HasValue)
                    op.OutSignal = (ushort)(op.SigL << op.SigR);
                break;
            case "RSHIFT":
                if (op.SigL.HasValue && op.SigR.HasValue)
                    op.OutSignal = (ushort)(op.SigL >> op.SigR);
                break;
            case "NOT":
                if (op.SigL.HasValue)
                    op.OutSignal = (ushort)(~op.SigL);
                break;
        }
    }
}



foreach (var item in ops.OrderBy(w => w.OutWire))
{
    Console.WriteLine($"{item.OutWire}: {item.OutSignal}");
}

// a = 16076

class Operation
{
    public ushort? SigL;
    public string? WireL;
    public ushort? SigR;
    public string? WireR;
    public string? OpType;
    public string OutWire;
    public ushort? OutSignal;
}
