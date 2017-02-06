using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORB.DARP
{
    class SequentialConstruction
    {
        private Instance Instance;

        private HillClimb HillClimber;

        private Queue<int> CustomersLeft;

        public SequentialConstruction(Instance instance)
        {
            Instance = instance;
            HillClimber = new HillClimb(Instance);
            CustomersLeft = new Queue<int>(Instance.Customers);

            for (int i = 1; i <= Instance.Customers; i++)
            {
                CustomersLeft.Enqueue(i);
            }
        }

        public ArrayList Construct()
        {
            var solution = new ArrayList(Instance.Vehicles);

            while (CustomersLeft.Count > 0)
            {
                var route = new List<int>();
                var tempCustomersLeft = CustomersLeft.ToArray();

                for (int i = 0; i < tempCustomersLeft.Length; i++)
                {
                    CustomersLeft.Dequeue();

                    route.Add(tempCustomersLeft[i]);
                    route.Add(tempCustomersLeft[i]);

                    route = HillClimber.Improve(route.ToArray());

                    if (route.Count/2 > 2) // TODO Real feasible check
                    {
                        CustomersLeft.Enqueue(tempCustomersLeft[i]);

                        route.Remove(tempCustomersLeft[i]);
                        route.Remove(tempCustomersLeft[i]);
                    }
                }

                solution.Add(route);
            }

            return solution;
        }
    }
}
