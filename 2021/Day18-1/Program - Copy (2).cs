foreach (var line in File.ReadLines("sample.txt"))
{
    var number = new LinkedList<(long Value, char Side, int level)>();
    int pos = 0, level = 1;
    char side = 'L';
    while (++pos < line.Length)
    {
        switch (line[pos])
        {
            case '[':
                level++;
                side = 'L';
                break;
            case ',':
                side = 'R';
                break;
            case ']':
                level--;
                break;
            default:
                var end = line.IndexOfAny(new[] { '[', ',', ']' }, pos);
                number.AddLast((long.Parse(line[pos..end]), side, level));
                pos = end - 1;
                break;
        }
    }
    Console.WriteLine(string.Concat(number));
}