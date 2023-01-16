using System.Drawing;

var lines = File.ReadAllLines("input.txt");

var draw = lines[0].Split(',').Select(byte.Parse).ToArray();

var cards = new List<Dictionary<(int X, int Y),byte>>();

for (int i = 2; i < lines.Length; i += 6)
{
    var grid = new Dictionary<(int X, int Y), byte>();
    for (int x = 0; x < 5; x++)
    {
        for (int y = 0; y < 5; y++)
        {
            grid[(x, y)] = byte.Parse(lines[y + i][(x * 3)..(x * 3 + 2)]);
        }
    }
    cards.Add(grid);
}

void PrintCards()
{
    foreach (var card in cards)
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                Console.Write($"{card[(x, y)],2} ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}

IEnumerable<(int Draw, int Count, int Score)> GetScores()
{
    for (int i = 0; i < cards.Count; i++)
    {
        var card = cards[i];
        var matches = new List<(int X, int Y)>();
        int count = 0;
        int score = 0;
        foreach (var num in draw)
        {
            count++;
            if (card.ContainsValue(num))
            {
                matches.Add(card.Where(c => c.Value == num).First().Key);
                var match = Enumerable.Range(0, 5).Any(x => Enumerable.Range(0, 5).All(y => matches.Contains((x, y))))
                         || Enumerable.Range(0, 5).Any(y => Enumerable.Range(0, 5).All(x => matches.Contains((x, y))));
                if (match)
                {
                    score = card.Where(c => !matches.Contains(c.Key)).Sum(c => c.Value) * num;
                    break;
                }
            }
        }
        Console.WriteLine($"{i} ({count}={score}): {string.Join(',', draw.Take(count))}");
        yield return (i, count, score);
    }
}

var winner = GetScores().MinBy(s => s.Count);
Console.WriteLine();
Console.WriteLine($"Winner: {winner}");