using System;
using System.Collections.Generic;

static class Brackets
{
    public static int Standard(string text)
    {
        var stack = new Stack<int>();
        return Scan(text, stack.Push, stack.Pop, () => stack.Count);
    }

    // Зберігаємо індекси відкривальних дужок, щоб повідомити місце помилки.
    public static int Scan(string text, Action<int> push, Func<int> pop, Func<int> count)
    {
        bool quoted = false;
        for (int i = 0; i < text.Length; i++)
        {
            char ch = text[i];
            if (quoted && ch == '\\' && i + 1 < text.Length) { i++; continue; }
            if (ch == '"') { quoted = !quoted; continue; }
            if (quoted) continue;
            if (ch is '(' or '[' or '{') { push(i); continue; }
            char opening = ch switch { ')' => '(', ']' => '[', '}' => '{', _ => '\0' };
            if (opening == '\0') continue;
            if (count() == 0 || text[pop()] != opening) return i;
        }
        int earliest = -1;
        while (count() > 0) earliest = pop();
        return earliest;
    }
}
