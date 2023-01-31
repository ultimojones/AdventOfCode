// target area: x=20..30, y=-10..-5
using System.Xml.Linq;

int xMin = 20, xMax = 30, yMin = -10, yMax = -5;
// target area: x=70..96, y=-179..-124
// int xMin = 70, xMax = 96, yMin = -179, yMax = -124;

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

void PrintPath((int X, int Y)[] path)
{
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
}

var successX = Enumerable.Range(1, xMin - 1).Where(xVel =>
{
    int x = 0, turns = 0;
    while (xVel > 0 && x <= xMax)
    {
        x += xVel;
        if (xMin <= x && x <= xMax) return true;
        xVel--;
    }
    return false;
});
Console.WriteLine(string.Join(',', successX));

var successY = Enumerable.Range(1, int.Abs(yMin) + int.Abs(yMax)).Select(i => i + yMax).Where(yVel =>
{
    int y = 0, turns = 0;
    while (y >= yMax)
    {
        y += yVel;
        if (yMin <= y && y <= yMax) return true;
        yVel--;
    }
    return false;
});
Console.WriteLine(string.Join(',', successY));
return;


//var maxYVel = int.Abs(yMin) - 1;
////var minXVel = Enumerable.Range(1, xMin).First(i => i * (i + 1) / 2 >= xMin);

//Console.WriteLine(minXVel);
//return;

int xVel = 0, xPos = 0;
var xVals = new List<int>();
while (xPos <= xMax)
{
    if (xPos >= xMin)
        xVals.Add(xVel);
    xVel++;
    xPos += xVel;
}
Console.WriteLine(string.Join(',', xVals));
return;

PrintPath(GetPath(6, 9));
return;

bool CheckPath((int X, int Y) Vel)
{
    int x = 0, y = 0;
    while (true)
    {
        x += Vel.X;
        y += Vel.Y;
        if (x > xMax || y < yMin) return false;
        if (x >= xMin && y <= yMax) return true;
        if (Vel.X is not 0) { Vel.X -= int.Sign(Vel.X); }
        Vel.Y--;
    }
}

var success = new List<(int X, int Y)>();
success.AddRange(Enumerable.Range(xMin, xMax - xMin + 1).SelectMany(x => Enumerable.Range(yMin, yMax - yMin + 1).Select(y => (x, y))));

Console.WriteLine(success.Count);
