using System.Xml.Linq;

// 2511 too low

// target area: x=20..30, y=-10..-5
// int xMin = 20, xMax = 30, yMin = -10, yMax = -5;
// target area: x=70..96, y=-179..-124
int xMin = 70, xMax = 96, yMin = -179, yMax = -124;

//PrintPath(GetPath(7, 9));
//return;

var succussXY = new List<(int X, int Y)>();

for (int xTest = 0; xTest <= xMax; xTest++)
{
    for (int yTest = yMin; yTest < int.Abs(yMin); yTest++)
    {
        int x = 0, y = 0, xVel = xTest, yVel = yTest;
        while (true)
        {
            x += xVel; y += yVel;
            if (x > xMax || y < yMin) break;
            if (x >= xMin && y <= yMax) { succussXY.Add((xTest, yTest)); break; }
            if (xVel > 0) xVel--;
            yVel--;
        }
    }
}

Console.WriteLine(succussXY.Count);
//return;

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

var stoppedX = new Dictionary<int, List<int>>();
var successX = new Dictionary<int, List<int>>();
var successY = new Dictionary<int, List<int>>();

for (int xVel = 1; xVel <= xMax; xVel++)
{
    int x = 0, turns = 0, xCurVel = xVel;
    while (xCurVel > 0 && x <= xMax)
    {
        x += xCurVel; xCurVel--; turns++;
        if (xMin <= x && x <= xMax)
        {
            if (xCurVel == 0)
                (stoppedX.TryGetValue(turns, out var xlist) ? xlist : stoppedX[turns] = new List<int>()).Add(xVel);
            else
                (successX.TryGetValue(turns, out var xlist) ? xlist : successX[turns] = new List<int>()).Add(xVel);
        }
    }
}

foreach (var item in stoppedX.OrderBy(s => s.Key))
{
    Console.WriteLine($"{item.Key}: {string.Join(',', item.Value)}");
}
Console.WriteLine();

foreach (var item in successX.OrderBy(s => s.Key))
{
    Console.WriteLine($"{item.Key}: {string.Join(',', item.Value)}");
}
Console.WriteLine();

for (int yVel = yMin; yVel < int.Abs(yMin); yVel++)
{
    int y = 0, turns = 0, yCurVel = yVel;
    while (y >= yMax)
    {
        y += yCurVel; yCurVel--; turns++;
        if (yMin <= y && y <= yMax)
            (successY.TryGetValue(turns, out var v) ? v : successY[turns] = new List<int>()).Add(yVel);
    }
}

foreach (var item in successY.OrderBy(s => s.Key))
{
    Console.WriteLine($"{item.Key}: {string.Join(',', item.Value)}");
}

var joined = successX.Join(successY, x => x.Key, y => y.Key, (x, y) => x.Value.SelectMany(xVel => y.Value.Select(yVel => (xVel, yVel)))).SelectMany(j => j).ToList();

var stoppedJoin = stoppedX.SelectMany(x => x.Value.SelectMany(xVel => successY.Where(y => y.Key >= x.Key).SelectMany(y => y.Value.Select(yVel => (xVel, yVel))))).Except(joined).ToList();


//Console.WriteLine(string.Concat(joined));
//Console.WriteLine(string.Concat(stoppedJoin));

Console.WriteLine();
Console.WriteLine(joined.Count + stoppedJoin.Count);

Console.WriteLine();
Console.WriteLine(string.Concat(succussXY.Except(joined.Concat(stoppedJoin)).ToArray()));