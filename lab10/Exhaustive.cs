static class Exhaustive
{
    public static int Solve(Item[] a, int capacity, bool prune, out long branches)
    {
        long count = 0;
        int best = 0;
        int[] suffix = new int[a.Length + 1];
        for (int i = a.Length-1; i >= 0; i--) suffix[i] = suffix[i+1] + a[i].Value;
        void Visit(int i, int weight, int value)
        {
            count++;
            if (prune && (weight > capacity || value + suffix[i] <= best)) return;
            if (i == a.Length) { if (weight <= capacity && value > best) best = value; return; }
            Visit(i+1, weight+a[i].Weight, value+a[i].Value);
            Visit(i+1, weight, value);
        }
        Visit(0,0,0); branches = count; return best;
    }
}
