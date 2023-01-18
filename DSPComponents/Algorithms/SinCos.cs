using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos: Algorithm
    {
        public string Type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> Samples { get; set; }
        public override void Run()
        {
            Samples = new List<float>();
            if (SamplingFrequency >= (2 * AnalogFrequency)) 
            {
                for (int i = 0; i < SamplingFrequency; i++)
                {
                    if (Type == "sin")
                    {
                        Samples.Add((float)(A * Math.Sin(((2 * Math.PI * (AnalogFrequency / SamplingFrequency) * i) + PhaseShift))));
                    }
                    else
                    {
                        Samples.Add((float)(A * Math.Cos(((2 * Math.PI * (AnalogFrequency / SamplingFrequency) * i) + PhaseShift))));
                    }
                }
            }
        }
    }
}
