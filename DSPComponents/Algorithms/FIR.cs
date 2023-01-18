using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }


        public override void Run()
        {
            OutputHn = new Signal(new List<float>(), false, new List<float>(), new List<float>(), new List<float>());
            OutputYn = new Signal(new List<float>(), false, new List<float>(), new List<float>(), new List<float>());

            int transition_width = Trans_width();
            int half_samples = transition_width / 2;

            for (int i = 0; i < transition_width; i++)
            {
                OutputHn.Samples.Add(0);
                OutputHn.SamplesIndices.Add(0);
            }

            double hD;

            if (InputCutOffFrequency != null)
            {
                if (InputFilterType == FILTER_TYPES.LOW)
                {
                    double f_dash = ((double)InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS;

                    for (int i = 0; i <= half_samples; i++)
                    {
                        if (i == 0)
                        {
                            hD = 2 * f_dash;
                        }
                        else
                        {
                            hD = 2 * f_dash * Math.Sin(i * 2 * Math.PI * f_dash) / (i * 2 * Math.PI * f_dash);
                        }

                        Window(i, transition_width, hD, half_samples);
                    }
                }
                else
                {
                    double f_dash = ((double)InputCutOffFrequency - (InputTransitionBand / 2)) / InputFS;

                    for (int i = 0; i <= half_samples; i++)
                    {
                        if (i == 0)
                        {
                            hD = 1 - (2 * f_dash);
                        }
                        else
                        {
                            hD = -2 * f_dash * Math.Sin(i * 2 * Math.PI * f_dash) / (i * 2 * Math.PI * f_dash);
                        }

                        Window(i, transition_width, hD, half_samples);
                    }
                }
            }
            else 
            {
                if (InputFilterType == FILTER_TYPES.BAND_PASS)
                {
                    double F1Dash = ((double)InputF1 - (InputTransitionBand / 2)) / InputFS;
                    double F2Dash = ((double)InputF2 + (InputTransitionBand / 2)) / InputFS;

                    for (int i = 0; i <= half_samples; i++)
                    {
                        if (i == 0)
                        {
                            hD = 2 * (F2Dash - F1Dash);
                        }
                        else
                        {
                            hD = 2 * F2Dash * Math.Sin(i * 2 * Math.PI * F2Dash) / (i * 2 * Math.PI * F2Dash) - (2 * F1Dash * Math.Sin(i * 2 * Math.PI * F1Dash)) / (i * 2 * Math.PI * F1Dash);
                        }

                        Window(i, transition_width, hD, half_samples);
                    }
                }
                else 
                {
                    double F1Dash = ((double)InputF1 + (InputTransitionBand / 2)) / InputFS;
                    double F2Dash = ((double)InputF2 - (InputTransitionBand / 2)) / InputFS;

                    for (int i = 0; i <= half_samples; i++)
                    {
                        if (i == 0)
                        {
                            hD = 1 - 2 * (F2Dash - F1Dash);
                        }
                        else
                        {
                            hD = 2 * F1Dash * Math.Sin(i * 2 * Math.PI * F1Dash) / (i * 2 * Math.PI * F1Dash) - (2 * F2Dash * Math.Sin(i * 2 * Math.PI * F2Dash)) / (i * 2 * Math.PI * F2Dash);
                        }

                        Window(i, transition_width, hD, half_samples);
                    }
                }
            }

            DirectConvolution convolution = new DirectConvolution
            {
                InputSignal1 = InputTimeDomainSignal,
                InputSignal2 = OutputHn
            };
            convolution.Run();
            OutputYn = convolution.OutputConvolvedSignal;
        }

        void Window(int i, int transition_width, double hD, int half_samples)
        {
            double window = 0;

            if (InputStopBandAttenuation <= 21)
            {
                window =  1;
            }
            else if (InputStopBandAttenuation <= 44)
            {
                window = 0.5 + 0.5 * Math.Cos(2 * Math.PI * i / transition_width);
            }
            else if (InputStopBandAttenuation <= 53)
            {
                window = 0.54 + 0.46 * Math.Cos(2 * Math.PI * i / transition_width);
            }
            else if (InputStopBandAttenuation <= 74)
            {
                window = 0.42 + 0.5 * Math.Cos(2 * Math.PI * i / (transition_width - 1)) + 0.08 * Math.Cos(4 * Math.PI * i / (transition_width - 1));
            } 

            OutputHn.Samples[half_samples - i] = (float)(hD * window);
            OutputHn.Samples[half_samples + i] = (float)(hD * window);
            OutputHn.SamplesIndices[half_samples - i] = -i;
            OutputHn.SamplesIndices[half_samples + i] = i;
        }

        int Trans_width()
        {
            int transition_width = 0;

            if (InputStopBandAttenuation <= 21)
            {
                transition_width = (int)(0.9 / (InputTransitionBand / InputFS));
            }
            else if (InputStopBandAttenuation <= 44)
            {
                transition_width = (int)(3.1 / (InputTransitionBand / InputFS));
            }
            else if (InputStopBandAttenuation <= 53)
            {
                transition_width = (int)(3.3 / (InputTransitionBand / InputFS));
            }
            else if (InputStopBandAttenuation <= 74)
            {
                transition_width = (int)(5.5 / (InputTransitionBand / InputFS));
            }

            if (transition_width % 2 == 0)
            {
                transition_width++;
            }

            return transition_width;
        }
    }
}