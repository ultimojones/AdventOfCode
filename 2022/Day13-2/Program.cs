var signals = File.ReadAllLines("input.txt").Where(l => l.Length > 0).ToList();
signals.Add("[[2]]");
signals.Add("[[6]]");

var sorted = signals.OrderBy(s => new Segment(s, 0)).ToList();

var mark2 = sorted.IndexOf("[[2]]") + 1;
var mark6 = sorted.IndexOf("[[6]]") + 1;

Console.WriteLine(mark2 * mark6);

class Segment : IComparable<Segment>
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

    public int CompareTo(Segment? other)
    {
        if (other == null)
            return 1;

        if (Integer.HasValue && other.Integer.HasValue)
            return Integer.Value - other.Integer.Value;

        if (Integer.HasValue)
            return new Segment { List = { this } }.CompareTo(other);

        if (other.Integer.HasValue)
            return CompareTo(new Segment { List = { other } });

        for (int i = 0; i < Math.Min(List.Count, other.List.Count); i++)
        {
            var compare = List[i].CompareTo(other.List[i]);
            if (compare != 0) return compare;
        }
        return List.Count - other.List.Count;
    }
}