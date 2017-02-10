﻿using System;
using System.Collections.Generic;

namespace ORB.DARP
{
    public class FeasibilityCheck
    {
        private Instance Instance;

        public int TotalRouteDuration { get; private set; }
        public int TotalTimeWindowsViolations { get; private set; }
        public int TotalCapacitiesViolations { get; private set; }

        public FeasibilityCheck(Instance instance)
        {
            Instance = instance;
        }

        private void CheckTimeWindows(int[] route)
        {
            TotalTimeWindowsViolations = 0;

            var helpRoute = new int[route.Length];
            helpRoute[0] = Instance.TransitTimes[0, route[0]];

            for (int i = 1; i < helpRoute.Length; i++)
            {
                helpRoute[i] =  Math.Max(helpRoute[i-1] + Instance.TransitTimes[route[i-1], route[i]], Instance.TimeWindows[0, route[i]-1]);
            }

            for (int i = 0; i < helpRoute.Length; i++)
            {
                if (helpRoute[i] > Instance.TimeWindows[1, route[i]-1] || helpRoute[i] > Instance.MaxTime)
                {
                    TotalTimeWindowsViolations++;
                }
            }

            TotalRouteDuration = helpRoute[route.Length - 1] + Instance.TransitTimes[route[route.Length - 1], 0];

            if (TotalRouteDuration > Instance.MaxTime)
            {
                TotalTimeWindowsViolations++;
            }
        }

        private void CheckCapacities(int[] route, int capacity)
        {
            TotalCapacitiesViolations = 0;
            var customers = 0;

            for (int i = 0; i < route.Length; i++)
            {
                if(route[i] <=  Instance.Customers)
                {
                    customers++;
                }
                else
                {
                    customers--;
                }

                if(customers > capacity)
                {
                    TotalCapacitiesViolations++;
                }
            }
        }

        public void CheckRoute(int[] route, int vehicle)
        {
            CheckTimeWindows(route);
            CheckCapacities(route, Instance.VehicleCapacities[vehicle]);
        }

        public bool IsFeasibleRoute()
        {
            if (TotalTimeWindowsViolations == 0 && TotalCapacitiesViolations == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsFeasibleSolution(Instance instance, List<int[]> solution)
        {
            var customerCount = 0;

            foreach (var route in solution)
            {
                customerCount += route.Length/2;
            }

            if (customerCount == instance.Customers)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
