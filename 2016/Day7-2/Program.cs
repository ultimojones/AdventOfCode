var lines = File.ReadAllLines("input.txt");

int count = 0;

foreach (var line in lines)
{
    var super = new List<string>();
    var hyper = new List<string>();

    for (int i = 0; i < line.Length; i++)
    {
        var nextHyper = line.IndexOf('[', i);
        if (nextHyper == -1)
        {
            super.Add(line[i..]);
            break;
        }
        else
        {
            if (nextHyper > i)
                super.Add(line[i..nextHyper]);
            var nextEndHyp = line.IndexOf("]", nextHyper);
            hyper.Add(line[(nextHyper + 1)..nextEndHyp]);
            i = nextEndHyp;
        }
    }

    var match = super.Any(s => Enumerable.Range(0, s.Length - 2)
        .Any(i => s[i] != s[i + 1] && s[i] == s[i + 2]
                && hyper.Any(h => h.Contains(new string(new[] { s[i + 1], s[i], s[i + 1] })))));

    if (match)
    {
        count++;

        Console.WriteLine(line);
        Console.WriteLine($" > {string.Join(",", super)}");
        Console.WriteLine($" > {string.Join(",", hyper)}");
    }

    //var abba = false;
    //var hypr = false;
    //var inhyp = false;
    //for (int i = 0; i < line.Length - 3; i++)
    //{
    //    if (line[i] == '[')
    //        inhyp = true;
    //    else if (line[i] == ']')
    //        inhyp = false;
    //    else
    //        if (line[i..(i + 4)].All(Char.IsAsciiLetterLower)
    //            && line[i] != line[i + 1] && line[i + 1] == line[i + 2] && line[i] == line[i + 3])
    //        {
    //            if (inhyp) { hypr = true; break; } else { abba = true; }
    //        }
    //}
    //if (abba && !hypr) count++;
}

Console.WriteLine(count);