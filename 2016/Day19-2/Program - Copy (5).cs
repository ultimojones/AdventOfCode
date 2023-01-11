using System.Collections;
using System.Diagnostics;

int total = 5;// 3018458;
var eList = new List<int>(Enumerable.Range(1, total).ToArray());
var eLnkd = new LinkedList<int>(Enumerable.Range(1, total).ToArray());
int i = 0;
int loop = 0;

var curr = eLnkd.First;
var next = eLnkd.Find(total / 2 + 1);

while (eList.Count > 1)
{
    var takee = (i + eList.Count / 2) % eList.Count;
    PrintCurrent(i, takee, eList);
    eList.RemoveAt(takee);
    i = i >= eList.Count - 1 ? 0 : i + 1;

    Console.WriteLine(new string('-', 40));

    var c = eLnkd.Select((p, n) => (p, n)).First(e => e.p == curr.Value).n;
    var n = eLnkd.Select((p, n) => (p, n)).First(e => e.p == next.Value).n;
    PrintCurrent(c, n, eLnkd);
    var following = next.Next ?? eLnkd.First;
    if (int.IsOddInteger(eLnkd.Count))
        following = following.Next ?? eLnkd.First;
    eLnkd.Remove(next);
    next = following;
    curr = curr.Next ?? eLnkd.First;

    Console.WriteLine(new string('=', 40));
}

Console.WriteLine($"Last elf = {eList.Single()}");
Console.WriteLine($"Last elf = {eLnkd.Single()}");

void PrintCurrent(int i, int o, ICollection<int> elves)
{
    Console.WriteLine($"                {i}/{elves.Count}");
    var values = elves.Concat(elves).Concat(elves).Skip(elves.Count + i - 2).Take(5);
    Console.WriteLine(string.Join("|", values.Select(e => $"{e,7}")));

    Console.WriteLine($"                {o}={elves.ElementAt(o)}");
    var oppVals = elves.Concat(elves).Concat(elves).Skip(elves.Count + o - 2).Take(5);
    Console.WriteLine(string.Join("|", oppVals.Select(e => $"{e,7}")));

    //Console.WriteLine(new string('-', 40));
}


// 26735 too low
// 2303265 too high
