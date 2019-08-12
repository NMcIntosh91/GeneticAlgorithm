using System;
using System.Threading;

namespace GeneticAlgolrithm
{
    public class Selector
    {
        private int _noOfCycles = 0;
        private int _selectionType = 0;
        private String _selectorName;
        private double _mutationRate = 0;
        private Random rand = new Random();

        public void SetNoOfCycles()
        {
            while (_noOfCycles < 1)
            {
                Console.WriteLine("How many Evolution Cycles do you want?");

                if (int.TryParse(Console.ReadLine(), out _noOfCycles) == false)
                {
                    Console.WriteLine("Enter a NUMBER!");
                    Console.WriteLine("");
                }
            }
        }

        public int GetNotOfCycles()
        {
            return _noOfCycles;
        }

        public int SetSelectionType()
        {
            _selectionType = 0;

            while (_selectionType != 1 && _selectionType != 2)
            {
                Console.WriteLine();
                Console.WriteLine("Choose Selection Algorithm:");
                Console.WriteLine("1 - Roulette Wheel Selection");
                Console.WriteLine("2 - Tournament Selection");
                Console.WriteLine("8 - Explanation");

                if (int.TryParse(Console.ReadLine(), out _selectionType))
                {
                    if (_selectionType == 8)
                    {
                        Console.WriteLine("Explanation:");
                        Console.WriteLine("1 - Roulette Wheel Selection - Individuals are selected based on their fitness in proportion to others.");
                        Console.WriteLine("2 - Tournament Selection - Individuals are selected based upon their ranking order." );
                    }
                    else if (_selectionType > 2)
                    {
                        Console.WriteLine("Please choose a correct option!");
                    }
                    else if (_selectionType == 2)
                    {
                        _selectorName = "Rank Based Selection";
                    }
                    else
                    {
                        _selectorName = "Roulette Wheel Selection";
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a NUMBER.");
                    Console.WriteLine();
                }
            }

            return _selectionType;
        }

        public int GetSelectionType()
        {
            return _selectionType;
        }

        public String GetSelectionName()
        {
            return _selectorName;
        }

        public void SetMutationRate()
        {
            Boolean done = false;
            int answer;

            while (done == false)
            {
                Console.WriteLine("Would you like to enter the Mutation Rate or have it randomly generated?");
                Console.WriteLine("1 - Manual");
                Console.WriteLine("2 - Auto");
                answer = int.Parse(Console.ReadLine());
                if (answer < 3 && answer > 0)
                {
                    if (answer == 1)
                    {
                        Console.WriteLine("Please select Mutation Rate between 0 and 0.1.");
                        _mutationRate = Double.Parse(Console.ReadLine());
                        done = true;
                    }
                    else
                    {
                        //Mutation Rate should be between 0 and 0.1
                        _mutationRate = _mutationRate = 1;
                        while (_mutationRate > 0.1)
                        {
                            _mutationRate = rand.NextDouble();
                        }

                        _mutationRate = Math.Round(_mutationRate, 2);

                        Console.WriteLine("Mutation Rate is: " + _mutationRate);
                        done = true;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong Input!!! Please Enter 1 or 2!!");
                }
                Console.WriteLine();
            }
        }

        public double GetMutationRate()
        {
            return _mutationRate;
        }

        public static void OrderSamples(int[,] world, int sampleLength, int populationSize)
        {
            var found = false;
            var j = 0;
            var pointsPosition = sampleLength;
            int[] tempArray = new int[sampleLength + 1];

            //Order Samples
            for (var i = 0; i < populationSize; i++)
            {
                j = i + 1;
                while (j < populationSize)
                {
                    //Console.WriteLine("Is " + world[j, pointsPosition] + " smaller than " + world[i, pointsPosition] + "? ");
                    if (world[j, pointsPosition] > world[i, pointsPosition])
                    {
                        for (var k = 0; k < sampleLength + 1; k++)
                        {
                            tempArray[k] = world[i, k];
                            world[i, k] = world[j, k];
                            world[j, k] = tempArray[k];
                        }
                    }
                    j++;
                }
            }

            //Assigned Ranks to Samples
            for (var i = 0; i < populationSize; i++)
            {
                world[i, sampleLength] = populationSize - i;
            }
        }

        public static int[,] RankSelection(Initializer initializer)
        {
            var highestRank = initializer.GetPopulationSize();

            return RouletteWheel(initializer, GetTotalRankingPoints(highestRank));
        }

        private static int GetTotalRankingPoints(int highRank)
        {
            int total = 0;

            while (highRank > 0)
            {
                total = total + highRank;
                highRank = highRank - 1;
            }

            return total;
        }

        public static int[,] RouletteWheel(Initializer initializer, int totalPoints)
        {
            Random rand = new Random();
            var populationSize = initializer.GetPopulationSize();
            var sampleLength = initializer.GetGenomeLength();
            int[,] currentPopulation = initializer.GetPopulation();
            int[,] chosenPopulation = new int [populationSize, sampleLength + 1];
            var target = 0;
            var found = false;
            var index = 0;
            var indexPoints = 0;

            //Until Chosen Population is complete
            for (var i = 0; i < populationSize; i++)
            {
                //Total Points is Roulette Wheel
                target = rand.Next(totalPoints);

                index = 0;
                indexPoints = 0;
                found = false;
                //Console.WriteLine("Target is " + target);
                Thread.Sleep(38);

                while (index < populationSize && found == false)
                {
                    indexPoints = indexPoints + currentPopulation[index, sampleLength];

                    if (target <= indexPoints)
                    {
                        //Sample selected for evolution

                        for (int j = 0; j < sampleLength; j++)
                        {
                            chosenPopulation[i, j] = currentPopulation[index, j];
                        }

                        found = true;
                    }
                    else
                    {
                        index++;
                    }
                }
            }
            return chosenPopulation;
        }

        public int[,] Mutation(Initializer initializer)
        {
            Random rand = new Random();
            double mutationRate = (GetMutationRate() * 100);
            int[,] world = initializer.GetPopulation();
            int population = initializer.GetPopulationSize();
            int length = initializer.GetGenomeLength();

            for (int i = 0; i < population; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (rand.Next(0, 101) <= mutationRate)
                    {
                        if (world[i, j] == 0)
                        {
                            world[i, j] = 1;
                        }
                        else
                        {
                            world[i, j] = 0;
                        }
                    }
                }
            }

            return world;
        }
    }
}