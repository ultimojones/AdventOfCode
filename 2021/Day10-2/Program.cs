var scores = File.ReadLines("input.txt").Select(l =>
{
    try
    {
        int pos = 0;
        var end = ValidateChunk(ref l, ref pos);
        long score = 0;
        for (int i = 0; i < end.Length; i++)
        {
            score *= 5;
            score += end[i] switch { ')' => 1, ']' => 2, '}' => 3, '>' => 4, _ => 0 };
        }
        Console.WriteLine($"{string.Concat(end)} {score}");
        return score;
    }
    catch (InvalidDataException)
    {
        return 0;
    }    
}).Where(s => s > 0).Order().ToArray();

Console.WriteLine(string.Join(",", scores));
Console.WriteLine(scores[scores.Length / 2]);

//var test = "[(()[<>])]({[<{<<[]>>(";
//var pos = 0;
//Console.WriteLine(ValidateChunk(ref test, ref pos));

char[] ValidateChunk(ref string chunk, ref int pos)
{
    var result = Enumerable.Empty<char>();

    while (pos < chunk.Length && chunk[pos] is '(' or '[' or '{' or '<')
    {
        var closing = chunk[pos] switch { '(' => ')', '[' => ']', '{' => '}', '<' => '>', _ => ' ' };
        pos++;

        while (pos < chunk.Length && chunk[pos] is '(' or '[' or '{' or '<')
        {
            result = result.Concat(ValidateChunk(ref chunk, ref pos));
        }

        if (pos >= chunk.Length)
            return result.Append(closing).ToArray();

        if (chunk[pos] != closing)
            throw new InvalidDataException();

        pos++;
    }
    return result.ToArray();
}
