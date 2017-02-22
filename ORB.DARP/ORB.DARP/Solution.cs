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
                var decoded = route.DecodedRouteToArray();

                costs += Instance.TransitCosts[0, decoded[0]];

                for (int i = 0; i <= decoded.Length - 2; i++)
                {
                    costs += Instance.TransitCosts[decoded[i], decoded[i+1]];

                    if (decoded[i] <= Instance.Customers)
                    {
                        costs += Instance.Preferences[vehicle, decoded[i]-1];
                    }
                }

                costs += Instance.TransitCosts[decoded[decoded.Length-1], 0];

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