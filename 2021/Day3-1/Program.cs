using System.Collections;

var report = File.ReadAllLines("input.txt");

var bitlen = report[0].Length;
var gamma = "";
var epsilon = "";

for (int i = 0; i < bitlen; i++)
{
    var cnt0 = report.Count(x => x[i] == '0');
    var cnt1 = report.Count(x => x[i] == '1');
    gamma += cnt0 > cnt1 ? '0' : '1';
    epsilon += cnt0 > cnt1 ? '1' : '0';
}

int ParseBinary(string input)
{
    var result = 0;
    for (int i = 0; i < input.Length; i++)
    {
        result = result * 2 + (input[i] - '0');
    }
    return result;
}

var bag = new BitArray(gamma.Select(c => c == '1').ToArray());

Console.WriteLine($"{gamma} {epsilon} {ParseBinary(gamma) * ParseBinary(epsilon)}");
