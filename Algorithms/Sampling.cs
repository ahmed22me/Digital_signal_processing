using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } 
        public int M { get; set; } 
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }




        public override void Run()
        {

            List<float> output = new List<float>();
            List<float> output1 = new List<float>();
            FIR f = new FIR();

            f.InputFilterType = FILTER_TYPES.LOW;
            f.InputFS = 8000;
            f.InputStopBandAttenuation = 50;
            f.InputTransitionBand = 500;
            f.InputCutOffFrequency = 1500;

            if (M == 0 && L != 0)
            {
                L = L - 1;
                int indexloop1 = 0;
                while ( indexloop1 < InputSignal.Samples.Count )
                {
                    output.Add(InputSignal.Samples[indexloop1]);
                    if (indexloop1 == InputSignal.Samples.Count - 1)
                    { break; }
                    int indexloop2 = 0;
                    while (indexloop2 < L)
                    {
                        output.Add(0);
                         indexloop2++;
                    }
                    indexloop1++;
                }

                f.InputTimeDomainSignal = new Signal(output, false);

                f.Run();
                OutputSignal = f.OutputYn;

            }


            else if (L == 0 && M != 0)
            {
                M = M - 1;



                f.InputTimeDomainSignal = InputSignal;

                f.Run();
                OutputSignal = f.OutputYn;

                int check = M;
                int i = 0;

                while (i < OutputSignal.Samples.Count)
                {
                    if (check == M)
                    {
                        output.Add(OutputSignal.Samples[i]);
                        check = 0;
                    }
                    else
                    {
                        check++;
                    }
                    i++;

                }

                OutputSignal = new Signal(output, false);

            }

            else if (L != 0 && M != 0)
            {
                L = (L - 1);
                M = (M - 1);
                int i = 0;

                while ( i < InputSignal.Samples.Count)
                {
                    output.Add(InputSignal.Samples[i]);

                    for (int j = 0; j < L; j++)
                    {
                        output.Add(0);
                    }
                    i++;
                }

                f.InputTimeDomainSignal = new Signal(output, false);

                f.Run();
                OutputSignal = f.OutputYn;

                int check = M;

                output = new List<float>();
                int v = 0;
                while ( v < OutputSignal.Samples.Count)
                {
                    if (check == M)
                    {
                        if (OutputSignal.Samples[v] != 0)
                        {
                            output.Add(OutputSignal.Samples[v]);
                        }
                        check = 0;
                    }
                    else
                    {
                        check++;
                    }
                     v++;

                }

                OutputSignal = new Signal(output, false);





            }

            else
            {
                Console.WriteLine("error");
            }

            
        }
    }
}