using System.Collections;

var report = File.ReadAllLines("input.txt");

var bitlen = report[0].Length;

var oxygen = report.ToList();
var co2 = report.ToList();

for (int i = 0; i < bitlen; i++)
{
    var cnt0 = oxygen.Count(x => x[i] == '0');
    var cnt1 = oxygen.Count(x => x[i] == '1');

    var num = cnt0 > cnt1 ? '0' : '1';
    oxygen = oxygen.Where(x => x[i] == num).ToList();
    if (oxygen.Count == 1)
        break;
}

for (int i = 0; i < bitlen; i++)
{
    var cnt0 = co2.Count(x => x[i] == '0');
    var cnt1 = co2.Count(x => x[i] == '1');

    var num = cnt0 <= cnt1 ? '0' : '1';
    co2 = co2.Where(x => x[i] == num).ToList();
    if (co2.Count == 1)
        break;
}


Console.WriteLine(oxygen[0]);
Console.WriteLine(co2[0]);
Console.WriteLine($"{ParseBinary(oxygen[0]) * ParseBinary(co2[0])}");


int ParseBinary(string input)
{
    var result = 0;
    for (int i = 0; i < input.Length; i++)
    {
        result = result * 2 + (input[i] - '0');
    }
    return result;
}

