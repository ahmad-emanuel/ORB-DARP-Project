using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORB.DARP
{
   public class FeasibilityCheck
    {
        private Instance Instance;

        public FeasibilityCheck(Instance instance)
        {
            Instance = instance;

        }

        public int CheckTimeWindows(int[] route)
        {
            var violations = 0;

            var helpRoute = new int[route.Length];
            helpRoute[0] = Instance.TransitTimes[0, route[0]];

            for (int i = 1; i < route.Length; i++)
            {
                helpRoute[i] =  Math.Max(helpRoute[i-1] + Instance.TransitTimes[route[i-1], route[i]], Instance.TimeWindows[0, route[i]-1]);
            }

            for (int i = 0; i < route.Length; i++)
            {
                Console.WriteLine("Helproute: " + helpRoute[i] + "  Route: " + Instance.TimeWindows[1, route[i]-1]);

                if (helpRoute[i] > Instance.TimeWindows[1, route[i]-1] || helpRoute[i] > Instance.MaxTime)
                {
                    violations++;
                }
            }

            return violations;
        }


        public int CheckCapacity(int[] route, int capacity)
        {
            var violations = 0;
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
                    violations++;
                }
            }

            return violations;
        }
    }
}
