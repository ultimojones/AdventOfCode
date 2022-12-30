int count = 0;
int prev = 0;
bool first = true;

var input = File.ReadLines("input.txt").Select(int.Parse).ToArray();

for (int i = 0, j = 3; j <= input.Length; i++, j++)
{
    var value = input[i..j].Sum();
    if (!first && value > prev) count++;
    prev = value;
    first = false;
}

Console.WriteLine(count);
