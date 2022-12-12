namespace Day7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Part2();
        }

        private static void Part2()
        {
            using var input = File.OpenText("input.txt");
            var line = input.ReadLine();

            var path = new Stack<string>();
            string dir = "";
            var files = new Dictionary<string, int>();

            while (line != null)
            {
                if (line[0] == '$')
                {
                    switch (line[2..4])
                    {
                        case "cd":
                            switch (line[5..])
                            {
                                case "/":
                                    path.Clear();
                                    break;
                                case "..":
                                    path.Pop();
                                    break;
                                default:
                                    path.Push(line[5..]);
                                    break;
                            }
                            dir = '/' + String.Join('/', path.Reverse());
                            files.TryAdd(dir, 0);
                            break;
                        case "ls":
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
                else if (char.IsDigit(line[0]))
                {
                    var parts = line.Split(' ');
                    var len = int.Parse(parts[0]);
                    var file = parts[1];
                    var cur = files[dir];
                    files[dir] = cur + len;
                }

                line = input.ReadLine();
            }

            var totals = new Dictionary<string, int>();

            foreach (var item in files)
            {
                var sum = files.Where(p => p.Key.StartsWith(item.Key)).Sum(p => p.Value);
                totals[item.Key] = sum;
            }

            foreach (var item in totals.Where(t => t.Value <= 100000))
            {
                Console.WriteLine($"{item.Key} = {item.Value}");
            }

            Console.WriteLine(totals.Where(t => t.Value <= 100000).Sum(t => t.Value));

            // 48518336
            // 21481664
            //Console.WriteLine(totals.Where(t => t.Value >= 70000000 - totals["/"])
            //    .OrderBy(t => t.Value).First().Value);

            var minSize = 30000000 - (70000000 - totals["/"]);
            var candidates = totals.Where(t => t.Value >= minSize).OrderBy(t => t.Value).ToList();
            Console.WriteLine(candidates.First());
        }

        private static void Part1()
        {
            using var input = File.OpenText("input.txt");
            var line = input.ReadLine();

            var path = new Stack<string>();
            string dir = "";
            var files = new Dictionary<string, int>();

            while (line != null)
            {
                if (line[0] == '$')
                {
                    switch (line[2..4])
                    {
                        case "cd":
                            switch (line[5..])
                            {
                                case "/":
                                    path.Clear();
                                    break;
                                case "..":
                                    path.Pop();
                                    break;
                                default:
                                    path.Push(line[5..]);
                                    break;
                            }
                            dir = '/' + String.Join('/', path.Reverse());
                            files.TryAdd(dir, 0);
                            break;
                        case "ls":
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
                else if (char.IsDigit(line[0]))
                {
                    var parts = line.Split(' ');
                    var len = int.Parse(parts[0]);
                    var file = parts[1];
                    var cur = files[dir];
                    files[dir] = cur + len;
                }

                line = input.ReadLine();
            }

            var totals = new Dictionary<string, int>();

            foreach (var item in files)
            {
                var sum = files.Where(p => p.Key.StartsWith(item.Key)).Sum(p => p.Value);
                totals[item.Key] = sum;
            }

            foreach (var item in totals.Where(t => t.Value <= 100000))
            {
                Console.WriteLine($"{item.Key} = {item.Value}");
            }

            Console.WriteLine(totals.Where(t => t.Value <= 100000).Sum(t => t.Value));
        }
    }
}