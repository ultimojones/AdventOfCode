var disks = new (int Positions, int Start)[]
{
    (5,2),
    (13,7),
    (17,10),
    (3,2),
    (19,9),
    (7,0),
    (11,0),
};

checked
{
    for (int i = 0; ; i++)
    {
        if (Enumerable.Range(0, disks.Length).All(d => (i + 1 + d + disks[d].Start) % disks[d].Positions == 0))
        {
            Console.WriteLine(i);
            break;
        }
    } 
}