//Player 1 starting position: 4
//Player 2 starting position: 8

//Player 1 starting position: 6
//Player 2 starting position: 4

var queued = new Queue<(int P1Pos, int P1Scr, int P2Pos, int P2Scr, string Rolls)>();
long p1Wins = 0, p2Wins = 0;
var timer = new Timer(s => Console.WriteLine($"{p1Wins,12} {p2Wins,12}"), null, 0, 60000);

queued.Enqueue((4, 0, 8, 0, ""));

while (queued.TryDequeue(out var status))
{
    foreach (var combo in GetRolls())
    {
        var result = status;
        result.Rolls += combo.Rolls;
        if (int.IsEvenInteger(status.Rolls.Length))
        {
            result.P1Scr += result.P1Pos = (result.P1Pos + combo.Sum - 1) % 10 + 1;
        }
        else
        {
            result.P2Scr += result.P2Pos = (result.P2Pos + combo.Sum - 1) % 10 + 1;
        }
        if (result.P1Scr > 20)
        {
            p1Wins++;
        }
        else if (result.P2Scr > 20)
        {
            p2Wins++;
        }
        else
        {
            queued.Enqueue(result);
        }
    }
}

Console.WriteLine(p1Wins);
Console.WriteLine(p2Wins);

IEnumerable<(string Rolls, int Sum)> GetRolls()
{
    var faces = Enumerable.Range(1, 3);
    return faces.SelectMany(a => faces.SelectMany(b => faces.Select(c =>
        (new string(new[] { (char)(a + 'a'), (char)(b + 'a'), (char)(c + 'a') }), a + b + c))));
}
