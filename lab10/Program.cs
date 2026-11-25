using System.Text;
using System.Globalization;
using static Measurements;
Console.OutputEncoding = Encoding.UTF8;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
Console.WriteLine("N = 14, K = 23, a = 2, b = 2, c = 4");
if (!File.Exists("prose.txt")) { Console.WriteLine("Додайте prose.txt (50 000+ символів української прози)."); return; }
string prose = File.ReadAllText("prose.txt");
if (prose.Length < 50000) throw new Exception("Недостатньо тексту");
string text = prose[..50000];
Console.WriteLine("Завдання 1");
foreach (int n in new[]{10000,100000})
{
    double slow = Time(()=> { string s=""; for(int i=0;i<n;i++) s+='Т'; return s; });
    double fast = Time(()=> { var s=new StringBuilder(); for(int i=0;i<n;i++) s.Append('Т'); return s.ToString(); });
    Console.WriteLine($"| {n} | {slow:F4} | {fast:F4} |");
}
var words = TextAlgorithms.Fast(text).Split(' ',StringSplitOptions.RemoveEmptyEntries);
var pairs = new (string A,string B)[100000];
var rng = new Random(14);
for(int i=0;i<pairs.Length;i++){ string w=words[rng.Next(words.Length)]; pairs[i]=(w,i%2==0?w.ToLowerInvariant():words[rng.Next(words.Length)]); }
double lower=Time(()=>{int count=0; foreach(var p in pairs) if(p.A.ToLower()==p.B.ToLower()) count++; return count;});
double ordinal=Time(()=>{int count=0; foreach(var p in pairs) if(string.Equals(p.A,p.B,StringComparison.OrdinalIgnoreCase)) count++; return count;});
Console.WriteLine($"ToLower: {lower:F4}; OrdinalIgnoreCase: {ordinal:F4}");
string[] alphabet={"ґанок","ганок","ґава","гавань","ґрунт","груша","ґудзик","гудок","ґедзь","герой","ґрати","грати","ґатунок","газета","ґлей","глина","ґніт","гніздо","ґуля","гуляти"};
var ord=(string[])alphabet.Clone(); var cult=(string[])alphabet.Clone();
Array.Sort(ord,StringComparer.Ordinal); Array.Sort(cult,StringComparer.Create(CultureInfo.GetCultureInfo("uk-UA"),false));
Console.WriteLine("Ordinal: "+string.Join(", ",ord)); Console.WriteLine("uk-UA: "+string.Join(", ",cult));
Console.WriteLine("Завдання 2");
foreach(int n in new[]{1000,10000,50000}) { string s=text[..n]; TextAlgorithms.Check(s); double x=Time(()=>TextAlgorithms.Slow(s)), y=Time(()=>TextAlgorithms.Fast(s)); Console.WriteLine($"| {n} | {x:F4} | {y:F4} | {x/y:F2} | збіг=True"); }
Console.WriteLine("Завдання 3");
string present="";
foreach(string word in text.Split((char[]?)null,StringSplitOptions.RemoveEmptyEntries)) if(word.Length>=6 && TextAlgorithms.IndexOf(text,word,out _) > 1000){present=word;break;}
if(present.Length==0) present=text.Substring(1000,30);
string absent="¤¤¤¤"; while(TextAlgorithms.IndexOf(text,absent,out _)>=0) absent=string.Concat(absent,"¤");
foreach(var pair in new[]{(text,present),(text,absent),(new string('Т',10000),new string('Т',1129)+"У")})
{
    int pos=TextAlgorithms.IndexOf(pair.Item1,pair.Item2,out int count); int reference=pair.Item1.IndexOf(pair.Item2,StringComparison.Ordinal); if(pos!=reference)throw new Exception("Пошук");
    double x=Time(()=>TextAlgorithms.IndexOf(pair.Item1,pair.Item2,out _)), y=Time(()=>pair.Item1.IndexOf(pair.Item2,StringComparison.Ordinal));
    Console.WriteLine($"| {pair.Item1.Length} | {(pair.Item2.Length>40?"Т×1129 + У":pair.Item2)} | {pos} | {count} | {x:F4} | {y:F4} |");
}
Console.WriteLine("Завдання 4");
Item[] Make(Random r,int n){var a=new Item[n];for(int i=0;i<n;i++)a[i]=new(r.Next(1,21),r.Next(1,101));return a;}
rng=new Random(14); int optimal=0; double worst=0; Item[]? smallest=null; int sg=0,so=0;
for(int t=0;t<100;t++){var a=Make(rng,rng.Next(1,16));int g=Greedy.Solve(a,33), o=Exhaustive.Solve(a,33,false,out _); if(g==o)optimal++;else if(smallest==null||a.Length<smallest.Length){smallest=a;sg=g;so=o;} worst=Math.Max(worst,100.0*(o-g)/Math.Max(1,o));}
Console.WriteLine($"Оптимум: {optimal}/100; найгірше відставання: {worst:F2}%");
if(smallest!=null)Console.WriteLine($"Найменший контрприклад: {string.Join<Item>(", ",smallest)}; жадібно={sg}; оптимум={so}");
foreach(int n in new[]{15,20}){var a=Make(rng,n);int x=Exhaustive.Solve(a,33,false,out long b1),y=Exhaustive.Solve(a,33,true,out long b2);if(x!=y)throw new Exception("Перебір");Console.WriteLine($"n={n}: без відсікання {b1}, з відсіканням {b2}; час без={Time(()=>Exhaustive.Solve(a,33,false,out _)):F4} мс; час з={Time(()=>Exhaustive.Solve(a,33,true,out _)):F4} мс; оптимум={x}");}
Console.WriteLine("Крайові випадки");
foreach(var s in new[]{"","а","аааа","  А  Б  "}){TextAlgorithms.Check(s);Console.WriteLine($"[{s}] -> [{TextAlgorithms.Fast(s)}]: OK");}
foreach(var p in new[]{("","а"),("а","аб"),("а",""),("","")}){if(TextAlgorithms.IndexOf(p.Item1,p.Item2,out _)!=p.Item1.IndexOf(p.Item2,StringComparison.Ordinal))throw new Exception("Крайовий випадок");Console.WriteLine($"Пошук [{p.Item1}]/[{p.Item2}]: OK");}
foreach(var a in new[]{Array.Empty<Item>(),new[]{new Item(2,5)}}){if(Greedy.Solve(a,33)!=Exhaustive.Solve(a,33,false,out _))throw new Exception("Рюкзак");Console.WriteLine($"Рюкзак {a.Length}: OK");}
