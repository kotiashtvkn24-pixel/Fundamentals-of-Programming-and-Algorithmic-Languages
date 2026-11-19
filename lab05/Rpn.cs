using System;
using System.Collections.Generic;
using System.Globalization;

static class Rpn
{
    public static (double Value, string Error) Evaluate(string expression)
    {
        var stack = new Stack<double>();
        foreach (string token in expression.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
        {
            if (double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out double number))
            {
                if (!double.IsFinite(number)) return (0, "Число поза діапазоном");
                stack.Push(number);
                continue;
            }
            if (token is not ("+" or "-" or "−" or "*" or "/" or "^")) return (0, "Невідомий токен: " + token);
            if (stack.Count < 2) return (0, "Недостатньо операндів");
            double right = stack.Pop();
            double left = stack.Pop();
            if (token == "/" && right == 0) return (0, "Ділення на нуль");
            double result = token switch
            {
                "+" => left + right, "-" or "−" => left - right,
                "*" => left * right, "/" => left / right, _ => Math.Pow(left, right)
            };
            if (!double.IsFinite(result)) return (0, "Результат поза діапазоном");
            stack.Push(result);
        }
        if (stack.Count == 0) return (0, "Порожній вираз");
        if (stack.Count > 1) return (0, "Зайві числа");
        return (stack.Peek(), "");
    }
}
