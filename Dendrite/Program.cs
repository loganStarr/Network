using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace Dendrite
{
    
    internal class Program
    {



        public static void Crossover(NeuralNetwork winner, NeuralNetwork loser, Random random)
        {
            //skip the inputs because they have no weights
            for (int i = 1; i < winner.layers.Length; i++)
            {
                //References to the Layers
                Layer winLayer = winner.layers[i];
                Layer childLayer = loser.layers[i];

                int cutPoint = random.Next(winLayer.Neurons.Length); //calculate a cut point for the layer
                bool flip = random.Next(2) == 0; //randomly decide which side of the cut point will come from winner

                //Either copy from 0->cutPoint or cutPoint->Neurons.Length from the winner based on the flip variable
                for (int j = (flip ? 0 : cutPoint); j < (flip ? cutPoint : winLayer.Neurons.Length); j++)
                {
                    //References to the Neurons
                    Neuron winNeuron = winLayer.Neurons[j];
                    Neuron childNeuron = childLayer.Neurons[j];

                    //Copy the winners Weights and Bias into the loser/child neuron
                    childNeuron.bias = winNeuron.bias;
                    for (int d = 0; d < winNeuron.dendrites.Length; d++)
                        childNeuron.dendrites[d].Weight = winNeuron.dendrites[d].Weight;
                }
            }
        }

        //This implementation will either change the weight/bias by some percentage change the sign of the weight/bias
        public static void Mutate(NeuralNetwork net, Random random, double mutationRate)
        {
            //skip the input layer because it has no dendrites
            foreach (Layer layer in net.layers.Skip(1))
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    //Mutate the Weights
                    for (int i = 0; i < neuron.dendrites.Length; i++)
                    {
                        if (random.NextDouble() < mutationRate)
                        {
                            if (random.Next(2) == 0)
                            {
                                neuron.dendrites[i].Weight *= random.NextDouble(); //scale weight
                            }
                            else
                            {
                                neuron.dendrites[i].Weight *= -1; //flip sign
                            }
                        }
                    }

                    //Mutate the Bias
                    if (random.NextDouble() < mutationRate)
                    {
                        if (random.Next(2) == 0)
                        {
                            neuron.bias *= random.NextDouble(); //scale weight
                        }
                        else
                        {
                            neuron.bias *= -1; //flip sign
                        }
                    }
                }
            }

        }
        public static double MinError = 10000;
        public static double[] Err;
        public static void Train((NeuralNetwork net, double fitness)[] population, Random random, double mutationRate, double[][] Inputs, double[][] Output)
        {

           
            for (int i = 0; i < population.Length; i++)
            {
                population[i].fitness = (population[i].net.GetError(Inputs,Output));
            }
            
            Array.Sort(population, (a, b) => a.fitness.CompareTo(b.fitness));
            if (population[0].net.GetError(Inputs, Output) < MinError)
            {
                MinError = population[0].net.GetError(Inputs, Output); // it's not jover 
            }
            int start = (int)(population.Length * 0.1);
            int end = (int)(population.Length * 0.9);
            
            //Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Best: {population[0].fitness}");
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine($"Network: {i}");
                for (int ins = 0; ins < 4; ins++)
                {
                    Console.WriteLine($"{Inputs[ins][0]},{Inputs[ins][1]}");
                    Console.WriteLine(population[i].net.Compute(Inputs[ins])[0]);
                }
            }
            //Notice that this process is only called on networks in the middle 80% of the array
            for (int i = start; i < end; i++)
            {
                Crossover( population[random.Next(start,end)].net,  population[i].net, random);
                Mutate( population[i].net, random, mutationRate);
            }
            
            //Removes the worst performing networks
            for (int i = end; i < population.Length; i++)
            {
                population[i].net.Randomize(random,0,1);
            }
            if(population[0].net.GetError(Inputs, Output) > MinError)
            {
                ;
            }

        }
        
        static void Main(string[] args)
        {
            int[] ints = new int[3];
            ints[0] = 2;
            ints[1] = 4;
            ints[2] = 1;
            double[][] inputs = new double[4][];
            inputs[0] = new double[] { 1, 1 };
            inputs[1] = new double[] { 1, 0 };
            inputs[2] = new double[] { 0, 1 };
            inputs[3] = new double[] { 0, 0 };
            double[][] outputs = new double[4][];
            outputs[0] = new double[] { 1 };
            outputs[1] = new double[] { 1 };
            outputs[2] = new double[] { 1 };
            outputs[3] = new double[] { 1 };

            (NeuralNetwork net, double fitness)[] population = new (NeuralNetwork net, double fitness)[50];
            for (int i = 0; i < population.Length; i++)
            {
                population[i].net = new NeuralNetwork(new ActivationFunction(null, null), new ErrorFunction(null, null), inputs, ints);
                population[i].net.Randomize(new Random(), 0,1);
                ;
            }
            int generation = 0;
            while (true)
            {
                generation++;
                Train(population,new Random(),.281,inputs,outputs);
                ;
            }
            Console.WriteLine("Hello, World!");
        }
    }
}