using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORB.DARP
{
   public class FeasibilityCheck
    {

        public int[] helproute { get; private set; }
        private Instance instance;
        public int violations { get; private set; }
        public int guests { get; private set; }


        public FeasibilityCheck(Instance instance)
        {
            this.instance = instance;

        }

    public int checkTimeWindow(int [] route)
        {
            violations = 0;
            helproute = new int[2 * instance.Customers];
            for (int i = 0; i < route.Length; i++)
            {
                helproute[i] = 0;
               
            }
            helproute[0] = instance.TransitTimes[0, route[0]];
            for (int i = 1; i < route.Length; i++)
            {
                helproute[i] =  Math.Max(helproute[i - 1] + instance.TransitTimes[route[i - 1], route[i]], instance.TimeWindow[0, route[i]-1]);
               
            }

            for (int i = 0; i < route.Length; i++)
            {
                Console.WriteLine("Helproute: " + helproute[i] + "  Route: " + instance.TimeWindow[1, route[i]-1]);

                if ((helproute[i] > instance.TimeWindow[1, route[i]-1]) || helproute[i] > instance.MaxTime){
                    violations++;
                    
               }
            }
            return violations;
        }


        public int checkCapacity(int [] route, int capacity)
        {
            violations = 0;
            guests = 0;

            for (int i = 0; i< route.Length; i++)
            {
                if(route[i] <=  instance.Customers)
                {
                    guests++;
                }
                else
                {
                    guests--;
                }
                if(guests > capacity)
                {
                    violations++;
                }
            }
            return violations;
        }
    }
}
