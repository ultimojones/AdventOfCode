using System.Drawing;

var santa = new Point(0, 0);
var robos = new Point(0, 0);
var visits = new List<Point>() { santa, robos };

var dirs = File.ReadAllText("input.txt");
for (int i = 0; i < dirs.Length; i++)
{
    var offset = dirs[i] switch
    {
        '^' => new Point(0, -1),
        'v' => new Point(0, +1),
        '<' => new Point(-1, 0),
        '>' => new Point(+1, 0),
        _ => default
    };
    if (i % 2 == 0)
    {
        santa.Offset(offset);
        visits.Add(santa);
    }
    else
    {
        robos.Offset(offset);
        visits.Add(robos);
    }
}

Console.WriteLine(visits.Distinct().Count());

