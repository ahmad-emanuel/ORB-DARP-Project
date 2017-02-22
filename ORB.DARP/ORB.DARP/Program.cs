using ORB.DARP;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class Programm
{
    private static Instance instance;
    private static SequentialConstruction sequentialConstruction;
    private static LNS lns;

    public static Solution solution { get; set; }

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
            solution = new Solution(instance);

            var stopWatch = Stopwatch.StartNew();

            FindSolution(1000);

            stopWatch.Stop();

            Output(true, stopWatch.ElapsedMilliseconds / 1000);
        }
        else if (args.Length == 2)
        {
            instance = new Instance(args[1]);
            solution = new Solution(instance);

            var stopWatch = Stopwatch.StartNew();
            var task = Task.Factory.StartNew(() => FindSolution(1000000));
            var noTimeout = task.Wait(int.Parse(args[0]) * 1000);

            stopWatch.Stop();

            Output(noTimeout, stopWatch.ElapsedMilliseconds / 1000);
        }
        
        //Console.WriteLine("\nPress any key to exit!");
        //Console.ReadKey();
    }

    private static void FindSolution(int iterations)
    {
        sequentialConstruction = new SequentialConstruction(instance, 0.01, 0.80, 0.19);

        while (iterations > 0 && !solution.IsFeasibleSolution())
        {
            sequentialConstruction.Construct();
            sequentialConstruction.Reset();

            iterations--;
        }

        Console.WriteLine("Initial objective: {0}", solution.GetObjective());

        lns = new LNS(instance, 0.01, 0.80, 0.19);
        lns.MinimizeCosts(instance.Customers/instance.Vehicles, 1, 50000, 0.25);
    }

    private static void Output(bool noTimeout, long cpuTime)
    {
        if (solution.IsFeasibleSolution())
        {
            solution.DecodeSolution();

            var sol = File.AppendText(instance.OutPath);

            Console.WriteLine("###RESULT: Feasible.");
            sol.WriteLine("###RESULT: Feasible.");
            Console.Write("###COST: {0}", solution.GetObjective());
            sol.Write("###COST: {0}", solution.GetObjective());

            for (int i = 1; i <= solution.GetVehicleCount(); i++)
            {
                Console.Write("\n###VEHICLE {0}: ", i);
                sol.Write("\n###VEHICLE {0}: ", i);
                foreach (var customer in solution.GetRoute(i-1).GetCustomers())
                {
                    Console.Write("{0} ", customer);
                    sol.Write("{0} ", customer);
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
}