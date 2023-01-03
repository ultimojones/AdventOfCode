var moves = File.ReadAllText("input.txt").Split(", ").Select(m => (Turn: m[0], Num: int.Parse(m[1..])));

int X = 0, Y = 0;
var pos = (X: 0, Y: 0, Dir: 'N');

foreach (var m in moves)
{
    pos = (pos.Dir, m.Turn) switch
    {
        ('N', 'R') or ('S', 'L') => (pos.X + m.Num, pos.Y, 'E'),
        ('N', 'L') or ('S', 'R') => (pos.X - m.Num, pos.Y, 'W'),
        ('E', 'R') or ('W', 'L') => (pos.X, pos.Y + m.Num, 'S'),
        ('E', 'L') or ('W', 'R') => (pos.X, pos.Y - m.Num, 'N'),
        _ => throw new NotImplementedException()
    };
    Console.WriteLine($"{m} = {pos}");
}

Console.WriteLine($"Distance = {Math.Abs(pos.X) + Math.Abs(pos.Y)}");