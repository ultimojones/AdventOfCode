
// 257 too high

var moves = File.ReadAllText("input.txt").Split(", ").Select(m => (Turn: m[0], Num: int.Parse(m[1..])));

int X = 0, Y = 0;
var pos = (X: 0, Y: 0);
var dir = 'N';
var visits = new HashSet<(int X, int Y)>();

foreach (var m in moves)
{
    var points = (dir, m.Turn) switch
    {
        ('N', 'R') or ('S', 'L') 
            => (Points: Enumerable.Range(pos.X + 1, m.Num).Select(x => (X: x, Y: pos.Y)), Dir: 'E'),
        ('N', 'L') or ('S', 'R') 
            => (Points: Enumerable.Range(pos.X - m.Num, m.Num).Reverse().Select(x => (X: x, Y: pos.Y)), Dir: 'W'),
        ('E', 'R') or ('W', 'L') 
            => (Points: Enumerable.Range(pos.Y + 1, m.Num).Select(y => (X: pos.X, Y: y)), Dir: 'S'),
        ('E', 'L') or ('W', 'R') 
            => (Points: Enumerable.Range(pos.Y - m.Num, m.Num).Reverse().Select(y => (X: pos.X, Y: y)), Dir: 'N'),
        _ => throw new NotImplementedException()
    };
    Console.WriteLine(string.Join(",", points.Points));
    foreach (var point in points.Points)
    {
        if (visits.Contains(point))
        {
            Console.WriteLine($"{point} Distance = {Math.Abs(point.X) + Math.Abs(point.Y)}");
            return;
        }
        visits.Add(point);
    }
    pos = points.Points.Last();
    dir = points.Dir;
}
