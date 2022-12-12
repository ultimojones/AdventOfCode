using System.Numerics;
using System.Security.Cryptography;

namespace Day11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Part2();
        }

        private static void Part2()
        {
            checked
            {
                var monkies = new List<Monkey>();
                Monkey monkey = null!;
                foreach (var line in File.ReadLines("input.txt"))
                {
                    if (line.StartsWith("Monkey ")) { monkies.Add(monkey = new Monkey { Id = int.Parse(line[7..^1]) }); }
                    if (line.StartsWith("  Starting items: ")) { line[18..].Split(", ").Select(i => int.Parse(i)).ToList().ForEach(i => monkey.Items.Enqueue(i)); }
                    if (line.StartsWith("  Operation: new = old "))
                    {
                        if (line[23..] == "* old") { monkey.Operator = "^"; monkey.Operand = 2; }
                        else { monkey.Operator = line[23].ToString(); monkey.Operand = int.Parse(line[25..]); }
                    }
                    if (line.StartsWith("  Test: divisible by ")) { monkey.Test = int.Parse(line[21..]); }
                    if (line.StartsWith("    If true: throw to monkey ")) { monkey.IfTrue = int.Parse(line[29..]); }
                    if (line.StartsWith("    If false: throw to monkey ")) { monkey.IfFalse = int.Parse(line[30..]); }
                }
                long factor = monkies[0].Test;
                for (int i = 0; i < monkies.Count; i++)
                    factor *= monkies[i].Test;
                Console.WriteLine(factor);

                for (int r = 0; r < 10000; r++)
                {
                    if (r % 10 == 0) Console.WriteLine(r);
                    foreach (var monk in monkies)
                    {
                        while (monk.Items.Count > 0)
                        {
                            monk.Counter++;
                            var item = monk.Items.Dequeue();
                            item = monk.Operator switch { "^" => BigInteger.Pow(item, monk.Operand), "+" => item + monk.Operand, "*" => item * monk.Operand, _ => default };
                            item = BigInteger.DivRem(item, factor).Remainder;
                            var dest = BigInteger.Remainder(item, monk.Test) == 0 ? monk.IfTrue : monk.IfFalse;
                            monkies[dest].Items.Enqueue(item);
                        }
                    }
                }

                for (int i = 0; i < monkies.Count; i++)
                {
                    monkey = monkies[i];
                    Console.WriteLine($"[{i}] Monkey {monkey.Id} inspected items {monkey.Counter} times.");
                }
                Console.WriteLine();
                var counts = monkies.Select(m => m.Counter).OrderDescending().ToArray();
                Console.WriteLine(counts[0] * counts[1]);

            }
        }


        class Monkey
        {
            public int Id { get; set; }
            public Queue<BigInteger> Items { get; } = new Queue<BigInteger>();
            public string Operator { get; set; } = default!;
            public int Operand { get; set; }
            public int Test { get; set; }
            public int IfTrue { get; set; }
            public int IfFalse { get; set; }
            public long Counter { get; set; }
        }

        //private static void Part1()
        //{
        //    var monkies = new List<Monkey>();
        //    Monkey monkey = null!;
        //    foreach (var line in File.ReadLines("sample.txt"))
        //    {
        //        if (line.StartsWith("Monkey ")) { monkies.Add(monkey = new Monkey { Id = int.Parse(line[7..^1]) }); }
        //        if (line.StartsWith("  Starting items: ")) { line[18..].Split(", ").Select(i => decimal.Parse(i)).ToList().ForEach(i => monkey.Items.Enqueue(i)); }
        //        if (line.StartsWith("  Operation: new = old "))
        //        {
        //            if (line[23..] == "* old") { monkey.Operator = "^"; monkey.Operand = 2; }
        //            else { monkey.Operator = line[23].ToString(); monkey.Operand = int.Parse(line[25..]); }
        //        }
        //        if (line.StartsWith("  Test: divisible by ")) { monkey.Test = int.Parse(line[21..]); }
        //        if (line.StartsWith("    If true: throw to monkey ")) { monkey.IfTrue = int.Parse(line[29..]); }
        //        if (line.StartsWith("    If false: throw to monkey ")) { monkey.IfFalse = int.Parse(line[30..]); }
        //    }

        //    for (int r = 0; r < 20; r++)
        //    {
        //        foreach (var monk in monkies)
        //        {
        //            while (monk.Items.Count > 0)
        //            {
        //                monk.Counter++;
        //                var item = monk.Items.Dequeue();
        //                var worry = monk.Operator switch { "^" => item * item, "+" => item + monk.Operand, "*" => item * monk.Operand, _ => default };
        //                item = worry;// / 3;
        //                monkies[item % monk.Test == 0 ? monk.IfTrue : monk.IfFalse].Items.Enqueue(item);
        //            }
        //        }
        //    }

        //    for (int i = 0; i < monkies.Count; i++)
        //    {
        //        monkey = monkies[i];
        //        Console.WriteLine($"[{i}] Monkey {monkey.Id} inspected items {monkey.Counter} times.");
        //    }
        //    Console.WriteLine();
        //    var counts = monkies.Select(m => m.Counter).OrderDescending().ToArray();
        //    Console.WriteLine(counts[0] * counts[1]);
        //}
    }
}