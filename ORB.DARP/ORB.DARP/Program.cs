using ORB.DARP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class Programm
{
    private static Instance instance;
    private static FeasibilityCheck feasibilityCheck;
    private static SequentialConstruction sequentialConstruction;

    private static List<List<int>> solution = new List<List<int>>();

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
            feasibilityCheck = new FeasibilityCheck(instance);

            var stopWatch = Stopwatch.StartNew();

            CreateInitialSolution(10000);

            stopWatch.Stop();

            Output(true, stopWatch.ElapsedMilliseconds / 1000);
        }
        else if (args.Length == 2)
        {
            instance = new Instance(args[1]);
            feasibilityCheck = new FeasibilityCheck(instance);

            var stopWatch = Stopwatch.StartNew();
            var task = Task.Factory.StartNew(() => CreateInitialSolution(1000000));
            var noTimeout = task.Wait(int.Parse(args[0]) * 1000);

            stopWatch.Stop();

            Output(noTimeout, stopWatch.ElapsedMilliseconds / 1000);
        }
        
        Console.WriteLine("\nPress any key to exit!");
        Console.ReadKey();
    }

    private static void CreateInitialSolution(int iterations)
    {
        while (iterations > 0 && !feasibilityCheck.IsFeasibleSolution(instance, solution))
        {
            sequentialConstruction = new SequentialConstruction(instance, 0.01, 0.80, 0.19);
            solution = sequentialConstruction.Construct();

            iterations--;
        }
    }

    private static void Output(bool noTimeout, long cpuTime)
    {
        solution = HillClimb.DecodeSolution(solution);

        if (feasibilityCheck.IsFeasibleSolution(instance, solution))
        {
            var sol = File.AppendText(instance.OutPath);

            Console.WriteLine("###RESULT: Feasible.");
            sol.WriteLine("###RESULT: Feasible.");
            Console.Write("###COST: {0}", GetObjective(solution));
            sol.Write("###COST: {0}", GetObjective(solution));

            for (int i = 1; i <= solution.Count; i++)
            {
                Console.Write("\n###VEHICLE {0}: ", i);
                sol.Write("\n###VEHICLE {0}: ", i);
                foreach (var node in solution[i - 1])
                {
                    Console.Write("{0} ", node);
                    sol.Write("{0} ", node);
                }
            }

            Console.WriteLine("\n###CPU-TIME: {0}", cpuTime);
            sol.WriteLine("\n###CPU-TIME: {0}", cpuTime);

            sol.Close();
        }
        else
        {
            if (noTimeout)
            {
                Console.WriteLine("###RESULT: Infeasible.");
            }
            else
            {
                Console.WriteLine("###RESULT: Timeout.");
            }
        }
        
    }

    public static int GetObjective(List<List<int>> solution)
    {
        var costs = 0;
        var vehicle = 0;

        foreach (var route in solution)
        {
            costs += instance.TransitCosts[0, route[0]];

            for (int i = 0; i <= route.Count-2; i++)
            {
                costs += instance.TransitCosts[route[i], route[i+1]];

                if (route[i] <= instance.Customers)
                {
                    costs += instance.Preferences[vehicle, route[i]-1];
                }
            }

            costs += instance.TransitCosts[route[route.Count-1], 0];

            vehicle++;
        }

        return costs;
    }
}