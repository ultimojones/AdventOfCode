using System.Drawing;

namespace Day12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Part1();
        }

        private static void Part1()
        {
            var lines = File.ReadAllLines("input.txt");
            int maxX = lines[0].Length;
            int maxY = lines.Length;

            var grid = new Dictionary<(int X, int Y), int>();
            var height = Enumerable.Range(0, maxX).SelectMany(x => Enumerable.Range(0, maxY).Select(y => (x, y)))
                .ToDictionary(i => i, i => lines[i.y][i.x]);
            var start = height.First(h => h.Value == 'S').Key;
            var end = height.First(h => h.Value == 'E').Key;
            height[start] = 'a';
            height[end] = 'z';

            grid[start] = 0;
            var nextSteps = GetNeighbours(start).ToList();

            for (int i = 0; i < 1000; i++)
            {
                var checking = nextSteps.Distinct().ToArray();
                nextSteps.Clear();

                foreach (var step in checking)
                {
                    var neighbours = GetNeighbours(step).ToArray();
                    var numSteps = neighbours.Where(n => height[n] - height[step] >= -1 && grid.ContainsKey(n)).Select(n => grid[n]).Min() + 1;
                    grid[step] = numSteps;
                    if (step == end) { Console.WriteLine($"Steps to end = {numSteps}"); break; }
                    nextSteps.AddRange(neighbours.Where(n => !grid.ContainsKey(n) && height[n] - height[step] <= 1));
                }
            }

            IEnumerable<(int X, int Y)> GetNeighbours((int X, int Y) cur) => 
                new (int X, int Y)[] { (cur.X - 1, cur.Y), (cur.X + 1, cur.Y), (cur.X, cur.Y - 1), (cur.X, cur.Y + 1) }
                .Where(n => n.X >= 0 && n.X < maxX && n.Y >= 0 && n.Y < maxY);
}
    }

}

        //    private static void Part1()
        //    {
        //        (int X, int Y) start = default, end = default;
        //        var lines = File.ReadAllLines("sample.txt");
        //        int maxx = lines[0].Length;
        //        int maxy = lines.Length;

//        var square = new Square { X = start.X, Y = start.Y, Z = 'a' };
//        var squares = new Dictionary<(int X, int Y), Square>();
//        var next = new List<(int X, int Y)>();

//        next.AddRange(
//            new (int X, int Y)[]
//            {
//                (square.X - 1, square.Y),
//                (square.X + 1, square.Y),
//                (square.X, square.Y - 1),
//                (square.X, square.Y + 1),
//            }.Where(n => n.X >= 0 && n.X < maxx && n.Y >= 0 && n.Y < maxy
//                    && !squares.ContainsKey(n)
//                    && lines[n.X][n.Y] - square.Z <= 1)
//        );

//        for (int i = 0; i < 100; i++)
//        {
//            var current = next.Distinct().ToArray();
//            next.Clear();

//            foreach (var c in current)
//            {
//                var steps =
//                    new (int X, int Y)[]
//                    {
//                        (c.X - 1, c.Y),
//                        (c.X + 1, c.Y),
//                        (c.X, c.Y - 1),
//                        (c.X, c.Y + 1),
//                    }.Select(d => squares[d]?.Steps).Min() + 1;
//                squares[c] = new Square { X = c.X, Y = c.Y, Z = lines[c.X][c.Y], Steps = steps.Value };

//                next.AddRange(
//                    new (int X, int Y)[]
//                    {
//                        (c.X - 1, c.Y),
//                        (c.X + 1, c.Y),
//                        (c.X, c.Y - 1),
//                        (c.X, c.Y + 1),
//                    }.Where(n => n.X >= 0 && n.X < maxx && n.Y >= 0 && n.Y < maxy
//                            && !squares.ContainsKey(n)
//                            && lines[n.X][n.Y] - square.Z <= 1));
//            }
//        }
//    }

//}

//class Square
//{
//    public int X { get; set; }
//    public int Y { get; set; }
//    public int Z { get; set; }
//    public int Steps { get; set; }
//}
//var grid = new char[lines[0].Length, lines.Length];
//for (int i = 0; i < lines.Length; i++)
//{
//    for (int j = 0; j < lines[i].Length; j++)
//    {
//        grid[j, i] = lines[i][j];
//        if (grid[j, i] == 'S') start = (j, i);
//        if (grid[j, i] == 'E') end = (j, i);
//    }
//}