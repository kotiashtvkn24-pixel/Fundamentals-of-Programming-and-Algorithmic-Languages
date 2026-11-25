using System.Diagnostics;
static class Measurements
{
    public delegate void Sorter<T>(T[] a,Func<T,T,int> cmp,out int c,out int m);
    public static double Time<T>(T[] input,Func<T,T,int> cmp,Action<T[]> sort) where T:notnull
    { var warm=(T[])input.Clone();sort(warm);Checks.Verify(input,warm,cmp);double best=double.MaxValue;for(int i=0;i<3;i++){var a=(T[])input.Clone();var sw=Stopwatch.StartNew();sort(a);sw.Stop();best=Math.Min(best,sw.Elapsed.TotalMilliseconds);Checks.Verify(input,a,cmp);}return best; }
    public static int[] Data(int n){var r=new Random(14);var a=new int[n];for(int i=0;i<n;i++)a[i]=r.Next(461);return a;}
    public static int[][] Inputs(int n){var a=Data(n);var b=(int[])a.Clone();Sorts.Quick(b,(x,y)=>x.CompareTo(y),out _,out _);Checks.Verify(a,b,(x,y)=>x.CompareTo(y));var c=(int[])b.Clone();for(int i=0;i<n/2;i++)(c[i],c[n-1-i])=(c[n-1-i],c[i]);var d=(int[])b.Clone();var r=new Random(14);for(int i=0;i<23;i++){int x=r.Next(n),y=r.Next(n);(d[x],d[y])=(d[y],d[x]);}return new[]{a,b,c,d};}
}
