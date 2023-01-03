var lines = File.ReadAllLines("input.txt").Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray();

int count = 0;

for (int i = 0; i < lines.Length; i += 3)
{
    for (int x = 0; x < 3; x++)
    {
        var sides = Enumerable.Range(i, 3).Select(y => lines[y][x]).Order().ToArray();
        if (sides[0..2].Sum() > sides[2])
            count++;
    }
}

Console.WriteLine(count);