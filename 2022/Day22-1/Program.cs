var grid = new Dictionary<(int X, int Y), char>();
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
            if (dir == '>' || dir == '<')
            {
                var axis = grid.Where(g => g.Key.Y == pos.Y);
                var min = axis.Min(g => g.Key.X);
                var max = axis.Max(g => g.Key.X);
                var range = max - min + 1;
                var start = pos;
                var offset = dir == '>' ? 1 : -1;
                for (int i = 0, x = start.X; i < cmd.Move.Value; i++)
                {
                    x = x == min && offset == -1 ? max
                      : x == max && offset == 1 ? min
                      : x + offset;
                    if (grid[(x, pos.Y)] == '#') break;

                    pos.X = x;
                    grid[pos] = dir;
                }
            }
            else if (dir == 'v' || dir == '^')
            {
                var axis = grid.Where(g => g.Key.X == pos.X);
                var min = axis.Min(g => g.Key.Y);
                var max = axis.Max(g => g.Key.Y);
                var range = max - min + 1;
                var start = pos;
                var offset = dir == 'v' ? 1 : -1;
                for (int i = 0, y = start.Y; i < cmd.Move.Value; i++)
                {
                    y = y == min && offset == -1 ? max
                      : y == max && offset == 1 ? min
                      : y + offset;
                    if (grid[(pos.X, y)] == '#') break;

                    pos.Y = y;
                    grid[pos] = dir;
                }
            }
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