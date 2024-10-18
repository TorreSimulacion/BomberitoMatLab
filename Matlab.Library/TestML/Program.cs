using OT.Ternium.Unity.MathModels;
using System;
using System.Diagnostics;
using System.Threading;

namespace TestML
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int sizeX = 50;
                int sizeY = 50;
                float tMax = 2000;
                float sFin = .01f;
                int numFocus = 1;
                float alpha = 20;
                float k = .25f;
                float Tig = 573;
                float Cs = 0.01f;
                float ST = 1200;
                int d = 10;
                float Arr = 187.93f;
                float B = 558.49f;

                int rows = sizeX/d;
                int coulumns = sizeY/d;

                FireController fc = new FireController(sizeX, sizeY, tMax, sFin, numFocus, null, alpha, k, Tig, Cs, ST, d, B, Arr);
                fc.Start();
                int aux = 0;

                while (aux < 200)
                {
                    Console.WriteLine("-----------------------------------------------------------");

                    float[,] temperature = fc.Temperature;

                    for (int i = 0; i < coulumns; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            Console.Write(string.Format("{0,10:f4}", temperature[i, j]));
                        }
                        Console.WriteLine(string.Empty);
                    }

                    aux++;
                    Thread.Sleep(1000);
                }

                fc.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("** " + ex.ToString());
            }

            Console.WriteLine("Finished!");
            Console.ReadKey();
        }
    }
}
