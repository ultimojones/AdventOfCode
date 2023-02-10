var input = File.ReadAllLines("input.txt");
var alg = input[0];
var img = new List<(int X, int Y)>();
for (int y = 2; y < input.Length; y++)
{
    for (int x = 0; x < input[y].Length; x++)
    {
        if (input[y][x] == '#')
            img.Add((x, y));
    }
}
PrintImage();


for (int i = 0; i < 2; i++)
{
    int xMin = img.Min(i => i.X);
    int xMax = img.Max(i => i.X);
    int yMin = img.Min(i => i.Y);
    int yMax = img.Max(i => i.Y);

    var next = Enumerable.Range(xMin - 1, xMax - xMin + 3)
        .SelectMany(x => Enumerable.Range(yMin - 1, yMax - yMin + 3)
            .Select(y => (x, y)).Where(p =>
            {
                var pixels = new[]
                {
                    (p.x - 1, p.y - 1), (p.x, p.y - 1), (p.x + 1, p.y - 1),
                    (p.x - 1, p.y),     (p.x, p.y),     (p.x + 1, p.y),
                    (p.x - 1, p.y + 1), (p.x, p.y + 1), (p.x + 1, p.y + 1),
                };
                int index = Enumerable.Range(0, 9).Aggregate(0, (a, i) => a * 2 + (img.Contains(pixels[i]) ? 1 : 0));
                return alg[index] == '#';
            })).ToList();
    img = next;
    PrintImage();
}

Console.WriteLine(img.Count);

// 5617 too high





void PrintImage()
{
    int xMin = img.Min(i => i.X);
    int xMax = img.Max(i => i.X);
    int yMin = img.Min(i => i.Y);
    int yMax = img.Max(i => i.Y);
    for (int y = yMin; y < yMax + 1; y++)
    {
        Console.WriteLine(Enumerable.Range(xMin, xMax - xMin + 1).Select(x => img.Contains((x, y)) ? '#' : '.').ToArray());
    }
    Console.WriteLine();
}