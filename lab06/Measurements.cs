using System;
using System.Collections.Generic;
using System.Diagnostics;
using static Algorithms;

static class Measurements
{
    static int Hits(List<int> values, List<int> queries)
    {
        int hits = 0;
        foreach (int q in queries) if (values.Contains(q)) hits++;
        return hits;
    }

    static int Hits(HashSet<int> values, List<int> queries)
    {
        int hits = 0;
        foreach (int q in queries) if (values.Contains(q)) hits++;
        return hits;
    }

    static double Time(Func<int> action, out int answer)
    {
        var sw = Stopwatch.StartNew();
        answer = action();
        sw.Stop();
        return sw.Elapsed.TotalMilliseconds;
    }

    static bool Pair(string name, Func<int> list, Func<int> hash, bool print)
    {
        double a = Time(list, out int left);
        double b = Time(hash, out int right);
        if (left != right) throw new Exception("Різні результати: " + name);
        if (print) Console.WriteLine($"{name} | {a:F4} | {b:F4} | {a/b:F2}");
        return a <= 60000;
    }

    static bool RunScale(int divisor, bool print)
    {
        var random = new Random(14);
        int size = 100000 / divisor;
        var values = new List<int>(size);
        for (int i = 1; i <= size; i++) values.Add(i);
        var set = new HashSet<int>(values);
        var queries = Numbers(random, 10000 / divisor, size * 2);
        var words = new List<string>();
        for (int i = 0; i < 50000 / divisor; i++)
            words.Add("слово" + random.Next(1, 2001));
        var numbers = Numbers(random, 20000 / divisor, 10000 / divisor);
        // Два набори різних чисел з частковим перекриттям.
        var left = new List<int>();
        var right = new List<int>();
        int n = 5000 / divisor;
        for (int i = 0; i < n; i++) { left.Add(i); right.Add(i + n / 2); }
        bool ok = Pair("Наявність", () => Hits(values, queries), () => Hits(set, queries), print);
        ok &= Pair("Частоти слів", () => CountList(words).Keys.Count, () => CountHash(words).Count, print);
        ok &= Pair("Дублікати", () => UniqueList(numbers).Count, () => UniqueOrdered(numbers).Count, print);
        ok &= Pair("Перетин", () => CommonList(left,right).Count, () => CommonHash(left,right).Count, print);
        return ok;
    }

    public static void Run()
    {
        RunScale(100, false);
        if (!RunScale(1, true)) { Console.WriteLine("Масштаб 1:10"); RunScale(10, true); }
        List<int> tiny = [1,2,3,4,5,6,7,8];
        var set = new HashSet<int>(tiny);
        var queries = Numbers(new Random(14), 10000, 16);
        Pair("Малий набір (8)", () => Hits(tiny, queries), () => Hits(set, queries), true);
    }
}
