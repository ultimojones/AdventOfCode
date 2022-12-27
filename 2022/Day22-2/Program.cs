var grid = new Dictionary<(int X, int Y), char>();
var links = new Dictionary<(int X, int Y, char dir), (int X, int Y, char dir)>();
var file = File.OpenText("input.txt");
string? line = file.ReadLine();
for (int y = 1; !string.IsNullOrEmpty(line); y++, line = file.ReadLine())
{
    for (int i = 0, x = 1; i < line.Length; i++, x++)
    {
        if (line[i] != ' ')
            grid[(x, y)] = line[i];
    }
}
var path = file.ReadLine();
var cmds = new List<(int? Move, char? Turn)>();
int NextDir(int i) => path.IndexOfAny(new[] { 'R', 'L' }, i);
for (int i = 0, j = NextDir(0); i < path.Length; i = j + 1, j = NextDir(i))
{
    if (j > 0)
    {
        cmds.Add((int.Parse(path[i..j]), null));
        cmds.Add((null, path[j]));
    }
    else
    {
        cmds.Add((int.Parse(path[i..]), null));
        break;
    }
}

for (int fromX = 1, toY = 51; fromX < 51; fromX++, toY++)
    links.Add((fromX, 100, '^'), (51, toY, '>'));
for (int fromX = 51, toY = 151; fromX < 101; fromX++, toY++)
    links.Add((fromX, 0, '^'), (1, toY, '>'));
for (int fromX = 101, toX = 1; fromX < 151; fromX++, toX++)
    links.Add((fromX, 0, '^'), (toX, 200, '^'));

for (int fromY = 1, toY = 150; fromY < 51; fromY++, toY--)
    links.Add((151, fromY, '>'), (100, toY, '<'));
for (int fromY = 51, toX = 101; fromY < 101; fromY++, toX++)
    links.Add((101, fromY, '>'), (toX, 50, '^'));
for (int fromY = 101, toY = 50; fromY < 151; fromY++, toY--)
    links.Add((101, fromY, '>'), (150, toY, '<'));
for (int fromY = 151, toX = 51; fromY < 201; fromY++, toX++)
    links.Add((51, fromY, '>'), (toX, 150, '^'));

for (int fromX = 1, toX = 101; fromX < 51; fromX++, toX++)
    links.Add((fromX, 201, 'v'), (toX, 1, 'v'));
for (int fromX = 51, toY = 151; fromX < 101; fromX++, toY++)
    links.Add((fromX, 151, 'v'), (50, toY, '<'));
for (int fromX = 101, toY = 51; fromX < 151; fromX++, toY++)
    links.Add((fromX, 51, 'v'), (100, toY, '<'));

for (int fromY = 1, toY = 150; fromY < 51; fromY++, toY--)
    links.Add((50, fromY, '<'), (1, toY, '>'));
for (int fromY = 51, toX = 1; fromY < 101; fromY++, toX++)
    links.Add((50, fromY, '<'), (toX, 101, 'v'));
for (int fromY = 101, toY = 50; fromY < 151; fromY++, toY--)
    links.Add((0, fromY, '<'), (51, toY, '>'));
for (int fromY = 151, toX = 51; fromY < 201; fromY++, toX++)
    links.Add((0, fromY, '<'), (toX, 1, 'v'));

var maxX = grid.Keys.Max(k => k.X);
var maxY = grid.Keys.Max(k => k.Y);

var pos = grid.Keys.Where(k => k.Y == 1).MinBy(k => k.X);
var dir = '>';
grid[pos] = dir;

try
{
    foreach (var cmd in cmds)
    {
        if (cmd.Move.HasValue)
        {
            for (int i = 0; i < cmd.Move.Value; i++)
            {
                (int X, int Y) next = dir switch
                {
                    '>' => (pos.X + 1, pos.Y),
                    'v' => (pos.X, pos.Y + 1),
                    '<' => (pos.X - 1, pos.Y),
                    '^' => (pos.X, pos.Y - 1),
                    _ => throw new NotImplementedException()
                };
                if (links.TryGetValue((next.X, next.Y, dir), out var dest))
                {
                    next = (dest.X, dest.Y);
                }
                if (grid[next] == '#') break;
                if (dest.dir > 0) dir = dest.dir;
                pos = next;
                grid[pos] = dir;
            }

            //if (dir == '>' || dir == '<')
            //{
            //    var axis = grid.Where(g => g.Key.Y == pos.Y);
            //    var min = axis.Min(g => g.Key.X);
            //    var max = axis.Max(g => g.Key.X);
            //    var range = max - min + 1;
            //    var start = pos;
            //    var offset = dir == '>' ? 1 : -1;
            //    for (int i = 0, x = start.X; i < cmd.Move.Value; i++)
            //    {
            //        x = x == min && offset == -1 ? max
            //          : x == max && offset == 1 ? min
            //          : x + offset;
            //        if (grid[(x, pos.Y)] == '#') break;

            //        pos.X = x;
            //        grid[pos] = dir;
            //    }
            //}
            //else if (dir == 'v' || dir == '^')
            //{
            //    var axis = grid.Where(g => g.Key.X == pos.X);
            //    var min = axis.Min(g => g.Key.Y);
            //    var max = axis.Max(g => g.Key.Y);
            //    var range = max - min + 1;
            //    var start = pos;
            //    var offset = dir == 'v' ? 1 : -1;
            //    for (int i = 0, y = start.Y; i < cmd.Move.Value; i++)
            //    {
            //        y = y == min && offset == -1 ? max
            //          : y == max && offset == 1 ? min
            //          : y + offset;
            //        if (grid[(pos.X, y)] == '#') break;

            //        pos.Y = y;
            //        grid[pos] = dir;
            //    }
            //}
        }
        else
        {
            grid[pos] = dir = (cmd.Turn, dir) switch
            {
                ('R', '>') => 'v',
                ('R', 'v') => '<',
                ('R', '<') => '^',
                ('R', '^') => '>',
                ('L', '>') => '^',
                ('L', 'v') => '>',
                ('L', '<') => 'v',
                ('L', '^') => '<',
                _ => throw new NotImplementedException()
            };
        }
    }

    PrintGrid();
    Console.WriteLine(1000 * pos.Y + 4 * pos.X + dir switch { '>' => 0, 'v' => 1, '<' => 2, '^' => 3 });

}
catch (Exception ex)
{
    PrintGrid();
    throw;
}


void PrintGrid()
{
    for (int y = 1; y < maxY; y++)
    {
        var chars = Enumerable.Repeat(' ', maxX).ToArray();
        foreach (var point in grid.Where(g => g.Key.Y == y))
        {
            chars[point.Key.X - 1] = point.Value;
        }
        Console.WriteLine(new string(chars));
    }
}

void PrintLinks()
{
    for (int y = 0; y < maxY + 2; y++)
    {
        var chars = Enumerable.Repeat(' ', maxX + 2).ToArray();
        foreach (var point in links.Where(g => g.Key.Y == y))
        {
            chars[point.Key.X] = point.Key.dir;
        }
        foreach (var point in links.Where(g => g.Value.Y == y))
        {
            chars[point.Value.X] = point.Value.dir;
        }
        Console.WriteLine(new string(chars));
    }
}