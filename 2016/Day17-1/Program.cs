using System.Security.Cryptography;
using System.Text;

byte[] code = Encoding.ASCII.GetBytes("vwbaicqe");
var codeLen = code.Length;

byte[] bestPathData = null;
string? bestPath = null;

byte U = Encoding.ASCII.GetBytes("U")[0];
byte D = Encoding.ASCII.GetBytes("D")[0];
byte L = Encoding.ASCII.GetBytes("L")[0];
byte R = Encoding.ASCII.GetBytes("R")[0];

CalcPaths((1, 1), code);

Console.WriteLine();
Console.WriteLine(bestPath);

void CalcPaths((int X, int Y) pos, byte[] pathData)
{
    if (pos == (4, 4))
    {
        if (bestPathData is null || pathData.Length < bestPathData.Length) 
        { 
            bestPathData = pathData;
            bestPath = Encoding.ASCII.GetString(pathData[codeLen..]);
            Console.WriteLine(bestPath);
        }
        return;
    }

    if (pathData.Length > 100 || bestPathData is not null && pathData.Length > bestPathData.Length)
    {
        return;
    }

    var hash = MD5.HashData(pathData);
    var hashVals = hash[0].ToString("x2") + hash[1].ToString("x2");
    Array.Resize(ref pathData, pathData.Length + 1);
    if (hashVals[0] >= 'b' && pos.Y > 1)
    {
        pathData[pathData.Length - 1] = U;
        CalcPaths((pos.X, pos.Y - 1), pathData);
    }
    if (hashVals[1] >= 'b' && pos.Y < 4)
    {
        pathData[pathData.Length - 1] = D;
        CalcPaths((pos.X, pos.Y + 1), pathData);
    }
    if (hashVals[2] >= 'b' && pos.X > 1)
    {
        pathData[pathData.Length - 1] = L;
        CalcPaths((pos.X - 1, pos.Y), pathData);
    }
    if (hashVals[3] >= 'b' && pos.X < 4)
    {
        pathData[pathData.Length - 1] = R;
        CalcPaths((pos.X + 1, pos.Y), pathData);
    }
}