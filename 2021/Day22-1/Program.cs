using System.Text.RegularExpressions;

long cubes = 0;
var onAreas = new List<Box>();
var workingArea = new Box(-50, 50, -50, 50, -50, 50);

foreach (var line in File.ReadLines("sample.txt"))
{
    var r = Regex.Match(line, @"^(?<action>on|off) x=(?<x1>-?\d+)\.\.(?<x2>-?\d+),y=(?<y1>-?\d+)\.\.(?<y2>-?\d+),z=(?<z1>-?\d+)\.\.(?<z2>-?\d+)$");
    var action = r.Groups["action"].Value;
    var x1 = int.Parse(r.Groups["x1"].Value);
    var x2 = int.Parse(r.Groups["x2"].Value);
    var y1 = int.Parse(r.Groups["y1"].Value);
    var y2 = int.Parse(r.Groups["y2"].Value);
    var z1 = int.Parse(r.Groups["z1"].Value);
    var z2 = int.Parse(r.Groups["z2"].Value);
    var area = new Box(x1, x2, y1, y2, z1, z2);
    if (!area.Intersects(workingArea, true)) continue;
    Console.WriteLine(new { action, area, area.Size });

    if (action == "on")
    {
        var newAreas = new List<Box> { area };
        foreach (var onArea in onAreas)
        {
            var nextAreas = new List<Box>();
            foreach (var newArea in newAreas)
            {
                nextAreas.AddRange(newArea.Except(onArea));
            }
            newAreas = nextAreas;
        }
        onAreas.AddRange(newAreas);
        cubes += newAreas.Sum(a => a.Size);
        Console.WriteLine(new { cubes });
    }
    else
    {
        var deleteAreas = new List<Box>();
        var newAreas = new List<Box>();
        foreach (var onArea in onAreas)
        {
            if (onArea.Intersect(area) is Box offArea)
            { 
                var excepts = onArea.Except(area);
                deleteAreas.Add(onArea);
                newAreas.AddRange(excepts);
                cubes -= offArea.Size;
            }
        }
        deleteAreas.ForEach(a => onAreas.Remove(a));
        onAreas.AddRange(newAreas);
        Console.WriteLine(new { cubes });
    }
}

struct Box
{
    public int X1;
    public int X2;
    public int Y1;
    public int Y2;
    public int Z1;
    public int Z2;
    public long Size => (X2 - X1 + 1) * (Y2 - Y1 + 1) * (Z2 - Z1 + 1);

    public Box(int x1, int x2, int y1, int y2, int z1, int z2)
    {
        if (x1 > x2 || y1 > y2 || z1 > z2) throw new ArgumentException("Axes values not in ascending order.");
        X1 = x1;
        X2 = x2;
        Y1 = y1;
        Y2 = y2;
        Z1 = z1;
        Z2 = z2;
    }

    public static bool Validate(int x1, int x2, int y1, int y2, int z1, int z2, out Box box)
    {
        if (x1 <= x2 && y1 <= y2 && z1 <= z2)
        {
            box = new Box(x1, x2, y1, y2, z1, z2);
            return true;
        }
        else
        {
            box = default(Box);
            return false;
        }
    }

    public Box? Intersect(Box target)
    {
        if (X1 <= target.X2 && target.X1 <= X2 && Y1 <= target.Y2 && target.Y1 <= Y2 && Z1 <= target.Z2 && target.Z1 <= Z2)
        {
            var x1 = X1 <= target.X1 ? target.X1 : X1;
            var x2 = X2 >= target.X2 ? target.X2 : X2;
            var y1 = Y1 <= target.Y1 ? target.Y1 : Y1;
            var y2 = Y2 >= target.Y2 ? target.Y2 : Y2;
            var z1 = Z1 <= target.Z1 ? target.Z1 : Z1;
            var z2 = Z2 >= target.Z2 ? target.Z2 : Z2;
            return new Box(x1, x2, y1, y2, z1, z2);
        }
        return null;
    }

    public Box[] Except(Box target)
    {
        var result = new List<Box>();

        if (X1 <= target.X2 && target.X1 <= X2 && Y1 <= target.Y2 && target.Y1 <= Y2 && Z1 <= target.Z2 && target.Z1 <= Z2)
        {
            var x1 = X1 <= target.X1 ? target.X1 : X1;
            var x2 = X2 >= target.X2 ? target.X2 : X2;
            var y1 = Y1 <= target.Y1 ? target.Y1 : Y1;
            var y2 = Y2 >= target.Y2 ? target.Y2 : Y2;
            var z1 = Z1 <= target.Z1 ? target.Z1 : Z1;
            var z2 = Z2 >= target.Z2 ? target.Z2 : Z2;

            if (X1 < target.X1 && Y1 < target.Y1 && Z1 < target.Z1) { /* front, bottom, left   */ if (Box.Validate(X1, x1 - 1, Y1, y1 - 1, Z1, z1 - 1, out var box)) result.Add(box); }
            if (X1 < target.X1 && Z1 < target.Z1)                   { /* front, middle, left   */ if (Box.Validate(X1, x1 - 1, y1, y2, Z1, z1 - 1, out var box)) result.Add(box); }
            if (X1 < target.X1 && Y2 > target.Y2 && Z1 < target.Z1) { /* front, top,    left   */ if (Box.Validate(X1, x1 - 1, y2 + 1, Y2, Z1, z1 - 1, out var box)) result.Add(box); }
            if (Y2 > target.Y2 && Z1 < target.Z1)                   { /* front, top,    middle */ if (Box.Validate(x1, x2, y2 + 1, Y2, Z1, z1 - 1, out var box)) result.Add(box); }
            if (X2 > target.X2 && Y2 > target.Y2 && Z1 < target.Z1) { /* front, top,    right  */ if (Box.Validate(x2 + 1, X2, y2 + 1, Y2, Z1, z1 - 1, out var box)) result.Add(box); }
            if (X2 > target.X2 && Z1 < target.Z1)                   { /* front, middle, right  */ if (Box.Validate(x2 + 1, X2, y1, y2, Z1, z1 - 1, out var box)) result.Add(box); }
            if (X2 > target.X2 && Y1 < target.Y1 && Z1 < target.Z1) { /* front, bottom, right  */ if (Box.Validate(x2 + 1, X2, Y1, y1 - 1, Z1, z1 - 1, out var box)) result.Add(box); }
            if (Y1 < target.Y1 && Z1 < target.Z1)                   { /* front, bottom, middle */ if (Box.Validate(x1, x2, Y1, y1 - 1, Z1, z1 - 1, out var box)) result.Add(box); }
            if (Z1 < target.Z1)                                     { /* front, middle, middle */ if (Box.Validate(x1, x2, y1, y2, Z1, z1 - 1, out var box)) result.Add(box); }

            if (X1 < target.X1 && Y2 > target.Y2) { /* left,   top,    middle */ if (Box.Validate(X1, x1 - 1, y2 + 1, Y2, z1, z2, out var box)) result.Add(box); }
            if (X1 < target.X1)                   { /* left,   middle, middle */ if (Box.Validate(X1, x1 - 1, y1, y2, z1, z2, out var box)) result.Add(box); }
            if (X1 < target.X1 && Y1 < target.Y1) { /* left,   bottom, middle */ if (Box.Validate(X1, x1 - 1, Y1, y1 - 1, z1, z2, out var box)) result.Add(box); }
            if (Y1 < target.Y1)                   { /* bottom, middle, middle */ if (Box.Validate(x1, x2, Y1, y1 - 1, z1, z2, out var box)) result.Add(box); }
            if (X2 > target.X2 && Y1 < target.Y1) { /* right,  bottom, middle */ if (Box.Validate(x2 + 1, X2, Y1, y1 - 1, z1, z2, out var box)) result.Add(box); }
            if (X2 > target.X2)                   { /* right,  middle, middle */ if (Box.Validate(x2 + 1, X2, y1, y2, z1, z2, out var box)) result.Add(box); }
            if (X2 > target.X2 && Y2 > target.Y2) { /* right,  top,    middle */ if (Box.Validate(x2 + 1, X2, y2 + 1, Y2, z1, z2, out var box)) result.Add(box); }
            if (Y2 > target.Y2)                   { /* top,    middle, middle */ if (Box.Validate(x1, x2, y2 + 1, Y2, z1, z2, out var box)) result.Add(box); }

            if (X1 < target.X1 && Y1 < target.Y1 && Z2 > target.Z2) { /* back, bottom, left   */ if (Box.Validate(X1, x1 - 1, Y1, y1 - 1, z2 + 1, Z2, out var box)) result.Add(box); }
            if (X1 < target.X1 && Z2 > target.Z2)                   { /* back, middle, left   */ if (Box.Validate(X1, x1 - 1, y1, y2, z2 + 1, Z2, out var box)) result.Add(box); }
            if (X1 < target.X1 && Y2 > target.Y2 && Z2 > target.Z2) { /* back, top,    left   */ if (Box.Validate(X1, x1 - 1, y2 + 1, Y2, z2 + 1, Z2, out var box)) result.Add(box); }
            if (Y2 > target.Y2 && Z2 > target.Z2)                   { /* back, top,    middle */ if (Box.Validate(x1, x2, y2 + 1, Y2, z2 + 1, Z2, out var box)) result.Add(box); }
            if (X2 > target.X2 && Y2 > target.Y2 && Z2 > target.Z2) { /* back, top,    right  */ if (Box.Validate(x2 + 1, X2, y2 + 1, Y2, z2 + 1, Z2, out var box)) result.Add(box); }
            if (X2 > target.X2 && Z2 > target.Z2)                   { /* back, middle, right  */ if (Box.Validate(x2 + 1, X2, y1, y2, z2 + 1, Z2, out var box)) result.Add(box); }
            if (X2 > target.X2 && Y1 < target.Y1 && Z2 > target.Z2) { /* back, bottom, right  */ if (Box.Validate(x2 + 1, X2, Y1, y1 - 1, z2 + 1, Z2, out var box)) result.Add(box); }
            if (Y1 < target.Y1 && Z2 > target.Z2)                   { /* back, bottom, middle */ if (Box.Validate(x1, x2, Y1, y1 - 1, z2 + 1, Z2, out var box)) result.Add(box); }
            if (Z2 > target.Z2)                                     { /* back, middle, middle */ if (Box.Validate(x1, x2, y1, y2, z2 + 1, Z2, out var box)) result.Add(box); }
        }
        else
        {
            result.Add(this);
        }

        return result.ToArray();
    }

    public bool Intersects(Box target, bool inclusive = true)
        => inclusive ? (X1 <= target.X2 && target.X1 <= X2 && Y1 <= target.Y2 && target.Y1 <= Y2 && Z1 <= target.Z2 && target.Z1 <= Z2)
                     : (X1 < target.X2 && target.X1 < X2 && Y1 < target.Y2 && target.Y1 < Y2 && Z1 < target.Z2 && target.Z1 < Z2);

    public override string? ToString()
    {
        return $"x={X1}..{X2},y={Y1}..{Y2},z={Z1}..{Z2}";
    }
}

//var a = new Box(1, 1, 1, 1, 1, 1);
//var b = new Box(0, 2, 0, 2, 0, 2);
//var ba = b.Except(a);
//foreach (var item in ba)
//{
//    Console.WriteLine(item);
//}
//Console.WriteLine();

//for (int i = 0; i < ba.Length - 1; i++)
//{
//    for (int j = i + 1; j < ba.Length; j++)
//    {
//        var inter = ba[i].Intersect(ba[j]);
//        if (ba[i].Intersect(ba[j]) is Box c)
//        {
//            Console.WriteLine(c);
//        }
//    }
//}
