using ORB.DARP;
using System;
using System.Collections.Generic;
using System.Diagnostics;

class Programm
{
    private static Instance instance;
    private static SequentialConstruction sequentialConstruction;

    private static List<int[]> solution = new List<int[]>();

    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No agruments passed!\n");
            Console.WriteLine("Usage: orbdar [maximum time] instance\n");
            Console.WriteLine("maximum time (optional): maximum execution time in seconds");
            Console.WriteLine("instance: full path to the input instance file");
        }
        else if (args.Length == 1)
        {
            instance = new Instance(args[0]);
        }
        else if (args.Length == 2)
        {
            var maxCpuTime = 0;
            int.TryParse(args[0], out maxCpuTime);

            instance = new Instance(args[1]);
        }

        if (args.Length != 0)
        {
            var stopWatch = Stopwatch.StartNew();

            CreateInitialSolution(1000);

            stopWatch.Stop();

            Print(stopWatch.ElapsedMilliseconds / 1000);
        }
        
        Console.WriteLine("\nPress any key to exit!");
        Console.ReadKey();
    }

    private static void CreateInitialSolution(int iterations)
    {
        while (iterations > 0 && !FeasibilityCheck.IsFeasibleSolution(instance, solution))
        {
            sequentialConstruction = new SequentialConstruction(instance, 0.01, 0.80, 0.19);
            solution = sequentialConstruction.Construct();

            iterations--;
        }
    }

    private static void Print(long cpuTime)
    {
        if (FeasibilityCheck.IsFeasibleSolution(instance, solution))
        {
            Console.WriteLine("###RESULT: Feasible.");
            Console.Write("###COST: {0}", GetObjective());

            for (int i = 1; i <= solution.Count; i++)
            {
                Console.Write("\n###VEHICLE {0}: ", i);
                foreach (var node in solution[i - 1])
                {
                    Console.Write("{0} ", node);
                }
            }

            Console.WriteLine("\n###CPU-TIME: {0}", cpuTime);
        }
        else
        {
            Console.WriteLine("###RESULT: Infeasible.");
        }
        
    }

    private static int GetObjective()
    {
        var costs = 0;
        var vehicle = 0;

        foreach (var route in solution)
        {
            costs += instance.TransitCosts[0, route[0]];

            for (int i = 0; i <= route.Length-2; i++)
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