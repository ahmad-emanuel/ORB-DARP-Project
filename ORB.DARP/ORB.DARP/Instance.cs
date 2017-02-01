using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORB.DARP
{
    public class Instance
    {

        private string Path;

        public int Customers { get; set; }
        public int MaxTime { get; set; }
        public int Vehicles { get; set; }
        public int[] VehicleCapacity { get; set; }
        public int[,] TransitTimes { get; set; }
        public int[,] TransitCosts { get; set; }
        public int[,] TimeWindow { get; set; }
        public int[,] preferences { get; set; }

        public Instance(string path)
        {
            this.Path = path;
        }

        public void Initialization()
        {
            var temp = File.ReadLines(Path)
                .Select(line => line.Split(' '))
                .ToArray();

            Customers = Int32.Parse(temp[0][1]);
            MaxTime = Int32.Parse(temp[1][1]);
            Vehicles = Int32.Parse(temp[2][1]);

            VehicleCapacity = new int[Vehicles];
            for (int i = 1; i < temp[3].Length; i++)
            {
                VehicleCapacity[i - 1] = Int32.Parse(temp[3][i]);
            }

            TransitTimes = new int[2 * Customers, 2 * Customers];
            for (int i = 5; i <= (2 * Customers) + 4; i++)
                for (int j = 0; j < temp[i].Length; j++)
                {
                    TransitTimes[i - 5, j] = Int32.Parse(temp[i][j]);
                }

            TransitCosts = new int[2 * Customers, 2 * Customers];
            for (int i = 5 + (2 * Customers) + 1; i <= (4 * Customers) + 5; i++)
                for (int j = 0; j < temp[i].Length; j++)
                {
                    TransitCosts[i - ((2 * Customers) + 6), j] = Int32.Parse(temp[i][j]);
                }

            TimeWindow = new int[2, 2 * Customers];
            for (int i = 7 + (4 * Customers); i <= (4 * Customers) + 8; i++)
                for (int j = 0; j < temp[i].Length; j++)
                {
                    TimeWindow[i - (7 + 4 * Customers), j] = Int32.Parse(temp[i][j]);
                }

            preferences = new int[Vehicles, Customers];
            for (int i = 10 + (4 * Customers); i <= (4 * Customers) + 9 + Vehicles; i++)
                for (int j = 0; j < temp[i].Length; j++)
                {
                    preferences[i - (10 + 4 * Customers), j] = Int32.Parse(temp[i][j]);
                }
        }
    }
}
