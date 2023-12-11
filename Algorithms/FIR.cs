using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.IO;

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
            List<int> pos_indicies = new List<int>();
            List<int> neg_indicies = new List<int>();
            List<int> indicies = new List<int>();
            float hD_of_zero = 0;
            float Normalized_InputTransitionBand = InputTransitionBand / InputFS;
            int N = 0;
            int windowNumber = 0;
            if (InputStopBandAttenuation <= 21)
            {
                N = (int)(0.9 / Normalized_InputTransitionBand);
                windowNumber = 1;
            }
            else if (InputStopBandAttenuation <= 44 && InputStopBandAttenuation > 21)
            {
                N = (int)(3.1 / Normalized_InputTransitionBand);
                windowNumber = 2;
            }
            else if (InputStopBandAttenuation <= 53 && InputStopBandAttenuation > 44)
            {
                N = (int)(3.3 / Normalized_InputTransitionBand);
                windowNumber = 3;
            }
            else if (InputStopBandAttenuation > 53)
            {
                N = (int)(5.5 / Normalized_InputTransitionBand);
                windowNumber = 4;
            }
            if (N % 2 == 0)
                N += 1;
            else
                N += 2;
            List<float> Wn = new List<float>();
            switch (windowNumber)
            {
                case 1:
                    {
                        for (int i = (N - 1) / -2; i <= 0; i++)
                        {
                            Wn.Add(1);
                        }
                        break;
                    }
                case 2:
                    {
                        for (int i = (N - 1) / -2; i <= 0; i++)
                        {
                            double numerator = 2 * Math.PI * i;
                            double res = 0.5 + (0.5 * Math.Cos((numerator / N)));
                            Wn.Add((float)res);
                        }
                        break;
                    }
                case 3:
                    {
                        for (int i = (N - 1) / -2; i <= 0; i++)
                        {
                            double numerator = 2 * Math.PI * i;
                            double res = 0.54 + (0.46 * Math.Cos((numerator / N)));
                            Wn.Add((float)res);
                        }
                        break;
                    }
                case 4:
                    {
                        for (int i = (N - 1) / -2; i <= 0; i++)
                        {
                            double numerator1 = 2 * Math.PI * i;
                            double numerator2 = 4 * Math.PI * i;
                            double denominator = N - 1;
                            double res = 0.42 + (0.5 * Math.Cos((numerator1 / denominator))) + (0.08 * Math.Cos((numerator2 / denominator)));
                            Wn.Add((float)res);
                        }
                        break;
                    }
            }
            if (InputFilterType == FILTER_TYPES.LOW || InputFilterType == FILTER_TYPES.HIGH)
            {
                float Normalized_InputCutOffFrequency = InputCutOffFrequency.Value / InputFS;
                float newCutoffFrequency = 0;
                List<float> hn = new List<float>();
                if (InputFilterType == FILTER_TYPES.LOW)
                {
                    newCutoffFrequency = Normalized_InputCutOffFrequency + (Normalized_InputTransitionBand / 2);
                    hD_of_zero = 2 * newCutoffFrequency;
                    for (int i = (N - 1) / 2; i >= 1; i--)
                    {
                        double Wc = 2 * Math.PI * newCutoffFrequency;
                        double res = 2 * newCutoffFrequency * (Math.Sin(i * Wc) / (i * Wc));
                        hn.Add((float)res);
                        neg_indicies.Add(-i);
                        pos_indicies.Add(i);
                    }
                }
                else if (InputFilterType == FILTER_TYPES.HIGH)
                {
                    newCutoffFrequency = Normalized_InputCutOffFrequency - (Normalized_InputTransitionBand / 2);
                    hD_of_zero = 1 - (2 * newCutoffFrequency);
                    for (int i = (N - 1) / 2; i >= 1; i--)
                    {
                        double Wc = 2 * Math.PI * newCutoffFrequency;
                        double numerator = 2 * newCutoffFrequency * Math.Sin(i * Wc);
                        double denominator = i * Wc;
                        double res = -(numerator / denominator);
                        hn.Add((float)res);
                        neg_indicies.Add(-i);
                        pos_indicies.Add(i);
                    }
                }

                indicies.AddRange(neg_indicies); //-
                indicies.Add(0);                 //0
                pos_indicies.Reverse();          //+
                indicies.AddRange(pos_indicies); //- 0 +
                hn.Add(hD_of_zero);
                List<float> x = new List<float>();
                List<float> y = new List<float>();
                for (int i = 0; i <= (N - 1) / 2; i++)
                {
                    x.Add(hn[i] * Wn[i]);
                }
                y.AddRange(x);
                x.RemoveAt((N - 1) / 2);
                x.Reverse();
                y.AddRange(x);
                OutputHn = new Signal(y, indicies, false);
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS || InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                float Normailzed_InputF1 = InputF1.Value / InputFS;
                float Normailzed_InputF2 = InputF2.Value / InputFS;
                float new_InputF1 = 0;
                float new_InputF2 = 0;
                if (InputFilterType == FILTER_TYPES.BAND_PASS)
                {
                    new_InputF1 = Normailzed_InputF1 - (Normalized_InputTransitionBand / 2);
                    new_InputF2 = Normailzed_InputF2 + (Normalized_InputTransitionBand / 2);
                    hD_of_zero = 2 * (new_InputF2 - new_InputF1);
                }
                else if (InputFilterType == FILTER_TYPES.BAND_STOP)
                {
                    new_InputF1 = Normailzed_InputF1 + (Normalized_InputTransitionBand / 2);
                    new_InputF2 = Normailzed_InputF2 - (Normalized_InputTransitionBand / 2);
                    hD_of_zero = 1 - (2 * (new_InputF2 - new_InputF1));
                }
                List<float> hn = new List<float>();
                double Wc1 = 2 * Math.PI * new_InputF1;
                double Wc2 = 2 * Math.PI * new_InputF2;
                if (InputFilterType == FILTER_TYPES.BAND_PASS)
                {
                    for (int i = (N - 1) / 2; i >= 1; i--)
                    {
                        double part1 = 2 * new_InputF2 * (Math.Sin((i * Wc2)) / (i * Wc2));
                        double part2 = 2 * new_InputF1 * (Math.Sin((i * Wc1)) / (i * Wc1));
                        double res = part1 - part2;
                        hn.Add((float)res);
                        neg_indicies.Add(-i);
                        pos_indicies.Add(i);
                    }
                }
                else if (InputFilterType == FILTER_TYPES.BAND_STOP)
                {
                    for (int i = (N - 1) / 2; i >= 1; i--)
                    {
                        double part1 = 2 * new_InputF1 * (Math.Sin((i * Wc1)) / (i * Wc1));
                        double part2 = 2 * new_InputF2 * (Math.Sin((i * Wc2)) / (i * Wc2));
                        double res = part1 - part2;
                        hn.Add((float)res);
                        neg_indicies.Add(-i);
                        pos_indicies.Add(i);
                    }
                }

                indicies.AddRange(neg_indicies);
                indicies.Add(0);
                pos_indicies.Reverse();
                indicies.AddRange(pos_indicies);

                hn.Add(hD_of_zero);
                List<float> x = new List<float>();
                List<float> y = new List<float>();
                for (int i = 0; i <= (N - 1) / 2; i++)
                {
                    x.Add(hn[i] * Wn[i]);
                }
                y.AddRange(x);
                x.RemoveAt((N - 1) / 2);
                x.Reverse();
                y.AddRange(x);
                OutputHn = new Signal(y, indicies, false);
            }
            //ConVolution Obj
            DirectConvolution dc = new DirectConvolution();
            dc.InputSignal1 = InputTimeDomainSignal;
            dc.InputSignal2 = OutputHn;
            dc.Run();
            OutputYn = dc.OutputConvolvedSignal;

            //Text File
            StreamWriter writeData = new StreamWriter("FIR Results.txt");
            writeData.WriteLine("H(n) Cofficients");
            writeData.WriteLine(OutputHn.Samples.Count);
            for (int i = 0; i < OutputHn.Samples.Count; i++)
            {
                writeData.WriteLine(i + " " + OutputHn.Samples[i]);
            }
            writeData.Close();
        }
    }
}
