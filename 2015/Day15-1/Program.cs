using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

var ingredients = File.ReadLines("input.txt").Select(l =>
{
    var r = Regex.Match(l, @"^(?<Name>\w+): capacity (?<Capacity>-?\d+), durability (?<Durability>-?\d+), flavor (?<Flavor>-?\d+), texture (?<Texture>-?\d+), calories (?<Calories>-?\d+)$");
    return (
        Name: r.Groups["Name"].Value,
        Capacity: int.Parse(r.Groups["Capacity"].Value),
        Durability: int.Parse(r.Groups["Durability"].Value),
        Flavor: int.Parse(r.Groups["Flavor"].Value),
        Texture: int.Parse(r.Groups["Texture"].Value),
        Calories: int.Parse(r.Groups["Calories"].Value)
    );
}).ToDictionary(i => i.Name);

const int max = 100;

var options = GetRecipes(Array.Empty<(string, int)>(), 0, ingredients.Keys.ToArray(), true);

var values = options.Select(item =>
{
    var capacity = int.Max(0, item.Sum(i => i.Amt * ingredients[i.Name].Capacity));
    var durability = int.Max(0, item.Sum(i => i.Amt * ingredients[i.Name].Durability));
    var flavor = int.Max(0, item.Sum(i => i.Amt * ingredients[i.Name].Flavor));
    var texture = int.Max(0, item.Sum(i => i.Amt * ingredients[i.Name].Texture));
    var total = capacity * durability * flavor * texture;
    return (Desc: string.Join(", ", item), Total: total);
});

Console.WriteLine(values.MaxBy(v => v.Total));


IEnumerable<(string Name, int Amt)[]> GetRecipes((string, int)[] recipe, int sum, string[] available, bool allowSingle)
{
    var nextavail = available.ToList();
    foreach (var ingredient in available)
    {
        nextavail.Remove(ingredient);
        if (nextavail.Count > 0)
        {
            foreach (var next in GetRecipes(recipe, sum, nextavail.ToArray(), false))
            {
                yield return next;
            }
        }
        for (int i = 1; i < max - sum; i++)
        {
            foreach (var next in GetRecipes(recipe.Append((ingredient, i)).ToArray(), sum + i, nextavail.ToArray(), false))
            {
                yield return next;
            }
        }
        if (sum > 0 || allowSingle)
            yield return recipe.Append((ingredient, max - sum)).ToArray();
    }
}