using System.Text.RegularExpressions;

Console.WriteLine(
    File.ReadLines("input.txt").AsParallel()
    //new[] { "uurcxstgmygtbstg" }
        .Count(w => Regex.IsMatch(w, @"([a-z]{2}).*\1") && Regex.IsMatch(w, @"([a-z]).\1"))
    );