/* 
Weapons:    Cost  Damage  Armor
Dagger        8     4       0
Shortsword   10     5       0
Warhammer    25     6       0
Longsword    40     7       0
Greataxe     74     8       0

Armor:      Cost  Damage  Armor
Leather      13     0       1
Chainmail    31     0       2
Splintmail   53     0       3
Bandedmail   75     0       4
Platemail   102     0       5

Rings:      Cost  Damage  Armor
Damage +1    25     1       0
Damage +2    50     2       0
Damage +3   100     3       0
Defense +1   20     0       1
Defense +2   40     0       2
Defense +3   80     0       3
*/

using static System.Net.Mime.MediaTypeNames;

var playerDmg = 5;
var playerAmr = 5;

var weapons = new List<(string Name, int Cost, int Dmg, int Arm)>
{
    ("Dagger"    ,  8, 4, 0),
    ("Shortsword", 10, 5, 0),
    ("Warhammer" , 25, 6, 0),
    ("Longsword" , 40, 7, 0),
    ("Greataxe"  , 74, 8, 0),
};

var armors = new List<(string Name, int Cost, int Dmg, int Arm)>
{
    ("None"      ,   0, 0, 0),
    ("Leather"   ,  13, 0, 1),
    ("Chainmail" ,  31, 0, 2),
    ("Splintmail",  53, 0, 3),
    ("Bandedmail",  75, 0, 4),
    ("Platemail" , 102, 0, 5),
};

var rings = new List<(string Name, int Cost, int Dmg, int Arm)>
{
    ("None"      ,   0, 0, 0),
    ("Damage +1" ,  25, 1, 0),
    ("Damage +2" ,  50, 2, 0),
    ("Damage +3" , 100, 3, 0),
    ("Defense +1",  20, 0, 1),
    ("Defense +2",  40, 0, 2),
    ("Defense +3",  80, 0, 3),
};

var build = GetOutfits().Where(b => PlayerWins(b.dmg, b.arm)).MinBy(b => b.cost);
Console.WriteLine($"{build}");

IEnumerable<(int dmg, int arm, string desc, int cost)> GetOutfits()
{
    foreach (var weapon in weapons)
    {
        foreach (var armor in armors)
        {
            foreach (var ring1 in rings)
            {
                foreach (var ring2 in rings.Where(r => (ring1.Name == "None" && r.Name == "None") || (ring1.Name != "None" && r.Name != ring1.Name)))
                {
                    yield return
                    (
                        weapon.Dmg + ring1.Dmg + ring2.Dmg,
                        armor.Arm + ring1.Arm + ring2.Arm,
                        $"{weapon.Name}+{armor.Name}+{ring1.Name}+{ring2.Name}",
                        weapon.Cost + armor.Cost + ring1.Cost + ring2.Cost
                    );
                }
            }
        }
    }
}

bool PlayerWins(int playerDmg, int playerAmr)
{
    /*
    Hit Points: 100
    Damage: 8
    Armor: 2
    */

    var bossHP = 100;
    var bossDmg = 8;
    var bossAmr = 2;

    var playerHP = 100;

    while (bossHP > 0 && playerHP > 0)
    {
        bossHP -= playerDmg - bossAmr;
        //Console.WriteLine($"The player deals 5-2 = 3 damage; the boss goes down to {bossHP} hit points.");
        if (bossHP > 0)
        {
            playerHP -= bossDmg - playerAmr;
            //Console.WriteLine($"The boss deals 7-5 = 2 damage; the player goes down to {playerHP} hit points.");
        }
    }

    return playerHP > 0;
}