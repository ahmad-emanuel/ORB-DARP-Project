using System;

namespace ORB.DARP
{
    public class FeasibilityCheck
    {
        private Instance Instance;

        public FeasibilityCheck(Instance instance)
        {
            Instance = instance;
        }

        public int[] CheckTimeWindows(int[] route)
        {
            if (route.Length == 0)
            {
                return new int[] { 0, 0 };
            }

            var totalTimeWindowsViolations = 0;

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
                    totalTimeWindowsViolations++;
                }
            }

            var totalRouteDuration = helpRoute[route.Length-1] + Instance.TransitTimes[route[route.Length-1], 0];

            if (totalRouteDuration > Instance.MaxTime)
            {
                totalTimeWindowsViolations++;
            }

            return new int[] { totalRouteDuration, totalTimeWindowsViolations };
        }

        public int CheckCapacities(int[] route, int capacity)
        {
            if (route.Length == 0)
            {
                return 0;
            }

            var totalCapacitiesViolations = 0;
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

                if (customers > capacity)
                {
                    totalCapacitiesViolations++;
                }
            }

            return totalCapacitiesViolations;
        }
    }
}