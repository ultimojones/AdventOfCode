// 4534266 too low

var list = File.ReadLines("input.txt").Select(l => l.Split('-').Select(long.Parse).ToArray()).OrderBy(l => l[0]).ToArray();

var count = 0;
long i = 0;
for (; i < uint.MaxValue; i++)
{
    if (!list.Any(l => l[0] <= i && i <= l[1]))
        break;
}
Console.WriteLine(i);

//int i = 0;
//while (list[i + 1][0] - list[i][1] <= 1) { i++; }


//for (int x = int.Max(0, i - 1); x < i + 2; x++)
//{
//    Console.WriteLine($"{list[x][0]}-{list[x][1]}");
//}

//Console.WriteLine();
//Console.WriteLine(list[i][1] + 1);

// 4294967295