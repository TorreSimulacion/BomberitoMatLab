using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using System;
using System.IO;
using System.Reflection;

namespace OT.Ternium.Math
{
	public class FireModel : IDisposable
	{
		private static MWMCR mcr;

		private static Exception ex_;

		private bool disposed;

        static FireModel()
        {
            if (MWMCR.MCRAppInitialized)
            {
                try
                {
                    Assembly executingAssembly = Assembly.GetExecutingAssembly();
                    string text = Path.GetDirectoryName(executingAssembly.Location);
                    string value = "Math.ctf";
                    Stream embeddedCtfStream = null;
                    string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
                    string[] array = manifestResourceNames;
                    for (int i = 0; i < array.Length; i++)
                    {
                        string text2 = array[i];
                        if (text2.Contains(value))
                        {
                            embeddedCtfStream = executingAssembly.GetManifestResourceStream(text2);
                            break;
                        }
                    }
                    FireModel.mcr = new MWMCR("", text, embeddedCtfStream, true);
                    return;
                }
                catch (Exception innerException)
                {
                    FireModel.ex_ = new Exception("MWArray assembly failed to be initialized", innerException);
                    return;
                }
            }
            FireModel.ex_ = new ApplicationException("MWArray assembly could not be initialized");
        }

        public FireModel()
		{
			if (FireModel.ex_ != null)
			{
				throw FireModel.ex_;
			}
		}

		~FireModel()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.disposed = true;
			}
		}

		public MWArray combustible()
		{
			return FireModel.mcr.EvaluateFunction("combustible", new MWArray[0]);
		}

		public MWArray combustible(MWArray A)
		{
			return FireModel.mcr.EvaluateFunction("combustible", new MWArray[]
			{
				A
			});
		}

		public MWArray[] combustible(int numArgsOut)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "combustible", new MWArray[0]);
		}

		public MWArray[] combustible(int numArgsOut, MWArray A)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "combustible", new MWArray[]
			{
				A
			});
		}

		public void combustible(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
		{
			FireModel.mcr.EvaluateFunction("combustible", numArgsOut, ref argsOut, argsIn);
		}

		public MWArray fire()
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[0]);
		}

		public MWArray fire(MWArray U_in1)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1
			});
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1,
				S_in1
			});
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W
			});
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax
			});
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin
			});
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha
			});
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k
			});
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k,
				Tig
			});
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k,
				Tig,
				Cs
			});
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k,
				Tig,
				Cs,
				ST
			});
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST, MWArray d)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k,
				Tig,
				Cs,
				ST,
				d
			});
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST, MWArray d, MWArray B)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k,
				Tig,
				Cs,
				ST,
				d,
				B
			});
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST, MWArray d, MWArray B, MWArray Arr)
		{
			return FireModel.mcr.EvaluateFunction("fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k,
				Tig,
				Cs,
				ST,
				d,
				B,
				Arr
			});
		}

		public MWArray[] fire(int numArgsOut)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[0]);
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1
			});
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1,
				S_in1
			});
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W
			});
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax
			});
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin
			});
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha
			});
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k
			});
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k,
				Tig
			});
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k,
				Tig,
				Cs
			});
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k,
				Tig,
				Cs,
				ST
			});
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST, MWArray d)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k,
				Tig,
				Cs,
				ST,
				d
			});
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST, MWArray d, MWArray B)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k,
				Tig,
				Cs,
				ST,
				d,
				B
			});
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST, MWArray d, MWArray B, MWArray Arr)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[]
			{
				U_in1,
				S_in1,
				W,
				Tmax,
				Sfin,
				alpha,
				k,
				Tig,
				Cs,
				ST,
				d,
				B,
				Arr
			});
		}

		public void fire(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
		{
			FireModel.mcr.EvaluateFunction("fire", numArgsOut, ref argsOut, argsIn);
		}

		public MWArray focos()
		{
			return FireModel.mcr.EvaluateFunction("focos", new MWArray[0]);
		}

		public MWArray focos(MWArray A)
		{
			return FireModel.mcr.EvaluateFunction("focos", new MWArray[]
			{
				A
			});
		}

		public MWArray focos(MWArray A, MWArray focos)
		{
			return FireModel.mcr.EvaluateFunction("focos", new MWArray[]
			{
				A,
				focos
			});
		}

		public MWArray[] focos(int numArgsOut)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "focos", new MWArray[0]);
		}

		public MWArray[] focos(int numArgsOut, MWArray A)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "focos", new MWArray[]
			{
				A
			});
		}

		public MWArray[] focos(int numArgsOut, MWArray A, MWArray focos)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "focos", new MWArray[]
			{
				A,
				focos
			});
		}

		public void focos(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
		{
			FireModel.mcr.EvaluateFunction("focos", numArgsOut, ref argsOut, argsIn);
		}

		public MWArray matriz()
		{
			return FireModel.mcr.EvaluateFunction("matriz", new MWArray[0]);
		}

		public MWArray matriz(MWArray Lx)
		{
			return FireModel.mcr.EvaluateFunction("matriz", new MWArray[]
			{
				Lx
			});
		}

		public MWArray matriz(MWArray Lx, MWArray Ly)
		{
			return FireModel.mcr.EvaluateFunction("matriz", new MWArray[]
			{
				Lx,
				Ly
			});
		}

		public MWArray matriz(MWArray Lx, MWArray Ly, MWArray d)
		{
			return FireModel.mcr.EvaluateFunction("matriz", new MWArray[]
			{
				Lx,
				Ly,
				d
			});
		}

		public MWArray[] matriz(int numArgsOut)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "matriz", new MWArray[0]);
		}

		public MWArray[] matriz(int numArgsOut, MWArray Lx)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "matriz", new MWArray[]
			{
				Lx
			});
		}

		public MWArray[] matriz(int numArgsOut, MWArray Lx, MWArray Ly)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "matriz", new MWArray[]
			{
				Lx,
				Ly
			});
		}

		public MWArray[] matriz(int numArgsOut, MWArray Lx, MWArray Ly, MWArray d)
		{
			return FireModel.mcr.EvaluateFunction(numArgsOut, "matriz", new MWArray[]
			{
				Lx,
				Ly,
				d
			});
		}

		public void matriz(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
		{
			FireModel.mcr.EvaluateFunction("matriz", numArgsOut, ref argsOut, argsIn);
		}

		public void WaitForFiguresToDie()
		{
			FireModel.mcr.WaitForFiguresToDie();
		}
	}
}
