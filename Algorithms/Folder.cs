using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {

            List<float> s = new List<float>();
            List<int> e = new List<int>();
            for(int i = InputSignal.Samples.Count - 1; i >= 0; i--)
            {
                s.Add(InputSignal.Samples[i]);
                InputSignal.SamplesIndices[i] = InputSignal.SamplesIndices[i] * -1;
                e.Add(InputSignal.SamplesIndices[i]);
            }
            
            
            OutputFoldedSignal = new Signal(s,e, !InputSignal.Periodic);
        }
    }
}
