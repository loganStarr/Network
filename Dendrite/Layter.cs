using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Dendrite.Program;

namespace Dendrite
{
     public class Layer
    {
        public Neuron[] Neurons { get; }

        public Layer(ActivationFunction activation, int neuronCount, Layer previousLayer)
        {


            Neurons = new Neuron[neuronCount];
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
            double[] doubles = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; i++)
            {
                doubles[i] = Neurons[i].Compute();
            }
            return doubles;
        }
    }
}

