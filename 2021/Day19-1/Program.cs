﻿using System.Linq;

var sensors = new List<List<Offset>>();
string[] rotations = Enumerable.Range(0, 6).SelectMany(d => Enumerable.Range(0, 4).Select(s => string.Concat(d, s))).ToArray();

List<Offset> adding = null!;
foreach (var line in File.ReadLines("input.txt"))
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

var sensorOffsets = new Dictionary<int, (string Rotation, Offset Offset)> { { 0, ("00", new Offset(0, 0, 0)) } };
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

    Console.WriteLine(sensorMatch);
    sensorOffsets[sensorMatch.s] = (sensorMatch.r, sensorMatch.o);
    foreach (var beacon in sensors[sensorMatch.s])
    {
        var found = Rotate(sensorMatch.r, beacon) + sensorMatch.o;
        if (!foundBeacons.Contains(found))
            foundBeacons.Add(found);
    }
}

Console.WriteLine(string.Join(Environment.NewLine, foundBeacons.Order()));
Console.WriteLine(string.Join(Environment.NewLine, sensorOffsets));
Console.WriteLine(foundBeacons.Count);


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

public class Offset : IComparable<Offset>
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
    public int CompareTo(Offset? other) => other is null ? int.MaxValue : X != other.X ? X.CompareTo(other.X) : Y != other.Y ? Y.CompareTo(other.Y) : Z.CompareTo(other.Z);
}