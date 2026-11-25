static class Search
{
    public static int LinearSearch<T>(T[] a,T target,Func<T,T,int> cmp,out int comparisons)
    { comparisons=0; for(int i=0;i<a.Length;i++){ comparisons++; if(cmp(a[i],target)==0)return i; } return -1; }
    public static int BinarySearch<T>(T[] a,T target,Func<T,T,int> cmp,out int comparisons)
        => Range(a,target,cmp,0,a.Length-1,out comparisons);
    static int Range<T>(T[] a,T target,Func<T,T,int> cmp,int lo,int hi,out int comparisons)
    { comparisons=0; while(lo<=hi){ int mid=lo+(hi-lo)/2; comparisons++; int d=cmp(a[mid],target); if(d==0)return mid; if(d<0)lo=mid+1; else hi=mid-1; } return -1; }
    public static int Boundary<T>(T[] a,Func<T,bool> condition,out int comparisons)
    { int lo=0,hi=a.Length; comparisons=0; while(lo<hi){int mid=lo+(hi-lo)/2; comparisons++; if(condition(a[mid]))hi=mid; else lo=mid+1;} return lo; }
    public static int Interpolation<T>(T[] a,T target,Func<T,T,int> cmp,Func<T,int> key,out int comparisons)
    { comparisons=0; if(a.Length==0)return -1; long min=key(a[0]),max=key(a[^1]); int mid=max==min?0:(int)Math.Clamp(((long)key(target)-min)*(a.Length-1)/(max-min),0L,a.Length-1L); comparisons++; int d=cmp(a[mid],target); if(d==0)return mid; int result=Range(a,target,cmp,d<0?mid+1:0,d<0?a.Length-1:mid-1,out int rest); comparisons+=rest; return result; }
}
