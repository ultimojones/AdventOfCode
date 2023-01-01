using System.Collections.Concurrent;

// 3272728

for (int i = 1; i < 36000000; i += 1000)
{
    var house = Enumerable.Range(i, 1000).AsParallel()
        .Select(i => (House: i, Presents: CalcPresents(i)))
        .Where(i => i.Presents > 36000000);
    if (house.Any())
    {
        var min = house.MinBy(h => h.House);
        Console.WriteLine(house.MinBy(h => h.House));
        for (int j = min.House - 49; j <= min.House; j++)
        {
            if (min.House % j == 0)
            {
                Console.WriteLine($"{j} = {j * 11}");
            }
        }
        break;
    }
}

int CalcPresents(int house)
{
    var first = int.Max(1, house / 50);
    return Enumerable.Range(first, house - first + 1).Sum(i => house % i == 0 ? i : 0) * 11;
}