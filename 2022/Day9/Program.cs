using System.Drawing;

namespace Day9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Part2();
        }

        private static void Part2()
        {
            var knots = new List<Knot>()
            {
                new ('H', 0, 0),
                new ('1', 0, 0),
                new ('2', 0, 0),
                new ('3', 0, 0),
                new ('4', 0, 0),
                new ('5', 0, 0),
                new ('6', 0, 0),
                new ('7', 0, 0),
                new ('8', 0, 0),
                new ('9', 0, 0),
            };
            var visits = new List<(int x, int y)> { (0, 0) };

            foreach (var line in File.ReadLines("input.txt"))
            {
                var moves = int.Parse(line.Substring(2));
                for (int i = 0; i < moves; i++)
                {
                    knots[0].X += line[0] switch { 'U' => -1, 'D' => 1, _ => 0 };
                    knots[0].Y += line[0] switch { 'L' => -1, 'R' => 1, _ => 0 };

                    for (int j = 1; j < knots.Count; j++)
                    {
                        var head = knots[j - 1];
                        var tail = knots[j];

                        var difx = head.X - tail.X;
                        var dify = head.Y - tail.Y;

                        if (Math.Abs(difx) > 1 && (Math.Abs(dify) > 1))
                        {
                            tail.X += Math.Sign(difx);
                            tail.Y += Math.Sign(dify);
                        }
                        else if (Math.Abs(difx) > 1)
                        {
                            tail.X += Math.Sign(difx);
                            tail.Y = head.Y;
                        }
                        else if (Math.Abs(dify) > 1)
                        {
                            tail.Y += Math.Sign(dify);
                            tail.X = head.X;
                        }
                    }
                    visits.Add((knots.Last().X, knots.Last().Y));
                }
            }

            Console.WriteLine(visits.Distinct().Count());
        }

        private static void Test1()
        {
            var knots = new List<Knot>()
            {
                new ('H', 0, 0),
                new ('1', 0, 0),
                new ('2', 0, 0),
                new ('3', 0, 0),
                new ('4', 0, 0),
                new ('5', 0, 0),
                new ('6', 0, 0),
                new ('7', 0, 0),
                new ('8', 0, 0),
                new ('9', 0, 0),
            };
            var visits = new List<(int x, int y)> { (0, 0) };

            foreach (var line in File.ReadLines("test1.txt"))
            {
                Console.WriteLine($"== {line} ==");
                Console.WriteLine();

                var moves = int.Parse(line.Substring(2));
                for (int i = 0; i < moves; i++)
                {
                    knots[0].X += line[0] switch { 'U' => -1, 'D' => 1, _ => 0 };
                    knots[0].Y += line[0] switch { 'L' => -1, 'R' => 1, _ => 0 };

                    for (int j = 1; j < knots.Count; j++)
                    {
                        var head = knots[j - 1];
                        var tail = knots[j];

                        var difx = head.X - tail.X;
                        var dify = head.Y - tail.Y;

                        if (Math.Abs(difx) > 1 && (Math.Abs(dify) > 1))
                        {
                            tail.X += Math.Sign(difx);
                            tail.Y += Math.Sign(dify);
                        }
                        else if (Math.Abs(difx) > 1)
                        {
                            tail.X += Math.Sign(difx);
                            tail.Y = head.Y;
                        }
                        else if (Math.Abs(dify) > 1)
                        {
                            tail.Y += Math.Sign(dify);
                            tail.X = head.X;
                        }
                    }
                    visits.Add((knots.Last().X, knots.Last().Y));

                    PrintGrid(knots);
                }
            }

            Console.WriteLine(visits.Distinct().Count());
        }

        static int top, bottom, left, right = 0;

        private static void PrintGrid(List<Knot> knots)
        {
            top = Math.Min(top, knots.Min(k => k.X));
            bottom = Math.Max(bottom, knots.Max(k => k.X));
            left = Math.Min(left, knots.Min(k => k.Y));
            right = Math.Max(right, knots.Max(k => k.Y));

            for (int i = top; i < bottom + 1; i++)
            {
                for (int j = left; j < right + 1; j++)
                {
                    if (knots.Any(k => k.X == i && k.Y == j))
                    {
                        Console.Write(knots.First(k => k.X == i && k.Y == j).Id);
                    }
                    else
                    {
                        Console.Write(i == 0 && j == 0 ? 's' : '.');
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        class Knot
        {
            public char Id { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public Knot(char id, int x, int y)
            {
                Id = id;
                X = x;
                Y = y;
            }
        }

        //private static void TestX1()
        //{
        //    var knots = new Dictionary<char, Point>()
        //    {
        //        { 'H', new Point(0, 0) },
        //        { 'T', new Point(0, 0) },
        //    };
        //    var visits = new List<Point> { new Point(0, 0) };

        //    foreach (var line in File.ReadLines("test1.txt"))
        //    {
        //        Console.WriteLine($"== {line} ==");
        //        Console.WriteLine();

        //        var moves = int.Parse(line.Substring(2));
        //        for (int i = 0; i < moves; i++)
        //        {
        //            knots['H'].Offset(line[0] switch
        //            {
        //                'U' => new Point(-1, 0),
        //                'D' => new Point(1, 0),
        //                'L' => new Point(0, -1),
        //                'R' => new Point(0, 1),
        //                _ => new Point(0, 0),
        //            });
        //            for (int j = 1; j < knots.Count; j++)
        //            {
        //                var head = knots.ElementAt(j - 1).Value;
        //                var tail = knots.ElementAt(j).Value;

        //                var difx = head.X - tail.X;
        //                var dify = head.Y - tail.Y;

        //                if (Math.Abs(difx) > 1)
        //                {
        //                    tail.X += Math.Sign(difx);
        //                    tail.Y = head.Y;
        //                }
        //                else if (Math.Abs(dify) > 1)
        //                {
        //                    tail.Y += Math.Sign(dify);
        //                    tail.X = head.X;
        //                }
        //            }
        //            visits.Add(knots.Last().Value);

        //            //PrintGrid(knots);
        //        }
        //    }
        //}

        private static void Part1()
        {
            var head = (x: 0, y: 0);
            var tail = (x: 0, y: 0);
            var visits = new List<(int x, int y)> { tail };

            using var input = File.OpenText("input.txt");
            var line = input.ReadLine();
            while (line != null)
            {
                var moves = int.Parse(line.Substring(2));
                for (int i = 0; i < moves; i++)
                    MoveOne(line[0]);
                line = input.ReadLine();
            }

            Console.WriteLine(visits.Distinct().Count());

            void MoveOne(char direction)
            {
                _ = direction switch
                {
                    'U' => head.x--,
                    'D' => head.x++,
                    'L' => head.y--,
                    'R' => head.y++,
                    _ => throw new NotImplementedException()
                };

                var difx = head.x - tail.x;
                var dify = head.y - tail.y;

                if (Math.Abs(difx) > 1)
                {
                    tail.x += Math.Sign(difx);
                    tail.y = head.y;
                }
                else if (Math.Abs(dify) > 1)
                {
                    tail.y += Math.Sign(dify);
                    tail.x = head.x;
                }

                visits.Add(tail);
                Console.WriteLine($"{head} {tail}");
            }
        }
    }
}