using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace MathWorks.MATLAB.NET.Utility
{
	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	internal sealed class MWSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		internal long unmanagedBytesAllocated_;

		public long UnmanagedBytesAllocated
		{
			set
			{
				unmanagedBytesAllocated_ = value;
				GC.AddMemoryPressure(value);
			}
		}

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxDestroyArray_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern void mxDestroyArray([In] IntPtr pMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMxRefDestroyArray_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern void mclMxRefDestroyArray([In] IntPtr pMXArray);

		internal MWSafeHandle()
			: base(ownsHandle: true)
		{
			SetHandle(IntPtr.Zero);
		}

		internal MWSafeHandle(IntPtr hMXArray)
			: base(ownsHandle: true)
		{
			SetHandle(hMXArray);
		}

		internal MWSafeHandle(IntPtr hMXArray, bool ownsHandle)
			: base(ownsHandle)
		{
			SetHandle(hMXArray);
		}

		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected override bool ReleaseHandle()
		{
			if (MWMCR.MCRAppInitialized && IntPtr.Zero != handle)
			{
				mclMxRefDestroyArray(handle);
				handle = IntPtr.Zero;
				if (unmanagedBytesAllocated_ > 0)
				{
					GC.RemoveMemoryPressure(unmanagedBytesAllocated_);
				}
			}
			return true;
		}
	}
}
