var repls = @"e => H
e => O
H => HO
H => OH
O => HH".Split("\r\n").Select(l => l.Split(" => "));

var start = "HOHOHO";

//var repls = @"Al => ThF
//Al => ThRnFAr
//B => BCa
//B => TiB
//B => TiRnFAr
//Ca => CaCa
//Ca => PB
//Ca => PRnFAr
//Ca => SiRnFYFAr
//Ca => SiRnMgAr
//Ca => SiTh
//F => CaF
//F => PMg
//F => SiAl
//H => CRnAlAr
//H => CRnFYFYFAr
//H => CRnFYMgAr
//H => CRnMgYFAr
//H => HCa
//H => NRnFYFAr
//H => NRnMgAr
//H => NTh
//H => OB
//H => ORnFAr
//Mg => BF
//Mg => TiMg
//N => CRnFAr
//N => HSi
//O => CRnFYFAr
//O => CRnMgAr
//O => HP
//O => NRnFAr
//O => OTi
//P => CaP
//P => PTi
//P => SiRnFAr
//Si => CaSi
//Th => ThCa
//Ti => BP
//Ti => TiTi
//e => HF
//e => NAl
//e => OMg".Split("\r\n").Select(l => l.Split(" => "));

//var start = "CRnCaCaCaSiRnBPTiMgArSiRnSiRnMgArSiRnCaFArTiTiBSiThFYCaFArCaCaSiThCaPBSiThSiThCaCaPTiRnPBSiThRnFArArCaCaSiThCaSiThSiRnMgArCaPTiBPRnFArSiThCaSiRnFArBCaSiRnCaPRnFArPMgYCaFArCaPTiTiTiBPBSiThCaPTiBPBSiRnFArBPBSiRnCaFArBPRnSiRnFArRnSiRnBFArCaFArCaCaCaSiThSiThCaCaPBPTiTiRnFArCaPTiBSiAlArPBCaCaCaCaCaSiRnMgArCaSiThFArThCaSiThCaSiRnCaFYCaSiRnFYFArFArCaSiRnFYFArCaSiRnBPMgArSiThPRnFArCaSiRnFArTiRnSiRnFYFArCaSiRnBFArCaSiRnTiMgArSiThCaSiThCaFArPRnFArSiRnFArTiTiTiTiBCaCaSiRnCaCaFYFArSiThCaPTiBPTiBCaSiThSiRnMgArCaF";

var results = Deconstruct(Array.Empty<string>(), start);

foreach (var item in results)
{
    Console.WriteLine(string.Join(", ", item));
}

IEnumerable<IEnumerable<string>> Deconstruct(IEnumerable<string> steps, string molecule)
{
    var cursteps = steps.ToArray();

    //var matches = 0;
    foreach (var r in repls)
    {
        if (r[0] == "e")
        {
            if (r[1] == molecule)
            {
                yield return cursteps.Append($"{r[0]} => {r[1]}");
            }
        }
        else
        {
            int i = 0;
            while ((i = molecule.IndexOf(r[1], i)) >= 0)
            {
                foreach (var result in Deconstruct(cursteps.Append($"{r[0]} => {r[1]}"), molecule[0..i] + r[0] + molecule[(i + r[1].Length)..]))
                {
                    yield return result;
                }
            }
        }
    }
}