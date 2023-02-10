//Player 1 starting position: 4
//Player 2 starting position: 8

//Player 1 starting position: 6
//Player 2 starting position: 4

using System.Collections.Concurrent;

var faces = Enumerable.Range(1, 3);
var combos = faces.SelectMany(a => faces.SelectMany(b => faces.Select(c => (Sum: a + b + c, Roll: string.Concat(a, b, c)))))
    .GroupBy(c => c.Sum).Select(c => (Roll: c.Key, Count: c.LongCount()));

var p1TurnWins = new Dictionary<int, long>();
var p1TurnNotWins = new Dictionary<int, long>();
GetVariants(0, 1, 0, 6, p1TurnWins, p1TurnNotWins);

var p2TurnWins = new Dictionary<int, long>();
var p2TurnNotWins = new Dictionary<int, long>();
GetVariants(0, 1, 0, 4, p2TurnWins, p2TurnNotWins);

long p1Wins = 0;
foreach (var p1 in p1TurnWins.OrderBy(d => d.Key))
{
    p1Wins += p1.Value * p2TurnNotWins[p1.Key - 1];
    Console.WriteLine($"{p1}={p1Wins}");
}
Console.WriteLine();

long p2Wins = 0;
foreach (var p2 in p2TurnWins.OrderBy(d => d.Key))
{
    p2Wins += p2.Value * (p1TurnNotWins.TryGetValue(p2.Key, out var v) ? v : 0);
    Console.WriteLine($"{p2}={p2Wins}");
}
Console.WriteLine();

Console.WriteLine($"P1={p1Wins}");
Console.WriteLine($"P2={p2Wins}");

// P1 = 444356092776315
// P2 = 341960390180808


void GetVariants(int turn, long variants, int score, int pos, Dictionary<int, long> turnWins, Dictionary<int, long> turnNotWins)
{
    if (score > 20)
    {
        turnWins[turn] = (turnWins.TryGetValue(turn, out var tw) ? tw : 0) + variants;
    }
    else
    {
        turnNotWins[turn] = (turnNotWins.TryGetValue(turn, out var tnw) ? tnw : 0) + variants;

        foreach (var combo in combos)
        {
            var nextPos = (pos + combo.Roll - 1) % 10 + 1;
            GetVariants(turn + 1, variants * combo.Count, score + nextPos, nextPos, turnWins, turnNotWins);
        }
    }
}
