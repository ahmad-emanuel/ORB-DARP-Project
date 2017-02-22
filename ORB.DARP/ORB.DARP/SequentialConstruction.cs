using System.Collections.Generic;

namespace ORB.DARP
{
    public class SequentialConstruction
    {
        private Instance Instance;
        private HillClimb Climber;

        private List<int> CustomersLeft;

        public SequentialConstruction(Instance instance, double w1, double w2, double w3)
        {
            Instance = instance;
            Climber = new HillClimb(Instance, w1, w2, w3);

            CustomersLeft = new List<int>(Instance.Customers);

            Reset();
        }

        public void Construct()
        {
            var solution = new Solution(Instance);

            while (CustomersLeft.Count > 0 && solution.GetVehicleCount() != Instance.Vehicles)
            {
                var route = new Route(Instance);

                for (int i = 0; i < CustomersLeft.Count; i++)
                {
                    var customer = CustomersLeft[RandomNumber.IntBetween(0, CustomersLeft.Count-1)];
                    CustomersLeft.Remove(customer);

                    route.AddCustomer(customer);

                    Climber.Improve(route, solution.GetVehicleCount());
                    
                    if (route.IsFeasibleRoute(solution.GetVehicleCount()))
                    {
                        i = -1;
                    }
                    else
                    {
                        CustomersLeft.Add(customer);

                        route.RemoveCustomer(customer);
                    }
                }

                solution.AddRouteToSolution(route);
            }

            Programm.solution = solution;
        }

        public void Reset()
        {
            CustomersLeft.Clear();

            for (int i = 1; i <= Instance.Customers; i++)
            {
                CustomersLeft.Add(i);
            }
        }
    }
}
