using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dendrite
{
    public class ActivationFunction
    {
        Func<double, double> function;
        Func<double, double> derivative;
        public ActivationFunction(Func<double, double> function, Func<double, double> derivative)
        {
            //this.function = function;
            //this.derivative = derivative;
        }

        public double Function(double input)
        {
            if (input <= 0) return 0;
            return 1;
        }

        public double Derivative(double input)
        {
            return input;
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
}
