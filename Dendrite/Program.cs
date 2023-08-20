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
        class NeuralNetwork
        {
            Layer[] layers;
            ErrorFunction errorFunc;

            public NeuralNetwork(ActivationFunction activation, ErrorFunction errorFunc,
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
            public double GetError(double[] inputs, double[] desiredOutputs)
            {
                double[] doubles = Compute(inputs);
                double error = 0;
                for (int i = 0; i < doubles.Length; i++)
                {
                    error = desiredOutputs[i] - doubles[i]; 
                }
                return error;
            }
        }
        class Layer
        {
            public Neuron[] Neurons { get; }
            public double[] Outputs;

            public Layer(ActivationFunction activation, int neuronCount, Layer previousLayer) 
            {


                Neurons= new Neuron[neuronCount];
                Outputs= new double[neuronCount];
                for (int i = 0; i < neuronCount; i++)
                {
                    Neurons[i] = new Neuron(activation,previousLayer.Neurons);
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

        class Neuron
        {
            double bias;
            Dendrite[] dendrites;
            public double Output;
            public double Input;
            public ActivationFunction Activation { get; set; }

           

            public Neuron(ActivationFunction activation, Neuron[] previousNerons)
            { 
                Activation = activation;
                dendrites = new Dendrite[previousNerons.Length];
                for (int i = 0; i < dendrites.Length; i++)
                {
                    dendrites[i] = new Dendrite(previousNerons[i],this, 0);
                }

            }
            public void Randomize(Random random, double min, double max)
            {
                for (int i = 0; i < dendrites.Length; i++)
                {
                    dendrites[i].Weight = random.NextDouble()*max-min;
                }
                bias = random.NextDouble() * max - min;
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

        class Dendrite
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
            NeuralNetwork network = new NeuralNetwork(new ActivationFunction());
            Console.WriteLine("Hello, World!");
        }
    }
}