using System;
using System.Security.Cryptography;
using System.Text;

var salt = "cuanljph";
var valid = new List<int>();
var hashes = new Dictionary<int, string>();
int key = -1;

while (valid.Count < 64)
{
    key++;
    var hash = GetHash(key);
    int i = 0;
    for (; i < 30; i++)
    {
        if (hash[i] == hash[i + 1] && hash[i] == hash[i + 2])
            break;
    }
    if (i == 30) continue;

    var match = new string(hash[i], 5);

    if (Enumerable.Range(key + 1, 1000).Any(x =>
    {
        var mat = GetHash(x).Contains(match);
        if (mat) 
            Console.WriteLine($"{key}>{x}");
        return mat;
    }))
        valid.Add(i);
}

Console.WriteLine(key);


string GetHash(int key)
{
    if (hashes!.TryGetValue(key, out var hash))
        return hash;

    var md5 = MD5.HashData(Encoding.ASCII.GetBytes(salt + key.ToString()));
    return hashes[key] = string.Concat(md5.Select(b => b.ToString("x2")));
}