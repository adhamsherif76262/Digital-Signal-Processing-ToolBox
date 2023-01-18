using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {

            List<Complex> Complex_List_1 = new List<Complex>();
            List<Complex> Complex_List_2 = new List<Complex>();
            List<float> Amps = new List<float>();
            List<float> PhaseShifts = new List<float>();
            List<float> Normalized_List = new List<float>();
            List<float> NonNormalized_List = new List<float>();

            float Signal_1_Sum = 0;
            float Signal_2_Sum = 0;
            float Norm_Result;

            DiscreteFourierTransform DFT1 = new DiscreteFourierTransform
            {
                InputTimeDomainSignal = InputSignal1
            };
            DFT1.Run();

            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                Complex Number = new Complex(DFT1.Real[i], DFT1.Img[i]);
                Complex_List_1.Add(Number);
            }

            if (InputSignal2 != null)
            {
                DiscreteFourierTransform DFT2 = new DiscreteFourierTransform
                {
                    InputTimeDomainSignal = InputSignal2
                };
                DFT2.Run();

                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                {
                    Complex Number = new Complex(DFT2.Real[i], DFT2.Img[i]);
                    Complex_List_2.Add(Number);
                }
            }

            List<Complex> ListComplex = new List<Complex>();

            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                Signal_1_Sum += (float)Math.Pow(InputSignal1.Samples[i], 2);
            }

            if (InputSignal2 == null)
            {
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    ListComplex.Add(Complex_List_1[i] * Complex.Conjugate(Complex_List_1[i]));
                }
                Norm_Result = (float)Math.Sqrt(Math.Pow(Signal_1_Sum, 2)) / InputSignal1.Samples.Count();
            }
            else
            {
                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                {
                    ListComplex.Add(Complex_List_2[i] * Complex.Conjugate(Complex_List_1[i]));
                    Signal_2_Sum += (float)Math.Pow(InputSignal2.Samples[i], 2);
                }
                Norm_Result = (float)Math.Sqrt(Signal_1_Sum * Signal_2_Sum) / InputSignal1.Samples.Count;
            }

            for (int i = 0; i < ListComplex.Count; i++)
            {
                Amps.Add((float)(Math.Sqrt(Math.Pow(ListComplex[i].Real, 2) + Math.Pow(ListComplex[i].Imaginary, 2))));
                PhaseShifts.Add((float)Math.Atan2(ListComplex[i].Imaginary, ListComplex[i].Real));
            }

            Signal Result = new Signal(false, new List<float>(), Amps, PhaseShifts);

            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform
            {
                InputFreqDomainSignal = Result
            };

            IDFT.Run();



            for (int i = 0; i < IDFT.OutputTimeDomainSignal.Samples.Count; i++)
            {
                Normalized_List.Add(IDFT.OutputTimeDomainSignal.Samples[i] / Norm_Result / IDFT.OutputTimeDomainSignal.Samples.Count);
                NonNormalized_List.Add(IDFT.OutputTimeDomainSignal.Samples[i] / IDFT.OutputTimeDomainSignal.Samples.Count);
            }

            OutputNonNormalizedCorrelation = NonNormalized_List;
            OutputNormalizedCorrelation = Normalized_List;
        }
    }
}