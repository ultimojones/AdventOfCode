using System.Collections;

int total = 3018458;
var elves = new LinkedList<int>(Enumerable.Range(0, total).ToArray());
int i = 0;
var old = elves.Find(total / 2);
int loop = 1;

while (elves.Count > 1)
{
    var next = old.Next ?? elves.First;
    elves.Remove(old);
    old = elves.Count == 3 ? next : next.Next ?? elves.First;
    if (loop++ % 10000 == 0) Console.WriteLine(elves.Count);
}

Console.WriteLine();
Console.WriteLine(elves.ElementAt(i) + 1);

// 26735 too low
// 2303265 too high
