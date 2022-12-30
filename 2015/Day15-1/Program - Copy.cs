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

var options = SelectIngredients(Enumerable.Empty<(string, int)>(), 0, ingredients.Select(i => i.Name));

foreach (var item in options)
{
    Console.WriteLine(String.Join(", ", item.Select(o => o.ToString())));
}

Console.WriteLine();

IEnumerable<IEnumerable<(string Name, int Amt)>> SelectIngredients(IEnumerable<(string Name, int Amt)> recipe, int total, IEnumerable<string> available)
{
    var rcp = recipe.ToArray();
    var avl = available.ToArray();

    //if (total == 10)
    //    yield return recipe;

    if (!available.Any())
        yield break;

    foreach (var ingredient in available)
    {
        foreach (var i in Enumerable.Range(0, 11 - total))
        {
            if (total + i == 10)
            {
                rcp = recipe.Append((ingredient, i)).ToArray();
                yield return rcp;
            }
            else
            {
                foreach (var selection in SelectIngredients(i > 0 ? recipe.Append((ingredient, i)) : recipe, total + i, available.Except(new[] { ingredient })))
                {
                    rcp = selection.ToArray();
                    yield return rcp;
                }
            }
        }
    }
}
