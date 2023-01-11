/*
Disc #1 has 5 positions; at time=0, it is at position  2.
Disc #2 has 13 positions; at time=0, it is at position 7.
Disc #3 has 17 positions; at time=0, it is at position 10.
Disc #4 has 3 positions; at time=0, it is at position  2.
Disc #5 has 19 positions; at time=0, it is at position 9.
Disc #6 has 7 positions; at time=0, it is at position  0.
---
Disc #1 has 5 positions; at time=0, it is at position 4.
Disc #2 has 2 positions; at time=0, it is at position 1. 
var disks = new (int Positions, int Start)[] { (5, 4), (2, 1) };
 */

//var d = (Size: 5, Start: 2);

//var second = 0;

//var currentPos = (second + d.Start) % d.Size;


var disks = new (int Positions, int Start)[]
{
    (5,2),
    (13,7),
    (17,10),
    (3,2),
    (19,9),
    (7,0),
};

checked
{
    for (int i = 0; ; i++)
    {
        if (Enumerable.Range(0, disks.Length).All(d =>
        {
            var pos = (i + 1 + d + disks[d].Start) % disks[d].Positions;
            return pos == 0;
        }))
        {
            Console.WriteLine(i);
            break;
        }
    } 
}