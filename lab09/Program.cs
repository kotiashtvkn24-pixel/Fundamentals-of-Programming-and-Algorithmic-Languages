using System.Globalization;
using System.Text;
record Student(string Name,string Group,int Score);
static class Program
{
    static int C(int a,int b)=>a.CompareTo(b);
    static int S(Student a,Student b)=>a.Score.CompareTo(b.Score);
    static int G(Student a,Student b)=>string.CompareOrdinal(a.Group,b.Group);
    static int Both(Student a,Student b){int c=G(a,b);return c!=0?c:S(a,b);}
    static Student[] Students()=>new Student[]{new("Коваленко","КН-3/1",88),new("Мельник","КН-3/2",64),new("Іщенко","КН-3/1",93),new("Бондар","КН-3/1",71),new("Савченко","КН-3/2",55),new("Ткачук","КН-3/2",90),new("Кравець","КН-3/1",76),new("Іванів","КН-3/1",48),new("Мороз","КН-3/2",82),new("Шевчук","КН-3/1",67),new("Бойко","КН-3/2",95),new("Кушнір","КН-3/2",73)};
    static void Main(){Console.OutputEncoding=Encoding.UTF8;CultureInfo.CurrentCulture=CultureInfo.InvariantCulture;
        Console.WriteLine("N = 14, K = 23, a = 2, b = 2, c = 4");
        var t=Measurements.Data(9);var orig=(int[])t.Clone();Console.WriteLine("Трасування: "+string.Join(", ",t));Sorts.BubbleCore(t,C,out _,out _,(p,a)=>Console.WriteLine($"| {p} | {string.Join(", ",a)} |"));Checks.Verify(orig,t,C);
        double ratio=0;
        foreach(var (name,sort) in new (string,Measurements.Sorter<int>)[]{("Bubble",Sorts.Bubble<int>),("Quick",Sorts.Quick<int>)}){Console.WriteLine(name+"\n| n | Випадковий | Відсортований | Навпаки | Майже |\n|---|---|---|---|---|");foreach(int n in new[]{100,1000,10000}){Console.Write($"| {n} |");int index=0;foreach(var a in Measurements.Inputs(n)){var b=(int[])a.Clone();sort(b,C,out int c,out int m);Checks.Verify(a,b,C);Console.Write($" {c}/{m} |");if(n==10000&&index==0){if(name=="Bubble")ratio=c;else ratio/=c;}index++;}Console.WriteLine();}}
        var equal=new int[10000];Sorts.Quick(equal,C,out int ec,out int em);Checks.Verify(new int[10000],equal,C);Console.WriteLine($"Quick, однакові: {ec}/{em}; схема з двома індексами не вироджується в квадрат.");Console.WriteLine($"Відношення Bubble/Quick: {ratio:F2}");
        var st=Students();var one=(Student[])st.Clone();Sorts.Quick(one,Both,out int oc,out int om);Checks.Verify(st,one,Both);var two=(Student[])st.Clone();Sorts.Bubble(two,S,out int c1,out int m1);Checks.Verify(st,two,S);Sorts.Bubble(two,G,out int c2,out int m2);Checks.Verify(st,two,Both);bool same=true;for(int i=0;i<one.Length;i++)if(one[i]!=two[i])same=false;Console.WriteLine($"Два ключі: один прохід {oc}/{om}; два стабільні проходи {c1+c2}/{m1+m2}; збіг={same}");foreach(var s in one)Console.WriteLine(s);
        Console.WriteLine("Час, мс\n| n | Bubble | Quick | Array.Sort |\n|---|---|---|---|");int limit=0;foreach(int n in new[]{1000,10000,100000,1000000}){var a=Measurements.Data(n);string b="—";if(n<=100000){double ms=Measurements.Time(a,C,x=>Sorts.Bubble(x,C,out _,out _));b=ms.ToString("F4");if(ms>=1000&&limit==0)limit=n;}double q=Measurements.Time(a,C,x=>Sorts.Quick(x,C,out _,out _));double std=Measurements.Time(a,C,x=>Array.Sort(x,C));Console.WriteLine($"| {n} | {b} | {q:F4} | {std:F4} |");}
        if(limit>0){int lo=limit/10,hi=limit;while(hi-lo>1000){int mid=(lo+hi)/2;double ms=Measurements.Time(Measurements.Data(mid),C,x=>Sorts.Bubble(x,C,out _,out _));if(ms>=1000)hi=mid;else lo=mid;}Console.WriteLine($"Межа 1 с (наближено): {hi}, інтервал {lo}–{hi}");}else Console.WriteLine("Межа 1 с не знайдена до 100000");
        Console.WriteLine("Час двох ключів на студентах ЛР6, мс: один="+Measurements.Time(st,Both,x=>Sorts.Quick(x,Both,out _,out _)).ToString("F4")+", два="+Measurements.Time(st,Both,x=>{Sorts.Bubble(x,S,out _,out _);Sorts.Bubble(x,G,out _,out _);}).ToString("F4"));
        // Початкові бали унікальні: додатковий набір створює повтори для експерименту.
        var dup=(Student[])st.Clone();for(int i=0;i<dup.Length;i++)dup[i]=dup[i] with {Score=dup[i].Score/10*10};
        foreach(var sample in new[]{st,dup}){Sorts.Bubble(sample,(x,y)=>string.CompareOrdinal(x.Name,y.Name),out _,out _);Console.WriteLine(ReferenceEquals(sample,st)?"Стабільність: ЛР6 без повторів":"Стабільність: додаткові повтори");foreach(var (name,action,expected) in new (string,Action<Student[]>,string)[]{("Bubble",x=>Sorts.Bubble(x,S,out _,out _),"так"),("Quick",x=>Sorts.Quick(x,S,out _,out _),"не гарантовано"),("Array.Sort",x=>Array.Sort(x,S),"не гарантовано"),("OrderBy",x=>{var y=System.Linq.Enumerable.ToArray(System.Linq.Enumerable.OrderBy(x,s=>s.Score));Array.Copy(y,x,x.Length);},"так")}){var a=(Student[])sample.Clone();action(a);Checks.Verify(sample,a,S);string counter="—";for(int i=1;i<a.Length;i++)if(a[i-1].Score==a[i].Score&&string.CompareOrdinal(a[i-1].Name,a[i].Name)>0){counter=$"{a[i-1].Name} / {a[i].Name} ({a[i].Score})";break;}Console.WriteLine($"| {name} | {expected} | {(counter=="—"?"порядок збережено":"ні")} | {counter} |");}}
        Console.WriteLine("Крайові випадки");foreach(var a in new int[][]{Array.Empty<int>(),new[]{1},new[]{1,2},new[]{2,1},new[]{3,3,3,3},new[]{2,2,1,1}}){foreach(var sort in new Measurements.Sorter<int>[]{Sorts.Bubble<int>,Sorts.Quick<int>}){var b=(int[])a.Clone();sort(b,C,out _,out _);Checks.Verify(a,b,C);}Console.WriteLine($"[{string.Join(",",a)}] OK");}
    }
}
