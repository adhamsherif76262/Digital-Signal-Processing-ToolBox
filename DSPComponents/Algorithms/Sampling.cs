﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> Samples = new List<float>();
            FIR Fir = new FIR
            {
                InputFilterType = 0,
                InputFS = 8000,
                InputStopBandAttenuation = 50,
                InputCutOffFrequency = 1500,
                InputTransitionBand = 500
            };

            if (M == 0 && L != 0)
            {
                for (int i = 0; i < InputSignal.Samples.Count - 1; i++)
                {
                    Samples.Add(InputSignal.Samples[i]);

                    for (int j = 0; j < L - 1; j++)
                    {
                        Samples.Add(0);
                    }
                }

                Samples.Add(InputSignal.Samples[InputSignal.Samples.Count - 1]);

                Fir.InputTimeDomainSignal = new Signal(Samples, false);
                Fir.Run();

                OutputSignal = new Signal(Fir.OutputYn.Samples, false);
            }
            else if (M != 0 && L == 0)
            {
                Fir.InputTimeDomainSignal = new Signal(InputSignal.Samples, false);
                Fir.Run();

                int counter = 0;

                for (int i = 0; i < Fir.OutputYn.Samples.Count; i++)
                {
                    if (counter == 0)
                    {
                        Samples.Add(Fir.OutputYn.Samples[i]);
                        counter = M - 1;
                    }
                    else
                    {
                        counter--;
                    }
                }

                OutputSignal = new Signal(Samples, false);
            }
            else if (M != 0 && L != 0)
            {
                for (int i = 0; i < InputSignal.Samples.Count - 1; i++)
                {
                    Samples.Add(InputSignal.Samples[i]);

                    for (int j = 0; j < L - 1; j++)
                    {
                        Samples.Add(0);
                    }
                }

                Samples.Add(InputSignal.Samples[InputSignal.Samples.Count - 1]);

                Fir.InputTimeDomainSignal = new Signal(Samples, false);
                Fir.Run();

                int counter = 0;
                Samples.Clear();

                for (int i = 0; i < Fir.OutputYn.Samples.Count; i++)
                {
                    if (counter == 0)
                    {
                        Samples.Add(Fir.OutputYn.Samples[i]);
                        counter = M - 1;
                    }
                    else
                    {
                        counter--;
                    }
                }

                OutputSignal = new Signal(Samples, false);
            }
            else 
            {
                throw new NotImplementedException();
            }
        }
    }

}