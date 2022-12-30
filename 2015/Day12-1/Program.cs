var input = File.ReadAllText("input.txt");

var pos = 0;
var total = 0;

var jsn = Parse();

Console.WriteLine(total);

Node Parse()
{
    if (input[pos] == '{')
    {
        pos++;
        var obj = new Obj();
        obj.Props.AddRange(ParseObj());
        return obj;
    }
    if (input[pos] == '[')
    {
        pos++;
        var arr = new Arr();
        arr.Items.AddRange(ParseArr());
        return arr;
    }
    var next = input.IndexOfAny(new[] { '}', ']', ',' }, pos);
    var val = new Val
    {
        Text = input[pos..next].Trim('"')
    };
    if (int.TryParse(val.Text, out var num))
    {
        val.Number = num;
        total += num;
    }
    pos = next;
    return val;
}

IEnumerable<(string, Node)> ParseObj()
{
    while (input[pos] != '}')
    {
        var sep = input.IndexOf(':', pos);
        string name = input[pos..sep].Trim('"');
        pos = sep + 1;
        var val = Parse();
        yield return (name, val);
        if (input[pos] == ',') { pos++; }
    }
    pos++;
}

IEnumerable<Node> ParseArr()
{
    while (input[pos] != ']')
    {
        var val = Parse();
        yield return val;
        if (input[pos] == ',') { pos++; }
    }
    pos++;
}

class Node
{
    public int Total { get; set; } = 0;
}

class Arr : Node
{
    public List<Node> Items { get; set; } = new List<Node>();
}

class Obj : Node
{
    public List<(string, Node)> Props { get; set; } = new List<(string, Node)>();
}

class Val : Node
{
    public int? Number { get; set; }
    public string? Text { get; set; }
}