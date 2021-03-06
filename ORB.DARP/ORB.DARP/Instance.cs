﻿using System.IO;
using System.Linq;

namespace ORB.DARP
{
    public class Instance
    {
        private string Path;

        public int Customers { get; private set; }
        public int MaxTime { get; private set; }
        public int Vehicles { get; private set; }
        public int[] VehicleCapacities { get; private set; }
        public int[,] TransitTimes { get; private set; }
        public int[,] TransitCosts { get; private set; }
        public int[,] TimeWindows { get; private set; }
        public int[,] Preferences { get; private set; }

        public Instance(string path)
        {
            Path = path;

            Initialization();
        }

        private void Initialization()
        {
            var temp = File.ReadLines(Path)
                .Select(line => line.Split(' '))
                .ToArray();

            Customers = int.Parse(temp[0][1]);
            MaxTime = int.Parse(temp[1][1]);
            Vehicles = int.Parse(temp[2][1]);

            VehicleCapacities = new int[Vehicles];
            for (int i = 1; i < temp[3].Length; i++)
            {
                VehicleCapacities[i - 1] = int.Parse(temp[3][i]);
            }

            TransitTimes = new int[2 * Customers + 1, 2 * Customers + 1];
            for (int i = 5; i <= (2 * Customers) + 5; i++)
            {
                for (int j = 0; j < (i-5); j++)
                    TransitTimes[i - 5, j] = TransitTimes[j,i-5];
                for (int j = (i-4); j < 2*Customers+1; j++)
                    TransitTimes[i - 5, j] = int.Parse(temp[i][j-i+4]);
            }

            TransitCosts = new int[2 * Customers + 1, 2 * Customers + 1];
            for (int i = 5 + (2 * Customers) + 1; i <= (4 * Customers) + 6; i++)
                {
                    for (int j = 0; j < i - (2 * Customers + 6); j++)
                        TransitCosts[i - (2 * Customers + 6), j] = TransitCosts[j, i - (2 * Customers + 6)];
                    for (int j = (i - (2 * Customers + 5)); j < 2 * Customers + 1; j++)
                        TransitCosts[i - (2 * Customers + 6), j] = int.Parse(temp[i][j - i + (2 * Customers + 5)]);
                }

            TimeWindows = new int[2, 2 * Customers];
            for (int i = 7 + (4 * Customers); i <= (4 * Customers) + 8; i++)
                for (int j = 0; j < temp[i].Length; j++)
                {
                    TimeWindows[i - (7 + 4 * Customers), j] = int.Parse(temp[i][j]);
                }

            Preferences = new int[Vehicles, Customers];
            for (int i = 10 + (4 * Customers); i <= (4 * Customers) + 9 + Vehicles; i++)
                for (int j = 0; j < temp[i].Length; j++)
                {
                    Preferences[i - (10 + 4 * Customers), j] = int.Parse(temp[i][j]);
                }
        }
    }
}