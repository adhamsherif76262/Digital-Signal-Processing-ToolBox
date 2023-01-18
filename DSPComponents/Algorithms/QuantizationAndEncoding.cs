using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }
        public override void Run()
        {
            OutputIntervalIndices = new List<int>();
            OutputSamplesError = new List<float>();
            OutputEncodedSignal = new List<string>();
            List<float> Begining = new List<float>();
            List<float> End = new List<float>();
            List<float> Mid_Point = new List<float>();
            List<float> Quantized_Signal = new List<float>();
            float Minimum = InputSignal.Samples.Min();
            float V = Minimum;
            int Counter = 0;
            float Maximum = InputSignal.Samples.Max();
            float Delta = (Maximum - Minimum) / InputLevel;

            if (InputLevel == 0)
            {
                double Signal_Bits = Convert.ToDouble(InputNumBits);
                InputLevel = Convert.ToInt32(Math.Pow(2, Signal_Bits));
            }

            if (InputNumBits == 0)
            {
                double Signal_Bits = Math.Log(Convert.ToDouble(InputLevel), 2);
                InputNumBits = Convert.ToInt32(Signal_Bits);

            }
            while (V < Maximum)
            {
                Begining.Add(V);
                V += Delta;
                End.Add(V);
                Mid_Point.Add((Begining[Counter] + End[Counter]) / 2);
                Counter++;
            }
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < InputLevel; j++)
                {
                    if (InputSignal.Samples[i] >= Begining[j] && InputSignal.Samples[i] < End[j] + 0.0001)
                    {
                        OutputIntervalIndices.Add(j + 1);
                        Quantized_Signal.Add((float)Math.Round((Decimal)Mid_Point[j], 3, MidpointRounding.AwayFromZero));
                        OutputSamplesError.Add(Mid_Point[j] - InputSignal.Samples[i]);
                    }
                }
            }
            for (int i = 0; i < OutputIntervalIndices.Count; i++)
            {
                string Output_Signal = Convert.ToString(OutputIntervalIndices[i] - 1, 2);
                while (Output_Signal.Length < InputNumBits)
                {
                    Output_Signal = Output_Signal.Insert(0, "0");
                }
                OutputEncodedSignal.Add(Output_Signal);
            }
            OutputQuantizedSignal = new Signal(Quantized_Signal, false);
        }
    }
}
