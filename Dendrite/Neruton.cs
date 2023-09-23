using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dendrite
{
    public class Neuron
    {
        public double bias;
        public Dendrite[] dendrites;
        public double Output;
        public double Sum;
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
            Sum = Total + bias;
            Output = Activation.Function(Sum);
            ;
            return Output;
        }
    }
}
