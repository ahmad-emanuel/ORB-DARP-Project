using System.Collections.Generic;

namespace ORB.DARP
{
    public class LNS
    {
        private List<List<int>> BestSolution;
        private List<List<int>> CurrentSolution;
        private List<int> RelaxedCustomers = new List<int>();

        private Instance Instance;
        private HillClimb Climber;
        private FeasibilityCheck Checker;

        public LNS(Instance instance, List<List<int>> solution, double w1, double w2, double w3)
        {
            Instance = instance;
            BestSolution = solution;
            CurrentSolution = solution;
            Checker = new FeasibilityCheck(Instance);
            Climber = new HillClimb(Instance, Checker, w1, w2, w3);
        }

        private void Destroy()
        {

        }

        private void Repair()
        {
            foreach (var customer in RelaxedCustomers)
            {
                var random = RandomNumber.Between(0, Instance.Vehicles - 1);
                var route = CurrentSolution[random];

                route.Add(customer);

                CurrentSolution[random] = Climber.Improve(route.ToArray(), random);
            }
        }

        public void MinimizeCosts()
        {

        }

        public List<List<int>> GetSolution()
        {
            return BestSolution;
        }
    }
}
