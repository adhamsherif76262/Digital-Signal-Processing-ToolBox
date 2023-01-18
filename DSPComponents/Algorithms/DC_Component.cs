using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float Sum = 0;
            List<float> Samples = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                Sum += InputSignal.Samples[i];
            }

            float Mean = Sum / InputSignal.Samples.Count;

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                Samples.Add(InputSignal.Samples[i] - Mean);
            }

            OutputSignal = new Signal(Samples, false);
        }
    }
}
