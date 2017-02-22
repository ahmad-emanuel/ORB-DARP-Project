using System.Collections.Generic;

namespace ORB.DARP
{
    public class Route
    {
        private Instance Instance;
        private FeasibilityCheck Checker;

        private List<int> Customers = new List<int>();

        public int TotalRouteDuration { get; private set; }
        public int TotalTimeWindowsViolations { get; private set; }
        public int TotalCapacitiesViolations { get; private set; }

        public Route(Instance instance)
        {
            Instance = instance;
            Checker = new FeasibilityCheck(Instance);
        }

        public Route(Route copy)
        {
            Instance = copy.Instance;
            Checker = copy.Checker;

            foreach (var customer in copy.Customers)
            {
                Customers.Add(customer);
            }
        }

        public void Check(int vehicle)
        {
            var temp = Checker.CheckTimeWindows(DecodedRouteToArray());
            TotalRouteDuration = temp[0];
            TotalTimeWindowsViolations = temp[1];

            TotalCapacitiesViolations = Checker.CheckCapacities(DecodedRouteToArray(), Instance.VehicleCapacities[vehicle]);
        }

        public void AddCustomer(int customer)
        {
            Customers.Add(customer);
            Customers.Add(customer);
        }

        public void RemoveCustomer(int customer)
        {
            Customers.Remove(customer);
            Customers.Remove(customer);
        }

        public void SwapCustomer(int c1, int c2)
        {
            var temp = Customers[c1];
            Customers[c1] = Customers[c2];
            Customers[c2] = temp;
        }

        public int GetCustomerCount()
        {
            return Customers.Count;
        }

        public int GetCustomer(int index)
        {
            return Customers[index];
        }

        public List<int> GetCustomers()
        {
            return Customers;
        }

        public int Decode(int customer, int index)
        {
            var i = 0;
            var counter = -1;

            while (i <= index)
            {
                if (Customers[i] == customer)
                {
                    counter++;
                }

                i++;
            }

            return counter * Instance.Customers + customer;
        }

        public void DecodeRoute()
        {
            for (int i = 0; i < Customers.Count; i++)
            {
                Customers[i] = Decode(Customers[i], i);
            }
        }

        public int[] DecodedRouteToArray()
        {
            var decoded = new int[Customers.Count];

            for (int i = 0; i < decoded.Length; i++)
            {
                decoded[i] = Decode(Customers[i], i);
            }

            return decoded;
        }

        public bool IsFeasibleRoute(int vehicle)
        {
            Check(vehicle);

            if (TotalTimeWindowsViolations == 0 && TotalCapacitiesViolations == 0)
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