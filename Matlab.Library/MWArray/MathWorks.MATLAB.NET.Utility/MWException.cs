using System;

namespace MathWorks.MATLAB.NET.Utility
{
	[Serializable]
	public class MWException : ApplicationException
	{
		private bool disposed;

		private string[] mwStack;

		public override string StackTrace
		{
			get
			{
				string str = "... Matlab M-code Stack Trace ...\n";
				string[] array = mwStack;
				foreach (string str2 in array)
				{
					str += "    at\n";
					str += str2;
					str += "\n";
				}
				str += "\n... .Application Stack Trace ...\n";
				return str + base.StackTrace;
			}
		}

		public MWException(string msg, string[] stack)
			: base(msg)
		{
			mwStack = stack;
		}

		~MWException()
		{
			Dispose(disposing: false);
		}

		internal void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		internal virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing && mwStack != null)
				{
					Array.Clear(mwStack, 0, mwStack.Length);
					mwStack = null;
				}
				disposed = true;
			}
		}
	}
}
