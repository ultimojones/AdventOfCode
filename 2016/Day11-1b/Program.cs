using System.Collections.Immutable;

// (107) (OG+PG, 2), (OG+OM, 3), (OG+OM, 4), (OG, 3), (OG, 2), (OG+PG, 3), (OG+PG, 4), (OM, 3), (OM, 2), (OM+PM, 3), (OM, 4), (OG+PG, 3), (OG, 2), (OG, 1), (CG+CM, 2), (CG+CM, 3), (CG+PG, 2), (CG, 1), (CG+OG, 2), (CG+PG, 3), (CG+CM, 2), (CG+OG, 3), (CG+OG, 4), (CG, 3), (CG+PG, 4), (OM, 3), (OM, 2), (CM+OM, 3), (OM+PM, 2), (OM, 3), (CM+OM, 2), (CM, 3), (CM, 4), (CG+CM, 3), (CG, 4), (CG+OG, 3), (CG+CM, 4), (PG, 3), (OG+PG, 2), (OG+OM, 1), (RG+RM, 2), (PG+PM, 3), (PG+PM, 4), (CG+CM, 3), (CG, 2), (CG+RG, 3), (CG+CM, 4), (CG+PG, 3), (RG, 2), (RG+RM, 3), (CG+PG, 4), (CG+CM, 3), (CG+RG, 4), (PM, 3), (CM+PM, 2), (CM, 3), (RM, 2), (PM, 3), (CM, 2), (RM, 3), (PM+RM, 2), (CM, 3), (CM, 4), (CG+CM, 3), (CG, 4), (CG+PG, 3), (CG+CM, 4), (RG, 3), (PG, 4), (CG+CM, 3), (CM, 2), (CM+RM, 3), (CG+CM, 4), (PG, 3), (PG+RG, 4), (RG, 3), (RG+RM, 4), (CM, 3), (CM, 2), (CM+PM, 3), (CM+PM, 4), (CG+CM, 3), (CG, 2), (CG, 1), (OG+OM, 2), (OG, 1), (CG+OG, 2), (CG+OG, 3), (CG+OG, 4), (CG, 3), (CG+CM, 4), (OG, 3), (OG, 2), (OG+OM, 3), (OG, 2), (OG, 1), (OG+TG, 2), (OG+TG, 3), (OG+TG, 4), (CM, 3), (CM+OM, 4), (CM, 3), (CM, 2), (CM, 1), (CM+TM, 2), (CM+TM, 3), (CM+TM, 4)

// (65) (OG+PG, 2), (OG+OM, 3), (OG+OM, 4), (OG, 3), (OG, 2), (OG+PG, 3), (OG+PG, 4), (OM, 3), (OM, 2), (OM+PM, 3), (OM+PM, 4), (OG+OM, 3), (OG, 2), (OG, 1), (CG+CM, 2), (CG, 1), (CG+OG, 2), (CG+OG, 3), (CG+OG, 4), (OG, 3), (OG+OM, 4), (CG, 3), (CG, 2), (CG+CM, 3), (CG+CM, 4), (CM+OM, 3), (CM, 4), (OG, 3), (OG, 2), (OG, 1), (RG+RM, 2), (RG, 1), (OG+RG, 2), (OG+RG, 3), (OG+OM, 4), (PG+PM, 3), (PG+RG, 4), (CM, 3), (CM, 2), (CM+RM, 3), (CM+PM, 2), (CM, 3), (CM+RM, 2), (PM+RM, 3), (PM+RM, 4), (CG, 3), (CG, 2), (CG+CM, 3), (CG+CM, 4), (CM+OM, 3), (CM, 4), (OG, 3), (OG, 2), (OG, 1), (OG+TG, 2), (OG+TG, 3), (OM, 2), (OM, 1), (OM+TM, 2), (OM+TM, 3), (OG+TG, 4), (CM, 3), (CM+OM, 4), (CM, 3), (CM+TM, 4)

// (61) (OG+PG, 2), (OG+OM, 3), (OG+OM, 4), (OG, 3), (OG, 2), (OG+PG, 3), (OG+PG, 4), (OM, 3), (OM, 2), (OM+PM, 3), (OM+PM, 4), (OG+OM, 3), (OG, 2), (OG, 1), (CG+CM, 2), (CG, 1), (CG+OG, 2), (CG+OG, 3), (CG+OG, 4), (OG, 3), (OG+OM, 4), (CG, 3), (CG, 2), (CG+CM, 3), (CG+CM, 4), (CM+OM, 3), (CM, 4), (OG, 3), (OG, 2), (OG, 1), (RG+RM, 2), (RG, 1), (OG+RG, 2), (OG+RG, 3), (OG+OM, 4), (PG+PM, 3), (PG+RG, 4), (CM, 3), (CM, 2), (CM+RM, 3), (CM+PM, 2), (CM, 3), (CM+RM, 2), (PM+RM, 3), (PM+RM, 4), (CG, 3), (CG, 2), (CG+CM, 3), (CG, 2), (CG, 1), (CG+TG, 2), (CG+TG, 3), (CG+TG, 4), (CG, 3), (CG+CM, 4), (CM, 3), (CM, 2), (CM, 1), (CM+TM, 2), (CM+TM, 3), (CM+TM, 4)

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

var actions = new List<(string Devices, int Floor)>
{
    ("OG+PG", 2), 
    ("OG+OM", 4), 
    ("OG+OM", 4), 
    ("OG", 3), 
    ("OG", 2), 
    //("OG+PG", 3), 
    //("OG+PG", 4), 
    //("PG", 3), 
    //("PG", 2), 
    //("PG+PM", 3), 
    //("PG", 2), 
    //("PG", 1),
    //("CG+CM", 2),
    //("CG", 1),
    //("CG+PG", 2),
    //("CG+PG", 3),
    //("CG+PG", 4),
    //("PG", 3),
    //("PG+PM", 4),
    //("CG", 3),
    //("CG", 2),
    //("CG+CM", 3),
    //("CG", 2),
    //("CG", 1),
    //("RG+RM", 2),
    //("RG", 1),
    //("CG+RG", 2),
    //("CG+RG", 3),
    //("CG+RG", 4),
    //("CG", 3),
    //("CG+CM", 4),
    //("CM", 3),
    //("CM", 2),
    //("CM+RM", 3),
    //("CM+RM", 4),
    //("RG+RM", 3),
    //("RG", 2),
    //("RG", 1),
    //("TG+TM", 2),
    //("TG", 1),
    //("RG+TG", 2),
    //("RG+TG", 3),
    //("RG+TG", 4),
    //("RG", 3),
    //("RG+RM", 4),
    //("TG", 3),
    //("TG", 2),
    //("TG+TM", 3),
    //("TG+TM", 4),
};

PrintActions(actions);

void PrintActions(IEnumerable<(string Devices, int Floor)> actions)
{
    var actionLayout = new Dictionary<string, int>(start!);
    PrintLayout(actionLayout, 1);
    int i = 1;
    var from = 1;
    foreach (var action in actions)
    {
        Console.WriteLine($"({i++}) {action}");
        foreach (var device in action.Devices.Split('+'))
        {
            if (actionLayout[device] != from)
            {
                Console.WriteLine("*** FAILED ****");
                return;
            }
            actionLayout[device] = action.Floor;
        }
        PrintLayout(actionLayout, action.Floor);
        if (!ValidateLayout(actionLayout))
        {
            Console.WriteLine("*** FAILED ****");
            return;
        }
        from = action.Floor;
    }
}

static bool ValidateLayout(Dictionary<string, int> layout) => 
    layout.Where(l => l.Key[1] == 'M').All(m => layout[m.Key[0] + "G"] == m.Value || layout.Count(g => g.Key[1] == 'G' && g.Value == m.Value) == 0);

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


/*
 
var actions = new List<(string Devices, int Floor)>
{
    ("OG+PG", 2), 
    ("OG+OM", 3), 
    ("OG+OM", 4), 
    ("OG", 3), 
    ("OG", 2), 
    ("OG+PG", 3), 
    ("OG+PG", 4), 
    ("PG", 3), 
    ("PG", 2), 
    ("PG+PM", 3), 
    ("PG", 2), 
    ("PG", 1),
    ("CG+CM", 2),
    ("CG", 1),
    ("CG+PG", 2),
    ("CG+PG", 3),
    ("CG+PG", 4),
    ("PG", 3),
    ("PG+PM", 4),
    ("CG", 3),
    ("CG", 2),
    ("CG+CM", 3),
    ("CG", 2),
    ("CG", 1),
    ("RG+RM", 2),
    ("RG", 1),
    ("CG+RG", 2),
    ("CG+RG", 3),
    ("CG+RG", 4),
    ("CG", 3),
    ("CG+CM", 4),
    ("CM", 3),
    ("CM", 2),
    ("CM+RM", 3),
    ("CM+RM", 4),
    ("RG+RM", 3),
    ("RG", 2),
    ("RG", 1),
    ("TG+TM", 2),
    ("TG", 1),
    ("RG+TG", 2),
    ("RG+TG", 3),
    ("RG+TG", 4),
    ("RG", 3),
    ("RG+RM", 4),
    ("TG", 3),
    ("TG", 2),
    ("TG+TM", 3),
    ("TG+TM", 4),
};

 
 var actions = new List<(string Devices, int Floor)>
{
    ("OG+PG", 2), 
    ("OG+OM", 3), 
    ("OG+OM", 4), 
    ("OG", 3), 
    ("OG", 2), 
    ("OG+PG", 3), 
    ("OG+PG", 4), 
    ("OM", 3), 
    ("OM", 2), 
    ("OM+PM", 3), 
    ("OM+PM", 4), 
    ("OG+OM", 3), 
    ("OG", 2), 
    ("OG", 1), 
    ("CG+CM", 2), 
    ("CG", 1), 
    ("CG+OG", 2), 
    ("CG+OG", 3), 
    ("CG+OG", 4),
    ("OG", 3), 
    ("OG+OM", 4), 
    ("CG", 3), 
    ("CG", 2),
    ("CG+CM", 3),
    ("CG", 2),
    ("CG", 1),
    ("RG+RM", 2),
    ("RG", 1),
    ("CG+RG", 2),
    ("CG+RG", 3),
    ("CG+RG", 4),
    ("CG", 3),
    ("CG+CM", 4),
    ("CM", 3),
    ("CM", 2),
    ("CM+RM", 3),
    ("CM+RM", 4),
    ("RG+RM", 3),
    ("RG", 2),
    ("RG", 1),
    ("TG+TM", 2),
    ("TG", 1),
    ("RG+TG", 2),
    ("RG+TG", 3),
    ("RG+TG", 4),
    ("RG", 3),
    ("RG+RM", 4),
    ("TG", 3),
    ("TG", 2),
    ("TG+TM", 3),
    ("TG+TM", 4),
};
*/