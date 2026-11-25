static class Checks
{
    public static bool IsSorted<T>(T[] a,Func<T,T,int> cmp){for(int i=1;i<a.Length;i++)if(cmp(a[i-1],a[i])>0)return false;return true;}
    public static bool IsPermutationOf<T>(T[] a,T[] b) where T:notnull {if(a.Length!=b.Length)return false;var counts=new Dictionary<T,int>();foreach(var x in a){counts.TryGetValue(x,out int n);counts[x]=n+1;}foreach(var x in b){if(!counts.TryGetValue(x,out int n)||n==0)return false;counts[x]=n-1;}return true;}
    public static void Verify<T>(T[] original,T[] result,Func<T,T,int> cmp) where T:notnull {if(!IsSorted(result,cmp)||!IsPermutationOf(original,result))throw new Exception("Помилка сортування");}
}
