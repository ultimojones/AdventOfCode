Console.WriteLine(
    File.ReadLines("input.txt").AsParallel()
        .Count(w =>
        {
            int vowels = 0;
            var hasDouble = false;
            for (int i = 0; i < w.Length; i++)
            {
                if (w[i] == 'a' || w[i] == 'e' || w[i] == 'i' || w[i] == 'o' || w[i] == 'u') vowels++;
                if (w.Length - i == 1) break;
                var twoChars = w.Substring(i, 2);
                if (twoChars == "ab" || twoChars == "cd" || twoChars == "pq" || twoChars == "xy")
                {
                    return false;
                }
                if (!hasDouble) hasDouble = twoChars[0] == twoChars[1];
            }
            return hasDouble && vowels >= 3;
        })
    );