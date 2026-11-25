using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static Algorithms;

Console.OutputEncoding = Encoding.UTF8;
const int N = 14, K = 23;
Console.WriteLine($"N = {N}, K = {K}, a = {N%3}, b = {N%4}, c = {N%5}");
var random = new Random(N);
var numbers = new List<int>();
for (int i = 0; i < 20; i++) numbers.Add(random.Next(-K,K+1));
var words = new List<string> { "код", "алгоритм", "ІТ", "веб-сайт", "дані", "функція", "база", "блок-схема", "клас", "делегат", "об'єкт", "інтерфейс" };
var students = new List<Student> {
    new("Коваленко","КН-3/1",88),
    new("Мельник","КН-3/2",64),
    new("Іщенко","КН-3/1",93),
    new("Бондар","КН-3/1",71),
    new("Савченко","КН-3/2",55),
    new("Ткачук","КН-3/2",90),
    new("Кравець","КН-3/1",76),
    new("Іванів","КН-3/1",48),
    new("Мороз","КН-3/2",82),
    new("Шевчук","КН-3/1",67),
    new("Бойко","КН-3/2",95),
    new("Кушнір","КН-3/2",73)
};
void Show<T>(string label, List<T> items) => Console.WriteLine($"{label}: {string.Join(", ",items)} (кількість: {items.Count})");
void Groups<T,TKey>(string label, Dictionary<TKey,List<T>> groups) where TKey : notnull
{
    Console.WriteLine(label);
    foreach (var pair in groups) Console.WriteLine($"{pair.Key}: {pair.Value.Count}");
}
int? Maximum(List<int> src) => Aggregate<int,int?>(src,null,(acc,x) => acc is null || x > acc.Value ? x : acc);
string Band(int score) => score < 60 ? "0–59" : score < 75 ? "60–74" : score < 90 ? "75–89" : "90–100";
Show("Числа",numbers); Show("Слова",words); Show("Студенти",students);
Console.WriteLine("\n1. Filter");
var filtered = Filter(numbers,x => x <= 0);
Show("Від'ємні або нуль",filtered);
Show("З дефісом",Filter(words,x => x.Contains('-')));
Show("Бал 60–89",Filter(students,x => x.Score >= 60 && x.Score <= 89));
Console.WriteLine("\n2. Map");
Show("Знаки",Map(numbers,x => Math.Sign(x)));
Show("Перші літери",Map(words,x => x[0]));
Show("Бали",Map(students,x => x.Score));
Show("Filter + Map",Map(filtered,x => Math.Sign(x)));
Console.WriteLine("\n3. Aggregate");
Console.WriteLine($"Сума: {Aggregate(numbers,0,(s,x) => s+x)}");
Console.WriteLine($"Добуток: {Aggregate(numbers,BigInteger.One,(s,x) => s*x)}");
Console.WriteLine($"Максимум: {Maximum(numbers)}");
var stats = Aggregate(numbers,(sum:0,count:0),(s,x) => (s.sum+x,s.count+1));
Console.WriteLine($"Середнє: {(double)stats.sum/stats.count:F2}");
Console.WriteLine("\n4. GroupBy");
Groups("За групою",GroupBy(students,x => x.Group));
Groups("За літерою прізвища",GroupBy(students,x => x.Name[0]));
Groups("За діапазоном балів",GroupBy(students,x => Band(x.Score)));
Groups("За довжиною слова",GroupBy(words,x => x.Length));
Console.WriteLine("\n5. Захоплення");
int threshold = K;
Func<int,bool> predicate = x => x > threshold;
Console.WriteLine($"До зміни: {predicate(K+1)}");
threshold = 1000;
Console.WriteLine($"Після зміни: {predicate(K+1)}");
var actions = new List<Func<int>>();
for (int i=0;i<3;i++) actions.Add(() => i);
Console.WriteLine("for: " + string.Join(", ",Map(actions,f => f())));
actions.Clear();
foreach (int i in new[] {0,1,2}) actions.Add(() => i);
Console.WriteLine("foreach: " + string.Join(", ",Map(actions,f => f())));
actions.Clear();
for (int i=0;i<3;i++) { int copy = i; actions.Add(() => copy); }
Console.WriteLine("Виправлений for: " + string.Join(", ",Map(actions,f => f())));
Console.WriteLine("\nПеревірка крайових випадків");
var empty = new List<int>();
void Check(string label,bool ok) { Console.WriteLine($"{label}: {(ok ? "OK" : "FAIL")}"); if (!ok) Environment.ExitCode = 1; }
Check("Порожній список: Filter, Map, GroupBy",Filter(empty,x=>true).Count==0 && Map(empty,x=>x).Count==0 && GroupBy(empty,x=>x).Count==0);
Check("Порожній максимум",Maximum(empty) is null);
Check("Немає збігів",Filter(numbers,x=>x>K).Count==0);
Check("Одна група",GroupBy(new List<int>{1,3,5},x=>x%2).Count==1);
var single = new List<int>{7};
var singleStats = Aggregate(single,(sum:0,count:0),(s,x)=>(s.sum+x,s.count+1));
Check("Один елемент",Maximum(single)==7 && Aggregate(single,0,(s,x)=>s+x)==7 && Aggregate(single,BigInteger.One,(s,x)=>s*x)==7 && singleStats==(7,1));

public record Student(string Name,string Group,int Score);
