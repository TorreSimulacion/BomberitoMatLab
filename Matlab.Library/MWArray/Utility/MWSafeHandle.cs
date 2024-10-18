// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.MWSafeHandle
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

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

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxDestroyArray_proxy")]
    private static extern void mxDestroyArray([In] IntPtr pMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMxRefDestroyArray_proxy")]
    private static extern void mclMxRefDestroyArray([In] IntPtr pMXArray);

    internal MWSafeHandle()
      : base(true)
    {
      this.SetHandle(IntPtr.Zero);
    }

    internal MWSafeHandle(IntPtr hMXArray)
      : base(true)
    {
      this.SetHandle(hMXArray);
    }

    internal MWSafeHandle(IntPtr hMXArray, bool ownsHandle)
      : base(ownsHandle)
    {
      this.SetHandle(hMXArray);
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    protected override bool ReleaseHandle()
    {
      if (MWMCR.MCRAppInitialized && IntPtr.Zero != this.handle)
      {
        MWSafeHandle.mclMxRefDestroyArray(this.handle);
        this.handle = IntPtr.Zero;
        if (this.unmanagedBytesAllocated_ > 0L)
          GC.RemoveMemoryPressure(this.unmanagedBytesAllocated_);
      }
      return true;
    }

    public long UnmanagedBytesAllocated
    {
      set
      {
        this.unmanagedBytesAllocated_ = value;
        GC.AddMemoryPressure(value);
      }
    }
  }
}
