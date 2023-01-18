using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;
namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        public override void Run()
        {
            DiscreteFourierTransform DFT1 = new DiscreteFourierTransform();
            DiscreteFourierTransform DFT2 = new DiscreteFourierTransform();
            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();

            int Size = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            List<float> Amps = new List<float>();
            List<float> PhaseShifts = new List<float>();

            for (int i = InputSignal1.Samples.Count; i < Size; i++) {
                InputSignal1.Samples.Add(0);
            }

            for (int i = InputSignal2.Samples.Count; i < Size; i++)
            {
                InputSignal2.Samples.Add(0);
            }

            DFT1.InputTimeDomainSignal = InputSignal1;
            DFT2.InputTimeDomainSignal = InputSignal2;
            DFT1.Run();
            DFT2.Run();
            InputSignal1 = DFT1.OutputFreqDomainSignal;
            InputSignal2 = DFT2.OutputFreqDomainSignal;

            List<Complex> Comp1 = new List<Complex>();
            List<Complex> Comp2 = new List<Complex>();
            List<Complex> MultiComp = new List<Complex>();

            for (int i = 0; i < Size; i++) {
                Complex Number = new Complex(DFT1.Real[i], DFT1.Img[i]);
                Comp1.Add(Number);
            }

            for (int i = 0; i < Size; i++)
            {
                Complex Number = new Complex(DFT2.Real[i], DFT2.Img[i]);
                Comp2.Add(Number);
            }

            for (int i = 0; i < Size; i++)
            {
                MultiComp.Add(Complex.Multiply(Comp1[i], Comp2[i]));
            }

            for (int i = 0; i < Size; i++) {
                Amps.Add((float)(Math.Sqrt(Math.Pow(MultiComp[i].Real, 2) + Math.Pow(MultiComp[i].Imaginary, 2))));
                PhaseShifts.Add((float)Math.Atan2(MultiComp[i].Imaginary, MultiComp[i].Real));
            }

            Signal Result = new Signal(false, new List<float>(), Amps, PhaseShifts);
            IDFT.InputFreqDomainSignal = Result;
            IDFT.Run();
            OutputConvolvedSignal = IDFT.OutputTimeDomainSignal;
        }
    }
}
