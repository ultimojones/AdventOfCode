using System.Text.RegularExpressions;

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
    var bestMatch = Enumerable.Range(0, sensors.Count).Where(b => b != a).Select(b =>
    {
        var aSen = sensors[a];
        var bSen = sensors[b];
        var aPairs = aSen.SelectMany(a1 => aSen.Where(a2 => a2 != a1).Select(a2 => (a1, a2)));
        var bPairs = bSen.SelectMany(b1 => bSen.Where(b2 => b2 != b1).Select(b2 => (b1, b2)));
        return aPairs.SelectMany(ap => bPairs
            .SelectMany(bp => GetRotations(bp.b1).Where(r => ap.a1 - r.Point == ap.a2 - Rotate(r.Direction, r.Spin, bp.b2))
            .Select(r => (r.Direction, r.Spin, Offset: ap.a1 - Rotate(r.Direction, r.Spin, bp.b1))))).Distinct()
            .Select(m => (b, m.Direction, m.Spin, m.Offset, Beacons: aSen.Intersect(bSen.Select(bi => Rotate(m.Direction, m.Spin, bi) + m.Offset))))
            .MaxBy(m => m.Beacons.Count());
    }).MaxBy(m => m.Beacons.Count());
    Console.WriteLine($"{bestMatch.b}{bestMatch.Direction}{bestMatch.Spin}{bestMatch.Offset} {string.Concat(bestMatch.Beacons)}");
}
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

    public static Offset operator +(Offset left, Offset right) => new Offset { X = left.X + right.X, Y = left.Y + right.Y, Z = left.Z + right.Z };
    public static Offset operator -(Offset left, Offset right) => new Offset { X = left.X - right.X, Y = left.Y - right.Y, Z = left.Z - right.Z };
    public static bool operator ==(Offset left, Offset right) => left.X == right.X && left.Y == right.Y && left.Z == right.Z;
    public static bool operator !=(Offset left, Offset right) => left.X != right.X || left.Y != right.Y || left.Z != right.Z;
    public override bool Equals(object? obj) => obj is Offset off && off.X == X && off.Y == Y && off.Z == Z;
    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
}