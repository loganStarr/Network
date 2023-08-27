using System.Runtime.CompilerServices;

namespace Dendrite
{
    
    internal class Program
    {
        public class ActivationFunction
        {
            Func<double, double> function;
            Func<double, double> derivative;
            public ActivationFunction(Func<double, double> function, Func<double, double> derivative)
            {
                this.function = function;
                //this.derivative = derivative;
            }

            public double Function(double input)
            {
                return function.Invoke(input);
            }

            public double Derivative(double input)
            {
                return Math.Tanh(input);
            }
        }
        public class ErrorFunction
        {
            Func<double, double, double> function;
            Func<double, double, double> derivative;
            public ErrorFunction(Func<double, double, double> function, Func<double, double, double> derivative)
            {
                this.function = function;
                //this.derivative = derivative;
            }

            public double Function(double output, double desiredOutput)
            {
                return function.Invoke(output, desiredOutput);
            }

            public double Derivative(double output, double desiredOutput)
            {
                double Sum = (output - desiredOutput);
                return Sum * 2;
            }
        }
        public class NeuralNetwork
        {
            public Layer[] layers;
            public ErrorFunction errorFunc;
            public double[][] Inputs;
            public NeuralNetwork(ActivationFunction activation, ErrorFunction errorFunc, double[][] Inputs,
            params int[] neuronsPerLayer)
            {
                layers = new Layer[neuronsPerLayer.Length];
                for (int i = 0; i < neuronsPerLayer.Length; i++)
                {
                    if (i == 0)
                    {
                        layers[i] = new Layer(activation, neuronsPerLayer[i], null);
                    }
                    else
                    {
                        layers[i] = new Layer(activation, neuronsPerLayer[i], layers[i-1]);
                    }
                }
                this.Inputs = Inputs;
            }
            public void Randomize(Random random, double min, double max)
            {
                for (int i = 0; i < layers.Length; i++)
                {
                    layers[i].Randomize(random, min, max);
                }
            }
            public double[] Compute(double[] inputs)
            {
                double[] doubles;
                layers[0].Outputs = inputs;
                for (int i = 1; i < inputs.Length; i++)
                {
                    doubles = layers[i].Compute();
                }
                return layers[layers.Length - 1].Compute();
            }
            public double GetError(double[][] inputs, double[][] desiredOutputs)
            {
                double error = 0;
                for (int i = 0; i < inputs.Length; i++)
                {
                    double[] doubles = Compute(inputs[i]);
                    for (int x = 0; x < doubles.Length; x++)
                    {
                        error = desiredOutputs[i][x] - doubles[x];
                    }
                }
                return error;
            }
        }
        public class Layer
        {
            public Neuron[] Neurons { get; }
            public double[] Outputs;

            public Layer(ActivationFunction activation, int neuronCount, Layer previousLayer) 
            {


                Neurons= new Neuron[neuronCount];
                Outputs= new double[neuronCount];
                for (int i = 0; i < neuronCount; i++)
                {
                    if (previousLayer != null)
                    {
                        Neurons[i] = new Neuron(activation, previousLayer.Neurons);
                    }
                    else
                    {
                        Neurons[i] = new Neuron(activation, null);
                    }
                }
            }
            public void Randomize(Random random, double min, double max)
            {
                for (int i = 0; i < Neurons.Length; i++)
                {
                    Neurons[i].Randomize(random, min, max);
                }
            }
            public double[] Compute() 
            {
                double[] doubles= new double[Neurons.Length];
                for (int i = 0; i < Neurons.Length; i++)
                {
                    doubles[i] = Neurons[i].Compute(); 
                }
                return doubles;
            }
        }

        public class Neuron
        {
            public double bias;
            public Dendrite[] dendrites;
            public double Output;
            public double Input;
            public ActivationFunction Activation { get; set; }

           

            public Neuron(ActivationFunction activation, Neuron[] previousNerons)
            { 
                Activation = activation;
                if (previousNerons != null)
                {
                    dendrites = new Dendrite[previousNerons.Length];
                    for (int i = 0; i < dendrites.Length; i++)
                    {
                        dendrites[i] = new Dendrite(previousNerons[i], this, 0);
                    }
                }
                

            }
            public void Randomize(Random random, double min, double max)
            {
                if (dendrites != null)
                {
                    for (int i = 0; i < dendrites.Length; i++)
                    {
                        dendrites[i].Weight = random.NextDouble() * max - min;
                    }
                    bias = random.NextDouble() * max - min;
                }

            }
            public double Compute()
            {
                double Total = 0;   
                for (int i = 0; i < dendrites.Length; i++)
                {
                    Total += dendrites[i].Compute();
                }
                return Total + bias;
            }
        }
        public static void Mutate(NeuralNetwork net, Random random, double mutationRate)
        {
            foreach (Layer layer in net.layers)
            {
                foreach (Neuron neuron in layer.Neurons)
                {
                    //Mutate the Weights
                    if (neuron.dendrites != null)
                    {
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
        public static void Crossover(NeuralNetwork winner, NeuralNetwork loser, Random random)
        {
            for (int i = 0; i < winner.layers.Length; i++)
            {
                //References to the Layers
                Layer winLayer = winner.layers[i];
                Layer childLayer = loser.layers[i];

                int cutPoint = random.Next(winLayer.Neurons.Length); //calculate a cut point for the layer
                bool flip = random.Next(2) == 0; //randomly decide which side of the cut point will come from winner

                //Either copy from 0->cutPoint or cutPoint->Neurons.Length from the winner based on the flip variable
                if (winLayer.Neurons[0].dendrites != null)
                {
                    for (int j = (flip ? 0 : cutPoint); j < (flip ? cutPoint : winLayer.Neurons.Length); j++)
                    {
                        //References to the Neurons
                        Neuron winNeuron = winLayer.Neurons[j];
                        Neuron childNeuron = childLayer.Neurons[j];

                        //Copy the winners Weights and Bias into the loser/child neuron
                        winNeuron.dendrites.CopyTo(childNeuron.dendrites, 0);
                        childNeuron.bias = winNeuron.bias;
                    }
                }
                
            }
        }

        public static void Train((NeuralNetwork net, double fitness)[] population, Random random, double mutationRate, double[][] Inputs, double[][] Output)
        {
            Array.Sort(population, (a, b) => (int)a.net.GetError(Inputs,Output) );

            int start = (int)(population.Length * 0.1);
            int end = (int)(population.Length * 0.9);

            //Notice that this process is only called on networks in the middle 80% of the array
            for (int i = start; i < end; i++)
            {
                Crossover(population[random.Next(start)].net, population[i].net, random);
                Mutate(population[i].net, random, mutationRate);
            }

            //Removes the worst performing networks
            for (int i = end; i < population.Length; i++)
            {
                population[i].net.Randomize(random,0,10);
            }
        }
        public class Dendrite
        {
            public Neuron Previous { get; set; }
            public Neuron Next { get; set; }
            public double Weight;

            public Dendrite(Neuron previous, Neuron next, double weight)
            {
                Previous = previous;
                Next = next;
                Weight = weight;   
            }
            public double Compute()
            {
                return Previous.Output * Weight;
            }
        }
        static void Main(string[] args)
        {
            int[] ints = new int[3];
            ints[0] = 2;
            ints[1] = 2;
            ints[2] = 1;
            double[][] inputs = new double[4][];
            inputs[0] = new double[] { 1, 1 };
            inputs[1] = new double[] { 1, 0 };
            inputs[2] = new double[] { 0, 1 };
            inputs[3] = new double[] { 0, 0 };
            double[] outputs = new double[] {0,1,1,0};
            
            (NeuralNetwork net, double fitness)[] population = new (NeuralNetwork net, double fitness)[50];
            for (int i = 0; i < population.Length; i++)
            {
                population[i].net = new NeuralNetwork(new ActivationFunction(null, null), new ErrorFunction(null, null), ints);
                population[i].net.Randomize(new Random(), 0,10);
            }
            while (true)
            {
                Train(population,new Random(),0.1,inputs,output);
            }
            Console.WriteLine("Hello, World!");
        }
    }
}