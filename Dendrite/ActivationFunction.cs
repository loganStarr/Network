//namespace Dendrite
//{

//    internal partial class Program
//    {
//        public class ActivationFunction
//        {
//            Func<double, double> function;
//            Func<double, double> derivative;
//            public ActivationFunction(Func<double, double> function, Func<double, double> derivative)
//            {
//                //this.function = function;
//                //this.derivative = derivative;
//            }

//            public double Function(double input)
//            {
//                if (input < 0)
//                {
//                    return 0;
//                }
//                return 1;
//            }

//            public double Derivative(double input)
//            {
//                return Math.Tanh(input);
//            }
//        }
//    }
//}