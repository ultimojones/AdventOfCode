int total = 3018458;
var eLnkd = new LinkedList<int>(Enumerable.Range(1, total).ToArray());

var next = eLnkd.Find(total / 2 + 1);

while (eLnkd.Count > 1)
{
    var following = next.Next ?? eLnkd.First;
    if (int.IsOddInteger(eLnkd.Count))
        following = following.Next ?? eLnkd.First;
    eLnkd.Remove(next);
    next = following;
}

Console.WriteLine($"Last elf = {eLnkd.Single()}");

// 26735 too low
// 2303265 too high
