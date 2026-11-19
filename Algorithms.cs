using System;
using System.Collections.Generic;

static class Algorithms
{
    public static List<int> Generate(int count, Random random)
    {
        var result = new List<int>(count);
        for (int i = 0; i < count; i++)
            result.Add(random.Next(-9, 10));
        return result;
    }

    public static List<int> PositiveOdd(List<int> source)
    {
        var result = new List<int>();
        foreach (int x in source)
            if (x > 0 && x % 2 != 0)
                result.Add(x);
        return result;
    }

    public static double Average(List<int> source)
    {
        if (source.Count == 0)
            throw new ArgumentException("Порожній список не має середнього.");
        long total = 0;
        foreach (int x in source)
            total += x;
        return (double)total / source.Count;
    }

    public static void RemoveBelowAverageForwardWrong(List<int> values)
    {
        if (values.Count == 0) return;
        double mean = Average(values);
        for (int i = 0; i < values.Count; i++)
            if (values[i] < mean)
                values.RemoveAt(i);
    }

    public static void RemoveBelowAverageBackward(List<int> values)
    {
        if (values.Count == 0) return;
        double mean = Average(values);
        for (int i = values.Count - 1; i >= 0; i--)
            if (values[i] < mean)
                values.RemoveAt(i);
    }

    public static List<int> KeepAtLeastAverage(List<int> source)
    {
        var result = new List<int>();
        if (source.Count == 0) return result;
        double mean = Average(source);
        foreach (int x in source)
            if (x >= mean)
                result.Add(x);
        return result;
    }

    public static List<int> GenerateSorted(int count, Random random)
    {
        var result = new List<int>(count);
        int value = 0;
        for (int i = 0; i < count; i++)
        {
            value += random.Next(1, 10);
            result.Add(value);
        }
        return result;
    }

    public static List<int> Merge(List<int> left, List<int> right)
    {
        var result = new List<int>(left.Count + right.Count);
        int i = 0, j = 0;
        while (i < left.Count && j < right.Count)
        {
            if (left[i] <= right[j]) result.Add(left[i++]);
            else result.Add(right[j++]);
        }
        while (i < left.Count) result.Add(left[i++]);
        while (j < right.Count) result.Add(right[j++]);
        return result;
    }

    public static bool IsSorted(List<int> values)
    {
        for (int i = 1; i < values.Count; i++)
            if (values[i] < values[i - 1]) return false;
        return true;
    }

    public static (List<int>, List<int>) Split(List<int> source, int threshold)
    {
        var smaller = new List<int>();
        var rest = new List<int>();
        foreach (int x in source)
        {
            if (x < threshold) smaller.Add(x);
            else rest.Add(x);
        }
        return (smaller, rest);
    }

    public static bool Equal(List<int> left, List<int> right)
    {
        if (left.Count != right.Count) return false;
        for (int i = 0; i < left.Count; i++)
            if (left[i] != right[i]) return false;
        return true;
    }
}
