var buttons = new List<int>();

int pos = 5;

foreach (var line in File.ReadLines("input.txt"))
{
	foreach (var dir in line)
	{
		pos = (pos, dir) switch
		{
            (4, 'U') or (2, 'L') => 1,
            (5, 'U') or (1, 'R') or (3, 'L') => 2,
            (6, 'U') or (2, 'R') => 3,
			(1, 'D') or (7, 'U') or (5, 'L') => 4,
			(2, 'D') or (8, 'U') or (4, 'R') or (6, 'L') => 5,
			(3, 'D') or (9, 'U') or (5, 'R') => 6,
            (4, 'D') or (8, 'L') => 7,
            (5, 'D') or (7, 'R') or (9, 'L') => 8,
            (6, 'D') or (8, 'R') => 9,
			_ => pos
        };
	}
	buttons.Add(pos);
	Console.WriteLine(pos);
}
Console.WriteLine(string.Join("", buttons));