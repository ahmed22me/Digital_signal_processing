using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MultiplySignalByConstant : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputConstant { get; set; }
        public Signal OutputMultipliedSignal { get; set; }

        public override void Run()
        {
            float res;
            List<float> result = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                res = InputSignal.Samples[i] * InputConstant;
                result.Add(res);
            }
            OutputMultipliedSignal = new Signal(result, true);
        }
    }
}
