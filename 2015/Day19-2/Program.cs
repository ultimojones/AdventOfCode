//var repls = @"e => H
//e => O
//H => HO
//H => OH
//O => HH".Split("\r\n").Select(l => l.Split(" => "));

//var start = "HOHOHO";

var repls = @"Al => ThF
Al => ThRnFAr
B => BCa
B => TiB
B => TiRnFAr
Ca => CaCa
Ca => PB
Ca => PRnFAr
Ca => SiRnFYFAr
Ca => SiRnMgAr
Ca => SiTh
F => CaF
F => PMg
F => SiAl
H => CRnAlAr
H => CRnFYFYFAr
H => CRnFYMgAr
H => CRnMgYFAr
H => HCa
H => NRnFYFAr
H => NRnMgAr
H => NTh
H => OB
H => ORnFAr
Mg => BF
Mg => TiMg
N => CRnFAr
N => HSi
O => CRnFYFAr
O => CRnMgAr
O => HP
O => NRnFAr
O => OTi
P => CaP
P => PTi
P => SiRnFAr
Si => CaSi
Th => ThCa
Ti => BP
Ti => TiTi
e => HF
e => NAl
e => OMg".Split("\r\n").Select(l => l.Split(" => "));

var start = "CRnCaCaCaSiRnBPTiMgArSiRnSiRnMgArSiRnCaFArTiTiBSiThFYCaFArCaCaSiThCaPBSiThSiThCaCaPTiRnPBSiThRnFArArCaCaSiThCaSiThSiRnMgArCaPTiBPRnFArSiThCaSiRnFArBCaSiRnCaPRnFArPMgYCaFArCaPTiTiTiBPBSiThCaPTiBPBSiRnFArBPBSiRnCaFArBPRnSiRnFArRnSiRnBFArCaFArCaCaCaSiThSiThCaCaPBPTiTiRnFArCaPTiBSiAlArPBCaCaCaCaCaSiRnMgArCaSiThFArThCaSiThCaSiRnCaFYCaSiRnFYFArFArCaSiRnFYFArCaSiRnBPMgArSiThPRnFArCaSiRnFArTiRnSiRnFYFArCaSiRnBFArCaSiRnTiMgArSiThCaSiThCaFArPRnFArSiRnFArTiTiTiTiBCaCaSiRnCaCaFYFArSiThCaPTiBPTiBCaSiThSiRnMgArCaF";

string[] bestSteps = null;

var results = Deconstruct(Array.Empty<string>(), start).ToArray();
var min = results.Min(r => r.Length);
foreach (var item in results.Where(r => r.Length == min))
{
    Console.WriteLine(string.Join(", ", item.Reverse()));
}

IEnumerable<string[]> Deconstruct(IEnumerable<string> steps, string molecule)
{
    var cursteps = steps.ToArray();
    if (bestSteps != null && cursteps.Length > bestSteps.Length)
        yield break;

    foreach (var r in repls.OrderByDescending(x => x[1].Length).AsParallel())
    {
        if (r[0] == "e")
        {
            if (r[1] == molecule)
            {
                var resultSteps = cursteps.Append($"{r[0]} => {r[1]}").ToArray();
                if (bestSteps == null || resultSteps.Length > bestSteps.Length)
                {
                    bestSteps = resultSteps;
                    Console.WriteLine($"{resultSteps.Length} = {string.Join(",", resultSteps.Reverse())}");
                }
                yield return resultSteps;
            }
        }
        else
        {
            int i = 0;
            while ((i = molecule.IndexOf(r[1], i)) >= 0)
            {
                foreach (var result in Deconstruct(cursteps.Append($"{r[0]}=>{r[1]}"), molecule[0..i] + r[0] + molecule[(i + r[1].Length)..]))
                {
                    yield return result;
                }
                i++;
            }
        }
    }
}