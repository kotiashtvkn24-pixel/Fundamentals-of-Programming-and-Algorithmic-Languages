using System;
using System.Collections.Generic;

sealed class Student(string name, string group, int score)
{
    public string Name = name;
    public string Group = group;
    public int Score = score;
}

static class Algorithms
{
    public static List<int> Lengths(string text)
    {
        var lengths = new List<int>();
        int length = 0;
        foreach (char c in text)
        {
            if (char.IsLetter(c)) length++;
            else if (length > 0) { lengths.Add(length); length = 0; }
        }
        if (length > 0) lengths.Add(length);
        return lengths;
    }

    public static Dictionary<T, int> CountHash<T>(List<T> items) where T : notnull
    {
        var counts = new Dictionary<T, int>();
        foreach (T item in items)
        {
            counts.TryGetValue(item, out int old);
            counts[item] = old + 1;
        }
        return counts;
    }

    public static (List<T> Keys, List<int> Counts) CountList<T>(List<T> items)
    {
        var keys = new List<T>();
        var counts = new List<int>();
        foreach (T item in items)
        {
            int i = keys.IndexOf(item);
            if (i < 0) { keys.Add(item); counts.Add(1); }
            else counts[i]++;
        }
        return (keys, counts);
    }

    static string Band(int score) => score switch
    {
        < 0 or > 100 => throw new ArgumentOutOfRangeException(nameof(score)),
        < 60 => "0–59", < 75 => "60–74", < 90 => "75–89", _ => "90–100"
    };

    public static Dictionary<string, List<Student>> GroupHash(List<Student> students)
    {
        var groups = new Dictionary<string, List<Student>>();
        foreach (var s in students)
        {
            string key = Band(s.Score);
            if (!groups.TryGetValue(key, out var group))
                groups[key] = group = new List<Student>();
            group.Add(s);
        }
        return groups;
    }

    public static (List<string> Keys, List<List<Student>> Groups) GroupList(List<Student> students)
    {
        var keys = new List<string>();
        var groups = new List<List<Student>>();
        foreach (var s in students)
        {
            string key = Band(s.Score);
            int i = keys.IndexOf(key);
            if (i < 0)
            {
                i = keys.Count;
                keys.Add(key);
                groups.Add(new List<Student>());
            }
            groups[i].Add(s);
        }
        return (keys, groups);
    }

    public static double Average(List<Student> group)
    {
        long total = 0;
        foreach (var student in group) total += student.Score;
        return (double)total / group.Count;
    }

    public static List<int> Numbers(Random random, int count, int max)
    {
        var list = new List<int>(count);
        for (int i = 0; i < count; i++) list.Add(random.Next(1, max + 1));
        return list;
    }

    public static HashSet<int> UniqueSet(List<int> source) => new HashSet<int>(source);

    public static List<int> UniqueOrdered(List<int> source)
    {
        var seen = new HashSet<int>();
        var result = new List<int>();
        foreach (int x in source)
            if (seen.Add(x)) result.Add(x);
        return result;
    }

    static void AppendUnique(List<int> result, List<int> source)
    {
        foreach (int x in source)
        {
            bool found = false;
            foreach (int y in result)
                if (x == y) { found = true; break; }
            if (!found) result.Add(x);
        }
    }

    public static List<int> UniqueList(List<int> source)
    {
        var result = new List<int>();
        AppendUnique(result, source);
        return result;
    }

    public static List<int> UnionList(List<int> left, List<int> right)
    {
        var result = UniqueList(left);
        AppendUnique(result, right);
        return result;
    }

    public static HashSet<int> UnionHash(List<int> left, List<int> right)
    {
        var result = new HashSet<int>(left);
        result.UnionWith(right);
        return result;
    }

    public static List<int> CommonList(List<int> left, List<int> right)
    {
        var result = new List<int>();
        foreach (int x in left)
            if (right.Contains(x) && !result.Contains(x)) result.Add(x);
        return result;
    }

    public static HashSet<int> CommonHash(List<int> left, List<int> right)
    {
        var result = new HashSet<int>(left);
        result.IntersectWith(right);
        return result;
    }
}
