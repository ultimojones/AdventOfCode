using Terminal.Gui;

Application.Init();

try
{
    var win = new Window()
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    (int X, int Y) start = default!;
    (int X, int Y) end = default!;

    var grid = new Dictionary<(int X, int Y), (int Height, char Char, Label Label)>();
    var lines = File.ReadAllLines("input.txt");

    for (int y = 0; y < lines.Length; y++)
    {
        for (int x = 0; x < lines[y].Length; x++)
        {
            var height = lines[y][x] switch
            {
                'S' => 0,
                'E' => 26,
                _ => lines[y][x] - 'a'
            };
            var label = new Label(x, y, lines[y][x].ToString());
            win.Add(label);
            grid[(x, y)] = (height, lines[y][x], label);
            if (lines[y][x] == 'S') start = (x, y);
            else if (lines[y][x] == 'E') end = (x, y);
        }
    }
    var maxX = grid.Keys.Max(g => g.X);
    var maxY = grid.Keys.Max(g => g.Y);

    var app = Task.Run(() => Application.Run(win));

    Task.Run(async () =>
    {
        await Task.CompletedTask;

        var routes = new Queue<IEnumerable<(int X, int Y)>>();
        var done = new List<(int X, int Y)>();
        (int X, int Y)[] bestRoute = default!;
        routes.Enqueue(new[] { start });

        while (routes.TryDequeue(out var steps))
        {
            var curSteps = steps.ToArray();
            var last = curSteps.Last();
            if (last == end)
            {
                bestRoute = curSteps;
                break;
            }
            if (done.Contains(last))
                continue;
            done.Add(last);
            var next = new List<(int X, int Y)>();
            if (last.X > 0) { next.Add((last.X - 1, last.Y)); }
            if (last.X < maxX) { next.Add((last.X + 1, last.Y)); }
            if (last.Y > 0) { next.Add((last.X, last.Y - 1)); }
            if (last.Y < maxY) { next.Add((last.X, last.Y + 1)); }
            foreach (var node in next.Where(n => grid[n].Height - grid[last].Height <= 1))
            {
                routes.Enqueue(curSteps.Append(node));
            }
        }

        Console.WriteLine($"Best route: {bestRoute.Length - 1}");
        var color = new ColorScheme() { Normal = new Terminal.Gui.Attribute(Color.Black, Color.DarkGray) };
        foreach (var node in bestRoute)
        {
            var l = grid[node].Label;
            l.ColorScheme = color;
            win.EnsureFocus();
            await Task.Delay(100);
        }
    });

    app.Wait();
}
finally
{
    Application.Shutdown();
}
