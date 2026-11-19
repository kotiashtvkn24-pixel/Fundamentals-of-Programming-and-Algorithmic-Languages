using System;
using System.Collections.Generic;
using System.Text;
using static Algorithms;

class Program
{
    const int N = 14, K = 23;
    static string Show(List<int> values) => "[" + string.Join(", ", values) + "]";
    static string Flag(bool value) => value ? "true" : "false";

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var source = Generate(20, new Random(N));
        Console.WriteLine($"Основний список: {Show(source)}");
        var odd = PositiveOdd(source);
        Console.WriteLine($"1. Додатні непарні: {Show(odd)}, Count = {odd.Count}");

        double mean = Average(source);
        var wrong = new List<int>(source);
        var backward = new List<int>(source);
        RemoveBelowAverageForwardWrong(wrong);
        RemoveBelowAverageBackward(backward);
        var kept = KeepAtLeastAverage(new List<int>(source));
        Console.WriteLine($"2. Початкове середнє: {mean}");
        Console.WriteLine($"Хибний цикл: {Show(wrong)}");
        var missed = new List<int>();
        foreach (int x in wrong)
            if (x < mean) missed.Add(x);
        Console.WriteLine($"Пропущені: {Show(missed)}");
        Console.WriteLine($"З кінця: {Show(backward)}");
        Console.WriteLine($"Новий список: {Show(kept)}");
        Console.WriteLine($"Збіг: {Flag(Equal(backward, kept))}");

        var left = GenerateSorted(10 + K % 5, new Random(N));
        var right = GenerateSorted(6 + K % 4, new Random(N + 100));
        var merged = Merge(left, right);
        Console.WriteLine($"3. Перший: {Show(left)}");
        Console.WriteLine($"Другий: {Show(right)}");
        Console.WriteLine($"Злиття: {Show(merged)}");
        Console.WriteLine($"Впорядковано: {Flag(IsSorted(merged))}");

        var (small, rest) = Split(source, K % 7);
        Console.WriteLine($"4. Менші за 2: {Show(small)}");
        Console.WriteLine($"Решта: {Show(rest)}");
        Console.WriteLine($"Сума Count правильна: {Flag(small.Count + rest.Count == source.Count)}");

        Console.WriteLine("Крайові випадки");
        EdgeCases.Run();
        Benchmarks.Run();
    }
}
