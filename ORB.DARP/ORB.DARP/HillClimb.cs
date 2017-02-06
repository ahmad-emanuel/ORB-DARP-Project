using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORB.DARP
{
    class HillClimb
    {
        private Instance Instance;

        public HillClimb(Instance instance)
        {
            Instance = instance;
        }

        public List<int> Improve(int[] route)
        {
            for (int i = 0; i < route.Length; i++)
            {
                for (int j = i+1; j < route.Length; j++)
                {
                    if (route[i] != route[j] && Instance.TimeWindows[1, Decode(route, route[i], i)-1] > Instance.TimeWindows[1, Decode(route, route[j], j)-1])
                    {
                        var temp = route[i];
                        route[i] = route[j];
                        route[j] = temp;
                    }

                    // TODO Check objective function if new route is accepted
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
    }
}
