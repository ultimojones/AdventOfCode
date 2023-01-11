int total = 5;// 3018458;
var elves = Enumerable.Range(1, total).ToList();
int i = 0;
int loop = 1;

// 26735 too low

while (true)
{
    var n = (i + elves.Count / 2) % elves.Count;
    elves.RemoveAt(n);
    if (elves.Count == 1) break;
    i = i + 1;
    if (i >= elves.Count) i = 0;
    if (loop++ % 10000 == 0) Console.WriteLine(elves.Count);
}

Console.WriteLine();
Console.WriteLine(elves[0]);

class ElfPresent
{
    public ElfPresent(int Elf, int Presents)
    {
        this.Elf = Elf;
        this.Presents = Presents;
    }

    public override string ToString()
    {
        return $"{Elf}: {Presents}";
    }

    public int Elf { get; set; }
    public int Presents { get; set; }
}