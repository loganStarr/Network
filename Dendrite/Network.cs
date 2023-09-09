//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Dendrite.Program;

//namespace Dendrite
//{
//    public class NeuralNetwork
//    {
//         Layer[] layers;
//         ErrorFunction errorFunc;
//         double[][] Inputs;
//         ActivationFunction Activation;
//         NeuralNetwork(ActivationFunction activation, ErrorFunction errorFunc, double[][] Inputs,
//        params int[] neuronsPerLayer)
//        {
//            layers = new Layer[neuronsPerLayer.Length];
//            for (int i = 0; i < neuronsPerLayer.Length; i++)
//            {
//                if (i == 0)
//                {
//                    layers[i] = new Layer(activation, neuronsPerLayer[i], null);
//                }
//                else
//                {
//                    layers[i] = new Layer(activation, neuronsPerLayer[i], layers[i - 1]);
//                }
//            }
//            ;
//            Activation = activation;
//            this.Inputs = Inputs;
//        }
//        public void Randomize(Random random, double min, double max)
//        {
//            for (int i = 0; i < layers.Length; i++)
//            {
//                layers[i].Randomize(random, min, max);
//            }
//        }
//        public double[] Compute(double[] inputs)
//        {
//            double[] doubles;

//            for (int i = 0; i < layers[0].Neurons.Length; i++)
//            {
//                layers[0].Neurons[i].Output = inputs[i];
//            }
//            for (int i = 1; i < layers.Length - 1; i++)
//            {
//                doubles = layers[i].Compute();
//            }
//            doubles = layers[layers.Length - 1].Compute();
//            for (int i = 0; i < doubles.Length; i++)
//            {
//                doubles[i] = Activation.Function(doubles[i]);
//            }
//            ;
//            return doubles;
//        }
//        public double GetError(double[][] inputs, double[][] desiredOutputs)
//        {
//            double error = 0;
//            for (int i = 0; i < inputs.Length; i++)
//            {
//                double[] doubles = Compute(inputs[i]);
//                for (int x = 0; x < doubles.Length; x++)
//                {
//                    error += Math.Abs(desiredOutputs[i][x] - doubles[x]);
//                }
//            }
//            return error;
//        }
//    }
//}
