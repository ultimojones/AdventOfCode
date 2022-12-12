namespace Day1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int floor = 0, i = 0;
            var input = File.ReadAllText("input.txt");
            for (; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '(':
                        floor++;
                        break;
                    case ')':
                        floor--;
                        break;
                }
                if (floor == -1)
                {
                    break;
                }
            }
            Console.WriteLine(i + 1);
        }
    }
}