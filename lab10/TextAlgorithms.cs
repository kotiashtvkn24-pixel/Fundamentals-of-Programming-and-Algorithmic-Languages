using System.Text;
static class TextAlgorithms
{
    public static string Slow(string text)
    {
        text = text.ToUpperInvariant();
        foreach (char c in text)
            if (char.IsWhiteSpace(c) && c != ' ') text = text.Replace(c, ' ');
        while (text.Contains("  ", StringComparison.Ordinal)) text = text.Replace("  ", " ", StringComparison.Ordinal);
        text = text.Trim(' ');
        var result = new StringBuilder();
        while (text.Length > 0)
        {
            int end = 0;
            while (end < text.Length && text[end] != ' ') end++;
            if (result.Length > 0) result.Append(' ');
            result.Append(text.Substring(0, end));
            text = end == text.Length ? "" : text.Substring(end + 1);
        }
        return result.ToString();
    }
    public static string Fast(string text)
    {
        var result = new StringBuilder();
        int left = 0;
        while (left < text.Length)
        {
            while (left < text.Length && char.IsWhiteSpace(text[left])) left++;
            int right = left;
            while (right < text.Length && !char.IsWhiteSpace(text[right])) right++;
            if (right > left)
            {
                if (result.Length > 0) result.Append(' ');
                for (int i = left; i < right; i++) result.Append(char.ToUpperInvariant(text[i]));
            }
            left = right;
        }
        return result.ToString();
    }
    public static int IndexOf(string text, string pat, out int comparisons)
    {
        comparisons = 0;
        for (int i = 0; i <= text.Length - pat.Length; i++)
        {
            int j = 0;
            while (j < pat.Length)
            {
                comparisons++;
                if (text[i+j] != pat[j]) break;
                j++;
            }
            if (j == pat.Length) return i;
        }
        return -1;
    }
    public static void Check(string text)
    {
        if (!string.Equals(Slow(text), Fast(text), StringComparison.Ordinal)) throw new Exception("Різні результати");
    }
}
