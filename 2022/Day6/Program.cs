namespace Day6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Part2();
        }

        private static void Part2()
        {
            // How many characters need to be processed before the first start-of-message marker is detected?

            var signal = File.ReadAllText("input.txt");

            for (int i = 0, j = 14; j < signal.Length; i++, j++)
            {
                if (signal[i..j].Distinct().Count() == 14)
                {
                    Console.WriteLine(j);
                    break;
                }
            }
        }

        private static void Part1()
        {
            // How many characters need to be processed before the first start-of-packet marker is detected?

            var signal = File.ReadAllText("input.txt");

            for (int i = 3; i < signal.Length; i++)
            {
                if (
                       signal[i]     != signal[i - 1]
                    && signal[i]     != signal[i - 2]
                    && signal[i]     != signal[i - 3]
                    && signal[i - 1] != signal[i - 2]
                    && signal[i - 1] != signal[i - 3]
                    && signal[i - 2] != signal[i - 3]
                 )
                {
                    Console.WriteLine(i + 1);
                    break;
                }
            }
        }
    }
}