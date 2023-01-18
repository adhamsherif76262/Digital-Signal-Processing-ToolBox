using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }
        public override void Run()
        {
            List<float> Samples = new List<float>();
            List<float> Frequencies = new List<float>();

            OutputTimeDomainSignal = new Signal(Samples, false, Frequencies, InputFreqDomainSignal.FrequenciesAmplitudes, InputFreqDomainSignal.FrequenciesPhaseShifts);

            for (int k = 0; k < InputFreqDomainSignal.FrequenciesAmplitudes.Count; k++)
            {
                float Real_Part = 0;
                float Imaginary_Part = 0;
                float A_Cos_Theta;
                float A_Sin_Theta;

                for (int n = 0; n < InputFreqDomainSignal.FrequenciesAmplitudes.Count; n++)
                {
                    A_Cos_Theta = InputFreqDomainSignal.FrequenciesAmplitudes[n] * (float)Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[n]);
                    A_Sin_Theta = InputFreqDomainSignal.FrequenciesAmplitudes[n] * (float)Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[n]);
                    Real_Part += A_Cos_Theta * ((float)Math.Cos((k * 2 * Math.PI * n / InputFreqDomainSignal.FrequenciesAmplitudes.Count)));
                    Imaginary_Part += -1 * A_Sin_Theta * ((float)Math.Sin((k * 2 * Math.PI * n / InputFreqDomainSignal.FrequenciesAmplitudes.Count)));
                }

                OutputTimeDomainSignal.Samples.Add((Real_Part + Imaginary_Part) / InputFreqDomainSignal.FrequenciesAmplitudes.Count);
            }
        }
    }
}
