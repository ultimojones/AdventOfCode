using System.Text.RegularExpressions;

var horizontal = 0;
var aim = 0;
var depth = 0;

foreach (var input in File.ReadLines("input.txt"))
{
    var match = Regex.Match(input, @"^(?<dir>\w+) (?<amt>\d+)$");
    var dir = match.Groups["dir"].Value;
    var amt = int.Parse(match.Groups["amt"].Value);
    switch (dir)
    {
        case "forward":
            horizontal += amt;
            depth += aim * amt;
            break;
        case "down":
            aim += amt;
            break;
        case "up":
            aim -= amt;
            break;
    }
}

Console.WriteLine($"{horizontal} * {depth} = {horizontal * depth}");
