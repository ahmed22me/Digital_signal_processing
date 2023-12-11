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
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            float maxAmp, minAmp, delta;
            List<float> quantizedSamples = new List<float>();
            List<Tuple<float, float>> interveals = new List<Tuple<float, float>>();



            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();

            if (InputLevel <= 0)
                InputLevel = (int)Math.Pow(2, InputNumBits);
            else if (InputNumBits <= 0)
                InputNumBits = (int)Math.Log(InputLevel, 2);


            maxAmp = InputSignal.Samples.Max();
            minAmp = InputSignal.Samples.Min();
            delta = (maxAmp - minAmp) / InputLevel;

            for (float i = minAmp; i < maxAmp; i += delta)
            {
                interveals.Add(new Tuple<float, float>(i, i + delta));
            }


            foreach (float sample in InputSignal.Samples)
            {
                for (int i = 0; i < interveals.Count; i++)
                {
                    if (sample >= interveals[i].Item1 && sample <= interveals[i].Item2 + 0.001)
                    {
                        float midpoint = (interveals[i].Item1 + interveals[i].Item2) / (float)2;
                        quantizedSamples.Add(midpoint);
                        OutputIntervalIndices.Add(i + 1);
                        OutputSamplesError.Add(midpoint - sample);
                        int len = Convert.ToString(i, 2).Length;
                        string zeros = "";
                        while (len < (int)Math.Log(InputLevel, 2))
                        {
                            zeros += "0";
                            len++;
                        }
                        OutputEncodedSignal.Add(zeros + Convert.ToString(i, 2));
                        break;
                    }
                }
            }
            OutputQuantizedSignal = new Signal(quantizedSamples, InputSignal.Periodic);
            Console.WriteLine(String.Join(", ", OutputEncodedSignal));
        }
    }
}
