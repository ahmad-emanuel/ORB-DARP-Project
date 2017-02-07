using ORB.DARP;
using System;
using System.Collections.Generic;
using System.Diagnostics;

class Programm
{
    public static void Main()
    {
        string path = @"C:\Users\Alexander\Dropbox\Uni\WS16-17\Operations Research B\Projekt\instances\test.darp";

        Instance inst1 = new Instance(path);
        inst1.Initialization();

        SequentialConstruction sc = new SequentialConstruction(inst1, 0.1, 0.7, 0.2);

        var stopWatch = Stopwatch.StartNew();
        var solution = sc.Construct();
        stopWatch.Stop();
        Print(solution, stopWatch.ElapsedMilliseconds/1000);

        Console.WriteLine("\nPress any Key to exit!");
        Console.ReadKey();
    }

    public static void Print(List<int[]> solution, long time)
    {
        Console.WriteLine("###RESULT: Feasible.");
        Console.Write("###COST: --");

        for (int i = 1; i <= solution.Count; i++)
        {
            Console.Write("\n###VEHICLE {0}: ", i);
            foreach (var node in solution[i-1])
            {
                Console.Write("{0} ", node);
            }
        }

        Console.WriteLine("\n###CPU-TIME: {0}", time);
    }
}