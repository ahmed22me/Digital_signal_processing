using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            List<int> values = new List<int>();
            bool check = false;
            for (int i = 0; i < InputSignal.Samples.Count; i++) {
                if (InputSignal.Periodic == true)
                {
                    values.Add(InputSignal.SamplesIndices[i] + ShiftingValue);
                    check = true;
                }
                else {
                    values.Add(InputSignal.SamplesIndices[i] - ShiftingValue);
                }
                OutputShiftedSignal = new Signal(InputSignal.Samples, values, check);
            }

           
        }
    }
}
