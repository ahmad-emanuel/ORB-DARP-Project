using ORB.DARP;
using System;
using System.Collections.Generic;
using System.Diagnostics;

class Programm
{
    public static void Main()
    {
        string path = @"D:\Universitaet\OperationsResearchB\darp_insts\instances\gen_500_50_100_10_20_2.darp";

        Instance inst = new Instance(path);
        SequentialConstruction sc = new SequentialConstruction(inst, 0.01, 0.8, 0.19);

        var stopWatch = Stopwatch.StartNew();
        var solution = sc.Construct();
        stopWatch.Stop();
        Print(inst, solution, stopWatch.ElapsedMilliseconds/1000);

        Console.WriteLine("\nPress any Key to exit!");
        Console.ReadKey();
    }

    public static void Print(Instance instance, List<int[]> solution, long time)
    {
        Console.WriteLine("###RESULT: Feasible.");
        Console.Write("###COST: {0}", GetObjective(instance, solution));

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

    public static int GetObjective(Instance instance, List<int[]> solution)
    {
        var costs = 0;
        var vehicle = 0;

        foreach (var route in solution)
        {
            costs += instance.TransitCosts[0, route[0]];

            for (int i = 0; i < route.Length-2; i++)
            {
                costs += instance.TransitCosts[route[i], route[i+1]];

                if (route[i] <= instance.Customers)
                {
                    costs += instance.Preferences[vehicle, route[i]-1];
                }
            }

            costs += instance.TransitCosts[route[route.Length-1], 0];

            vehicle++;
        }

        return costs;
    }
}