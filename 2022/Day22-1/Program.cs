var grid = new Dictionary<(int X, int Y), char>();
var file = File.OpenText("sample.txt");
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

foreach (var cmd in cmds.Take(1))
{
    if (cmd.Move.HasValue)
    {
        if (dir == '>')
        {
            var axis = grid.Where(g => g.Key.Y == pos.Y);
            var min = axis.Min(g => g.Key.X);
            var max = axis.Max(g => g.Key.X);
            var range = max - min + 1;
            var start = pos;
            for (int i = 1, x = start.X; i < cmd.Move.Value ; i++)
            {
                x = (x + 1 - min) % range + min;
                if (grid[(x, pos.Y)] == '#') break;

                pos.X = x;
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