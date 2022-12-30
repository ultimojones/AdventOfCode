using System.Collections.Concurrent;
using System.Text.RegularExpressions;

var number = "1113122113";

var rx = new Regex(@"(?<digit>(\d)\1*)");

for (int i = 0; i < 50; i++)
{
    var matches = rx.Matches(number);
    number = string.Concat(matches.Select(m => m.Length.ToString() + m.ValueSpan[0]));
    Console.WriteLine(number.Length);
}
