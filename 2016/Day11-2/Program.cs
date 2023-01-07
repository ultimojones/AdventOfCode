using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

//var start = new Dictionary<string, int>
//{
//    { "HG", 2 },
//    { "HM", 1 },
//    { "LG", 3 },
//    { "LM", 1 },
//}.ToImmutableDictionary();

var start = new Dictionary<string, int>
{
    { "CG", 1 },
    { "CM", 1 },
    { "DG", 1 },
    { "DM", 1 },
    { "EG", 1 },
    { "EM", 1 },
    { "OG", 1 },
    { "OM", 2 },
    { "PG", 1 },
    { "PM", 2 },
    { "RG", 1 },
    { "RM", 1 },
    { "TG", 1 },
    { "TM", 1 },
}.ToImmutableDictionary();

var layout = new Dictionary<string, int>(start);

List<(string Devices, int Floor)> bestActions = default!;
var currentActions = new LinkedList<(string Devices, int Floor)>();
var currentLayouts = new HashSet<string>();
var checkedLayouts = new HashSet<(string, int)>();
long testTotal = 0;
long testInt = 0;
var timer = Stopwatch.StartNew();
var timerInt = Stopwatch.StartNew();

CheckActions();
PrintActions(bestActions);
Console.WriteLine($"({bestActions.Count}) {string.Join(", ", bestActions)}");

void CheckActions()
{
    if (layout.Values.All(v => v == 4))
    {
        if (bestActions is null || currentActions.Count < bestActions.Count)
        {
            bestActions = currentActions.ToList();
            //PrintActions(bestActions);
            Console.WriteLine($"({bestActions.Count}) {string.Join(", ", bestActions)}");
            Console.WriteLine();
        }
        return;
    }
    if (currentActions.Count >= 1000 || bestActions is not null && currentActions.Count >= bestActions.Count)
    {
        return;
    }
    if (currentActions.Count % 20 == 1 &&
        ((layout.Values.Sum() - 16) / (double)currentActions.Count
            < (40 / (double)(bestActions is null ? 400 : bestActions.Count))))
    {
        return;
    }

    var floor = int.Max(1, currentActions.LastOrDefault().Floor);
    var newActions = new Queue<(string Devices, int Floor)>();
    var localDevices = layout.Where(l => l.Value == floor).OrderBy(d => d.Key).Select(d => d.Key).ToArray();
    var localGenerators = localDevices.Where(d => d[1] == 'G').ToArray();
    var localMicrochips = localDevices.Where(d => d[1] == 'M').ToArray();

    void EnqueueNew((string Devices, int Floor) action) { if (!newActions.Contains(action)) { newActions.Enqueue(action); } }

    if (floor < 4)
    {
        foreach (var element in localDevices.GroupBy(d => d[0]).Where(e => e.Count() == 2))
        {
            EnqueueNew(($"{element.Key}G+{element.Key}M", floor + 1));
        }
        for (int i = 0; i < localGenerators.Length; i++)
        {
            for (int j = i + 1; j < localGenerators.Length; j++)
            {
                EnqueueNew((localGenerators[i] + "+" + localGenerators[j], floor + 1));
            }
        }
        for (int i = 0; i < localDevices.Length; i++)
        {
            EnqueueNew((localDevices[i], floor + 1));
        }
    }

    if (floor > 1)
    {
        for (int i = 0; i < localDevices.Length; i++)
        {
            EnqueueNew((localDevices[i], floor - 1));
        }
        foreach (var element in localDevices.GroupBy(d => d[0]).Where(e => e.Count() == 2))
        {
            EnqueueNew(($"{element.Key}G+{element.Key}M", floor - 1));
        }
        for (int i = 0; i < localGenerators.Length; i++)
        {
            for (int j = i + 1; j < localGenerators.Length; j++)
            {
                EnqueueNew((localGenerators[i] + "+" + localGenerators[j], floor - 1));
            }
        }
        for (int i = 0; i < localMicrochips.Length; i++)
        {
            for (int j = i + 1; j < localMicrochips.Length; j++)
            {
                EnqueueNew((localMicrochips[i] + "+" + localMicrochips[j], floor - 1));
            }
        }
    }

    if (floor < 4)
    {
        for (int i = 0; i < localMicrochips.Length; i++)
        {
            for (int j = i + 1; j < localMicrochips.Length; j++)
            {
                EnqueueNew((localMicrochips[i] + "+" + localMicrochips[j], floor + 1));
            }
        }
    }

    int valid = 0;
    while (newActions.TryDequeue(out var action))
    {
        //if (currentActions.Count == 0) { Console.WriteLine(action); }
        testTotal++; testInt++;
        var devices = action.Devices.Split("+");
        foreach (var device in devices)
        {
            layout[device] = action.Floor;
        }
        if (ValidateLayout())
        {
            var layoutKey = action.Floor.ToString() + string.Concat(Enumerable.Range(0, layout.Count / 2)
                .Select(i => string.Concat(layout.ElementAt(i * 2).Value, layout.ElementAt(i * 2 + 1).Value)).OrderDescending());
            if (!currentLayouts.Contains(layoutKey) && !checkedLayouts.Contains((layoutKey, currentActions.Count + 1)))
            {
                valid++;
                currentActions.AddLast(action);
                currentLayouts.Add(layoutKey);
                checkedLayouts.Add((layoutKey, currentActions.Count));
                CheckActions();
                currentActions.RemoveLast();
                currentLayouts.Remove(layoutKey);
            }
        }
        foreach (var device in devices)
        {
            layout[device] = floor;
        }
    }
    if (valid == 0)
    {
        return;
    }

    bool ValidateLayout() =>
        layout.Where(l => l.Key[1] == 'M').All(m => layout[m.Key[0] + "G"] == m.Value || layout.Count(g => g.Key[1] == 'G' && g.Value == m.Value) == 0);
}

void PrintActions(IEnumerable<(string Devices, int Floor)> actions)
{
    var actionLayout = new Dictionary<string, int>(start!);
    PrintLayout(actionLayout, 1);
    int i = 1;
    foreach (var action in actions)
    {
        Console.WriteLine($"({i++}) {action}");
        foreach (var device in action.Devices.Split('+'))
        {
            actionLayout[device] = action.Floor;
        }
        PrintLayout(actionLayout, action.Floor);
        if (!ValidateLayout(actionLayout))
        {
            Console.WriteLine("*** FAILED ****");
            return;
        }
    }
}

static bool ValidateLayout(Dictionary<string, int> checkinglayout) => 
    checkinglayout.Where(l => l.Key[1] == 'M').All(m => checkinglayout[m.Key[0] + "G"] == m.Value || checkinglayout.Count(g => g.Key[1] == 'G' && g.Value == m.Value) == 0);

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
