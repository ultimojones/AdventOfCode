using System.Linq;

var sensors = new List<List<Offset>>();
string[] rotations = Enumerable.Range(0, 6).SelectMany(d => Enumerable.Range(0, 4).Select(s => string.Concat(d, s))).ToArray();

List<Offset> adding = null!;
foreach (var line in File.ReadLines("sample.txt"))
{
    if (line.StartsWith("---"))
    {
        adding = new List<Offset>();
        sensors.Add(adding);
    }
    else if (line.Length > 0)
    {
        adding.Add(Offset.Parse(line));
    }
}

var sensorOffsets = new Dictionary<int, (int Direction, int Spin, Offset Offset)> { { 0, (0, 0, new Offset(0, 0, 0)) } };
var foundBeacons = sensors[0].ToHashSet();

// While we have sensors that aren't matched.
while (sensorOffsets.Count < sensors.Count)
{
    // For each unmatched sensor
    (int s, string r, Offset o, int count) sensorMatch = Enumerable.Range(0, sensors.Count).Except(sensorOffsets.Keys).Select(s =>
    {
        // For each rotation
        (int s, string r, Offset o, int count) rotationMatch = rotations.Select(r =>
        {
            //      Find the highest possible matches to found beacons

            // Rotate the beacons
            Offset[] rBeacons = sensors[s].Select(b => Rotate(r, b)).ToArray();

            (Offset o, int count) result = rBeacons.SelectMany(b =>
            {
                // Calc offset to each found beacon
                var offsets = foundBeacons.Select(f => f - b);
                // Get the offset that produces the highest number of matches for this rotation
                return offsets.Select(o => (o, count: rBeacons.Select(bo => bo + o).Intersect(foundBeacons).Count()));
            }).MaxBy(o => o.count);

            return (s, r, result.o, result.count);

        }).MaxBy(r => r.count);

        return rotationMatch;
    }).MaxBy(r => r.count);

    // For each sensor beacon
    // For offset to each found beacon
    // Count other beacons with same offset
    // Return offset and count
    // Return sensor, rotation, offset and count
    // For highest count, add to found becaons



    //// For each beacon
    //var rMatches = Enumerable.Range(0, rBeacons.Length - 1).Select(r1 =>
    //{
    //    // Count matches for if it matches each found beacon
    //    return foundBeacons.Select(f => f - rBeacons[r1]).Select(o => new { o, count = rBeacons.Select(b => b + o).Intersect(foundBeacons).Count() });
    //});


    //var fPairs = foundBeacons.SelectMany(f1 => foundBeacons.Where(f2 => f2 != f1).Select(f2 => (f1, f2)));
    //var sensorMatch = Enumerable.Range(0, sensors.Count).Except(sensorOffsets.Keys).Select(s =>
    //{
    //    var sensor = sensors[s];
    //    var sPairs = sensor.SelectMany(s1 => sensor.Where(s2 => s2 != s1).Select(s2 => (s1, s2)));
    //    var matchedPairs = sPairs.Select(sp => fPairs.Select(fp => GetRotations(sp.s1).Where(sr => fp.f1 - sr.Point == fp.f2 - Rotate(sr.Direction, sr.Spin, sp.s2))));

    //    return fPairs.SelectMany<(Offset a1, Offset a2),(int Direction, int Spin, Offset Offset)>(fp => sPairs
    //        .SelectMany(sp => GetRotations(sp.s1).Where(r => fp.a1 - r.Point == fp.a2 - Rotate(r.Direction, r.Spin, sp.s2))
    //        .Select(r => (r.Direction, r.Spin, Offset: fp.a1 - Rotate(r.Direction, r.Spin, sp.s1))))).Distinct();
    //});

    //.Select(sr => (sr.Direction, sr.Spin))

    //var sensorMatch = Enumerable.Range(0, sensors.Count).Except(sensorOffsets.Keys).Select(b =>
    //{
    //    var bSen = sensors[b];
    //    var bPairs = bSen.SelectMany(b1 => bSen.Where(b2 => b2 != b1).Select(b2 => (b1, b2)));

    //    var matches = aPairs.SelectMany(ap => bPairs
    //        .SelectMany(bp => GetRotations(bp.b1).Where(r => ap.a1 - r.Point == ap.a2 - Rotate(r.Direction, r.Spin, bp.b2))
    //        .Select(r => (r.Direction, r.Spin, Offset: ap.a1 - Rotate(r.Direction, r.Spin, bp.b1))))).Distinct();
    //    return matches
    //        .Select(m => (b, m.Direction, m.Spin, m.Offset, Beacons: foundBeacons.Intersect(bSen.Select(bi => Rotate(m.Direction, m.Spin, bi) + m.Offset)).Count()))
    //        .MaxBy(m => m.Beacons);
    //}).MaxBy(m => m.Beacons);

    //Console.WriteLine(sensorMatch);
    //sensorOffsets[sensorMatch.b] = (sensorMatch.Direction, sensorMatch.Spin, sensorMatch.Offset);
    //foreach (var beacon in sensors[sensorMatch.b])
    //{
    //    var found = Rotate(sensorMatch.Direction, sensorMatch.Spin, beacon) + sensorMatch.Offset;
    //     if (!foundBeacons.Contains(found))
    //        foundBeacons.Add(found);
    //}
}

//Console.WriteLine(string.Concat(foundBeacons));
//foreach (var item in sensorOffsets)
//{
//    Console.WriteLine(item);
//}
//Console.WriteLine(foundBeacons.Count);


Offset Rotate(string rotation, Offset point)
{
    var result = point;

    result = rotation[0] switch
    {
        '0' => result,
        '1' => new Offset(result.X, -result.Z, result.Y),
        '2' => new Offset(result.X, -result.Y, -result.Z),
        '3' => new Offset(result.X, result.Z, -result.Y),
        '4' => new Offset(result.Z, result.Y, -result.X),
        '5' => new Offset(-result.Z, result.Y, result.X),
    };

    result = rotation[1] switch
    {
        '0' => result,
        '1' => new Offset(result.Y, -result.X, result.Z),
        '2' => new Offset(-result.X, -result.Y, result.Z),
        '3' => new Offset(-result.Y, result.X, result.Z),
    };

    return result;
}

//IEnumerable<(int Direction, int Spin, Offset Point)> GetRotations(Offset point)
//{
//    for (int d = 0; d < 6; d++)
//    {
//        for (int s = 0; s < 4; s++)
//        {
//            yield return (d, s, Rotate(d, s, point));
//        }
//    }
//}

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