using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using System;
using System.IO;
using System.Reflection;

namespace Math
{
	public class Class1 : IDisposable
	{
		private static MWMCR mcr;

		private static Exception ex_;

		private bool disposed;

		static Class1()
		{
			if (MWMCR.MCRAppInitialized)
			{
				try
				{
					Assembly executingAssembly = Assembly.GetExecutingAssembly();
					string location = executingAssembly.Location;
					int num = location.LastIndexOf("\\");
					location = location.Remove(num, location.Length - num);
					string value = "Math.ctf";
					Stream embeddedCtfStream = null;
					string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
					string[] array = manifestResourceNames;
					foreach (string text in array)
					{
						if (text.Contains(value))
						{
							embeddedCtfStream = executingAssembly.GetManifestResourceStream(text);
							break;
						}
					}
					mcr = new MWMCR("", location, embeddedCtfStream, isLibrary: true);
				}
				catch (Exception innerException)
				{
					ex_ = new Exception("MWArray assembly failed to be initialized", innerException);
				}
			}
			else
			{
				ex_ = new ApplicationException("MWArray assembly could not be initialized");
			}
		}

		public Class1()
		{
			if (ex_ != null)
			{
				throw ex_;
			}
		}

		~Class1()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				disposed = true;
			}
		}

		public MWArray combustible()
		{
			return mcr.EvaluateFunction("combustible", new MWArray[0]);
		}

		public MWArray combustible(MWArray A)
		{
			return mcr.EvaluateFunction("combustible", A);
		}

		public MWArray[] combustible(int numArgsOut)
		{
			return mcr.EvaluateFunction(numArgsOut, "combustible", new MWArray[0]);
		}

		public MWArray[] combustible(int numArgsOut, MWArray A)
		{
			return mcr.EvaluateFunction(numArgsOut, "combustible", A);
		}

		public void combustible(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
		{
			mcr.EvaluateFunction("combustible", numArgsOut, ref argsOut, argsIn);
		}

		public MWArray fire()
		{
			return mcr.EvaluateFunction("fire", new MWArray[0]);
		}

		public MWArray fire(MWArray U_in1)
		{
			return mcr.EvaluateFunction("fire", U_in1);
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1)
		{
			return mcr.EvaluateFunction("fire", U_in1, S_in1);
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W)
		{
			return mcr.EvaluateFunction("fire", U_in1, S_in1, W);
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax)
		{
			return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax);
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin)
		{
			return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin);
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha)
		{
			return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin, alpha);
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k)
		{
			return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k);
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig)
		{
			return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig);
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs)
		{
			return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig, Cs);
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST)
		{
			return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig, Cs, ST);
		}

		public MWArray fire(MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST, MWArray d)
		{
			return mcr.EvaluateFunction("fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig, Cs, ST, d);
		}

		public MWArray[] fire(int numArgsOut)
		{
			return mcr.EvaluateFunction(numArgsOut, "fire", new MWArray[0]);
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1)
		{
			return mcr.EvaluateFunction(numArgsOut, "fire", U_in1);
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1)
		{
			return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1);
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W)
		{
			return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W);
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax)
		{
			return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax);
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin)
		{
			return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin);
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha)
		{
			return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin, alpha);
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k)
		{
			return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k);
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig)
		{
			return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig);
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs)
		{
			return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig, Cs);
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST)
		{
			return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig, Cs, ST);
		}

		public MWArray[] fire(int numArgsOut, MWArray U_in1, MWArray S_in1, MWArray W, MWArray Tmax, MWArray Sfin, MWArray alpha, MWArray k, MWArray Tig, MWArray Cs, MWArray ST, MWArray d)
		{
			return mcr.EvaluateFunction(numArgsOut, "fire", U_in1, S_in1, W, Tmax, Sfin, alpha, k, Tig, Cs, ST, d);
		}

		public void fire(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
		{
			mcr.EvaluateFunction("fire", numArgsOut, ref argsOut, argsIn);
		}

		public MWArray focos()
		{
			return mcr.EvaluateFunction("focos", new MWArray[0]);
		}

		public MWArray focos(MWArray A)
		{
			return mcr.EvaluateFunction("focos", A);
		}

		public MWArray focos(MWArray A, MWArray focos)
		{
			return mcr.EvaluateFunction("focos", A, focos);
		}

		public MWArray[] focos(int numArgsOut)
		{
			return mcr.EvaluateFunction(numArgsOut, "focos", new MWArray[0]);
		}

		public MWArray[] focos(int numArgsOut, MWArray A)
		{
			return mcr.EvaluateFunction(numArgsOut, "focos", A);
		}

		public MWArray[] focos(int numArgsOut, MWArray A, MWArray focos)
		{
			return mcr.EvaluateFunction(numArgsOut, "focos", A, focos);
		}

		public void focos(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
		{
			mcr.EvaluateFunction("focos", numArgsOut, ref argsOut, argsIn);
		}

		public MWArray matriz()
		{
			return mcr.EvaluateFunction("matriz", new MWArray[0]);
		}

		public MWArray matriz(MWArray Lx)
		{
			return mcr.EvaluateFunction("matriz", Lx);
		}

		public MWArray matriz(MWArray Lx, MWArray Ly)
		{
			return mcr.EvaluateFunction("matriz", Lx, Ly);
		}

		public MWArray matriz(MWArray Lx, MWArray Ly, MWArray d)
		{
			return mcr.EvaluateFunction("matriz", Lx, Ly, d);
		}

		public MWArray[] matriz(int numArgsOut)
		{
			return mcr.EvaluateFunction(numArgsOut, "matriz", new MWArray[0]);
		}

		public MWArray[] matriz(int numArgsOut, MWArray Lx)
		{
			return mcr.EvaluateFunction(numArgsOut, "matriz", Lx);
		}

		public MWArray[] matriz(int numArgsOut, MWArray Lx, MWArray Ly)
		{
			return mcr.EvaluateFunction(numArgsOut, "matriz", Lx, Ly);
		}

		public MWArray[] matriz(int numArgsOut, MWArray Lx, MWArray Ly, MWArray d)
		{
			return mcr.EvaluateFunction(numArgsOut, "matriz", Lx, Ly, d);
		}

		public void matriz(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
		{
			mcr.EvaluateFunction("matriz", numArgsOut, ref argsOut, argsIn);
		}

		public void WaitForFiguresToDie()
		{
			mcr.WaitForFiguresToDie();
		}
	}
}
