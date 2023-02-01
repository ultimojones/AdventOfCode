var sensors = new List<List<Offset>>();

List<Offset> sensor = null!;
foreach (var line in File.ReadLines("sample.txt"))
{
    if (line.StartsWith("---"))
    {
        sensor = new List<Offset>();
        sensors.Add(sensor);
    }
    else if (line.Length > 0)
    {
        sensor.Add(Offset.Parse(line));
    }
}

for (int a = 0; a < sensors.Count - 1; a++)
{
    for (int b = 0; b < sensors.Count; b++)
    {
        if (a == b) continue;
        var aSen = sensors[a];
        var bSen = sensors[b];
        var rotation = aSen.SelectMany(a1 => aSen.Where(a2 => a2 != a1).SelectMany(a2 =>
        {
            var aDiff = a1 - a2;
            var aVals = new[] { aDiff.X, aDiff.Y, aDiff.Z }.Select(int.Abs).Order().ToArray();
            return bSen.SelectMany(b1 => bSen.Where(b2 => b2 != b1).Select(b2 => b1 - b2)
                .Where(bDiff => aVals.SequenceEqual(new[] { bDiff.X, bDiff.Y, bDiff.Z }.Select(int.Abs).Order()))).Distinct()
                .SelectMany(r => GetRotations(r).Where(br => br.Point == aDiff).Select(rt => (rt.Direction, rt.Spin)));
        })).Distinct().ToList();

        Console.WriteLine($"{a}-{b}");
        rotation.ForEach(r => Console.WriteLine(r));

        var counts = rotation.Select(r => aSen.SelectMany(a1 => aSen.Where(a2 => a2 != a1).SelectMany(a2 =>
        {

        })
    }
}


//var results = aSen.Select(a1 => aSen.Where(a2 => a2 != a1).SelectMany(a2 =>
//{
//    var result = bSen.SelectMany(b1 => bSen.Where(b2 => b2 != b1).Select(b2 =>
//    {
//        var aDiff = a1 - a2;
//        var bDiff = b1 - b2;
//        return new
//        {
//            aDiff,
//            bDiff,
//            aVals = new[] { aDiff.X, aDiff.Y, aDiff.Z }.Select(int.Abs).Order().ToArray(),
//            bVals = new[] { bDiff.X, bDiff.Y, bDiff.Z }.Select(int.Abs).Order().ToArray(),
//        };
//    }).FirstOrDefault(r => r.aVals.SequenceEqual(r.bVals))
//    .Select(r => GetRotations(r.bDiff).Single(br => br.Point == r.aDiff)));
//}));


//int na = 0, nb = 0, na1 = 0, na2 = 0, nb1 = 0, nb2 = 0;

//var comparing = sensors.ToList();
//foreach (var a in sensors.Take(0))
//{
//    na++; nb = 0;
//    comparing.Remove(a);
//    foreach (var b in comparing.Take(1))
//    {
//        nb++; na1 = 0;
//        foreach (var a1 in a)
//        {
//            na1++; na2 = 0;
//            foreach (var a2 in a.Where(x => x != a1))
//            {
//                na2++; nb1 = 0;
//                foreach (var b1 in b)
//                {
//                    nb1++; nb2 = 0;
//                    foreach (var b2 in b.Where(x => x != b1))
//                    {
//                        nb2++;
//                        var aDiff = a1 - a2;
//                        var bDiff = b1 - b2;
//                        var aVals = new[] { aDiff.X, aDiff.Y, aDiff.Z }.Select(int.Abs).Order().ToArray();
//                        var bVals = new[] { bDiff.X, bDiff.Y, bDiff.Z }.Select(int.Abs).Order().ToArray();
//                        if (aVals.SequenceEqual(bVals))
//                        {
//                            foreach (var rotation in GetRotations(bDiff))
//                            {
//                                if (aDiff == rotation.Point) { Console.WriteLine(rotation); }
//                            }

//                            //break;
//                        }
//                    }
//                }
//            }
//        }
//    }
//}

Offset Rotate(int direction, int spin, Offset point)
{
    var result = point;

    result = direction switch
    {
        0 => result,
        1 => new Offset(result.X, -result.Z, result.Y),
        2 => new Offset(result.X, -result.Y, -result.Z),
        3 => new Offset(result.X, result.Z, -result.Y),
        4 => new Offset(result.Z, result.Y, -result.X),
        5 => new Offset(-result.Z, result.Y, result.X),
    };

    result = spin switch
    {
        0 => result,
        1 => new Offset(result.Y, -result.X, result.Z),
        2 => new Offset(-result.X, -result.Y, result.Z),
        3 => new Offset(-result.Y, result.X, result.Z),
    };

    return result;
}

IEnumerable<(int Direction, int Spin, Offset Point)> GetRotations(Offset point)
{
    for (int d = 0; d < 6; d++)
    {
        for (int s = 0; s < 4; s++)
        {
            yield return (d, s, Rotate(d, s, point));
        }
    }
}


bool Matching(Offset a1, Offset a2, Offset b1, Offset b2)
{
    var aDiff = a1 - a2;
    var bDiff = b1 - b2;
    var aVals = new[] { aDiff.X, aDiff.Y, aDiff.Z }.Select(int.Abs).Order();
    var bVals = new[] { bDiff.X, bDiff.Y, bDiff.Z }.Select(int.Abs).Order();
    return aVals.SequenceEqual(bVals);
}

public class Offset
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public Offset(int x = 0, int y = 0, int z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Offset Parse(string line)
    {
        var vals = line.Split(',').Select(int.Parse).ToArray();
        return new Offset { X = vals[0], Y = vals[1], Z = vals[2] };
    }

    public override string ToString() => $"({X},{Y},{Z})";

    public static Offset operator -(Offset left, Offset right) => new Offset { X = left.X - right.X, Y = left.Y - right.Y, Z = left.Z - right.Z };
    public static bool operator ==(Offset left, Offset right) => left.X == right.X && left.Y == right.Y && left.Z == right.Z;
    public static bool operator !=(Offset left, Offset right) => left.X != right.X || left.Y != right.Y || left.Z != right.Z;
    public override bool Equals(object? obj) => obj is Offset off && off.X == X && off.Y == Y && off.Z == Z;
    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
}