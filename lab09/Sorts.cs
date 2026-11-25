static class Sorts
{
    public static void Bubble<T>(T[] a, Func<T,T,int> cmp, out int comparisons, out int moves) => BubbleCore(a,cmp,out comparisons,out moves,null);
    public static void BubbleCore<T>(T[] a, Func<T,T,int> cmp, out int comparisons, out int moves, Action<int,T[]>? trace)
    {
        comparisons=moves=0; int pass=0;
        for(int end=a.Length-1;end>0;end--){ bool changed=false;
            for(int i=0;i<end;i++){ comparisons++; if(cmp(a[i],a[i+1])>0){(a[i],a[i+1])=(a[i+1],a[i]); moves+=2; changed=true;} }
            trace?.Invoke(++pass,a); if(!changed)break;
        }
    }
    public static void Quick<T>(T[] a, Func<T,T,int> cmp, out int comparisons, out int moves)
    {
        int c=0,m=0;
        void Q(int lo,int hi){while(lo<hi){int i=lo,j=hi; T pivot=a[lo+(hi-lo)/2];
            while(i<=j){while(i<=hi){c++; if(cmp(a[i],pivot)>=0)break; i++;} while(j>=lo){c++; if(cmp(a[j],pivot)<=0)break; j--;}
                if(i<=j){if(i!=j){(a[i],a[j])=(a[j],a[i]);m+=2;}i++;j--;}}
            if(j-lo<hi-i){if(lo<j)Q(lo,j);lo=i;}else{if(i<hi)Q(i,hi);hi=j;}
        }}
        Q(0,a.Length-1);comparisons=c;moves=m;
    }
}
