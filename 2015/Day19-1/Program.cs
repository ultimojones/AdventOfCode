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

var molecules = repls.SelectMany(r => Enumerable.Range(0, start.Length - r[0].Length + 1).Where(i => start[i..(i + r[0].Length)] == r[0]).Select(i => start[0..i] + r[1] + start[(i + r[0].Length)..])).Distinct().ToList();

molecules.ForEach(Console.WriteLine);
Console.WriteLine();
Console.WriteLine(molecules.Count);