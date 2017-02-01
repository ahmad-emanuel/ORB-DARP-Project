using ORB.DARP;
using System;
using System.IO;
using System.Linq;

class Test
{

    public static void Main()
    {
        string path = @"F:\Z Studiumsablauf Z\5. Semester\OR2\Project\instances\gen_500_50_100_10_20_5.darp";

        Instance inst1 = new Instance(path);
        inst1.Initialization();

        Console.WriteLine(inst1.Customers + "\n" + inst1.MaxTime + "\n" + inst1.Vehicles);
        Console.WriteLine();
        ShowArray(inst1.TransitTimes);

        Console.WriteLine("\nPress any key to exit");
        Console.ReadKey();
    }

    public static void ShowArray(int[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            Console.WriteLine();
            for (int j = 0; j < array.GetLength(1); j++)
                Console.Write("|" + array[i,j] +"|");
        }
    }
}