namespace ORB.DARP
{
    public class HillClimb
    {
        private Instance Instance;

        private double w1;
        private double w2;
        private double w3;

        public HillClimb(Instance instance, double w1, double w2, double w3)
        {
            Instance = instance;

            this.w1 = w1;
            this.w2 = w2;
            this.w3 = w3;
        }

        public void Improve(Route route, int vehicle)
        {
            var improvements = 0;

            do
            {
                improvements = 0;

                for (int i = 0; i < route.GetCustomerCount(); i++)
                {
                    for (int j = i + 1; j < route.GetCustomerCount(); j++)
                    {
                        if (route.GetCustomer(i) != route.GetCustomer(j) && Instance.TimeWindows[1, route.Decode(route.GetCustomer(i), i)-1] > Instance.TimeWindows[1, route.Decode(route.GetCustomer(j), j)-1])
                        {
                            var costOldRoute = GetObjective(route, vehicle);

                            route.SwapCustomer(i, j);

                            var costNewRoute = GetObjective(route, vehicle);

                            if (costNewRoute - costOldRoute >= 0)
                            {
                                route.SwapCustomer(i, j);
                            }
                            else
                            {
                                improvements++;
                            }
                        }
                    }
                }
            } while (improvements != 0);
        }

        private double GetObjective(Route route, int vehicle)
        {
            route.Check(vehicle);

            return w1 * route.TotalRouteDuration + w2 * route.TotalTimeWindowsViolations + w3 * route.TotalCapacitiesViolations;
        }
    }
}