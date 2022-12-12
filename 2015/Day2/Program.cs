namespace Day2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Part2();
        }

        private static void Part2()
        {
            int total = 0;
            foreach (var line in File.ReadLines("input.txt"))
            {
                var dims = line.Split('x').Select(d => int.Parse(d)).Order().ToArray();
                var ribbon = 2 * dims[0] + 2 * dims[1] + dims[0] * dims[1] * dims[2];
                total += ribbon;
            }
            Console.WriteLine(total);
        }

        private static void Part1()
        {
            int total = 0;
            foreach (var line in File.ReadLines("input.txt"))
            {
                var parts = line.Split('x');
                var x = int.Parse(parts[0]);
                var y = int.Parse(parts[1]);
                var z = int.Parse(parts[2]);
                var xy = x * y;
                var xz = x * z;
                var yz = y * z;
                var pack = (2 * xy + 2 * xz + 2 * yz + new[] { xy, xz, yz }.Min());
                total += pack;
            }
            Console.WriteLine(total);
        }
    }
}