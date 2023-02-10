//Player 1 starting position: 4
//Player 2 starting position: 8

//Player 1 starting position: 6
//Player 2 starting position: 4

using System.Collections.Concurrent;

var combos = GetRolls().ToArray();
var queued = new ConcurrentStack<(int P1Pos, int P1Scr, int P2Pos, int P2Scr, byte Turn)>();
long p1Wins = 0, p2Wins = 0;
var timer = new Timer(s => Console.WriteLine($"{p1Wins,12} {p2Wins,12}"), null, 0, 60000);
queued.Push((4, 0, 8, 0, 1));

var tasks = Enumerable.Range(0, 12).Select(i => new Task(RunTest, TaskCreationOptions.LongRunning)).ToList();
tasks.ForEach(t => t.Start());
Task.WaitAll(tasks.ToArray());

Console.WriteLine(p1Wins);
Console.WriteLine(p2Wins);


void RunTest()
{
    var running = false;
    while (!running)
    {
        while (queued.TryPop(out var status))
        {
            running = true;
            foreach (var combo in combos)
            {
                var result = status;
                if (status.Turn == 1)
                {
                    result.P1Scr += result.P1Pos = (result.P1Pos + combo - 1) % 10 + 1;
                    result.Turn = 2;
                }
                else
                {
                    result.P2Scr += result.P2Pos = (result.P2Pos + combo - 1) % 10 + 1;
                    result.Turn = 1;
                }
                if (result.P1Scr > 20)
                {
                    Interlocked.Increment(ref p1Wins);
                }
                else if (result.P2Scr > 20)
                {
                    Interlocked.Increment(ref p2Wins);
                }
                else
                {
                    queued.Push(result);
                }
            }
        }
        Task.Yield();
    }
}

IEnumerable<int> GetRolls()
{
    var faces = Enumerable.Range(1, 3);
    return faces.SelectMany(a => faces.SelectMany(b => faces.Select(c => (a + b + c))));
}
