using System.Collections.Generic;
using System.Text.RegularExpressions;

var ingredients = File.ReadLines("sample.txt").Select(l =>
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
}).ToList();

const int max = 10;

var options = GetRecipes(Array.Empty<(string, int)>(), 0, ingredients.Select(i => i.Name).ToArray());

foreach (var item in options)
{
    Console.WriteLine(string.Join(", ", item));
}

IEnumerable<(string, int)[]> GetRecipes((string, int)[] recipe, int sum, string[] available)
{
    foreach (var ingredient in available)
    {
        var nextavail = available.Except(new[] { ingredient }).ToArray();
        if (nextavail.Length > 0)
        {
            foreach (var next in GetRecipes(recipe, sum, nextavail))
            {
                yield return next;
            }
        }
        for (int i = 1; i < max - sum; i++)
        {
            foreach (var next in GetRecipes(recipe.Append((ingredient, i)).ToArray(), sum + i, nextavail))
            {
                yield return next;
            }
        }
        yield return recipe.Append((ingredient, max - sum)).ToArray();
    }
}