using System;
using System.Collections.Generic;
using static Algorithms;

static class EdgeCases
{
    static void Check(string name, List<int> actual, List<int> expected)
    {
        if (!Equal(actual, expected))
            throw new InvalidOperationException($"Не пройдено: {name}");
        Console.WriteLine($"{name}: [{string.Join(", ", actual)}]");
    }

    static void CheckRemoval(string name, List<int> input, List<int> expected)
    {
        var backward = new List<int>(input);
        RemoveBelowAverageBackward(backward);
        Check(name + " (з кінця)", backward, expected);
        Check(name + " (новий список)", KeepAtLeastAverage(new List<int>(input)), expected);
        var forward = new List<int>(input);
        RemoveBelowAverageForwardWrong(forward);
        Check(name + " (хибний цикл)", forward, expected);
    }

    static void CheckSplit(List<int> source, List<int> small, List<int> rest)
    {
        var (a, b) = Split(source, 2);
        Check("Менші за 2", a, small);
        Check("Решта", b, rest);
    }

    public static void Run()
    {
        Check("Побудова, порожній", PositiveOdd([]), []);
        CheckRemoval("Вилучення, порожній", [], []);
        Check("Злиття, обидва порожні", Merge([], []), []);
        CheckSplit([], [], []);
        Check("Побудова, один", PositiveOdd([3]), [3]);
        CheckRemoval("Вилучення, один", [3], [3]);
        CheckSplit([3], [], [3]);
        Check("Не підходить жоден", PositiveOdd([-2, 0, 4]), []);
        CheckRemoval("Жоден не менший за середнє", [2, 2], [2, 2]);
        Check("Підходять усі", PositiveOdd([1, 3, 5]), [1, 3, 5]);
        Check("Порожній лівий", Merge([], [2, 4]), [2, 4]);
        Check("Порожній правий", Merge([2, 4], []), [2, 4]);
    }
}
