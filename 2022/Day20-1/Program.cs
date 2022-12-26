using System.Runtime.InteropServices;

var items = File.ReadLines("input.txt").Select((x, p) => (p, x: int.Parse(x))).ToArray();
var list = new LinkedList<(int p, int x)>(items);
var len = items.Length;

foreach (var item in items)
{
    //Console.WriteLine(item);
    var cur = list.Find(item)!;
    for (int i = 0; i < Math.Abs(cur.Value.x); i++)
    {
        LinkedListNode<(int, int)> next;
        if (cur.Value.x > 0)
        {
            next = cur.Next ?? list.First!;
        }
        else
        {
            if (cur.Previous == null)
                next = list.Last!.Previous!;
            else if (cur.Previous.Previous == null)
                next = list.Last!;
            else
                next = cur.Previous.Previous;
        }
        list.Remove(cur);
        list.AddAfter(next, cur);
    }
    //Console.WriteLine(string.Join(", ", list.Select(x => x.x)));
}

var pos = list.TakeWhile(x => x.x != 0).Count();
var calc1000 = list.ElementAt((pos + 1000) % len).x;
var calc2000 = list.ElementAt((pos + 2000) % len).x;
var calc3000 = list.ElementAt((pos + 3000) % len).x;
Console.WriteLine(calc1000 + calc2000 + calc3000);