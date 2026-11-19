using System;

sealed class IntStack
{
    private int[] items;
    public int Count { get; private set; }
    public int Capacity => items.Length;

    public IntStack(int capacity = 5)
    {
        if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity));
        items = new int[capacity];
    }

    public void Push(int value)
    {
        if (Count == Capacity) Array.Resize(ref items, Capacity * 2);
        items[Count++] = value;
    }

    public int Peek()
    {
        if (Count == 0) throw new InvalidOperationException("Стек порожній");
        return items[Count - 1];
    }

    public int Pop()
    {
        int top = Peek();
        Count--;
        return top;
    }
}
