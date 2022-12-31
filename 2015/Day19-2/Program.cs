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
    Console.WriteLine(string.Join(",", item.Reverse()));
}
Console.WriteLine(min);


IEnumerable<string[]> Deconstruct(IEnumerable<string> steps, string molecule)
{
    var final = repls.FirstOrDefault(r => r[0] == "e" && r[1] == molecule);
    if (final != null)
    {
        var finalSteps = steps.Append($"{final[0]}=>{final[1]}").ToArray();
        if (bestSteps == null || finalSteps.Length > bestSteps.Length)
        {
            bestSteps = finalSteps;
            Console.WriteLine($"{finalSteps.Length} = {string.Join(",", finalSteps.Reverse())}");
            yield return finalSteps;
        }
    }
    else
    {
        foreach (var results in repls.Where(r => molecule.Contains(r[1]))
            .OrderByDescending(o => o[1].Length).SelectMany(r => DeconstructReplace(steps, molecule, r)))
        {
            yield return results;
        }
    }
}

IEnumerable<string[]> DeconstructReplace(IEnumerable<string> steps, string molecule, string[] r)
{
    var nextStep = steps.Append($"{r[0]}=>{r[1]}");
    int i = 0;
    while ((i = molecule.IndexOf(r[1], i)) >= 0)
    {
        foreach (var result in Deconstruct(nextStep, molecule[0..i] + r[0] + molecule[(i + r[1].Length)..]))
        {
            yield return result;
        }
        i++;
    }
}
