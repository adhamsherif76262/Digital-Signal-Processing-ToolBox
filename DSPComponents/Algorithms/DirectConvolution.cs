using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        public override void Run()
        {
            List<float> convolution_List = new List<float>();
            List<int> indexing_List = new List<int>();
            int Limit = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;

            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                for (int j = 0; j < InputSignal2.Samples.Count; j++)
                {
                    int Index = InputSignal1.SamplesIndices[i] + InputSignal2.SamplesIndices[j];
                    if (!indexing_List.Contains(Index))
                    {
                        indexing_List.Add(Index);
                    }
                }
            }

            for (int i = 0; i < Limit; i++)
            {
                float Signal_Samples_1, Signal_Samples_2, Convolved_Signal = 0;

                for (int j = 0; j <= i; j++)
                {
                    // X(K)
                    if (j >= InputSignal1.Samples.Count)
                    {
                        Signal_Samples_1 = 0;
                    }
                    else
                    {
                        Signal_Samples_1 = InputSignal1.Samples[j];
                    }
                    //X(n-K)
                    if (i - j >= InputSignal2.Samples.Count)
                    {
                        Signal_Samples_2 = 0;
                    }
                    else
                    {
                        Signal_Samples_2 = InputSignal2.Samples[i - j];
                    }

                    Convolved_Signal += Signal_Samples_1 * Signal_Samples_2;
                }
                convolution_List.Add(Convolved_Signal);
            }

            while (convolution_List[0] == 0)
            {
                convolution_List.RemoveAt(0);
                indexing_List.RemoveAt(0);
            }

            while (convolution_List[convolution_List.Count - 1] == 0)
            {
                convolution_List.RemoveAt(convolution_List.Count - 1);
                indexing_List.RemoveAt(indexing_List.Count - 1);
            }

            OutputConvolvedSignal = new Signal(convolution_List, indexing_List, false);
        }
    }
}
