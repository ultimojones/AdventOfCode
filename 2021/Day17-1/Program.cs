// target area: x=20..30, y=-10..-5
// int xMin = 20, xMax = 30, yMin = -10, yMax = -5;
// target area: x=70..96, y=-179..-124
int xMin = 70, xMax = 96, yMin = -179, yMax = -124;

(int X, int Y)[] GetPath(int xVel, int yVel)
{
    var path = new List<(int X, int Y)>();
    int x = 0, y = 0;
    path.Add((x, y));

    while (true)
    {
        x += xVel;
        y += yVel;
        path.Add((x, y));
        if (x > xMax || y < yMin) return path.ToArray();
        if (x >= xMin && y <= yMax) return path.ToArray();
        if (xVel is not 0) { xVel -= int.Sign(xVel); }
        yVel--;
    }
}

var path = GetPath(12, 178);
Console.WriteLine(string.Concat(path));

var xGridMax = int.Max(path.Max(p => p.X), xMax);
var yGridMax = path.Max(p => p.Y);
var yGridMin = int.Min(path.Min(p => p.Y), yMin);

for (int y = yGridMax; y >= yGridMin; y--)
{
    Console.WriteLine(Enumerable.Range(0, xGridMax + 1).Select(x =>
    {
        if (path.Contains((x, y)))
            return '#';
        else if (xMin <= x && x <= xMax && yMin <= y && y <= yMax)
            return 'T';
        else
            return '.';
    }).ToArray());
}

Console.WriteLine();
Console.WriteLine(yGridMax);