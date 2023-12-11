using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {

            List<float> fisrt = new List<float>();
            for (int i = 1; i < InputSignal.Samples.Count; i++)
            {
                fisrt.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);
            }
            FirstDerivative = new Signal(fisrt, false);
            List<float> second = new List<float>();
            for (int i = 1; i < InputSignal.Samples.Count; i++)
            {
                try
                {
                    second.Add(InputSignal.Samples[i + 1] - 2 * InputSignal.Samples[i] + InputSignal.Samples[i - 1]);
                }
                catch (Exception ex)
                {
                    second.Add(0);
                }
            }
            SecondDerivative = new Signal(second, false);
        }
    }
}
