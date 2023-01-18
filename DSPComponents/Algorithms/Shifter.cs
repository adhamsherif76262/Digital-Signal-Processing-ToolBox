using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            List<int> Indecies = new List<int>();

            if (InputSignal.Periodic)
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    Indecies.Add(InputSignal.SamplesIndices[i] + ShiftingValue);
                }
            }
            else 
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    Indecies.Add(InputSignal.SamplesIndices[i] + (ShiftingValue * -1));
                }
            }

            OutputShiftedSignal = new Signal(InputSignal.Samples, Indecies, InputSignal.Periodic);
        }
    }
}
