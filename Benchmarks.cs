using System;
using System.Collections.Generic;
using System.Diagnostics;

static class Benchmarks
{
    static double MeasureAdd(int count, bool reserve)
    {
        var list = reserve ? new List<int>(count) : new List<int>();
        var timer = Stopwatch.StartNew();
        for (int i = 0; i < count; i++) list.Add(i);
        timer.Stop();
        GC.KeepAlive(list);
        return timer.Elapsed.TotalMilliseconds;
    }

    static double MeasureInsert(int count)
    {
        var list = new List<int>();
        var timer = Stopwatch.StartNew();
        for (int i = 0; i < count; i++) list.Insert(0, i);
        timer.Stop();
        GC.KeepAlive(list);
        return timer.Elapsed.TotalMilliseconds;
    }

    static double MeasureContains(int count, int repeats)
    {
        var list = new List<int>(count);
        for (int i = 0; i < count; i++) list.Add(i);
        int found = 0;
        var timer = Stopwatch.StartNew();
        for (int i = 0; i < repeats; i++)
            if (list.Contains(-1)) found++;
        timer.Stop();
        if (found != 0) throw new InvalidOperationException();
        return timer.Elapsed.TotalMilliseconds;
    }

    public static void Run()
    {
        MeasureAdd(1000, false);
        MeasureAdd(1000, true);
        MeasureInsert(1000);
        MeasureContains(1000, 1000);
        int count = 100000, repeats = 1000;
        double insert = MeasureInsert(count);
        if (insert > 30000)
        {
            count /= 10;
            repeats /= 10;
            insert = MeasureInsert(count);
        }
        double add = MeasureAdd(count, false);
        double reserved = MeasureAdd(count, true);
        double contains = MeasureContains(count, repeats);
        Console.WriteLine($"5. Елементів: {count}, пошуків: {repeats}");
        Console.WriteLine($"Add: {add:F4} мс");
        Console.WriteLine($"Add з місткістю: {reserved:F4} мс");
        Console.WriteLine($"Insert: {insert:F4} мс");
        Console.WriteLine($"Contains: {contains:F4} мс");
        Console.WriteLine($"Insert / Add: {insert / add:F1}");
    }
}
