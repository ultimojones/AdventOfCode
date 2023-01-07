var lobby = new List<(string Type, string Element, int Floor)>
{
    ( " ", "E", 1 ),
    ( "G", "H", 2 ),
    ( "M", "H", 1 ),
    ( "G", "L", 3 ),
    ( "M", "L", 1 )
};

for (int i = 4; i > 0; i--)
{
    Console.Write($"F{i} ");
    foreach (var item in lobby)
    {
        Console.Write(item.Floor == i ? item.Element + item.Type + ' ' : ".  ");
    }
    Console.WriteLine();
}

Console.WriteLine(IsValid(lobby));

while (true)
{
    var floor = lobby.First(i => i.Element == "E").Floor;

    var singles = new List<(string Type, string Element, int Floor)>();
    var pairs = new List<(string Type, string Element, int Floor)[]>();

    var items = lobby.Where(i => i.Floor == floor && i.Element != "E");
    foreach (var item in items)
    {
        singles.Add(item);
        foreach (var second in items.Except(singles))
        {
            pairs.Add(new[] { item, second });
        }
    }

    foreach (var item in singles.Select(s => new[] { s }).Concat(pairs))
    {
        foreach (var i in item)
        {
            lobby.Remove(i);
        }
        if (floor < 4)
        {
            foreach (var i in item)
            {
                lobby.Add((i.Type, i.Element, floor + 1));
            }

            if (IsValid())
            {
                DoOptions();
            }
        }

    }
}

void DoOptions()
{
    if (!IsValid())
    {
        return;
    }
}


bool IsValid()
{
    return lobby.Where(c => c.Type == "M").All(c =>
    {
        return !lobby.Any(n => n.Floor == c.Floor && n.Type == "G") || lobby.Contains(("G", c.Element, c.Floor));
    });
}