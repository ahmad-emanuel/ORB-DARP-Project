using System.Collections.Generic;
using System.Linq;

namespace ORB.DARP
{
    class HillClimb
    {
        private Instance Instance;
        private FeasibilityCheck Checker;

        private double w1;
        private double w2;
        private double w3;

        public HillClimb(Instance instance, FeasibilityCheck checker, double w1, double w2, double w3)
        {
            Instance = instance;
            Checker = checker;

            this.w1 = w1;
            this.w2 = w2;
            this.w3 = w3;
        }

        public List<int> Improve(int[] route, int vehicle)
        {
            for (int count = 0; count < 10; count++) // TODO Better!!
            {
                for (int i = 0; i < route.Length; i++)
                {
                    for (int j = i + 1; j < route.Length; j++)
                    {
                        if (route[i] != route[j] && Instance.TimeWindows[1, Decode(route, route[i], i) - 1] > Instance.TimeWindows[1, Decode(route, route[j], j) - 1])
                        {
                            var costOldRoute = GetObjective(route, vehicle);

                            var temp = route[i];
                            route[i] = route[j];
                            route[j] = temp;

                            var costNewRoute = GetObjective(route, vehicle);

                            if (costNewRoute - costOldRoute > 0)
                            {
                                temp = route[i];
                                route[i] = route[j];
                                route[j] = temp;
                            }
                        }
                    }
                }
            }

            return route.ToList();
        }

        private int Decode(int[] route, int customer, int index)
        {
            var i = 0;
            var counter = -1;

            while (i <= index)
            {
                if (route[i] == customer)
                {
                    counter++;
                }

                i++;
            }

            return counter * Instance.Customers + customer;
        }

        public int[] DecodeRoute(int[] route)
        {
            var decoded = new int[route.Length];

            for (int i = 0; i < decoded.Length; i++)
            {
                decoded[i] = Decode(route, route[i], i);
            }

            return decoded;
        }

        private double GetObjective(int[] route, int vehicle)
        {
            Checker.CheckRoute(DecodeRoute(route), vehicle);

            return w1 * Checker.TotalRouteDuration + w2 * Checker.TotalTimeWindowsViolations + w3 * Checker.TotalCapacitiesViolations;
        }
    }
}
