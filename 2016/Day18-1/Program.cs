var grid = new LinkedList<string>();

grid.AddFirst(".^^^.^.^^^^^..^^^..^..^..^^..^.^.^.^^.^^....^.^...^.^^.^^.^^..^^..^.^..^^^.^^...^...^^....^^.^^^^^^^");

while (grid.Count < 40)
{
	var last = grid.Last();
	var next = new string(Enumerable.Range(0, last.Length).Select(i =>
	{
		return (i > 0 && last[i - 1] == '^', last[i] == '^', i < last.Length - 1 && last[i + 1] == '^') switch
		{
			(true, true, false) => '^',
			(false, true, true) => '^',
			(false, false, true) => '^',
			(true, false, false) => '^',
			_ => '.'
		};
	}).ToArray());
	grid.AddLast(next);
}

grid.ToList().ForEach(Console.WriteLine);
Console.WriteLine(grid.Sum(l => l.Count(c => c == '.')));