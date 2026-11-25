using System.Globalization;
record Student(string Name,string Group,int Score);
static class Program
{
    internal static int Compare(Student a,Student b)=>a.Score.CompareTo(b.Score);
    static readonly Student[] Templates=[new("Коваленко","КН-3/1",88),new("Мельник","КН-3/2",64),new("Іщенко","КН-3/1",93),new("Бондар","КН-3/1",71),new("Савченко","КН-3/2",55),new("Ткачук","КН-3/2",90),new("Кравець","КН-3/1",76),new("Іванів","КН-3/1",48),new("Мороз","КН-3/2",82),new("Шевчук","КН-3/1",67),new("Бойко","КН-3/2",95),new("Кушнір","КН-3/2",73)];
    internal static Student[] Data(int n){var r=new Random(14);var a=new Student[n];for(int i=0;i<n;i++){var s=Templates[i%Templates.Length];a[i]=s with {Score=r.Next(101)};}return a;}
    static Student Target(int score)=>new("","",score);
    static void Main(){CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;var raw=Data(100000);var a=(Student[])raw.Clone();Array.Sort(a,Compare);var x=Target(73);var missing=Target(101);
    Console.WriteLine("N = 14, K = 23, a = 2, b = 2, c = 4");
    Console.WriteLine("\nЗавдання 1, 2, 4: | Запит | Лінійний індекс/порівняння | Бінарний | Інтерполяційний |");
    foreach(var t in new[]{x,a[0],missing}){int l=Search.LinearSearch(a,t,Compare,out int lc),b=Search.BinarySearch(a,t,Compare,out int bc),p=Search.Interpolation(a,t,Compare,s=>s.Score,out int pc);Console.WriteLine($"| {t.Score} | {l}/{lc} | {b}/{bc} | {p}/{pc} |");}
    Console.WriteLine("\nПоломки:"); var one=new[]{x};int z=Broken.Run(one,x,Compare,1,out int c,out _);Console.WriteLine($"1: [73], X=73: {z}, порівнянь {c}");
    z=Broken.Run(a,missing,Compare,2,out c,out bool stalled);Console.WriteLine($"2: X=101: {z}, порівнянь {c}, зупинено лімітом: {stalled}");
    z=Broken.Run(a,x,Compare,3,out c,out _);int lo=1073741824,hi=1073741824;Console.WriteLine($"3: X=73: {z}, порівнянь {c}; модель переповнення mid={unchecked(lo+hi)/2}, правильний={lo+(hi-lo)/2}");
    var r=new Random(114);int misses=0,total=0;for(int i=0;i<100;i++){var t=raw[r.Next(raw.Length)];if(Broken.Run(raw,t,Compare,4,out c,out _)<0)misses++;total+=c;}Console.WriteLine($"4: пропущено {misses}/100, порівнянь {total}");
    Console.WriteLine("\nЗавдання 3");int lower=Search.Boundary(a,s=>Compare(s,x)>=0,out int lc2),upper=Search.Boundary(a,s=>Compare(s,x)>0,out int uc);int linearLower=0,linearCount=0,first7=-1;for(int i=0;i<a.Length;i++){int d=Compare(a[i],x);if(d<0)linearLower++;if(d==0)linearCount++;if(first7<0&&a[i].Score%7==0)first7=i;}Console.WriteLine($"Lower={lower} ({lc2}), Upper={upper} ({uc}), кількість={upper-lower} ({lc2+uc} порівнянь); лінійно lower={linearLower}, кількість={linearCount}, {a.Length} порівнянь");int wrong=Search.Boundary(a,s=>s.Score%7==0,out c);Console.WriteLine($"Кратність 7: межа={wrong}, правильний={first7}, збіг={wrong==first7}, порівнянь={c}");
    Edges();Measurements.Run();}
    static void Edges(){Console.WriteLine("\nКрайові випадки");void Test(string label,int[] a,int x,int expectedSearch,int expectedLower,int expectedUpper){int b=Search.BinarySearch(a,x,(u,v)=>u.CompareTo(v),out _),l=Search.Boundary(a,v=>v>=x,out _),h=Search.Boundary(a,v=>v>x,out _);bool ok=(expectedSearch==-2?b>=0:b==expectedSearch)&&l==expectedLower&&h==expectedUpper;Console.WriteLine($"{label}: пошук={b}, lower={l}, upper={h}, {(ok?"OK":"FAIL")}");}Test("Порожній",[],1,-1,0,0);Test("Один",[1],1,0,0,1);Test("Два",[1,2],2,1,1,2);Test("Менше",[2,3],1,-1,0,0);Test("Більше",[2,3],4,-1,2,2);Test("Однакові",[2,2,2],2,-2,0,3);}
}
