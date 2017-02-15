using System.Collections.Generic;

namespace ORB.DARP
{
    public class LNS
    {
        private Instance Instance;
        private HillClimb Climber;
        private FeasibilityCheck Checker;

        private List<List<int>> BestSolution;
        private List<List<int>> CurrentSolution;
        private List<List<int>> NewSolution;

        public LNS(Instance instance, List<List<int>> solution, double w1, double w2, double w3)
        {
            Instance = instance;
            Checker = new FeasibilityCheck(Instance);
            Climber = new HillClimb(Instance, Checker, w1, w2, w3);

            BestSolution = solution;
            CurrentSolution = solution;
        }

        private List<int> Destroy(int requests)
        {
            NewSolution = CurrentSolution;
            
            var relaxedCustomer = new List<int>();

            for (int i = 0; i < requests; i++)
            {
                var randomCustomer = RandomNumber.Between(1, Instance.Customers);

                for (int j = 0; j < NewSolution.Count; j++)
                {
                    if (NewSolution[j].Contains(randomCustomer))
                    {
                        NewSolution[j].Remove(randomCustomer);
                        NewSolution[j].Remove(randomCustomer);

                        relaxedCustomer.Add(randomCustomer);
                    }
                }
            }

            return relaxedCustomer;
        }

        private void Repair(List<int> relaxedCustomer)
        {
            foreach (var customer in relaxedCustomer)
            {
                var random = RandomNumber.Between(0, Instance.Vehicles - 1);
                var route = NewSolution[random];

                route.Add(customer);
                route.Add(customer);

                NewSolution[random] = Climber.Improve(route.ToArray(), random);
            }
        }

        public List<List<int>> MinimizeCosts(int maxSize, int range, int iterations)
        {
            for (int i = 2; i <= maxSize-range; i++)
            {
                for (int j = 0; j <= range; j++)
                {
                    for (int k = 0; k < iterations; k++)
                    {
                        Repair(Destroy(i + j)); // Check Feasiblity

                        if (Programm.GetObjective(NewSolution) < Programm.GetObjective(CurrentSolution))
                        {
                            CurrentSolution = NewSolution;

                            if (Programm.GetObjective(CurrentSolution) < Programm.GetObjective(BestSolution))
                            {
                                BestSolution = CurrentSolution;
                            }
                        }
                    }
                }
            }

            return BestSolution;
        }
    }
}
