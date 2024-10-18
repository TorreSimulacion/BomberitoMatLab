using System;

namespace MathWorks.MATLAB.NET.Utility
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	public class NativeGCAttribute : Attribute
	{
		private bool gcEnabled;

		private int gcBlockSize;

		public bool GCEnabled => gcEnabled;

		public int GCBlockSize
		{
			get
			{
				return gcBlockSize;
			}
			set
			{
				gcBlockSize = value;
			}
		}

		public NativeGCAttribute(bool enableGCTrigger)
		{
			gcEnabled = enableGCTrigger;
		}

		public NativeGCAttribute(bool enableGCTrigger, int GCBlockSize)
		{
			gcEnabled = enableGCTrigger;
			gcBlockSize = GCBlockSize;
		}
	}
}
