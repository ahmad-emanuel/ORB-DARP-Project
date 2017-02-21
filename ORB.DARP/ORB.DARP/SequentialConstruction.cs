using System.Collections.Generic;
using System;

namespace ORB.DARP
{
    public class SequentialConstruction
    {
        private Instance Instance;
        private HillClimb Climber;
        private FeasibilityCheck Checker;

        private List<int> CustomersLeft;

        public SequentialConstruction(Instance instance, double w1, double w2, double w3)
        {
            Instance = instance;
            Checker = new FeasibilityCheck(Instance);
            Climber = new HillClimb(Instance, Checker, w1, w2, w3);

            CustomersLeft = new List<int>(Instance.Customers);

            for (int i = 1; i <= Instance.Customers; i++)
            {
                CustomersLeft.Add(i);
            }
        }

        public List<List<int>> Construct()
        {
            var solution = new List<List<int>>(Instance.Vehicles);

            while (solution.Count != Instance.Vehicles)
            {
                var route = new List<int>();

                for (int i = 0; i < CustomersLeft.Count; i++)
                {
                    var customer = CustomersLeft[RandomNumber.Between(0, CustomersLeft.Count-1)];
                    CustomersLeft.Remove(customer);

                    route.Add(customer);
                    route.Add(customer);

                    route = Climber.Improve(route.ToArray(), solution.Count);
                    
                    if (Checker.IsFeasibleRoute(HillClimb.DecodeRoute(route.ToArray()), solution.Count))
                    {
                        i = -1;
                    }
                    else
                    {
                        CustomersLeft.Add(customer);

                        route.Remove(customer);
                        route.Remove(customer);
                    }
                }

                solution.Add(route);
            }


            while (CustomersLeft.Count > 0)
            {
                

                for (int j = 0; j < Instance.Vehicles; j++)
                {
                    var customer = CustomersLeft[0];
                    CustomersLeft.Remove(customer);

                    solution[j].Add(customer);
                    solution[j].Add(customer);

                    solution[j] = Climber.ExtendedImprove(solution[j].ToArray(), j,customer);

                    if (Checker.IsFeasibleRoute(HillClimb.DecodeRoute(solution[j].ToArray()), j))
                    {
                        Console.WriteLine(customer + " is added");
                        break;
                    }
                    else
                    {
                        Console.WriteLine(customer + " is deleted");
                        CustomersLeft.Add(customer);

                        solution[j].Remove(customer);
                        solution[j].Remove(customer);
                    }
                }
            }

            Console.WriteLine(CustomersLeft.ToArray().ToString());

            return solution;
        }
    }
}
