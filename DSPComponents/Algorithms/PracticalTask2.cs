﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);
            FIR Fir = new FIR
            {
                InputTimeDomainSignal = InputSignal,
                InputFilterType = FILTER_TYPES.BAND_PASS,
                InputStopBandAttenuation = 50,
                InputTransitionBand = 500,
                InputF1 = miniF,
                InputF2 = maxF,
                InputFS = Fs
            };
            Fir.Run();
            /*
            StreamWriter FIRWriter = new StreamWriter("fir.ds");
            FIRWriter.WriteLine("0");
            FIRWriter.WriteLine("0");
            FIRWriter.WriteLine(Fir.OutputYn.Samples.Count().ToString());
            for (int i = 0; i < Fir.OutputYn.Samples.Count(); i++)
            {
                FIRWriter.WriteLine(Fir.OutputYn.SamplesIndices[i].ToString() + " " + Fir.OutputYn.Samples[i].ToString());
            }
            */
            writer(Fir.OutputYn.Samples, Fir.OutputYn.SamplesIndices, "fir.ds");
            Thread.Sleep(1000);
            if (newFs < maxF * 2)
            {
                Console.WriteLine("NewFs is not valid");
            }
            else
            {
                Sampling Samp = new Sampling
                {
                    InputSignal = Fir.OutputYn,
                    L = L,
                    M = M
                };
                Samp.Run();
                /*StreamWriter SampWriter = new StreamWriter("Sampling.ds");
                SampWriter.WriteLine("0");
                SampWriter.WriteLine("0");
                SampWriter.WriteLine(Samp.OutputSignal.Samples.Count().ToString());
                for (int i = 0; i < Samp.OutputSignal.Samples.Count(); i++)
                {
                    SampWriter.WriteLine(Samp.OutputSignal.SamplesIndices[i].ToString() + " " + Samp.OutputSignal.Samples[i].ToString());
                }*/
                writer(Samp.OutputSignal.Samples, Samp.OutputSignal.SamplesIndices, "sampling.ds");
                Thread.Sleep(1000);

                DC_Component DC_Comp = new DC_Component
                {
                    InputSignal = Samp.OutputSignal
                };
                DC_Comp.Run();
                /*StreamWriter DCWriter = new StreamWriter("DCcomponent.ds");
                
                DCWriter.WriteLine("0");
                DCWriter.WriteLine("0");
                DCWriter.WriteLine(DC_Comp.OutputSignal.Samples.Count().ToString());
                for (int i = 0; i < DC_Comp.OutputSignal.Samples.Count(); i++)
                {
                    DCWriter.WriteLine(DC_Comp.OutputSignal.SamplesIndices[i].ToString() + " " + DC_Comp.OutputSignal.Samples[i].ToString());
                }*/
                writer(DC_Comp.OutputSignal.Samples, DC_Comp.OutputSignal.SamplesIndices, "dc.ds");
                Thread.Sleep(1000);
                Normalizer Norm = new Normalizer
                {
                    InputSignal = DC_Comp.OutputSignal,
                    InputMinRange = -1,
                    InputMaxRange = 1
                };
                Norm.Run();
                /*StreamWriter NormWriter = new StreamWriter("Norm.ds");
                NormWriter.WriteLine("0");
                NormWriter.WriteLine("0");
                NormWriter.WriteLine(Norm.OutputNormalizedSignal.Samples.Count().ToString());
                for (int i = 0; i < Norm.OutputNormalizedSignal.Samples.Count(); i++)
                {
                    NormWriter.WriteLine(Norm.OutputNormalizedSignal.SamplesIndices[i].ToString() + " " + Norm.OutputNormalizedSignal.Samples[i].ToString());
                }
                */
                writer(Norm.OutputNormalizedSignal.Samples, Norm.OutputNormalizedSignal.SamplesIndices, "norm.ds");
                Thread.Sleep(1000);
                DiscreteFourierTransform DFT = new DiscreteFourierTransform
                {
                    InputTimeDomainSignal = Norm.OutputNormalizedSignal,
                    InputSamplingFrequency = newFs
                };
                DFT.Run();
                
                for (int i = 0; i < DFT.OutputFreqDomainSignal.Frequencies.Count; i++)
                {
                    DFT.OutputFreqDomainSignal.Frequencies[i] = (float)Math.Round((double)DFT.OutputFreqDomainSignal.Frequencies[i], 1);
                }
                OutputFreqDomainSignal = DFT.OutputFreqDomainSignal;
                StreamWriter DFTWriter = new StreamWriter("DFT.ds");
                DFTWriter.WriteLine("1");
                DFTWriter.WriteLine("0");
                DFTWriter.WriteLine(DFT.OutputFreqDomainSignal.Frequencies.Count().ToString());
                for (int i = 0; i < DFT.OutputFreqDomainSignal.Frequencies.Count(); i++)
                {
                    DFTWriter.WriteLine(DFT.OutputFreqDomainSignal.Frequencies[i].ToString() + " " + DFT.OutputFreqDomainSignal.FrequenciesAmplitudes[i].ToString() + " " + DFT.OutputFreqDomainSignal.FrequenciesPhaseShifts[i].ToString());
                }
            }
        }

        void writer(List<float> Samples, List<int> Indecies, string name)
        {
            StreamWriter NormWriter = new StreamWriter(name);
            NormWriter.WriteLine("0");
            NormWriter.WriteLine("0");
            NormWriter.WriteLine(Samples.Count);
            for (int i = 0; i < Samples.Count; i++)
            {
                NormWriter.WriteLine(Indecies[i] + " " + Samples[i]);
            }
        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
