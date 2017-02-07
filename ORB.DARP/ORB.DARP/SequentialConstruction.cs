using System.Collections.Generic;

namespace ORB.DARP
{
    class SequentialConstruction
    {
        private Instance Instance;
        private HillClimb Climber;
        private FeasibilityCheck Checker;

        private Queue<int> CustomersLeft;

        public SequentialConstruction(Instance instance, double w1, double w2, double w3)
        {
            Instance = instance;
            Checker = new FeasibilityCheck(Instance);
            Climber = new HillClimb(Instance, Checker, w1, w2, w3);
            CustomersLeft = new Queue<int>(Instance.Customers);

            for (int i = 1; i <= Instance.Customers; i++)
            {
                CustomersLeft.Enqueue(i);
            }
        }

        public List<int[]> Construct()
        {
            var solution = new List<int[]>(Instance.Vehicles);

            while (CustomersLeft.Count > 0 && solution.Count != Instance.Vehicles)
            {
                var route = new List<int>();

                for (int i = 0; i < CustomersLeft.Count; i++)
                {
                    var customer = CustomersLeft.Dequeue(); // TODO Get random new customer??

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
                        CustomersLeft.Enqueue(customer);

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
