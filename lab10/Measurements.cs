using System.Diagnostics;
static class Measurements
{
    static object? sink;
    public static double Time(Func<object> action)
    {
        sink = action();
        double best = double.PositiveInfinity;
        for (int i=0;i<3;i++) { var sw=Stopwatch.StartNew(); sink=action(); sw.Stop(); best=Math.Min(best,sw.Elapsed.TotalMilliseconds); }
        GC.KeepAlive(sink); return best;
    }
}
