using System.Collections.Generic;
using System;

namespace ORB.DARP
{
    public class LNS
    {
        private Instance Instance;
        private HillClimb Climber;

        private Solution BestSolution;
        private Solution CurrentSolution;
        private Solution NewSolution;

        public LNS(Instance instance, Solution solution, double w1, double w2, double w3)
        {
            Instance = instance;
            Climber = new HillClimb(Instance, w1, w2, w3);

            BestSolution = solution;
            CurrentSolution = solution;
        }

        private List<int> Destroy(int requests)
        {
            NewSolution = new Solution(CurrentSolution);
            
            var relaxedCustomer = new List<int>();

            for (int i = 0; i < requests; i++)
            {
                var randomCustomer = RandomNumber.IntBetween(1, Instance.Customers);

                for (int j = 0; j < NewSolution.GetVehicleCount(); j++)
                {
                    if (NewSolution.GetRoute(j).GetCustomers().Contains(randomCustomer))
                    {
                        NewSolution.GetRoute(j).RemoveCustomer(randomCustomer);

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
                var random = RandomNumber.IntBetween(0, Instance.Vehicles - 1);
                NewSolution.GetRoute(random).AddCustomer(customer);

                Climber.Improve(NewSolution.GetRoute(random), random);
            }
        }

        public Solution MinimizeCosts(int maxSize, int range, int iterations, double probability)
        {
            for (int i = 2; i <= maxSize-range; i++)
            {
                for (int j = 0; j <= range; j++)
                {
                    for (int k = 0; k < iterations; k++)
                    {
                        Repair(Destroy(i + j));

                        if (NewSolution.IsFeasibleSolution())
                        {
                            if (NewSolution.GetObjective() < CurrentSolution.GetObjective() || RandomNumber.DoubleBetween(0, 1) < probability)
                            {
                                CurrentSolution = NewSolution;

                                if (CurrentSolution.GetObjective() < BestSolution.GetObjective())
                                {
                                    BestSolution = CurrentSolution;
                                }
                            }
                        }
                    }
                }
            }

            return BestSolution;
        }
    }
}
