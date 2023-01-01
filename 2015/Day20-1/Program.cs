using System.Collections.Concurrent;

// 861840

for (int i = 1; i < 36000000; i += 1000)
{
    var house = Enumerable.Range(i, 1000).AsParallel()
        .Select(i => (House: i, Presents: CalcPresents(i)))
        .Where(i => i.Presents > 36000000);
    if (house.Any())
    {
        Console.WriteLine(house.MinBy(h => h.House));
        break;
    }
}

int CalcPresents(int house)
{
	return Enumerable.Range(1, house).Sum(i => house % i == 0 ? i : 0) * 10;
}