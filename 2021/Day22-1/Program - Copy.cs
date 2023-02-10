using System.Text.RegularExpressions;

long cubes = 0;
var areas = new List<(string Action, Box Area)>();
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
        var confirmedAreas = new List<Box>();
        var newAreas = new Queue<Box>();
        newAreas.Enqueue(area);
        while (newAreas.TryDequeue(out var areaCheck))
        {
            bool split = false;
            foreach (var oldArea in areas)
            {
                if (areaCheck.Intersects(oldArea.Area))
                {
                    split = true;
                    foreach (var except in areaCheck.Except(oldArea.Area))
                    {
                        newAreas.Enqueue(except);
                    }
                    break;
                }
            }
            if (!split)
            {
                confirmedAreas.Add(areaCheck);
                cubes += areaCheck.Size;
                Console.WriteLine(new { cubes });
            }
        }
        areas.AddRange(confirmedAreas.Select(c => (action, c)));
    }
    else
    {
        var removedAreas = new List<(string Action, Box Area)>();
        var newAreas = new List<(string Action, Box Area)>();
        foreach (var onArea in areas)
        {
            if (area.Intersects(onArea.Area))
            {
                newAreas.AddRange(onArea.Area.Except(area).Select(a => (onArea.Action, a)));
                removedAreas.Add(onArea);
                cubes -= area.Intersect(onArea.Area).Size;
            }
        }
        foreach (var remove in removedAreas)
        {
            areas.Remove(remove);
        }
        areas.AddRange(newAreas);
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

    public Box Intersect(Box target)
    {
        if (X1 <= target.X2 && target.X1 <= X2 && Y1 <= target.Y2 && target.Y1 <= Y2 && Z1 <= target.Z2 && target.Z1 <= Z2)
        {
            var x1 = X1 < target.X1 ? target.X1 : X1;
            var x2 = X2 > target.X2 ? target.X2 : X2;
            var y1 = Y1 < target.Y1 ? target.Y1 : Y1;
            var y2 = Y2 > target.Y2 ? target.Y2 : Y2;
            var z1 = Z1 < target.Z1 ? target.Z1 : Z1;
            var z2 = Z2 > target.Z2 ? target.Z2 : Z2;

            return new Box(x1, x2, y1, y2, z1, z2);
        }
        return default(Box);
    }

    public Box[] Except(Box target)
    {
        var result = new List<Box>();

        if (X1 <= target.X2 && target.X1 <= X2 && Y1 <= target.Y2 && target.Y1 <= Y2 && Z1 <= target.Z2 && target.Z1 <= Z2)
        {
            if (X1 < target.X1 || X2 > target.X2 || Y1 < target.Y1 || Y2 > target.Y2 || Z1 < target.Z1 || Z2 > target.Z2)
            {
                var x1 = X1 <= target.X1 ? target.X1 - 1 : X1 + 1;
                var x2 = X2 >= target.X2 ? target.X2 + 1 : X2 - 1;
                var y1 = Y1 <= target.Y1 ? target.Y1 - 1 : Y1 + 1;
                var y2 = Y2 >= target.Y2 ? target.Y2 + 1 : Y2 - 1;
                var z1 = Z1 <= target.Z1 ? target.Z1 - 1 : Z1 + 1;
                var z2 = Z2 >= target.Z2 ? target.Z2 + 1 : Z2 - 1;

                if (X1 < target.X1 && Y1 < target.Y1 && Z1 < target.Z1) { /* front, bottom, left */ result.Add(new(X1, x1, Y1, y1, Z1, z1)); }
                if (X1 < target.X1 && Z1 < target.Z1) { /* front, middle, left */ result.Add(new(X1, x1, y1, y2, Z1, z1)); }
                if (X1 < target.X1 && Y2 > target.Y2 && Z1 < target.Z1) { /* front, top, left */ result.Add(new(X1, x1, y2, Y2, Z1, z1)); }
                if (Y2 > target.Y2 && Z1 < target.Z1) { /* front, top, middle */ result.Add(new(x1, x2, y2, Y2, Z1, z1)); }
                if (X2 > target.X2 && Y2 > target.Y2 && Z1 < target.Z1) { /* front, top, right */ result.Add(new(x2, X2, y2, Y2, Z1, z1)); }
                if (X2 > target.X2 && Z1 < target.Z1) { /* front, middle, right */ result.Add(new(x2, X2, y1, y2, Z1, z1)); }
                if (X2 > target.X2 && Y1 < target.Y1 && Z1 < target.Z1) { /* front, bottom, right */ result.Add(new(x2, X2, Y1, y1, Z1, z1)); }
                if (Y1 < target.Y1 && Z1 < target.Z1) { /* front, bottom, middle */ result.Add(new(x1, x2, Y1, y1, Z1, z1)); }
                if (Z1 < target.Z1) { /* front, middle, middle */ result.Add(new(x1, x2, y1, y2, Z1, z1)); }

                if (X1 < target.X1 && Y2 > target.Y2) { /* left, top, middle */ result.Add(new(X1, x1, y2, Y2, z1, z2)); }
                if (X1 < target.X1) { /* left, middle, middle */ result.Add(new(X1, x1, y1, y2, z1, z2)); }
                if (X1 < target.X1 && Y1 < target.Y1) { /* left, bottom, middle */ result.Add(new(X1, x1, Y1, y1, z1, z2)); }
                if (Y1 < target.Y1) { /* bottom, middle, middle */ result.Add(new(x1, x2, Y1, y1, z1, z2)); }
                if (X2 > target.X2 && Y1 < target.Y1) { /* right, bottom, middle */ result.Add(new(x2, X2, Y1, y1, z1, z2)); }
                if (X2 > target.X2) { /* right, middle, middle */ result.Add(new(x2, X2, y1, y2, z1, z2)); }
                if (X2 > target.X2 && Y2 > target.Y2) { /* right, top, middle */ result.Add(new(x2, X2, y2, Y2, z1, z2)); }
                if (Y2 > target.Y2) { /* top, middle, middle */ result.Add(new(x1, x2, y2, Y2, z1, z2)); }

                if (X1 < target.X1 && Y1 < target.Y1 && Z2 > target.Z2) { /* back, bottom, left */ result.Add(new(X1, x1, Y1, y1, z2, Z2)); }
                if (X1 < target.X1 && Z2 > target.Z2) { /* back, middle, left */ result.Add(new(X1, x1, y1, y2, z2, Z2)); }
                if (X1 < target.X1 && Y2 > target.Y2 && Z2 > target.Z2) { /* back, top, left */ result.Add(new(X1, x1, y2, Y2, z2, Z2)); }
                if (Y2 > target.Y2 && Z2 > target.Z2) { /* back, top, middle */ result.Add(new(x1, x2, y2, Y2, z2, Z2)); }
                if (X2 > target.X2 && Y2 > target.Y2 && Z2 > target.Z2) { /* back, top, right */ result.Add(new(x2, X2, y2, Y2, z2, Z2)); }
                if (X2 > target.X2 && Z2 > target.Z2) { /* back, middle, right */ result.Add(new(x2, X2, y1, y2, z2, Z2)); }
                if (X2 > target.X2 && Y1 < target.Y1 && Z2 > target.Z2) { /* back, bottom, right */ result.Add(new(x2, X2, Y1, y1, z2, Z2)); }
                if (Y1 < target.Y1 && Z2 > target.Z2) { /* back, bottom, middle */ result.Add(new(x1, x2, Y1, y1, z2, Z2)); }
                if (Z2 > target.Z2) { /* back, middle, middle */ result.Add(new(x1, x2, y1, y2, z2, Z2)); }
            }
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

//var target = new Box(5, 10, 5, 10, 5, 10);
//var test = new Box(0, 15, 0, 15, 0, 15);
//Console.WriteLine(new { test, target });
//var result = test.Except(target);
//for (int i = 0; i < result.Length; i++)
//{
//    for (int j = i + 1; j < result.Length; j++)
//    {
//        if (result[i].Intersects(result[j], false))
//            Console.WriteLine(new { source = result[i], target = result[j] });
//    }
//}
//foreach (var item in result)
//{
//    Console.WriteLine(item);
//}
//return;