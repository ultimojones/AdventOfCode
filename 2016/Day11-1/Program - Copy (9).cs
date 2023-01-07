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
var currentActions = new Stack<(string Devices, int Floor)>();
var currentLayouts = new Dictionary<string, int[]>();
var checkedLayouts = new HashSet<(string, int)>();
long testTotal = 0;
long testInt = 0;
var timer = Stopwatch.StartNew();
var timerInt = Stopwatch.StartNew();

CheckActions();

void CheckActions()
{
    if (layout.Values.All(v => v == 4))
    {
        if (bestActions is null || currentActions.Count < bestActions.Count)
        {
            bestActions = currentActions.Reverse().ToList();
            PrintActions(bestActions);
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
        ((layout.Values.Sum() - 12) / (double)currentActions.Count 
            < (28 / (double)(bestActions is null ? 200 : bestActions.Count))))
    {
        //PrintActions(currentActions);
        //Console.WriteLine($"({currentActions.Count}) {string.Join(", ", currentActions)}");
        //Console.WriteLine("**** REGRESSION ****");
        return;
    }

    var floor = int.Max(1, currentActions.FirstOrDefault().Floor);
    var newActions = new Queue<(string Devices, int Floor)>();
    var localDevices = layout.Where(l => l.Value == floor).OrderBy(d => d.Key).Select(d => d.Key).ToArray();

    if (floor < 4)
    {
        for (int i = 0; i < localDevices.Length; i++)
        {
            for (int j = i + 1; j < localDevices.Length; j++)
            {
                if (!(localDevices[i][1] == 'M' && localDevices[j][1] == 'M'))
                    newActions.Enqueue((localDevices[i] + "+" + localDevices[j], floor + 1));
            }
        }
    }

    if (floor > 1)
    {
        //if (layout.Any(l => l.Value < floor))
        {
            for (int i = 0; i < localDevices.Length; i++)
            {
                newActions.Enqueue((localDevices[i], floor - 1));
            }
        }
        for (int i = 0; i < localDevices.Length; i++)
        {
            for (int j = i + 1; j < localDevices.Length; j++)
            {
                newActions.Enqueue((localDevices[i] + "+" + localDevices[j], floor - 1));
            }
        }
    }

    if (floor < 4)
    {
        for (int i = 0; i < localDevices.Length; i++)
        {
            for (int j = i + 1; j < localDevices.Length; j++)
            {
                if (localDevices[i][1] == 'M' && localDevices[j][1] == 'M')
                    newActions.Enqueue((localDevices[i] + "+" + localDevices[j], floor + 1));
            }
        }
        //if (layout.Any(l => l.Value > floor))
        {
            for (int i = 0; i < localDevices.Length; i++)
            {
                newActions.Enqueue((localDevices[i], floor + 1));
            }
        }
    }

    int valid = 0;
    while (newActions.TryDequeue(out var action))
    {
        if (currentActions.Count == 0) { Console.WriteLine(action); }
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
            if (!currentLayouts.ContainsKey(layoutKey) && !checkedLayouts.Contains((layoutKey, currentActions.Count + 1)))
            {
                valid++;
                currentActions.Push(action);
                currentLayouts.Add(layoutKey, new[] { 0 });
                checkedLayouts.Add((layoutKey, currentActions.Count + 1));
                CheckActions();
                currentActions.Pop();
                currentLayouts.Remove(layoutKey);
            }
        }
        foreach (var device in devices)
        {
            layout[device] = floor;
        }
        //if (timerInt.ElapsedMilliseconds > 10000)
        //{
        //    Console.WriteLine($"{testInt / timerInt.Elapsed.Seconds} tests/sec. Total tests: {testTotal} ElapsedTime: {timer.Elapsed}");
        //    testInt = 0;
        //    timerInt.Restart();
        //}
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
