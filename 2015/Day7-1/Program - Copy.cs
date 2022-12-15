using System.Text.RegularExpressions;

var wires = new Dictionary<string, ushort>();
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
    var wire = match.Groups["wire"].Value;
    var command = match.Groups["command"].Value;
    wires.TryGetValue(wire, out var signal);

    match = matchSignal.Match(command);
    if (match.Success)
    {
        var input = match.Groups["wire"].Success ? (wires.TryGetValue(match.Groups["wire"].Value, out var leftVal) ? leftVal : (ushort)0) : ushort.Parse(match.Groups["signal"].Value);
        wires[wire] = input;
        continue;
    }

    match = matchAnd.Match(command);
    if (match.Success)
    {
        var left = match.Groups["left"].Success ? (wires.TryGetValue(match.Groups["left"].Value, out var leftVal) ? leftVal : (ushort)0) : ushort.Parse(match.Groups["signal"].Value);
        var right = match.Groups["right"].Value;
        wires[wire] = (ushort)(left & (wires.TryGetValue(right, out var rightVal) ? rightVal : 0));
        continue;
    }

    match = matchOr.Match(command);
    if (match.Success)
    {
        var left = match.Groups["left"].Value;
        var right = match.Groups["right"].Value;
        wires[wire] = (ushort)((wires.TryGetValue(left, out var leftVal) ? leftVal : 0) | (wires.TryGetValue(right, out var rightVal) ? rightVal : 0));
        continue;
    }

    match = matchLshift.Match(command);
    if (match.Success)
    {
        var left = match.Groups["left"].Value;
        var right = ushort.Parse(match.Groups["right"].Value);
        wires[wire] = (ushort)((wires.TryGetValue(left, out var leftVal) ? leftVal : 0) << right);
        continue;
    }

    match = matchRshift.Match(command);
    if (match.Success)
    {
        var left = match.Groups["left"].Value;
        var right = ushort.Parse(match.Groups["right"].Value);
        wires[wire] = (ushort)((wires.TryGetValue(left, out var leftVal) ? leftVal : 0) >> right);
        continue;
    }

    match = matchNot.Match(command);
    if (match.Success)
    {
        var left = match.Groups["left"].Value;
        wires[wire] = (ushort)(~(wires.TryGetValue(left, out var leftVal) ? leftVal : 0));
        continue;
    }

    throw new InvalidOperationException();
}

foreach (var item in wires.OrderBy(w => w.Key))
{
    Console.WriteLine($"{item.Key}: {item.Value}");
}