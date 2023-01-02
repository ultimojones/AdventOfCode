using System.Numerics;

// 1771

var bossStartHP = 71;
var bossDmg = 10;

var playerStartHP = 50;
var playerStartMana = 500;

string[] bestActions = null!;
int lowestCost = int.MaxValue;

CalculateRound(Array.Empty<string>(), 0, 1, playerStartHP, 0, playerStartMana, bossStartHP, 0, 0, 0);

//Console.WriteLine();
//Console.WriteLine(lowestCost);
//Console.WriteLine(string.Join(", ", bestActions));

void CalculateRound(IEnumerable<string> actions, int cost, int turn, int playerHP, int playerArmor, int playerMana, int bossHP, int effectShield, int effectPoison, int effectRecharge)
{
    if (cost > lowestCost)
        return;

    //Console.WriteLine();
    //Console.WriteLine(int.IsOddInteger(turn) ? $"-- Player turn ({turn})--" : $"-- Boss turn ({turn})--");
    //Console.WriteLine($"- Player has {playerHP} hit points, {playerArmor} armor, {playerMana} mana");
    //Console.WriteLine($"- Boss has {bossHP} hit points");

    if (effectShield > 0)
    {
        effectShield--;
        //Console.WriteLine($"Shield's timer is now {effectShield}.");
        if (effectShield == 0)
        {
            //Console.WriteLine($"Shield wears off, decreasing armor by 7.");
            playerArmor -= 7;
        }
    }

    if (effectPoison > 0)
    {
        bossHP -= 3;
        if (bossHP <= 0)
        {
            //Console.WriteLine("Poison deals 3 damage. This kills the boss, and the player wins.");
            if (cost < lowestCost)
            {
                lowestCost = cost;
                bestActions = actions.ToArray();
                Console.WriteLine($"{lowestCost,5} = {string.Join(",", bestActions)}");
            }
            return;
        }
        effectPoison--;
        //Console.WriteLine($"Poison deals 3 damage; its timer is now {effectPoison}.");
        if (effectPoison == 0)
        {
            //Console.WriteLine("Poison wears off.");
        }
    }

    if (effectRecharge > 0)
    {
        playerMana += 101;
        effectRecharge--;
        //Console.WriteLine($"Recharge provides 101 mana; its timer is now {effectRecharge}.");
        if (effectRecharge == 0)
        {
            //Console.WriteLine("Recharge wears off.");
        }
    }

    if (int.IsEvenInteger(turn))
    {
        if (playerArmor > 0)
        {
            playerHP -= (bossDmg - playerArmor);
            //Console.WriteLine($"Boss attacks for {bossDmg} - {playerArmor} =  {bossDmg - playerArmor} damage!");
        }
        else
        {
            playerHP -= bossDmg;
            //Console.WriteLine($"Boss attacks for {bossDmg} damage!");
        }
        if (playerHP <= 0)
        {
            //Console.WriteLine("This kills the player. Boss wins.");
            return;
        }
        CalculateRound(actions, cost, turn + 1, playerHP, playerArmor, playerMana, bossHP, effectShield, effectPoison, effectRecharge);
        return;
    }

    if (playerMana < 53)
    {
        //Console.WriteLine("The player does not have enough mana. Boss wins.");
        return;
    }

    if (playerMana >= 229 && effectRecharge == 0)
    {
        //Console.WriteLine("Player casts Recharge.");
        CalculateRound(actions.Append("Recharge"), cost + 229, turn + 1, playerHP, playerArmor, playerMana - 229, bossHP, effectShield, effectPoison, effectRecharge + 5);
    }

    if (playerMana >= 113 && effectShield == 0)
    {
        //Console.WriteLine("Player casts Shield, increasing armor by 7.");
        CalculateRound(actions.Append("Shield"), cost + 113, turn + 1, playerHP, playerArmor + 7, playerMana - 113, bossHP, effectShield + 6, effectPoison, effectRecharge);
    }

    if (playerMana >= 73)
    {
        if (bossHP <= 2)
        {
            //Console.WriteLine("Player casts Drain, dealing 2 damage, and healing 2 hit points. This kills the boss, and the player wins.");
            if (cost < lowestCost)
            {
                lowestCost = cost;
                bestActions = actions.ToArray();
                Console.WriteLine($"{lowestCost,5} = {string.Join(",", bestActions)}");
            }
            return;
        }
        //Console.WriteLine("Player casts Drain, dealing 2 damage, and healing 2 hit points.");
        CalculateRound(actions.Append("Drain"), cost + 73, turn + 1, playerHP + 2, playerArmor, playerMana - 73, bossHP - 2, effectShield, effectPoison, effectRecharge);
    }

    if (playerMana >= 173 && effectPoison == 0)
    {
        //Console.WriteLine("Player casts Poison.");
        CalculateRound(actions.Append("Poison"), cost + 173, turn + 1, playerHP, playerArmor, playerMana - 173, bossHP, effectShield, effectPoison + 6, effectRecharge);
    }

    if (playerMana >= 53)
    {
        if (bossHP <= 4)
        {
            //Console.WriteLine("Player casts Magic Missile, dealing 4 damage. This kills the boss, and the player wins.");
            if (cost < lowestCost)
            {
                lowestCost = cost;
                bestActions = actions.ToArray();
                Console.WriteLine($"{lowestCost,5} = {string.Join(",", bestActions)}");
            }
            return;
        }
        //Console.WriteLine("Player casts Magic Missile, dealing 4 damage.");
        CalculateRound(actions.Append("MagicMissile"), cost + 53, turn + 1, playerHP, playerArmor, playerMana - 53, bossHP - 4, effectShield, effectPoison, effectRecharge);
    }
}