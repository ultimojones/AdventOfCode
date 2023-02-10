using System.Collections;

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


for (int i = 0; i < 2; i++)
{
    int xMin = img.Min(i => i.X);
    int xMax = img.Max(i => i.X);
    int yMin = img.Min(i => i.Y);
    int yMax = img.Max(i => i.Y);
    zeros.Clear();

    var next = Enumerable.Range(xMin - 1, xMax - xMin + 3)
        .SelectMany(x => Enumerable.Range(yMin - 1, yMax - yMin + 3)
            .Select(y => (x, y)).Where(p =>
            {
                //Console.Write(p);
                var pixels = new[]
                {
                    (p.x - 1, p.y - 1), (p.x, p.y - 1), (p.x + 1, p.y - 1),
                    (p.x - 1, p.y),     (p.x, p.y),     (p.x + 1, p.y),
                    (p.x - 1, p.y + 1), (p.x, p.y + 1), (p.x + 1, p.y + 1),
                };
                int index = Enumerable.Range(0, 9).Aggregate(0, (a, i) => a * 2 + (img.Contains(pixels[i]) ? 1 : 0));
                //if (index == 0) zeros.Add(p);
                //var binary = new int[1];
                //var ba = new BitArray(pixels.Select(p => img.Contains(p)).Reverse().ToArray()) { Length = 32 };
                //ba.CopyTo(binary, 0);
                //var index2 = binary[0];
                ////Console.WriteLine($"{string.Concat(pixels.Select(p => img.Contains(p) ? '1' : '0'))}={index2}");
                //if (index != index2) throw new Exception();
                return alg[index] == '#';
                //return index > 0 && alg[index] == '#';
            })).ToList();
    //Console.WriteLine();
    img = next;
    PrintImage();
}

// 5375 too low

// 5538 wrong

// 5617 too high
// 6322 too high




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
        //Console.WriteLine(Enumerable.Range(xMin - 2, xMax - xMin + 5)
        //    .Select(x => zeros.Contains((x, y)) ? '*' : img.Contains((x, y)) ? '#' : '.').ToArray());
    }
    Console.WriteLine(img.Count);
    Console.WriteLine();
}