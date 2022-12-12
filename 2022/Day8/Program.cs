using System.Security.Cryptography.X509Certificates;

namespace Day8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Part2();
        }

        private static void Part2()
        {
            var grid = File.ReadAllLines("input.txt")
                .SelectMany((l, x) => l.Select((c, y) => new { x, y, h = int.Parse(c.ToString()) }))
                .ToDictionary(a => (a.x, a.y), a => (a.h, v: 0));

            for (int x = 1; x < 98; x++)
            {
                for (int y = 1; y < 98; y++)
                {
                    int l, r, u, d;
                    for (l = 1; l < y; l++) { if (grid[(x, y - l)].h >= grid[(x, y)].h) break; }
                    for (r = 1; r < 98 - y; r++) { if (grid[(x, y + r)].h >= grid[(x, y)].h) break; }
                    for (u = 1; u < x; u++) { if (grid[(x - u, y)].h >= grid[(x, y)].h) break; }
                    for (d = 1; d < 98 - x; d++) { if (grid[(x + d, y)].h >= grid[(x, y)].h) break; }
                    grid[(x, y)] = (grid[(x, y)].h, l * r * u * d);
                }
            }

            Console.WriteLine(grid.Max(x => x.Value.v));
        }

        private static void Part1()
        {
            var grid = LoadGrid();
            int count = 0;
            for (int i = 0; i < 99; i++)
            {
                for (int j = 0; j < 99; j++)
                {
                    var tree = grid[i, j];
                    var up = Enumerable.Range(0, j).All(x => grid[i, x].height < tree.height);
                    var down = Enumerable.Range(j + 1, 98 - j).All(x => grid[i, x].height < tree.height);
                    var left = Enumerable.Range(0, i).All(x => grid[x, j].height < tree.height);
                    var right = Enumerable.Range(i + 1, 98 - i).All(x => grid[x, j].height < tree.height);
                    if (grid[i, j].visible = up || down || left || right)
                        count++;
                }
            }
            Console.WriteLine(count);
        }

        private static (int height, bool visible)[,] LoadGrid()
        {
            var grid = new (int, bool)[99, 99];
            using var input = File.OpenText("input.txt");
            var line = input.ReadLine();
            int count = 0;
            while (line != null)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    grid[count, i] = (int.Parse(line[i].ToString()), false);
                }
                count++;
                line = input.ReadLine();
            }
            return grid;
        }
    }
}