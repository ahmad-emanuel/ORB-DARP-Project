using System.Collections.Generic;
using System;

namespace ORB.DARP
{
    class SequentialConstruction
    {
        private Instance Instance;
        private HillClimb Climber;
        private FeasibilityCheck Checker;

        private List<int> CustomersLeft;

        public SequentialConstruction(Instance instance, double w1, double w2, double w3)
        {
            Instance = instance;
            Checker = new FeasibilityCheck(Instance);
            Climber = new HillClimb(Instance, Checker, w1, w2, w3);
            CustomersLeft = new List<int>(Instance.Customers);

            for (int i = 1; i <= Instance.Customers; i++)
            {
                CustomersLeft.Add(i);
            }
        }

        public List<int[]> Construct()
        {
            var solution = new List<int[]>(Instance.Vehicles);
            var random = new Random();

            while (CustomersLeft.Count > 0 && solution.Count != Instance.Vehicles)
            {
                var route = new List<int>();

                for (int i = 0; i < CustomersLeft.Count; i++)
                {
       
                    var customer = CustomersLeft[random.Next(0, CustomersLeft.Count-1)]; // Gets a random customer
                    CustomersLeft.Remove(customer);

                    route.Add(customer);
                    route.Add(customer);

                    route = Climber.Improve(route.ToArray(), solution.Count);
                    Checker.CheckRoute(Climber.DecodeRoute(route.ToArray()), solution.Count);

                    if (Checker.IsFeasibleRoute())
                    {
                        i = -1;
                    }
                    else
                    {
                        CustomersLeft.Add(customer);

                        route.Remove(customer);
                        route.Remove(customer);
                    }
                }

                solution.Add(Climber.DecodeRoute(route.ToArray()));
            }

            return solution;
        }
    }
}
