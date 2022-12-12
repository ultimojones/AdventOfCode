namespace Day5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Part2();
        }

        private static void Part2()
        {
            // After the rearrangement procedure completes, what crate ends up on top of each stack?

            var stacks = Enumerable.Range(0, 9).Select(_ => new Stack<char>()).ToArray();

            foreach (var row in File.ReadAllLines("input-containers.txt").Reverse())
                for (int i = 1, s = 0; i < 34; i += 4, s++)
                    if (row[i] != ' ')
                        stacks[s].Push(row[i]);

            using var moves = File.OpenText("input-moves.txt");
            var line = moves.ReadLine();

            while (line != null)
            {
                var parts = line.Split(' ');
                var num = int.Parse(parts[1]);
                var from = int.Parse(parts[3]) - 1;
                var to = int.Parse(parts[5]) - 1;

                foreach (var item in Enumerable.Range(0, num).Select(_ => stacks[from].Pop()).Reverse())
                {
                    stacks[to].Push(item);
                }

                line = moves.ReadLine();
            }

            foreach (var stack in stacks)
                Console.WriteLine(String.Join(' ', stack.ToArray()));
            Console.WriteLine();

            Console.WriteLine(String.Concat(stacks.Select(s => s.Peek()).ToArray()));
        }

        private static void Part1()
        {
            // After the rearrangement procedure completes, what crate ends up on top of each stack?

            var stacks = Enumerable.Range(0, 9).Select(_ => new Stack<char>()).ToArray();

            foreach (var row in File.ReadAllLines("input-containers.txt").Reverse())
                for (int i = 1, s = 0; i < 34; i += 4, s++)
                    if (row[i] != ' ')
                        stacks[s].Push(row[i]);

            foreach (var stack in stacks)
                Console.WriteLine(String.Join(' ', stack.ToArray()));
            Console.WriteLine();

            using var moves = File.OpenText("input-moves.txt");
            var line = moves.ReadLine();

            while (line != null)
            {
                var parts = line.Split(' ');
                var num = int.Parse(parts[1]);
                var from = int.Parse(parts[3]) - 1;
                var to = int.Parse(parts[5]) - 1;

                for (int i = 0; i < num; i++)
                {
                    stacks[to].Push(stacks[from].Pop());
                }

                line = moves.ReadLine();
            }

            foreach (var stack in stacks)
                Console.WriteLine(String.Join(' ', stack.ToArray()));
            Console.WriteLine();

            Console.WriteLine(String.Concat(stacks.Select(s => s.Peek()).ToArray()));
        }
    }
}