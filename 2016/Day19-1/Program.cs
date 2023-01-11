var elves = Enumerable.Repeat(1, 3018458).ToArray();

int i = 0;

while (true)
{
	while (elves[i] == 0) { i = (i + 1) % elves.Length; }

    var n = (i + 1) % elves.Length;
	while (true)
	{
		if (elves[n] > 0)
		{
			elves[i] += elves[n];
			elves[n] = 0;
			break;
		}
		n = (n + 1) % elves.Length;
	}
	if (elves[i] == elves.Length)
		break;
	i = (n + 1) % elves.Length;
}

Console.WriteLine(i + 1);