using System.Security.Cryptography;

// 344266178323 too high
// 10439961859

var pkgs = File.ReadAllLines("input.txt").Select(int.Parse).OrderDescending().ToArray();

var target = pkgs.Sum() / 3;
int bestPkgs = int.MaxValue;
long bestQE = long.MaxValue;
int[] bestCombo = null!;

checked
{
    foreach (var combo in GetCombos(Array.Empty<int>(), 0, pkgs)
            .Where(c1 => c1.Length <= bestPkgs 
                && GetCombos(Array.Empty<int>(), 0, pkgs.Except(c1))
                    .Any(c2 => c1.Length <= c2.Length 
                    && GetCombos(Array.Empty<int>(), 0, pkgs.Except(c1).Except(c2)).Any(c3 =>
                    {
                        Console.WriteLine($"{string.Join(',',c1)} {string.Join(',', c2)} {string.Join(',', c3)}");
                        return c1.Length <= c3.Length;
                    }))))
    {
        if (combo.Length < bestPkgs)
        {
            bestPkgs = combo.Length;
            bestCombo = combo;
        }
        if (combo.Length == bestPkgs)
        {
            long qe = combo[0];
            for (int i = 1; i < combo.Length; i++) { qe *= combo[i]; }
            if (qe < bestQE)
            {
                bestQE = qe;
                bestCombo = combo;
                Console.WriteLine($"{string.Join(",", combo)} ({qe})");
            }
        }
    }

    Console.WriteLine($"{string.Join(",", bestCombo!)} ({bestQE})");
}

IEnumerable<int[]> GetCombos(IEnumerable<int> combo, int total, IEnumerable<int> pkgs)
{
    var remain = target - total;
    var pending = pkgs.ToArray();

    for (int i = 0; i < pending.Length; i++)
    {
        if (pending[i] == remain)
        {
            var result = combo.Append(pending[i]).ToArray();
            yield return result;
        }
        if (pending[i] < remain)
        {
            foreach (var c in GetCombos(combo.Append(pending[i]), total + pending[i], pending[(i + 1)..]))
            {
                yield return c;
            }
        }
    }
}
