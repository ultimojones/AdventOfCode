var buckets = File.ReadLines("input.txt").Select((l,p) => new { p, v = int.Parse(l) }).ToDictionary(p => p.p, p => p.v);
const int target = 150;

var combos = GetCombos(Array.Empty<int>(), 0, buckets).ToArray();

foreach (var item in combos.Where(c => c.Count() == combos.Min(d => d.Count())))
{
    Console.WriteLine(string.Join(",", item));
}

Console.WriteLine(combos.Count(c => c.Count() == combos.Min(d => d.Count())));
    
IEnumerable<IEnumerable<int>> GetCombos(IEnumerable<int> combo, int sum, IEnumerable<KeyValuePair<int, int>> available)
{
    if (sum > target)
        yield break;

    var completed = new List<KeyValuePair<int, int>>();

    foreach (var item in available)
    {
        completed.Add(item);
        available.Except(completed);

        var newCombo = combo.Append(item.Value);
        if (item.Value + sum == target)
        {
            yield return newCombo;
        }
        else
        {
            foreach (var result in GetCombos(newCombo, sum + item.Value, available.Except(completed)))
            {
                yield return result;
            }
        }
    }
}