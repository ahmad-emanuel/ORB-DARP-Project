using System.Collections.Generic;

namespace ORB.DARP
{
    public class Solution
    {
        private Instance Instance;

        private List<Route> Routes = new List<Route>();

        public Solution(Instance instance)
        {
            Instance = instance;
        }

        public Solution(Solution copy)
        {
            Instance = copy.Instance;

            foreach (var route in copy.Routes)
            {
                Routes.Add(new Route(route));
            }
        }

        public void AddRouteToSolution(Route route)
        {
            Routes.Add(route);
        }

        public Route GetRoute(int index)
        {
            return Routes[index];
        }

        public int GetVehicleCount()
        {
            return Routes.Count;
        }

        public int GetObjective()
        {
            var costs = 0;
            var vehicle = 0;

            foreach (var route in Routes)
            {
                costs += Instance.TransitCosts[0, route.GetCustomers()[0]];

                for (int i = 0; i <= route.GetCustomerCount() - 2; i++)
                {
                    costs += Instance.TransitCosts[route.GetCustomers()[i], route.GetCustomers()[i+1]];

                    if (route.GetCustomers()[i] <= Instance.Customers)
                    {
                        costs += Instance.Preferences[vehicle, route.GetCustomers()[i]-1];
                    }
                }

                costs += Instance.TransitCosts[route.GetCustomers()[route.GetCustomerCount()-1], 0];

                vehicle++;
            }

            return costs;
        }

        public void DecodeSolution()
        {
            for (int i = 0; i < Routes.Count; i++)
            {
                Routes[i].DecodeRoute();
            }
        }

        public bool IsFeasibleSolution()
        {
            var customerCount = 0;

            for (int i = 0; i < Routes.Count; i++)
            {
                if (!Routes[i].IsFeasibleRoute(i))
                {
                    return false;
                }

                customerCount += Routes[i].GetCustomerCount() / 2;
            }

            if (customerCount == Instance.Customers)
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
