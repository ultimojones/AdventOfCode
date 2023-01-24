var pairs = new List<(string left, string right)>();
var correct = new List<int>();
var input = File.ReadAllLines("input.txt");
for (int i = 0; i < input.Length; i += 3)
{
    pairs.Add((input[i], input[i + 1]));
}

for (int i = 0; i < pairs.Count; i++)
{
    var left = new Segment(pairs[i].left, 0);
    var right = new Segment(pairs[i].right, 0);

    if (CompareSegments(left, right) < 0)
        correct.Add(i + 1);
}

Console.WriteLine(correct.Sum());

int CompareSegments(Segment left, Segment right)
{
    if (left.Integer.HasValue && right.Integer.HasValue)
        return left.Integer.Value - right.Integer.Value;

    if (left.Integer.HasValue)
        return CompareSegments(new Segment { List = { left } }, right);

    if (right.Integer.HasValue)
        return CompareSegments(left, new Segment { List = { right } });

    for (int i = 0; i < Math.Min(left.List.Count, right.List.Count); i++)
    {
        var compare = CompareSegments(left.List[i], right.List[i]);
        if (compare != 0) return compare;
    }
    return left.List.Count - right.List.Count;
}

class Segment
{
    public int? Integer { get; set; }
    public List<Segment> List { get; set; } = new List<Segment>();
    public int Start { get; set; }
    public int Length { get; set; }

    public Segment() { }

    public Segment(string text, int start)
    {
        Start = start;

        if (char.IsDigit(text[start]))
        {
            var value = new string(text.Skip(start).TakeWhile(char.IsDigit).ToArray());
            Integer = int.Parse(value);
            Length = value.Length;
        }
        else if (text[start] == '[')
        {
            Length = 1;
            while (text[start + Length] != ']')
            {
                if (text[start + Length] == ',')
                {
                    Length++;
                }
                else
                {
                    var inner = new Segment(text, start + Length);
                    List.Add(inner);
                    Length += inner.Length;
                }
            }
            Length++;
        }
    }
}