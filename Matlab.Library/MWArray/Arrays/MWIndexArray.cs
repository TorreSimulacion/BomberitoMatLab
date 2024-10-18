// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Arrays.MWIndexArray
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

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
    internal static readonly MWCharArray TypeFieldName = (MWCharArray) "type";
    internal static readonly MWCharArray SubsFieldName = (MWCharArray) "subs";
    internal static readonly MWCharArray ArrayIndex = (MWCharArray) "()";
    internal static readonly MWCharArray CellIndex = (MWCharArray) "{}";
    internal static readonly MWCharArray ColonIndexer = new MWCharArray(":");
    internal double start;
    internal double step;

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxCreateDoubleScalar_proxy")]
    internal static extern MWSafeHandle mxCreateDoubleScalar([In] double value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetNzmax_700_proxy")]
    internal static extern int mxGetNzmax([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMXArrayGetIndexArrays_proxy")]
    internal static extern int mclMXArrayGetIndexArrays(
      out MWSafeHandle hMXArrayRows,
      out MWSafeHandle hMXArrayCols,
      [In] MWSafeHandle hMXArray);

    internal MWIndexArray(MWSafeHandle hMXArray)
      : base(hMXArray)
    {
      this.array_Type = MWArrayType.Index;
    }

    protected MWIndexArray()
    {
    }

    private MWIndexArray(double scalar)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        this.SetMXArray(MWIndexArray.mxCreateDoubleScalar(scalar), MWArrayType.Index, dimensions.Length, dimensions);
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
      this.array_Type = MWArrayType.Index;
    }

    protected override void Dispose(bool disposing)
    {
      if (this.IsDisposed)
        return;
      try
      {
        int num = disposing ? 1 : 0;
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    public static implicit operator MWIndexArray(int scalar)
    {
      return new MWIndexArray((double) scalar);
    }

    public static implicit operator MWIndexArray(byte[] array)
    {
      return (MWIndexArray) new MWNumericArray(1, array.Length, array);
    }

    public static implicit operator MWIndexArray(short[] array)
    {
      return (MWIndexArray) new MWNumericArray(1, array.Length, array);
    }

    public static implicit operator MWIndexArray(int[] array)
    {
      return (MWIndexArray) new MWNumericArray(1, array.Length, array);
    }

    public static implicit operator MWIndexArray(long[] array)
    {
      return (MWIndexArray) new MWNumericArray(1, array.Length, array);
    }

    protected MWIndexArray(SerializationInfo serializationInfo, StreamingContext context)
      : base(serializationInfo, context)
    {
    }

    public bool IsSparse
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWArray.mxIsSparse(this.MXArrayHandle);
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
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return this.IsSparse ? MWIndexArray.mxGetNzmax(this.MXArrayHandle) : this.NumberOfElements;
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public override object Clone()
    {
      MWIndexArray mwIndexArray = (MWIndexArray) this.MemberwiseClone();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        mwIndexArray.SetMXArray(MWArray.mxDuplicateArray(this.MXArrayHandle), MWArrayType.Index);
        return (object) mwIndexArray;
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
      return base.Equals((object) (MWIndexArray) obj);
    }
  }
}
