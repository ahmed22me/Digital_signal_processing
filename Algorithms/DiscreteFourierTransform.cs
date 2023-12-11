using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }
        public List<KeyValuePair<float, float>> complex { get; set; }
        public override void Run()
        {
            //Declare Our Lists are needed
            List<float> output_signal_frequenciesAmplitudes = new List<float>();
            List<float> output_signal_frequenciesPhaseShifts = new List<float>();
            List<float> output_signal_frequencies = new List<float>();
            complex = new List<KeyValuePair<float, float>>();
            OutputFreqDomainSignal = new Signal(InputTimeDomainSignal.Samples, false);
            int indexloop1 = 0;
           while ( indexloop1 < InputTimeDomainSignal.Samples.Count() )
            {
                int indexloop2 = 0;
                float the_imagine = 0;
                float real_number = 0;


              
                while ( indexloop2 < InputTimeDomainSignal.Samples.Count())
                {
                    if (indexloop1 == 0 || indexloop2 == 0) 
                    {
                        real_number += InputTimeDomainSignal.Samples[indexloop2];
                    }
                    else
                    {
                        int N = InputTimeDomainSignal.Samples.Count();
                        float temp_power = (float)((2 * Math.PI * indexloop1 * indexloop2) / N);
                        real_number += (float)(InputTimeDomainSignal.Samples[indexloop2] * Math.Cos(temp_power));
                        the_imagine += (float)(-1 * InputTimeDomainSignal.Samples[indexloop2] * Math.Sin(temp_power));
                    }
                    indexloop2++;
                }
                float omega = (float)((2 * Math.PI * InputSamplingFrequency) / InputTimeDomainSignal.Samples.Count());

                //genrate complex list
                complex.Add(new KeyValuePair<float, float>(real_number, the_imagine));
                //freq value
                output_signal_frequencies.Add(indexloop1 * omega);
                //freq amplitude
                output_signal_frequenciesAmplitudes.Add((float)Math.Sqrt(Math.Pow(complex[indexloop1].Key, 2) + Math.Pow(complex[indexloop1].Value, 2)));
                //phaseshift for freq
                output_signal_frequenciesPhaseShifts.Add((float)Math.Atan2(complex[indexloop1].Value, complex[indexloop1].Key));
                indexloop1++;
            }
            OutputFreqDomainSignal = new Signal(false, output_signal_frequencies, output_signal_frequenciesAmplitudes, output_signal_frequenciesPhaseShifts);
        }
    }
}