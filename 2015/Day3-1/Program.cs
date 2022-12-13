using System.Drawing;

var santa = new Point(0, 0);
var visits = new List<Point>() { santa };

var directions = File.ReadAllText("input.txt");
foreach (var dir in directions)
{
    santa.Offset(dir switch
    {
        '^' => new Point(0, -1),
        'v' => new Point(0, +1),
        '<' => new Point(-1, 0),
        '>' => new Point(+1, 0),
        _ => default
    });
    visits.Add(santa);
}

Console.WriteLine(visits.Distinct().Count());

