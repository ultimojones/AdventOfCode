//Player 1 starting position: 4
//Player 2 starting position: 8

//Player 1 starting position: 6
//Player 2 starting position: 4

int p1Pos = 6, p2Pos = 4;
int p1Scr = 0, p2Scr = 0, dice = 1, rolls = 0;

while (p1Scr < 1000 && p2Scr < 1000)
{
    p1Scr += p1Pos = (p1Pos + RollDice() - 1) % 10 + 1;
    if (p1Scr >= 1000) break;

    p2Scr += p2Pos = (p2Pos + RollDice() - 1) % 10 + 1;
}

Console.WriteLine($"Losing = {int.Min(p1Scr, p2Scr)}");
Console.WriteLine($"Rolls = {rolls}");
Console.WriteLine($"Total = {int.Min(p1Scr, p2Scr) * rolls}");

int RollDice()
{
    var nums = Enumerable.Range(dice, 4).Select(d => (d - 1) % 100 + 1).ToArray();
    dice = nums[3];
    rolls += 3;
    return nums[0..3].Sum();
}