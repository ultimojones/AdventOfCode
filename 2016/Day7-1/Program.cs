using System.Text.RegularExpressions;

//var abba = new Regex(@"([a-z])(?!\1)([a-z])\2\1");
//var hypr = new Regex(@"\[.*?([a-z])(?!\1)([a-z])\2\1.*?\]");

//var count = File.ReadAllLines("input.txt").Count(l =>
//{
//    //Enumerable.Range(0, l.Length - 3).Any();
//    return abba.IsMatch(l) && !hypr.IsMatch(l);
//});

//Console.WriteLine(count);

var lines = File.ReadAllLines("input.txt");

int count = 0;

foreach (var line in lines)
{
    var abba = false;
    var hypr = false;
    var inhyp = false;
    for (int i = 0; i < line.Length - 3; i++)
    {
        if (line[i] == '[')
            inhyp = true;
        else if (line[i] == ']')
            inhyp = false;
        else
            if (line[i..(i + 4)].All(Char.IsAsciiLetterLower)
                && line[i] != line[i + 1] && line[i + 1] == line[i + 2] && line[i] == line[i + 3])
            {
                if (inhyp) { hypr = true; break; } else { abba = true; }
            }
    }
    if (abba && !hypr) count++;
}

Console.WriteLine(count);