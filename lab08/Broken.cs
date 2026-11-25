static class Broken
{
    public static int Run<T>(T[] a,T target,Func<T,T,int> cmp,int mode,out int comparisons,out bool stalled)
    { int lo=0,hi=a.Length-1; comparisons=0; stalled=false; while(mode==1?lo<hi:lo<=hi){ if(comparisons>=100){stalled=true;return -1;} int mid=mode==3?(lo+hi)/2:lo+(hi-lo)/2; comparisons++; int d=cmp(a[mid],target); if(d==0)return mid; if(d<0)lo=mode==2?mid:mid+1; else hi=mid-1; } return -1; }
}
