using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {

            OutputTimeDomainSignal = new Signal(new List<float>(), false);

            int SamplesCount = InputFreqDomainSignal.FrequenciesAmplitudes.Count;

            for (int i = 0; i < SamplesCount; i++)
            {

                double first = 0;
                double second = 0;

                for (int j = 0; j < SamplesCount; j++)
                {
                    //x = im * cos(angel)
                    double x =
                        InputFreqDomainSignal.FrequenciesAmplitudes[j] *
                        Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[j]);
                    //y = im * sin(angel)
                    double y =
                        InputFreqDomainSignal.FrequenciesAmplitudes[j] *
                        Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[j]);
                    // power p= J*K*2*PI / N
                    double pow_e = (2 * i * j * Math.PI) / SamplesCount;
                    // x*cos power e
                    first += x * Math.Cos(pow_e);
                    // y*cos power e 
                    second -= y * Math.Sin(pow_e);

                }
                //first+second/N
                OutputTimeDomainSignal.Samples.Add((float)((first + second) / SamplesCount));

            }
        }
    }
}