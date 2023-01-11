using System.Collections;

int total = 3018458;
var elves = new BitArray(Enumerable.Repeat(true, total).ToArray());
int i = 0;
int loop = 1;
int left = total;

while (left > 1)
{
    var take = left / 2;
    var n = i;
    while (take > 0) { n = (n + 1) % total; if (elves[n]) { take--; } }
    elves[n] = false;
    left--;
    do { i = (i + 1) % total; } while (!elves[i]);
    if (loop++ % 10000 == 0) Console.WriteLine(left);
}

Console.WriteLine();
Console.WriteLine(i + 1);

// 26735 too low
