using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORB.DARP
{
    public class LNS
    {
        private List<List<int>> Solution;
        private List<int> RelaxedCustomers = new List<int>();

        private Instance Instance;
        private HillClimb Climber;
        private FeasibilityCheck Checker;

        public LNS(Instance instance, List<List<int>> solution, double w1, double w2, double w3)
        {
            Instance = instance;
            Solution = solution;
            Checker = new FeasibilityCheck(Instance);
            Climber = new HillClimb(Instance, Checker, w1, w2, w3);
        }

        public void Destroy()
        {

        }

        public void Repair()
        {
            foreach (var customer in RelaxedCustomers)
            {
                var random = RandomNumber.Between(0, Instance.Vehicles - 1);
                var route = Solution[random];

                route.Add(customer);

                Solution[random] = Climber.Improve(route.ToArray(), random);
            }
        }
    }
}
