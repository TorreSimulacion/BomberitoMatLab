using MathWorks.MATLAB.NET.Utility;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace MathWorks.MATLAB.NET.Arrays
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MWIndexArray : MWArray
	{
		private static readonly MWNumericArray _Empty = new MWNumericArray(MWArrayComponent.Real, 0, 0);

		internal static readonly MWCharArray TypeFieldName = "type";

		internal static readonly MWCharArray SubsFieldName = "subs";

		internal static readonly MWCharArray ArrayIndex = "()";

		internal static readonly MWCharArray CellIndex = "{}";

		internal static readonly MWCharArray ColonIndexer = new MWCharArray(":");

		internal double start;

		internal double step;

		public bool IsSparse
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return 1 == MWArray.mxIsSparse(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public int NonZeroMaxStorage
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return IsSparse ? mxGetNzmax(MXArrayHandle) : base.NumberOfElements;
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxCreateDoubleScalar_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern MWSafeHandle mxCreateDoubleScalar([In] double value);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetNzmax_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mxGetNzmax([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMXArrayGetIndexArrays_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mclMXArrayGetIndexArrays(out MWSafeHandle hMXArrayRows, out MWSafeHandle hMXArrayCols, [In] MWSafeHandle hMXArray);

		internal MWIndexArray(MWSafeHandle hMXArray)
			: base(hMXArray)
		{
			array_Type = MWArrayType.Index;
		}

		protected MWIndexArray()
		{
		}

		private MWIndexArray(double scalar)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				SetMXArray(mxCreateDoubleScalar(scalar), MWArrayType.Index, array.Length, array);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		private MWIndexArray(double start, double step)
		{
			this.start = start;
			this.step = step;
			array_Type = MWArrayType.Index;
		}

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{
				try
				{
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		public static implicit operator MWIndexArray(int scalar)
		{
			return new MWIndexArray(scalar);
		}

		public static implicit operator MWIndexArray(byte[] array)
		{
			return new MWNumericArray(1, array.Length, array);
		}

		public static implicit operator MWIndexArray(short[] array)
		{
			return new MWNumericArray(1, array.Length, array);
		}

		public static implicit operator MWIndexArray(int[] array)
		{
			return new MWNumericArray(1, array.Length, array);
		}

		public static implicit operator MWIndexArray(long[] array)
		{
			return new MWNumericArray(1, array.Length, array);
		}

		protected MWIndexArray(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}

		public override object Clone()
		{
			MWIndexArray mWIndexArray = (MWIndexArray)MemberwiseClone();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				mWIndexArray.SetMXArray(MWArray.mxDuplicateArray(MXArrayHandle), MWArrayType.Index);
				return mWIndexArray;
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override bool Equals(object obj)
		{
			return base.Equals((object)(MWIndexArray)obj);
		}
	}
}
