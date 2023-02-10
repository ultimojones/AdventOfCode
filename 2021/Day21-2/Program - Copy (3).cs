//Player 1 starting position: 4
//Player 2 starting position: 8

//Player 1 starting position: 6
//Player 2 starting position: 4

using System.Collections.Concurrent;

var faces = Enumerable.Range(1, 3);
var combos = faces.SelectMany(a => faces.SelectMany(b => faces.Select(c => (Sum: a + b + c, Roll: string.Concat(a, b, c)))))
    .GroupBy(c => c.Sum).Select(c => (Roll: c.Key, Count: c.LongCount()));
long p1Wins = 0, p2Wins = 0;

var p1Results = GetWins(0, 1, 0, 4).GroupBy(r => r.Turns).Select(r => (Turns: r.Key, Variants: r.Select(v => v.Variants).Sum())).Order();
foreach (var item in p1Results.Order())
{
    Console.WriteLine(item);
}

Console.WriteLine();
var p2Results = GetWins(0, 1, 0, 8).GroupBy(r => r.Turns).Select(r => (Turns: r.Key, Variants: r.Select(v => v.Variants).Sum())).Order();
foreach (var item in p2Results.Order())
{
    Console.WriteLine(item);
}

checked
{
    Console.WriteLine();
    foreach (var p1 in p1Results)
    {
        var p2winners = p2Results.Where(p2 => p2.Turns < p1.Turns).Sum(p2 => p2.Variants);
        p2Wins += p2winners * p1.Variants;
        Console.WriteLine($"{p1.Turns,2}={p2winners,12}={p2winners * p1.Variants,24}={p2Wins}");
    }
    Console.WriteLine(p2Wins);
}

//      13224261574820084

// P1 = 444356092776315
// P2 = 341960390180808

IEnumerable<(int Turns, long Variants)> GetWins(int turn, long variants, int score, int pos)
{
    if (score > 20)
    {
        yield return (turn, variants);
    }
    else
    {
        foreach (var combo in combos)
        {
            var nextPos = (pos + combo.Roll - 1) % 10 + 1;
            foreach (var result in GetWins(turn + 1, variants * combo.Count, score + nextPos, nextPos))
            {
                yield return result;
            }
        }
    }
}
