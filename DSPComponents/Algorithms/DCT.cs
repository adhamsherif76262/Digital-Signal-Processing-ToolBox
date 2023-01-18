using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            double Constant = Math.Sqrt(2f / InputSignal.Samples.Count);
            double Sum;

            List<float> Samples = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                Sum = 0;

                for (int j = 0; j < InputSignal.Samples.Count; j++)
                {
                    Sum += InputSignal.Samples[j] * Math.Cos(((2 * j) - 1) * ((2 * i) - 1) * Math.PI / (4 * InputSignal.Samples.Count));
                }

                Samples.Add((float)(Sum * Constant));
            }
            OutputSignal = new Signal(Samples, false);
        }
    }
}