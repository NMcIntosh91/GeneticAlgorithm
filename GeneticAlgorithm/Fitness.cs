using System;
using System.Net.Security;
using System.Runtime.CompilerServices;

namespace GeneticAlgolrithm
{
    public class Fitness
    {
        private int _fitnessNo;
        private int _rankPoints;
        private int _totalPoints;
        private String _name;

        public void SetFitnessMethod()
        {
            _fitnessNo = 0;

            while ((_fitnessNo != 1 && _fitnessNo != 2))
            {
                Console.WriteLine();
                Console.WriteLine("Select the Fitness Scheme to wish to use: ");
                Console.WriteLine("1 - Digit Banking");
                Console.WriteLine("2 - Binary Ranking");
                Console.WriteLine("8 - Explanation");

                if (int.TryParse(Console.ReadLine(), out _fitnessNo))
                {
                    if (_fitnessNo == 8)
                    {
                        Console.WriteLine("Explanation:");
                        Console.WriteLine("1 - Digit Ranking: Individuals will be ranked by the no of 1 in the Genomes.");
                        Console.WriteLine("2 - Binary Ranking: Individuals are ranked suing the Binary Formula.");
                        
                    }
                    else if (_fitnessNo > 2)
                    {
                        Console.WriteLine("Please choose a correct option!");
                    }
                    else if (_fitnessNo == 2)
                    {
                        _name = "Binary Fitness";
                    }
                    else
                    {
                        _name = "Digit Fitness";
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a NUMBER.");
                    Console.WriteLine();
                }
            }
        }

        public int GetFitnessMethod()
        {
            return _fitnessNo;
        }

        public void AddRankPoints(int points)
        {
            _rankPoints += points;
            _totalPoints += points;
        }

        public void ResetRankPoints()
        {
            _rankPoints = 0;
        }
        
        public int GetRankPoints()
        {
            return _rankPoints;
        }
        
        public void ResetTotalPoints()
        {
            _totalPoints = 0;
        }

        public int GetTotalPoints()
        {
            return _totalPoints;
        }


        public String GetFitnessName()
        {
            return _name;
        }

        public decimal CalculateAverageFitness(int population)
        {
            decimal result = _totalPoints / population;
            return result;
        }

        //Calculate Fitness of Binary Representation
        public int CalculateBinaryFitness(int bitSize, int bitPosition)
        {
            var total = 0;
            var indices = ((bitSize - 1) - bitPosition);

            if (indices > 0)
            {
                total = 2;
            }
            else
            {
                total = 1;
            }

            for (var k = 1; k < indices; k++)
            {
                total =+ total * 2;
            }

            return total;
        }
    }
}