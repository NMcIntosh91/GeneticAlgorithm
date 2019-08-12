using System;
using System.Xml.Schema;
using System.Threading;

namespace GeneticAlgolrithm
{
    public class Crossover
    {
        Random rand = new Random();
        private int _temp = 0;
        private int _methodNo = 0;
        private String _name = null;
        
        public void SetCrossoverMethod()
        {

            _methodNo = 0;
            
            while (_methodNo != 1 && _methodNo != 2)
            {
                Console.WriteLine("Select your Reproduction/Crossover Method?");
                Console.WriteLine("1 - Single Point Crossover");
                Console.WriteLine("2 - Uniform Crossover");
                Console.WriteLine("8 - Explanation");
                
                if (int.TryParse(Console.ReadLine(), out _methodNo))
                {
                    if (_methodNo == 8)
                    {
                        Console.WriteLine("Explanation:");
                        Console.WriteLine("1 - Single Point Crossover - A point on both parents' chromosomes is picked randomly, and designated a 'crossover point'.");
                        Console.WriteLine("2 - Uniform Crossover - Each bit is chosen from either parent with equal probability.");
                    }
                    else if (_methodNo == 1)
                    {
                        _name = "Single Point Crossover";
                    }
                    else if (_methodNo == 2)
                    {
                        _name = "Uniform Crossover";
                    }
                    else
                    {
                        Console.WriteLine("Please enter a correct number");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a NUMBER.");
                    Console.WriteLine();
                }
            }
        }

        public String GetCrossoverName()
        {
            return _name;
        }

        public int GetCrossoverMethod()
        {
            return _methodNo;
        }
        
        public int[,] SinglePointCrossover(Initializer initializer)
        {
            var length = initializer.GetGenomeLength();
            var population = initializer.GetPopulationSize();
            int [,] world = initializer.GetPopulation();
            var point = 0;

            //Every Pair is a Parent
            for (var i = 0; i < (population - 1); i = i + 2)
            {
                Thread.Sleep(15);
                point = rand.Next(length);

                for (var j = 0; j < point; j++)
                {
                    _temp = world[i, j];
                    world[i, j] = world[i + 1, j];
                    world[i + 1, j] = _temp;
                }
                
            }
           
            return world;
        }

        //Every Pair is a Parent
        public int[,] UniformCrossover(Initializer initializer)
        {
            var length = initializer.GetGenomeLength();
            var population = initializer.GetPopulationSize();
            int [,] world = initializer.GetPopulation();

            for (var i = 0; i < (population - 1); i = i + 2)

                    for (var j = 0; j < length; j++)
                    {
                        Thread.Sleep(15);
                        if (rand.Next(2) == 1)
                        {
                            _temp = world[i, j];
                            world[i, j] = world[i + 1, j];
                            world[i + 1, j] = _temp;
                        }
                    }

            return world;
        }

      
    }
}