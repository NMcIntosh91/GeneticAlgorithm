using System;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;

namespace GeneticAlgolrithm
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var optionsNo = 0;

            Console.WriteLine("*****START*******");
            Console.WriteLine();
            
            Initializer initializer = new Initializer();
            Fitness fitness = new Fitness();
            Selector selector = new Selector();
            Crossover crossover = new Crossover();

            StartUp();

            while (optionsNo != 9)
            {
                switch (DisplayMenu())
                {
                    case 1:
                        Console.WriteLine("GENERATION 1");
                        initializer.DisplayPopulation(initializer.GetPopulation(), fitness);
                        break;
                    
                    case 2:
                        FullCycle();
                        break;

                    case 3:
                        fitness.SetFitnessMethod();
                        initializer.DisplayPopulation(initializer.GetPopulation(), fitness);
                        Wait();
                        break;
                    
                    case 8:
                        initializer.CreatePopulation();
                        initializer.DisplayPopulation(initializer.GetPopulation(), fitness);
                        Wait();
                        break;
                    
                    case 9:
                        Console.WriteLine("This Program is over. Bye!");
                        break;
                    
                    default:
                        Console.WriteLine("Try Again!");
                        break;
                }
            }

            void StartUp()
            {
                initializer.CreatePopulation();
                fitness.SetFitnessMethod();
                initializer.DisplayPopulation(initializer.GetPopulation(), fitness);
                Wait();
            }

            int DisplayMenu()
            {
                Console.WriteLine();
                Console.WriteLine("OPTIONS: ");
                Console.WriteLine("------------------");
                Console.WriteLine("1- Display Population");
                Console.WriteLine("2- Full Cycle");
                Console.WriteLine("3- Change Fitness Function");
                Console.WriteLine("8- Re-Initiation");
                Console.WriteLine("9- Exit");
                Console.WriteLine();
                optionsNo = initializer.CheckIntegerInput("Please Select an Option:");
                return optionsNo;

            }

            void Wait()
            {
                Console.WriteLine();
                Console.WriteLine("Next...");
                Console.ReadLine();
            }

            void FullCycle()
            {
                //Set Up Generation Cycle and Parameters
                var isPhaseSkip = false;
                var isFound = false;
                var inputNo = 0;
                initializer.ResetStats();
                selector.SetNoOfCycles();
                selector.SetSelectionType();
                crossover.SetCrossoverMethod();
                selector.SetMutationRate();


                while (isFound == false)
                {
                    Console.WriteLine("Would you like to skip the individual steps on each generations (Selection, Mutation, etc) and just the final results of each generation ? ");
                    Console.WriteLine("1 - Yes");
                    Console.WriteLine("2 - No");
                    inputNo = initializer.CheckIntegerInput();

                    if (inputNo == 1 || inputNo == 0)
                    {
                        isFound = true;
                        
                        if (inputNo == 1)
                        {
                            isPhaseSkip = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please choose a enter option");
                    }

                }

                //For Each Cycle
                for (int i = 0; i < selector.GetNotOfCycles(); i++)
                {

                    Console.WriteLine("GENERATION " + (i + 1));

                    //Selection Phase
                    if (selector.GetSelectionType() == 2)
                    {
                        Selector.OrderSamples(initializer.GetPopulation(), initializer.GetGenomeLength(), initializer.GetPopulationSize());
                        initializer.SetSelectionCandidates(Selector.RankSelection(initializer));
                    }
                    //Roulette Wheel Selection
                    else
                    {
                        initializer.SetSelectionCandidates(Selector.RouletteWheel(initializer, fitness.GetTotalPoints()));
                    }

                    if (isPhaseSkip == false)
                    {
                        //Display Selected Candidates
                        Console.WriteLine("After Selection...");
                        initializer.DisplayPopulation(initializer.GetPopulation(), fitness, selector.GetSelectionName(), crossover.GetCrossoverName(), selector.GetMutationRate());
                        Wait();
                    }
                   
                    
                    
                    //Reproduction Phase
                    if (crossover.GetCrossoverMethod() == 1)
                    {
                        //Single Point Crossover
                        initializer.SetSelectionCandidates(crossover.SinglePointCrossover(initializer));
                    }
                    else
                    {
                        //Uniform Crossover
                        initializer.SetSelectionCandidates(crossover.UniformCrossover(initializer));
                    }

                    if (isPhaseSkip == false)
                    {
                        Console.WriteLine("After Crossover");
                        initializer.DisplayPopulation(initializer.GetPopulation(), fitness, selector.GetSelectionName(), crossover.GetCrossoverName(), selector.GetMutationRate());
                        Wait();
                    }
                    
                    initializer.SetSelectionCandidates(selector.Mutation(initializer));

                    if (isPhaseSkip == false)
                    {
                        //Mutation Phase
                        Console.WriteLine("After Mutation...");
                        initializer.DisplayPopulation(initializer.GetPopulation(), fitness,  selector.GetSelectionName(), crossover.GetCrossoverName(), selector.GetMutationRate());
                        Wait();
                    }

                    if (isPhaseSkip)
                    {
                        initializer.DisplayPopulation(initializer.GetPopulation(), fitness,  selector.GetSelectionName(), crossover.GetCrossoverName(), selector.GetMutationRate());
                        Wait();
                    }
                    
                    //Q FOR QUIT
                    

                }
                
                initializer.ResetStats();
            }
        }
    }
}