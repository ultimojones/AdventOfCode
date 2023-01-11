using System.Collections;

var start = ".^^^.^.^^^^^..^^^..^..^..^^..^.^.^.^^.^^....^.^...^.^^.^^.^^..^^..^.^..^^^.^^...^...^^....^^.^^^^^^^";
var last = start.Select(c => c == '^').ToArray();
var total = last.Count(b => !b);

for (int j = 1; j < 400000; j++)
{
	last = Enumerable.Range(0, last.Length).Select(i =>
	{
		return (i > 0 && last[i - 1], last[i], i < last.Length - 1 && last[i + 1]) switch
		{
			(true, true, false) => true,
			(false, true, true) => true,
			(false, false, true) => true,
			(true, false, false) => true,
			_ => false
		};
	}).ToArray();
	total += last.Count(b => !b);
    if (j % 1000 == 0)
	{
		Console.WriteLine(j);
	}
}

Console.WriteLine(total);