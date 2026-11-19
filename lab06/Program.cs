using System;
using System.Collections.Generic;
using System.Globalization;
using static Algorithms;

static class Program
{
    const int N = 14, K = 23;
    public const string Text = "Алгоритм описує послідовність дій. Алгоритм можна записати словами, а можна записати кодом. Кодом алгоритм стає тоді, коли його виконує машина, а не людина. Одна задача — багато алгоритмів.";

    static void Check(bool success)
    {
        if (!success) throw new Exception("Результат перевірки хибний");
    }

    static bool Same<T>(List<T> a, List<T> b)
    {
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
            if (!EqualityComparer<T>.Default.Equals(a[i], b[i])) return false;
        return true;
    }

    static Dictionary<int,int> Frequencies(string text)
    {
        var lengths = Lengths(text);
        var hash = CountHash(lengths);
        var list = CountList(lengths);
        Check(hash.Count == list.Keys.Count);
        for (int i = 0; i < list.Keys.Count; i++)
            Check(hash.TryGetValue(list.Keys[i], out int value) && value == list.Counts[i]);
        return hash;
    }

    static Dictionary<string,List<Student>> Groups(List<Student> students)
    {
        var hash = GroupHash(students);
        var list = GroupList(students);
        Check(hash.Count == list.Keys.Count);
        for (int i = 0; i < list.Keys.Count; i++)
            Check(hash.TryGetValue(list.Keys[i], out var group) && Same(group, list.Groups[i]));
        return hash;
    }

    static void ShowExamples()
    {
        var freq = Frequencies(Text);
        Console.WriteLine($"Різних довжин: {freq.Count}");
        foreach (var pair in freq)
            if (pair.Value >= K % 3 + 2) Console.WriteLine($"Довжина {pair.Key}: {pair.Value}");
        List<Student> students = [
            new("Коваленко","КН-3/1",88), new("Мельник","КН-3/2",64),
            new("Іщенко","КН-3/1",93), new("Бондар","КН-3/1",71),
            new("Савченко","КН-3/2",55), new("Ткачук","КН-3/2",90),
            new("Кравець","КН-3/1",76), new("Іванів","КН-3/1",48),
            new("Мороз","КН-3/2",82), new("Шевчук","КН-3/1",67),
            new("Бойко","КН-3/2",95), new("Кушнір","КН-3/2",73)
        ];
        foreach (var group in Groups(students))
            Console.WriteLine($"Бали {group.Key}: {group.Value.Count} студентів, середній {Average(group.Value):F2}");
        var values = Numbers(new Random(N), 25, K % 10 + 6);
        var set = UniqueSet(values);
        var ordered = UniqueOrdered(values);
        Check(set.SetEquals(ordered) && Same(ordered, UniqueList(values)));
        Console.WriteLine("Вхід: " + string.Join(", ", values));
        Console.WriteLine("HashSet: " + string.Join(", ", set));
        Console.WriteLine("Перші входження: " + string.Join(", ", ordered));
        var a = Numbers(new Random(N), 30, 20 + K);
        var b = Numbers(new Random(N + 100), 30, 20 + K);
        var union = UnionHash(a, b);
        var manual = UnionList(a, b);
        Check(union.Count == manual.Count && union.SetEquals(manual));
        Console.WriteLine("Об’єднання: " + string.Join(", ", union));
        Console.WriteLine($"|A|={UniqueSet(a).Count}, |B|={UniqueSet(b).Count}, |A ∪ B|={union.Count}, |A ∩ B|={CommonHash(a,b).Count}; збіг: true");
    }

    static void Edges()
    {
        Check(Frequencies("").Count == 0);
        var f = Frequencies("кіт лис дуб рак");
        Check(f.Count == 1 && f.TryGetValue(3, out int count) && count == 4);
        var g = Groups([new("А","КН-3/1",75),new("Б","КН-3/2",89)]);
        Check(g.Count == 1 && g.TryGetValue("75–89", out var band) && band.Count == 2 && Average(band) == 82);
        List<int> unique = [3,1,2];
        Check(Same(unique, UniqueList(unique)) && Same(unique, UniqueOrdered(unique)) && UniqueSet(unique).SetEquals(unique));
        Check(UnionHash([1,2],[3,4]).SetEquals([1,2,3,4]) && Same(UnionList([1,2],[3,4]),[1,2,3,4]));
        Check(UnionHash([1,2],[1,2]).SetEquals([1,2]) && Same(UnionList([1,2],[1,2]),[1,2]));
        Console.WriteLine("Крайові випадки: 6 із 6 пройдено.");
    }

    static void Main()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        ShowExamples();
        Edges();
        Measurements.Run();
    }
}
