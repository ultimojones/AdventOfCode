var buttons = new List<char>();

char pos = '5';

foreach (var line in File.ReadLines("input.txt"))
{
	foreach (var dir in line)
	{
		pos = (pos, dir) switch
		{
			('3', 'U') => '1',
			('3', 'L') or ('6', 'U') => '2',
			('1', 'D') or ('2', 'R') or ('4', 'L') or ('7', 'U') => '3',
            ('3', 'R') or ('8', 'U') => '4',
			('6', 'L') => '5',
			('2', 'D') or ('5', 'R') or ('7', 'L') or ('A', 'U') => '6',
			('3', 'D') or ('6', 'R') or ('8', 'L') or ('B', 'U') => '7',
			('4', 'D') or ('7', 'R') or ('9', 'L') or ('C', 'U') => '8',
            ('8', 'R') => '9',
            ('6', 'D') or ('B', 'L') => 'A',
            ('7', 'D') or ('A', 'R') or ('C', 'L') or ('D', 'U') => 'B',
            ('8', 'D') or ('B', 'R') => 'C',
            ('B', 'D') => 'D',
            _ => pos
        };
		Console.WriteLine($"{dir}={pos}");
	}
	buttons.Add(pos);
	Console.WriteLine(pos);
}
Console.WriteLine(string.Join("", buttons));