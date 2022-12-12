namespace Day4
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Part2();

        }

        private static void Part2()
        {
            // In how many assignment pairs do the ranges overlap?

            int total = 0;
            using var input = File.OpenText("input.txt");
            var line = input.ReadLine();
            while (line != null)
            {
                var pairs = line.Split(',').Select(e => e.Split('-').Select(int.Parse).ToArray()).ToArray();
                if ((pairs[0][0] <= pairs[1][0] && pairs[1][1] <= pairs[0][1])
                    || (pairs[1][0] <= pairs[0][0] && pairs[0][1] <= pairs[1][1])
                    || (pairs[1][0] <= pairs[0][0] && pairs[0][0] <= pairs[1][1])
                    || (pairs[1][0] <= pairs[0][1] && pairs[0][1] <= pairs[1][1]))
                {
                    total++;
                }
                line = input.ReadLine();
            }
            Console.WriteLine(total);
        }

        private static void Part1()
        {
            // In how many assignment pairs does one range fully contain the other?

            int total = 0;
            using var input = File.OpenText("input.txt");
            var line = input.ReadLine();
            while (line != null)
            {
                var pairs = line.Split(',').Select(e => e.Split('-').Select(int.Parse).ToArray()).ToArray();
                if ((pairs[0][0] <= pairs[1][0] && pairs[1][1] <= pairs[0][1])
                    || (pairs[1][0] <= pairs[0][0] && pairs[0][1] <= pairs[1][1]))
                {
                    total++;
                }
                line = input.ReadLine();
            }
            Console.WriteLine(total);
        }
    }
}