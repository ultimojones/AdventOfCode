using System.Collections;
using System.Collections.Immutable;

//var start = new Dictionary<string, int>
//{
//    { "HG", 2 },
//    { "HM", 1 },
//    { "LG", 3 },
//    { "LM", 1 },
//}.ToImmutableDictionary();

var start = new Dictionary<string, int>
{
    { "OG", 1 },
    { "OM", 2 },
    { "TG", 1 },
    { "TM", 1 },
    { "PG", 1 },
    { "PM", 2 },
    { "RG", 1 },
    { "RM", 1 },
    { "CG", 1 },
    { "CM", 1 },
}.ToImmutableDictionary();

var layout = new Dictionary<string, int>(start);

ImmutableList<(int From, int To, string[] Devices)> bestActions = default!;
var currentActions = new List<(int From, int To, string[] Devices)>();
var currentLayouts = new HashSet<string>();

CheckActions();

void CheckActions()
{
    if (layout.Values.All(v => v == 4))
    {
        if (bestActions is null || currentActions.Count < bestActions.Count)
        {
            bestActions = currentActions.ToImmutableList();
            PrintActions(bestActions);
            Console.WriteLine($"({bestActions.Count}) {string.Join(", ", bestActions.Select(a => FormatAction(a)))}");
            Console.WriteLine();
        }
        return;
    }
    if (currentActions.Count >= 200 || bestActions is not null && currentActions.Count >= bestActions.Count)
    {
        PrintActions(currentActions);
        Console.WriteLine($"({currentActions.Count}) {string.Join(", ", currentActions.Select(a => FormatAction(a)))}");
        return;
    }

    var last = currentActions.LastOrDefault();
    var floor = int.Max(1, last.To);
    var newActions = new Queue<(int From, int To, string[] Devices)>();
    var localMicrochips = layout.Where(l => l.Key[1] == 'M' && l.Value == floor).OrderBy(d => d.Key).ToArray();
    var localGenerators = layout.Where(l => l.Key[1] == 'G' && l.Value == floor).OrderBy(d => d.Key).ToArray();
    var localDevices = layout.Where(l => l.Value == floor).OrderBy(d => d.Key).ToArray();

    // Move down 1 device if devices are below
    if (layout.Any(l => l.Value < floor))
    {
        for (int i = 0; i < localMicrochips.Length; i++)
        {
            newActions.Enqueue((floor, floor - 1, new[] { localMicrochips[i].Key }));
        }
        for (int i = 0; i < localMicrochips.Length; i++)
        {
            for (int j = 0; j < localGenerators.Length; j++)
            {
                newActions.Enqueue((floor, floor - 1, new[] { localMicrochips[i].Key, localGenerators[j].Key }));
            }
        }
        for (int i = 0; i < localGenerators.Length; i++)
        {
            newActions.Enqueue((floor, floor - 1, new[] { localGenerators[i].Key }));
        }
    }

    // Move items up
    if (floor < 4)
    {
        for (int i = 0; i < localGenerators.Length; i++)
        {
            for (int j = i + 1; j < localGenerators.Length; j++)
            {
                newActions.Enqueue((floor, floor + 1, new[] { localGenerators[i].Key, localGenerators[j].Key }));
            }
        }
        for (int i = 0; i < localGenerators.Length; i++)
        {
            for (int j = 0; j < localMicrochips.Length; j++)
            {
                newActions.Enqueue((floor, floor + 1, new[] { localGenerators[i].Key, localMicrochips[j].Key }));
            }
        }
        for (int i = 0; i < localMicrochips.Length; i++)
        {
            for (int j = i + 1; j < localMicrochips.Length; j++)
            {
                newActions.Enqueue((floor, floor + 1, new[] { localMicrochips[i].Key, localMicrochips[j].Key }));
            }
        }
        for (int i = 0; i < localGenerators.Length; i++)
        {
            newActions.Enqueue((floor, floor + 1, new[] { localGenerators[i].Key }));
        }
        for (int i = 0; i < localMicrochips.Length; i++)
        {
            newActions.Enqueue((floor, floor + 1, new[] { localMicrochips[i].Key }));
        }
    }

    int valid = 0;
    while (newActions.TryDequeue(out var action))
    {
        if (action.To == last.From && action.Devices.Length == last.Devices.Length && action.Devices.Except(last.Devices).Count() == 0)
        {
            // Don't undo last action.
            continue;
        }
        foreach (var device in action.Devices)
        {
            layout[device] = action.To;
        }
        var layoutValues = string.Concat(layout.OrderBy(l => l.Key).Select(l => l.Value));
        if (IsValid() && !currentLayouts.Contains(layoutValues))
        {
            valid++;
            currentActions.Add(action);
            currentLayouts.Add(layoutValues);
            CheckActions();
            currentActions.Remove(action);
            currentLayouts.Remove(layoutValues);
        }
        foreach (var device in action.Devices)
        {
            layout[device] = action.From;
        }
    }
    if (valid > 0)
    {
        PrintActions(currentActions);
        Console.WriteLine($"({currentActions.Count}) {string.Join(", ", currentActions.Select(a => FormatAction(a)))}");
        return;
    }
}

bool IsValid()
{
    return layout.Where(c => c.Key[1] == 'M').All(c =>
    {
        return !layout.Any(n => n.Value == c.Value && n.Key[1] == 'G') || layout[$"{c.Key[0]}G"] == c.Value;
    });
}

void PrintActions(IEnumerable<(int From, int To, string[] Devices)> actions)
{
    var actionLayout = new Dictionary<string, int>(start!);
    PrintLayout(actionLayout, 1);
    int i = 1;
    foreach (var action in actions)
    {
        Console.WriteLine($"({i++}) {FormatAction(action)}");
        foreach (var device in action.Devices)
        {
            actionLayout[device] = action.To;
        }
        PrintLayout(actionLayout, action.To);
    }
}

void PrintLayout(Dictionary<string, int> printLayout, int elevator)
{
    for (int i = 4; i > 0; i--)
    {
        Console.Write($"F{i} ");
        Console.Write(elevator == i ? $"E  " : ".  ");
        foreach (var device in printLayout.OrderBy(d => d.Key))
        {
            Console.Write(device.Value == i ? device.Key + ' ' : ".  ");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}
string FormatAction((int From, int To, string[] Devices) a)
{
    return $"{a.From}>{a.To}{(a.Devices.Length > 0 ? ":" + string.Join('.', a.Devices) : "")}";
}
