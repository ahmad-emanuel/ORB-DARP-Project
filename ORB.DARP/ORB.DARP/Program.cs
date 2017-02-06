using ORB.DARP;
using System;
using System.IO;
using System.Linq;

class Programm
{

    public static void Main()
    {
        string path = @"C:\Users\Alexander\Dropbox\Uni\WS16-17\Operations Research B\Projekt\instances\test.darp";

        Instance inst1 = new Instance(path);
        inst1.Initialization();

        FeasibilityCheck feasibilityChecker = new FeasibilityCheck(inst1);
        int[] testroute = new int[] { 3, 4, 9, 2, 8, 7};

        Console.WriteLine(inst1.Customers + "\n" + inst1.MaxTime + "\n" + inst1.Vehicles);
        Console.WriteLine();
        ShowArray(inst1.TransitTimes);
        Console.WriteLine("\n\n");
        ShowArray(inst1.TransitCosts);
        Console.WriteLine("\n\n");

        Console.WriteLine("Number of time window violations: " + feasibilityChecker.CheckTimeWindows(testroute));
        Console.WriteLine("Number of capacity violations: " + feasibilityChecker.CheckCapacity(testroute, inst1.VehicleCapacities[1]));
        Console.WriteLine("\nPress any key to exit");
        Console.ReadKey();
    }

    public static void ShowArray(int[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            Console.WriteLine();
            for (int j = 0; j < array.GetLength(1); j++)
                Console.Write(array[i,j] +",");
        }
    }
}