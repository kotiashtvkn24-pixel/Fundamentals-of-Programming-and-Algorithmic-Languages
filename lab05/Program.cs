using System;
using System.Globalization;
using System.Text;

static class Program
{
    const int N = 14, K = 23;
    static void Check(bool passed)
    {
        if (!passed) throw new Exception("Перевірка не пройдена");
    }

    static void TestBrackets()
    {
        Console.WriteLine("Дужки | стандартний | власний (індекс помилки)");
        (string Text, int Index)[] cases = [
            ("(a[b]{c})", -1), ("([)]", 2), (")(", 0), ("((", 0), ("", -1),
            ("текст", -1), ("[\"})\"]", -1), (new string('(', K) + new string(')', K - 1), 0)
        ];
        foreach (var (text, expected) in cases)
        {
            int standard = Brackets.Standard(text);
            var own = new IntStack(K % 4 + 2);
            int custom = Brackets.Scan(text, own.Push, own.Pop, () => own.Count);
            Check(standard == expected && custom == expected);
            Console.WriteLine($"{text} | {standard} | {custom}");
        }
    }

    static void TestRpn()
    {
        Console.WriteLine("Зворотний польський запис");
        (string Input, string Expected)[] cases = [
            ("3 4 + 2 *", "14"), ("5 3 -", "2"), ("8 2 /", "4"), ("2 3 4 * +", "14"),
            ("5 0 /", "Ділення на нуль"), ("2 4 ^ 3 2 * + 1 +", "23"),
            ("", "Порожній вираз"), ("7", "7"), ("3 xyz", "Невідомий токен: xyz"),
            ("2 3", "Зайві числа"), ("2 +", "Недостатньо операндів")
        ];
        foreach (var (input, expected) in cases)
        {
            var (value, error) = Rpn.Evaluate(input);
            string actual = error.Length > 0 ? error : value.ToString(CultureInfo.InvariantCulture);
            Check(actual == expected);
            Console.WriteLine($"{input} → {actual}");
        }
    }

    static void TestService()
    {
        Console.WriteLine("Заявка | Вага | Початок | Завершення");
        int quantum = K % 3 + 2;
        var run = Service.Run(8 + K % 5, N, quantum);
        int total = 0;
        foreach (var job in run.Jobs)
        {
            Console.WriteLine($"{job.Id} | {job.Weight} | {job.Start} | {job.Finish}");
            Check(job.Remaining == 0 && job.Finish - job.Start >= job.Weight);
            total += job.Weight;
        }
        Check(total == run.Minutes && run.Maximum == run.Jobs.Length);
        Console.WriteLine($"Максимум: {run.Maximum}; хвилин: {run.Minutes}");
        var empty = Service.Run(0, N, quantum);
        Check(empty.Maximum == 0 && empty.Minutes == 0 && empty.Jobs.Length == 0);
        Console.WriteLine("Порожня черга: 0 заявок, 0 хв, максимум 0");
    }

    static void TestStack()
    {
        int initial = K % 4 + 2;
        var stack = new IntStack(initial);
        for (int i = 1; i <= initial + 1; i++)
        {
            stack.Push(i);
            Check(stack.Count == i && stack.Peek() == i && stack.Capacity == (i > initial ? initial * 2 : initial));
            Console.WriteLine($"Push({i}): Count={stack.Count}, Capacity={stack.Capacity}");
        }
        for (int expected = initial + 1; expected > 0; expected--)
            Check(stack.Count > 0 && stack.Pop() == expected);
        if (stack.Count == 0)
        {
            try { stack.Pop(); }
            catch (InvalidOperationException e) { Console.WriteLine($"Порожній Pop: {e.Message}"); return; }
            throw new Exception("Очікувався виняток порожнього стеку");
        }
    }

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        TestBrackets();
        TestRpn();
        TestService();
        TestStack();
        Console.WriteLine("Усі перевірки пройдено.");
    }
}
