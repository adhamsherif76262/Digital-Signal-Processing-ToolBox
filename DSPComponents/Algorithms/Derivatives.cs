using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> FirstDer = new List<float>();
            List<float> SecondDer = new List<float>();

            FirstDer.Add(InputSignal.Samples[0]);
            SecondDer.Add(InputSignal.Samples[1] - 2 * InputSignal.Samples[0]);

            for (int i = 1; i < InputSignal.Samples.Count - 1; i++)
            {
                FirstDer.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);
                SecondDer.Add(InputSignal.Samples[i + 1] - 2 * InputSignal.Samples[i] + InputSignal.Samples[i - 1]);
            }

            FirstDerivative = new Signal(FirstDer, false);
            SecondDerivative = new Signal(SecondDer, false);
        }
    }
}
