using System;
using System.Collections.Generic;

sealed class Request(int id, int weight)
{
    public int Id { get; } = id;
    public int Weight { get; } = weight;
    public int Remaining = weight;
    public int Start = -1;
    public int Finish;
}

static class Service
{
    public static (Request[] Jobs, int Maximum, int Minutes) Run(int count, int seed, int quantum)
    {
        if (count < 0 || quantum <= 0) throw new ArgumentOutOfRangeException();
        var random = new Random(seed);
        var jobs = new Request[count];
        var queue = new Queue<Request>();
        for (int i = 0; i < count; i++)
        {
            jobs[i] = new Request(i + 1, random.Next(1, 10));
            queue.Enqueue(jobs[i]);
        }
        int maximum = queue.Count, time = 0;
        while (queue.Count > 0)
        {
            var job = queue.Dequeue();
            if (job.Start < 0) job.Start = time;
            int slice = Math.Min(quantum, job.Remaining);
            time += slice;
            job.Remaining -= slice;
            if (job.Remaining == 0) job.Finish = time;
            else queue.Enqueue(job);
            maximum = Math.Max(maximum, queue.Count);
        }
        return (jobs, maximum, time);
    }
}
