using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float Sum;
            List<float> Samples = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                Sum = 0;

                for (int j = i; j >= 0; j--)
                {
                    Sum += InputSignal.Samples[j];
                }

                Samples.Add(Sum);
            }
            OutputSignal = new Signal(Samples, false);
        }
    }
}
