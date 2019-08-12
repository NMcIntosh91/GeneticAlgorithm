using System;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;
using System.Xml.Serialization;

namespace GeneticAlgolrithm
{
    public class Initializer
    {
        private Random rand = new Random();
        private int [,] _currentCandidates;
        private int _populationSize;
        private int _genomeLength;
        private int _topGenerationCandidate;
        private int _topCycleCandidate;

        private int SetPopulationSize()
        {
            String message = "Enter Population Size";
            return _populationSize = CheckIntegerInput(message);
        }

        public int CheckIntegerInput(String message = null)
        {
            var value = 0;
            var found = false;

            while (found == false)
            {
                if (message != null)
                {
                    Console.WriteLine(message);
                }
                
                if (int.TryParse(Console.ReadLine(), out value))
                {
                    found = true;
                }
                else
                {
                    Console.WriteLine("Please enter a NUMBER.");
                    Console.WriteLine();
                }
            }

            return value;
        }

        public int GetPopulationSize()
        {
            return _populationSize;
        }

        private int SetGenomeLength()
        {
            //Establish Representation Size
            var message = "Enter Genome Length: ";
            return _genomeLength = CheckIntegerInput(message);
            
        }

        public int GetGenomeLength()
        {
            return _genomeLength;
        }

        public int[,] CreatePopulation()
        {
            _populationSize = SetPopulationSize();
            _genomeLength = SetGenomeLength();

            //Create Population Array
            Console.WriteLine("Starting Initiation");
            Console.WriteLine("...");
            Console.WriteLine("...");

            _currentCandidates = new int[_populationSize, _genomeLength + 1];

            //For each Population Member
            for (var i = 0; i < _populationSize; i++)
            {
                //For each bit within member
                for (var j = 0; j < _genomeLength; j++)
                {
                    _currentCandidates[i, j] = rand.Next(2);
                    Thread.Sleep(90);
                }
            }

            _topGenerationCandidate = 0;
            
            return _currentCandidates;
        }

        public int[,] GetPopulation()
        {
            return _currentCandidates;
        }

        public void DisplayPopulation(int[,] population, Fitness fitness, String selectorName = null, String crossoverName = null, Double rate = 0)
        {
            var fitnessNo = fitness.GetFitnessMethod();
            var fitnessIndex = _genomeLength;
            fitness.ResetTotalPoints();
            _topGenerationCandidate = 0;
            
            //Console.WriteLine(message);
            Console.WriteLine("--------------------");
            Console.WriteLine("Fitness Function: " + fitness.GetFitnessName());

            if (selectorName != null)
            {
                Console.WriteLine("Selection Function: " + selectorName);
            }

            if (crossoverName != null)
            {
                Console.WriteLine("Crossover Operator: " +  crossoverName);
            }

            if (rate > 0)
            {
                Console.WriteLine("Mutation Rate: " + rate);
            }

            Console.WriteLine("--------------------");

            for (var i = 0; i < _populationSize; i++)
            {
                Console.Write("Genome " + (i + 1) + ": ");
                fitness.ResetRankPoints();

                for (var j = 0; j < _genomeLength; j++)
                {
                    Console.Write(population[i, j] + " ");

                    //Only for Digit Ranking
                    if (fitnessNo == 1)
                    {
                        fitness.AddRankPoints(population[i, j]);
                        //population[i, fitnessIndex] = fitness.GetRankPoints();
                    }
                    else if (fitnessNo == 2 && population[i, j] == 1)
                    {
                        fitness.AddRankPoints(fitness.CalculateBinaryFitness(_genomeLength, j));
                    }
                }
                
                Console.Write("Fitness: " + fitness.GetRankPoints());
                population[i, fitnessIndex] = fitness.GetRankPoints();

                if (population[i, fitnessIndex] > _topGenerationCandidate)
                {
                    _topGenerationCandidate = fitness.GetRankPoints();
                }

                Console.WriteLine();
            }
            
            if (_topGenerationCandidate > _topCycleCandidate)
            {
                _topCycleCandidate = _topGenerationCandidate;
            }
            
            Console.WriteLine();
            Console.WriteLine("Total Fitness: " + fitness.GetTotalPoints());
            Console.WriteLine("Average Fitness: " + fitness.CalculateAverageFitness(_populationSize));
            Console.WriteLine("Top Candidate: " + _topGenerationCandidate);
            Console.WriteLine("Best Cycle Candidate: " + _topCycleCandidate);
        }

        public void SetSelectionCandidates(int[,] chosenCandidates)
        {
            _currentCandidates = chosenCandidates;
        }

        public void ResetStats()
        {
            _topCycleCandidate = 0;
            _topGenerationCandidate = 0;
        }
    }
}