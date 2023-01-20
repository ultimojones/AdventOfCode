var score = File.ReadLines("input.txt").Sum(l =>
{
    int pos = 0;
    return ValidateChunk(ref l, ref pos) switch
    {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => 0
    };
});

Console.WriteLine(score);

char? ValidateChunk(ref string chunk, ref int pos)
{
    var closing = chunk[pos] switch { '(' => ')', '[' => ']', '{' => '}', '<' => '>', _ => ' ' };
    pos++;
    while (pos < chunk.Length && chunk[pos] is '(' or '[' or '{' or '<')
    {
        var closingChar = ValidateChunk(ref chunk, ref pos);
        if (closingChar is not null) { return closingChar; }
    }
    if (pos < chunk.Length && chunk[pos] == closing)
    {
        pos++;
        return null;
    }
    return pos < chunk.Length ? chunk[pos] : null;
}