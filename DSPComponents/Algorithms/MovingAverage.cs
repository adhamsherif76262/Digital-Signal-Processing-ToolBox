using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            List<float> Result = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count - InputWindowSize + 1; i++)
            {
                float Sum = 0;

                for (int j = 0; j < InputWindowSize; j++)
                {
                    Sum += InputSignal.Samples[j + i];
                }

                Result.Add(Sum / InputWindowSize);
            }

            OutputAverageSignal = new Signal(Result, new List<int>(), false);
        }
    }
}
