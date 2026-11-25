record Item(int Weight, int Value);
static class Greedy
{
    public static int Solve(Item[] items, int capacity)
    {
        var a = (Item[])items.Clone();
        Array.Sort(a, (x,y) => ((long)y.Value*x.Weight).CompareTo((long)x.Value*y.Weight));
        int value = 0;
        foreach (var item in a) if (item.Weight <= capacity) { capacity -= item.Weight; value += item.Value; }
        return value;
    }
}
