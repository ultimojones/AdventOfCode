var count = File.ReadAllLines("input.txt")
    .Count(l =>
    {
        var sides = l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Order().ToArray();
        return sides[0..2].Sum() > sides[2];
    });

Console.WriteLine(count);