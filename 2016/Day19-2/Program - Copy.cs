int total = 3018458;
var elves = new List<ElfPresent>(Enumerable.Range(1, total).Select(i => new ElfPresent(i, 1)));
int i = 0;
int loop = 1;

while (true)
{
    var n = (i + elves.Count / 2) % elves.Count;
    elves[i].Presents += elves[n].Presents;
    elves.RemoveAt(n);
    if (elves.Count == 1) break;
    i = i + 1;
    if (i >= elves.Count) i = 0;
    if (loop++ % 10000 == 0) Console.WriteLine(elves.Count);
}

Console.WriteLine(elves[0].Elf);

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