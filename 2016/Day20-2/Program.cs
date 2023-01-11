var list = File.ReadLines("input.txt")
    .Select(l => { var p = l.Split('-'); return (From: long.Parse(p[0]), To: long.Parse(p[1])); }).OrderBy(l => l.From).ToArray();
var merge = new List<(long From, long To)>();

foreach (var i in list)
{
    var matches = merge.Where(w => (w.From <= i.From && i.From <= w.To + 1) || (w.From - 1 <= i.To && i.To <= w.To + 1)).ToArray();
    var newMerges = matches.Append(i);
    var newItem = (newMerges.Min(l => l.From), newMerges.Max(l => l.To));
    foreach (var item in matches) { merge.Remove(item); }
    merge.Add(newItem);
}

const long max = uint.MaxValue;
long count = merge[0].From;
int j = 0;
for (; j < merge.Count - 1; j++)
{
    count += merge[j + 1].From - merge[j].To - 1;
}
count += max - merge[j].To;

Console.WriteLine(count);
