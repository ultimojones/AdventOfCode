var input = File.ReadAllLines("input.txt");
var alg = input[0];
var img = new List<(int X, int Y)>();
var zeros = new List<(int X, int Y)>();
for (int i = 2, y = 0; i < input.Length; i++, y++)
{
    for (int x = 0; x < input[i].Length; x++)
    {
        if (input[i][x] == '#')
            img.Add((x, y));
    }
}
PrintImage();

int xMin = img.Min(i => i.X) - 100;
int xMax = img.Max(i => i.X) + 100;
int yMin = img.Min(i => i.Y) - 100;
int yMax = img.Max(i => i.Y) + 100;

for (int i = 0; i < 50; i++)
{
    xMin++; xMax--; yMin++; yMax--;
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
}

PrintImage();



void PrintImage()
{
    int xMin = img.Min(i => i.X);
    int xMax = img.Max(i => i.X);
    int yMin = img.Min(i => i.Y);
    int yMax = img.Max(i => i.Y);
    for (int y = yMin - 2; y < yMax + 3; y++)
    {
        Console.WriteLine(Enumerable.Range(xMin - 2, xMax - xMin + 5)
            .Select(x => img.Contains((x, y)) ? '#' : '.').ToArray());
    }
    Console.WriteLine(img.Count);
    Console.WriteLine();
}