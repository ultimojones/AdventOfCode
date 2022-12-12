namespace Day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Part2();
        }

        private static void Part2()
        {
            var signal = 1;
            var signals = new List<int>() { signal };
            foreach (var line in File.ReadLines("input.txt"))
            {
                signals.Add(signal);
                if (line.StartsWith("addx"))
                {
                    signals.Add(signal += int.Parse(line.Substring(5)));
                }
            }

            for (int i = 0; i < 240; i++)
            {
                var pos = i % 40;
                Console.Write(Math.Abs(signals[i] - pos) <= 1 ? '█' : '.');

                if (pos == 39) Console.WriteLine();
            }
        }

        private static void Part1()
        {
            var signal = 1;
            var signals = new List<int>() { signal };
            foreach (var line in File.ReadLines("input.txt"))
            {
                signals.Add(signal);
                if (line.StartsWith("addx"))
                {
                    signals.Add(signal += int.Parse(line.Substring(5)));
                }
            }

            int CalcStrength(int cycle) => cycle * signals![cycle - 1];
            var total = CalcStrength(20) + CalcStrength(60) + CalcStrength(100) + CalcStrength(140) + CalcStrength(180) + CalcStrength(220);
            Console.WriteLine(total);
        }
    }
}