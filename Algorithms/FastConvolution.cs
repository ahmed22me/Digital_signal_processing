using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<float> l1 = new List<float>();
            List<float> l2 = new List<float>();

            for (int i = 0; i < InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1; i++)
            {
                if (i < InputSignal1.Samples.Count)
                    l1.Add(InputSignal1.Samples[i]);
                else
                    l1.Add(0);

                if (i < InputSignal2.Samples.Count)
                    l2.Add(InputSignal2.Samples[i]);
                else
                    l2.Add(0);
            }

            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputTimeDomainSignal = new Signal(l1, InputSignal1.Periodic);
            dft.Run();

            List<float> r1 = new List<float>();
            List<float> i1 = new List<float>();
            for (int i = 0; i < l1.Count; i++)
            {
                r1.Add(dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                i1.Add(dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
            }

            dft = new DiscreteFourierTransform();
            dft.InputTimeDomainSignal = new Signal(l2, InputSignal2.Periodic);
            dft.Run();

            List<float> r2 = new List<float>();
            List<float> i2 = new List<float>();
            for (int i = 0; i < l2.Count; i++)
            {
                r2.Add(dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                i2.Add(dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
            }

            List<float> rres = new List<float>();
            List<float> ires = new List<float>();
            for (int i = 0; i < l1.Count; i++)
            {
                rres.Add(r1[i] * r2[i] - i1[i] * i2[i]);
                ires.Add(r1[i] * i2[i] + i1[i] * r2[i]);
            }

            List<float> PhaseShifts = new List<float>();
            List<float> Amplitudes = new List<float>();
            for (int i = 0; i < l1.Count; i++)
            {
                PhaseShifts.Add((float)Math.Atan2(ires[i], rres[i]));
                Amplitudes.Add((float)Math.Sqrt(ires[i] * ires[i] + rres[i] * rres[i]));
            }

            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            idft.InputFreqDomainSignal = new Signal(false, new List<float>(), Amplitudes, PhaseShifts);
            idft.Run();


            OutputConvolvedSignal = idft.OutputTimeDomainSignal;

        }
    }
}