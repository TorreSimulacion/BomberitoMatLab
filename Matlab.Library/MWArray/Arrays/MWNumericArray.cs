// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Arrays.MWNumericArray
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using MathWorks.MATLAB.NET.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace MathWorks.MATLAB.NET.Arrays
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public class MWNumericArray : MWIndexArray, IEquatable<MWNumericArray>
  {
    public static readonly MWNumericArray Inf = MWNumericArray._Inf;
    public static readonly MWNumericArray NaN = MWNumericArray._NaN;
    private static readonly MWNumericArray _Empty = new MWNumericArray(MWArrayComponent.Real, 0, 0);
    private static IDictionary systemTypeToNumericType = (IDictionary) null;
    private static Dictionary<MWNumericType, Type> numericTypeToSystemType = (Dictionary<MWNumericType, Type>) null;

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxCreateDoubleMatrix_700_proxy")]
    private static extern MWSafeHandle mxCreateDoubleMatrix(
      [In] int row,
      [In] int column,
      [In] MWArrayComplexity complexityFlag);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxCreateNumericArray_700_proxy")]
    private static extern MWSafeHandle mxCreateNumericArray(
      [In] int rank,
      [In] int[] dimensions,
      [In] MWNumericType elementType,
      [In] MWArrayComplexity complexityFlag);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxFastZeros_proxy")]
    private static extern MWSafeHandle mxFastZeros(
      [In] MWArrayComplexity complexityFlag,
      [In] int rows,
      [In] int columns);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetEps_proxy")]
    private static extern double mxGetEps();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetImagData_proxy")]
    internal static extern IntPtr mxGetImagData([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetInf_proxy")]
    private static extern double mxGetInf();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetNaN_proxy")]
    private static extern double mxGetNaN();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetPi_proxy")]
    internal static extern IntPtr mxGetPi([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetPr_proxy")]
    internal static extern IntPtr mxGetPr([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsComplex_proxy")]
    internal static extern byte mxIsComplex([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsFinite_proxy")]
    private static extern byte mxIsFinite([In] double value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsInf_proxy")]
    private static extern byte mxIsInf([In] double value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsNaN_proxy")]
    private static extern byte mxIsNaN([In] double value);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsUint8_proxy")]
    internal static extern byte mxIsUint8([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsInt16_proxy")]
    internal static extern byte mxIsInt16([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsInt32_proxy")]
    internal static extern byte mxIsInt32([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsInt64_proxy")]
    internal static extern byte mxIsInt64([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsSingle_proxy")]
    internal static extern byte mxIsSingle([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsDouble_proxy")]
    internal static extern byte mxIsDouble([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "array_handle_imag_proxy")]
    protected static extern IntPtr array_handle_imag([In] IntPtr pMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "array_handle_real_proxy")]
    protected static extern IntPtr array_handle_real([In] IntPtr pMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMXArrayGetReal_proxy")]
    internal static extern int mclMXArrayGetReal(
      out MWSafeHandle hMXArraySrcElem,
      [In] MWSafeHandle hMXArraySrcArray,
      [In] IntPtr numIndices,
      [In] IntPtr[] indices);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMXArrayGetImag_proxy")]
    internal static extern int mclMXArrayGetImag(
      out MWSafeHandle hMXArraySrcElem,
      [In] MWSafeHandle hMXArraySrcArray,
      [In] IntPtr numIndices,
      [In] IntPtr[] indices);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMXArraySetReal_proxy")]
    internal static extern int mclMXArraySetReal(
      [In] MWSafeHandle hMXArrayTrg,
      [In] MWSafeHandle hMXArraySrcElem,
      [In] IntPtr numIndices,
      [In] IntPtr[] indices);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMXArraySetImag_proxy")]
    internal static extern int mclMXArraySetImag(
      [In] MWSafeHandle hMXArrayTrg,
      [In] MWSafeHandle hMXArraySrcElem,
      [In] IntPtr numIndices,
      [In] IntPtr[] indices);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclGetNumericSparse_proxy")]
    internal static extern int mclGetNumericSparse(
      out IntPtr pMXArray,
      [In] IntPtr rowIndexSize,
      [In] IntPtr[] rowindex,
      [In] IntPtr colIndexSize,
      [In] IntPtr[] columnindex,
      [In] IntPtr dataSize,
      [In] double[] realData,
      [In] double[] imaginaryData,
      [In] IntPtr rows,
      [In] IntPtr columns,
      [In] IntPtr nonZeroMax,
      [In] MWArray.MATLABArrayType arrayType,
      [In] MWArrayComplexity complexityFlag);

    static MWNumericArray()
    {
      MWNumericArray.systemTypeToNumericType = (IDictionary) new Hashtable(6);
      MWNumericArray.systemTypeToNumericType.Add((object) typeof (byte).Name, (object) MWNumericType.UInt8);
      MWNumericArray.systemTypeToNumericType.Add((object) typeof (short).Name, (object) MWNumericType.Int16);
      MWNumericArray.systemTypeToNumericType.Add((object) typeof (int).Name, (object) MWNumericType.Int32);
      MWNumericArray.systemTypeToNumericType.Add((object) typeof (long).Name, (object) MWNumericType.Int64);
      MWNumericArray.systemTypeToNumericType.Add((object) typeof (float).Name, (object) MWNumericType.Single);
      MWNumericArray.systemTypeToNumericType.Add((object) typeof (double).Name, (object) MWNumericType.Double);
      MWNumericArray.numericTypeToSystemType = new Dictionary<MWNumericType, Type>();
      MWNumericArray.numericTypeToSystemType.Add(MWNumericType.UInt8, typeof (byte));
      MWNumericArray.numericTypeToSystemType.Add(MWNumericType.Int16, typeof (short));
      MWNumericArray.numericTypeToSystemType.Add(MWNumericType.Int32, typeof (int));
      MWNumericArray.numericTypeToSystemType.Add(MWNumericType.Int64, typeof (long));
      MWNumericArray.numericTypeToSystemType.Add(MWNumericType.Single, typeof (float));
      MWNumericArray.numericTypeToSystemType.Add(MWNumericType.Double, typeof (double));
    }

    public MWNumericArray()
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2];
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, MWArrayComplexity.Real), MWArrayType.Numeric, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(MWNumericType dataType)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[1];
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, dataType, MWArrayComplexity.Real), MWArrayType.Numeric, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(byte scalar)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        double num = (double) scalar;
        int[] dimensions = new int[2]{ 1, 1 };
        this.SetMXArray(MWIndexArray.mxCreateDoubleScalar(num), MWArrayType.Numeric, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(byte scalar, bool makeDouble)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        if (makeDouble)
        {
          this.SetMXArray(MWIndexArray.mxCreateDoubleScalar((double) scalar), MWArrayType.Numeric, dimensions.Length, dimensions);
        }
        else
        {
          byte[] source = new byte[1]{ scalar };
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.UInt8, MWArrayComplexity.Real), MWArrayType.Numeric, dimensions.Length, dimensions);
          Marshal.Copy(source, 0, MWArray.mxGetData(this.MXArrayHandle), 1);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(byte realValue, byte imaginaryValue)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        double[] source1 = new double[1]
        {
          (double) realValue
        };
        double[] source2 = new double[1]
        {
          (double) imaginaryValue
        };
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, MWArrayComplexity.Complex), MWArrayType.Numeric, dimensions.Length, dimensions, true);
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        IntPtr imagData = MWNumericArray.mxGetImagData(this.MXArrayHandle);
        Marshal.Copy(source1, 0, data, 1);
        Marshal.Copy(source2, 0, imagData, 1);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(byte realValue, byte imaginaryValue, bool makeDouble)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        MWNumericType elementType = makeDouble ? MWNumericType.Double : MWNumericType.UInt8;
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, elementType, MWArrayComplexity.Complex), MWArrayType.Numeric, dimensions.Length, dimensions, true);
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        IntPtr imagData = MWNumericArray.mxGetImagData(this.MXArrayHandle);
        if (makeDouble)
        {
          double[] source1 = new double[1]
          {
            (double) realValue
          };
          double[] source2 = new double[1]
          {
            (double) imaginaryValue
          };
          Marshal.Copy(source1, 0, data, 1);
          Marshal.Copy(source2, 0, imagData, 1);
        }
        else
        {
          byte[] source1 = new byte[1]{ realValue };
          byte[] source2 = new byte[1]{ imaginaryValue };
          Marshal.Copy(source1, 0, data, 1);
          Marshal.Copy(source2, 0, imagData, 1);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(short scalar)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        double num = (double) scalar;
        int[] dimensions = new int[2]{ 1, 1 };
        this.SetMXArray(MWIndexArray.mxCreateDoubleScalar(num), MWArrayType.Numeric, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(short scalar, bool makeDouble)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        if (makeDouble)
        {
          this.SetMXArray(MWIndexArray.mxCreateDoubleScalar((double) scalar), MWArrayType.Numeric, dimensions.Length, dimensions);
        }
        else
        {
          short[] source = new short[1]{ scalar };
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Int16, MWArrayComplexity.Real), MWArrayType.Numeric, dimensions.Length, dimensions);
          Marshal.Copy(source, 0, MWArray.mxGetData(this.MXArrayHandle), 1);
        }
        this.array_Type = MWArrayType.Numeric;
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(short realValue, short imaginaryValue)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[1]{ 1 };
        double[] source1 = new double[1]
        {
          (double) realValue
        };
        double[] source2 = new double[1]
        {
          (double) imaginaryValue
        };
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, MWArrayComplexity.Complex), MWArrayType.Numeric, dimensions.Length, dimensions, true);
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        IntPtr imagData = MWNumericArray.mxGetImagData(this.MXArrayHandle);
        Marshal.Copy(source1, 0, data, 1);
        Marshal.Copy(source2, 0, imagData, 1);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(short realValue, short imaginaryValue, bool makeDouble)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[1]{ 1 };
        MWNumericType elementType = makeDouble ? MWNumericType.Double : MWNumericType.Int16;
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, elementType, MWArrayComplexity.Complex), MWArrayType.Numeric, dimensions.Length, dimensions, true);
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        IntPtr imagData = MWNumericArray.mxGetImagData(this.MXArrayHandle);
        if (makeDouble)
        {
          double[] source1 = new double[1]
          {
            (double) realValue
          };
          double[] source2 = new double[1]
          {
            (double) imaginaryValue
          };
          Marshal.Copy(source1, 0, data, 1);
          Marshal.Copy(source2, 0, imagData, 1);
        }
        else
        {
          short[] source1 = new short[1]{ realValue };
          short[] source2 = new short[1]{ imaginaryValue };
          Marshal.Copy(source1, 0, data, 1);
          Marshal.Copy(source2, 0, imagData, 1);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(int scalar)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        double num = (double) scalar;
        int[] dimensions = new int[2]{ 1, 1 };
        this.SetMXArray(MWIndexArray.mxCreateDoubleScalar(num), MWArrayType.Numeric, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(int scalar, bool makeDouble)
    {
      int[] dimensions = new int[2]{ 1, 1 };
      try
      {
        Monitor.Enter(MWArray.mxSync);
        if (makeDouble)
        {
          this.SetMXArray(MWIndexArray.mxCreateDoubleScalar((double) scalar), MWArrayType.Numeric, dimensions.Length, dimensions);
        }
        else
        {
          int[] source = new int[1]{ scalar };
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Int32, MWArrayComplexity.Real), MWArrayType.Numeric, dimensions.Length, dimensions);
          Marshal.Copy(source, 0, MWArray.mxGetData(this.MXArrayHandle), 1);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(int realValue, int imaginaryValue)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[1]{ 1 };
        double[] source1 = new double[1]
        {
          (double) realValue
        };
        double[] source2 = new double[1]
        {
          (double) imaginaryValue
        };
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, MWArrayComplexity.Complex), MWArrayType.Numeric, dimensions.Length, dimensions, true);
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        IntPtr imagData = MWNumericArray.mxGetImagData(this.MXArrayHandle);
        Marshal.Copy(source1, 0, data, 1);
        Marshal.Copy(source2, 0, imagData, 1);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(int realValue, int imaginaryValue, bool makeDouble)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[1]{ 1 };
        MWNumericType elementType = makeDouble ? MWNumericType.Double : MWNumericType.Int32;
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, elementType, MWArrayComplexity.Complex), MWArrayType.Numeric, dimensions.Length, dimensions, true);
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        IntPtr imagData = MWNumericArray.mxGetImagData(this.MXArrayHandle);
        if (makeDouble)
        {
          double[] source1 = new double[1]
          {
            (double) realValue
          };
          double[] source2 = new double[1]
          {
            (double) imaginaryValue
          };
          Marshal.Copy(source1, 0, data, 1);
          Marshal.Copy(source2, 0, imagData, 1);
        }
        else
        {
          int[] source1 = new int[1]{ realValue };
          int[] source2 = new int[1]{ imaginaryValue };
          Marshal.Copy(source1, 0, data, 1);
          Marshal.Copy(source2, 0, imagData, 1);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(long scalar)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        double num = (double) scalar;
        int[] dimensions = new int[2]{ 1, 1 };
        this.SetMXArray(MWIndexArray.mxCreateDoubleScalar(num), MWArrayType.Numeric, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(long scalar, bool makeDouble)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        if (makeDouble)
        {
          this.SetMXArray(MWIndexArray.mxCreateDoubleScalar((double) scalar), MWArrayType.Numeric, dimensions.Length, dimensions);
        }
        else
        {
          long[] source = new long[1]{ scalar };
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Int64, MWArrayComplexity.Real), MWArrayType.Numeric, dimensions.Length, dimensions);
          Marshal.Copy(source, 0, MWArray.mxGetData(this.MXArrayHandle), 1);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(long realValue, long imaginaryValue)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        double[] source1 = new double[1]
        {
          (double) realValue
        };
        double[] source2 = new double[1]
        {
          (double) imaginaryValue
        };
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, MWArrayComplexity.Complex), MWArrayType.Numeric, dimensions.Length, dimensions, true);
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        IntPtr imagData = MWNumericArray.mxGetImagData(this.MXArrayHandle);
        Marshal.Copy(source1, 0, data, 1);
        Marshal.Copy(source2, 0, imagData, 1);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(long realValue, long imaginaryValue, bool makeDouble)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        MWNumericType elementType = makeDouble ? MWNumericType.Double : MWNumericType.Int64;
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, elementType, MWArrayComplexity.Complex), MWArrayType.Numeric, dimensions.Length, dimensions, true);
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        IntPtr imagData = MWNumericArray.mxGetImagData(this.MXArrayHandle);
        if (makeDouble)
        {
          double[] source1 = new double[1]
          {
            (double) realValue
          };
          double[] source2 = new double[1]
          {
            (double) imaginaryValue
          };
          Marshal.Copy(source1, 0, data, 1);
          Marshal.Copy(source2, 0, imagData, 1);
        }
        else
        {
          long[] source1 = new long[1]{ realValue };
          long[] source2 = new long[1]{ imaginaryValue };
          Marshal.Copy(source1, 0, data, 1);
          Marshal.Copy(source2, 0, imagData, 1);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(float scalar)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        double num = (double) scalar;
        int[] dimensions = new int[2]{ 1, 1 };
        this.SetMXArray(MWIndexArray.mxCreateDoubleScalar(num), MWArrayType.Numeric, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(float scalar, bool makeDouble)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        if (makeDouble)
        {
          this.SetMXArray(MWIndexArray.mxCreateDoubleScalar((double) scalar), MWArrayType.Numeric, dimensions.Length, dimensions);
        }
        else
        {
          float[] source = new float[1]{ scalar };
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Single, MWArrayComplexity.Real), MWArrayType.Numeric, dimensions.Length, dimensions);
          Marshal.Copy(source, 0, MWArray.mxGetData(this.MXArrayHandle), 1);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(float realValue, float imaginaryValue)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[1]{ 1 };
        double[] source1 = new double[1]
        {
          (double) realValue
        };
        double[] source2 = new double[1]
        {
          (double) imaginaryValue
        };
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, MWArrayComplexity.Complex), MWArrayType.Numeric, dimensions.Length, dimensions, true);
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        IntPtr imagData = MWNumericArray.mxGetImagData(this.MXArrayHandle);
        Marshal.Copy(source1, 0, data, 1);
        Marshal.Copy(source2, 0, imagData, 1);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(float realValue, float imaginaryValue, bool makeDouble)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        MWNumericType elementType = makeDouble ? MWNumericType.Double : MWNumericType.Single;
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, elementType, MWArrayComplexity.Complex), MWArrayType.Numeric, dimensions.Length, dimensions, true);
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        IntPtr imagData = MWNumericArray.mxGetImagData(this.MXArrayHandle);
        if (makeDouble)
        {
          double[] source1 = new double[1]
          {
            (double) realValue
          };
          double[] source2 = new double[1]
          {
            (double) imaginaryValue
          };
          Marshal.Copy(source1, 0, data, 1);
          Marshal.Copy(source2, 0, imagData, 1);
        }
        else
        {
          float[] source1 = new float[1]{ realValue };
          float[] source2 = new float[1]{ imaginaryValue };
          Marshal.Copy(source1, 0, data, 1);
          Marshal.Copy(source2, 0, imagData, 1);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(double scalar)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        this.SetMXArray(MWIndexArray.mxCreateDoubleScalar(scalar), MWArrayType.Numeric, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(double realValue, double imaginaryValue)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        double[] source1 = new double[1]{ realValue };
        double[] source2 = new double[1]{ imaginaryValue };
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, MWArrayComplexity.Complex), MWArrayType.Numeric, dimensions.Length, dimensions, true);
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        IntPtr imagData = MWNumericArray.mxGetImagData(this.MXArrayHandle);
        Marshal.Copy(source1, 0, data, 1);
        Marshal.Copy(source2, 0, imagData, 1);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(params int[] dimensions)
    {
      if (dimensions == null)
        throw new ArgumentNullException(nameof (dimensions));
      try
      {
        Monitor.Enter(MWArray.mxSync);
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, MWArrayComplexity.Real), MWArrayType.Numeric, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(MWArrayComplexity complexity, params int[] dimensions)
    {
      if (dimensions == null)
        throw new ArgumentNullException(nameof (dimensions));
      try
      {
        Monitor.Enter(MWArray.mxSync);
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, complexity), MWArrayType.Numeric, dimensions.Length, dimensions, complexity == MWArrayComplexity.Complex);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(
      MWArrayComplexity complexity,
      MWNumericType dataType,
      params int[] dimensions)
    {
      if (dimensions == null)
        throw new ArgumentNullException(nameof (dimensions));
      try
      {
        Monitor.Enter(MWArray.mxSync);
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, dataType, complexity), MWArrayType.Numeric, dimensions.Length, dimensions, complexity == MWArrayComplexity.Complex);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(int rows, int columns, byte[] realData)
      : this(rows, columns, realData, (byte[]) null, true, true)
    {
    }

    public MWNumericArray(
      int rows,
      int columns,
      byte[] realData,
      bool makeDouble,
      bool rowMajorData)
      : this(rows, columns, realData, (byte[]) null, makeDouble, rowMajorData)
    {
    }

    public MWNumericArray(int rows, int columns, byte[] realData, byte[] imaginaryData)
      : this(rows, columns, realData, imaginaryData, true, true)
    {
    }

    public MWNumericArray(
      int rows,
      int columns,
      byte[] realData,
      byte[] imaginaryData,
      bool makeDouble,
      bool rowMajorData)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        MWNumericArray.validateInput(rows * columns, (Array) realData, (Array) imaginaryData);
        int[] dimensions = new int[2]{ rows, columns };
        int length = realData.Length;
        MWArrayComplexity complexityFlag = imaginaryData != null ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
        if (!makeDouble)
        {
          byte[] source1 = rowMajorData ? new byte[length] : realData;
          byte[] source2 = imaginaryData;
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.UInt8, complexityFlag), MWArrayType.Numeric, dimensions.Length, dimensions, complexityFlag == MWArrayComplexity.Complex);
          if (rowMajorData)
          {
            source2 = imaginaryData != null ? new byte[length] : (byte[]) null;
            for (int index1 = 0; index1 < rows; ++index1)
            {
              for (int index2 = 0; index2 < columns; ++index2)
              {
                source1[index2 * rows + index1] = realData[index1 * columns + index2];
                if (source2 != null)
                  source2[index2 * rows + index1] = imaginaryData[index1 * columns + index2];
              }
            }
          }
          IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
          IntPtr destination = imaginaryData != null ? MWNumericArray.mxGetImagData(this.MXArrayHandle) : IntPtr.Zero;
          if (IntPtr.Zero != data)
            Marshal.Copy(source1, 0, data, length);
          if (imaginaryData == null || !(IntPtr.Zero != destination))
            return;
          Marshal.Copy(source2, 0, destination, length);
        }
        else
        {
          double[] source1 = new double[length];
          double[] source2 = imaginaryData != null ? new double[length] : (double[]) null;
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, complexityFlag), MWArrayType.Numeric, dimensions.Length, dimensions, complexityFlag == MWArrayComplexity.Complex);
          if (!rowMajorData)
          {
            for (int index = 0; index < length; ++index)
            {
              source1[index] = (double) realData[index];
              if (source2 != null)
                source2[index] = (double) imaginaryData[index];
            }
          }
          else
          {
            for (int index1 = 0; index1 < rows; ++index1)
            {
              for (int index2 = 0; index2 < columns; ++index2)
              {
                source1[index2 * rows + index1] = (double) realData[index1 * columns + index2];
                if (source2 != null)
                  source2[index2 * rows + index1] = (double) imaginaryData[index1 * columns + index2];
              }
            }
          }
          IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
          IntPtr destination = imaginaryData != null ? MWNumericArray.mxGetImagData(this.MXArrayHandle) : IntPtr.Zero;
          if (IntPtr.Zero != data)
            Marshal.Copy(source1, 0, data, length);
          if (imaginaryData == null || !(IntPtr.Zero != destination))
            return;
          Marshal.Copy(source2, 0, destination, length);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(int rows, int columns, short[] realData)
      : this(rows, columns, realData, (short[]) null, true, true)
    {
    }

    public MWNumericArray(
      int rows,
      int columns,
      short[] realData,
      bool makeDouble,
      bool rowMajorData)
      : this(rows, columns, realData, (short[]) null, makeDouble, rowMajorData)
    {
    }

    public MWNumericArray(int rows, int columns, short[] realData, short[] imaginaryData)
      : this(rows, columns, realData, imaginaryData, true, true)
    {
    }

    public MWNumericArray(
      int rows,
      int columns,
      short[] realData,
      short[] imaginaryData,
      bool makeDouble,
      bool rowMajorData)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        MWNumericArray.validateInput(rows * columns, (Array) realData, (Array) imaginaryData);
        int[] dimensions = new int[2]{ rows, columns };
        int length = realData.Length;
        MWArrayComplexity complexityFlag = imaginaryData != null ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
        if (!makeDouble)
        {
          short[] source1 = rowMajorData ? new short[realData.Length] : realData;
          short[] source2 = imaginaryData;
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Int16, complexityFlag), MWArrayType.Numeric, dimensions.Length, dimensions, complexityFlag == MWArrayComplexity.Complex);
          if (rowMajorData)
          {
            source2 = imaginaryData != null ? new short[realData.Length] : (short[]) null;
            for (int index1 = 0; index1 < rows; ++index1)
            {
              for (int index2 = 0; index2 < columns; ++index2)
              {
                source1[index2 * rows + index1] = realData[index1 * columns + index2];
                if (source2 != null)
                  source2[index2 * rows + index1] = imaginaryData[index1 * columns + index2];
              }
            }
          }
          IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
          IntPtr destination = imaginaryData != null ? MWNumericArray.mxGetImagData(this.MXArrayHandle) : IntPtr.Zero;
          if (IntPtr.Zero != data)
            Marshal.Copy(source1, 0, data, length);
          if (imaginaryData == null || !(IntPtr.Zero != destination))
            return;
          Marshal.Copy(source2, 0, destination, length);
        }
        else
        {
          double[] source1 = new double[length];
          double[] source2 = imaginaryData != null ? new double[imaginaryData.Length] : (double[]) null;
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, complexityFlag), MWArrayType.Numeric, dimensions.Length, dimensions, complexityFlag == MWArrayComplexity.Complex);
          if (!rowMajorData)
          {
            for (int index = 0; index < length; ++index)
            {
              source1[index] = (double) realData[index];
              if (source2 != null)
                source2[index] = (double) imaginaryData[index];
            }
          }
          else
          {
            for (int index1 = 0; index1 < rows; ++index1)
            {
              for (int index2 = 0; index2 < columns; ++index2)
              {
                source1[index2 * rows + index1] = (double) realData[index1 * columns + index2];
                if (source2 != null)
                  source2[index2 * rows + index1] = (double) imaginaryData[index1 * columns + index2];
              }
            }
          }
          IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
          IntPtr destination = imaginaryData != null ? MWNumericArray.mxGetImagData(this.MXArrayHandle) : IntPtr.Zero;
          if (IntPtr.Zero != data)
            Marshal.Copy(source1, 0, data, length);
          if (imaginaryData == null || !(IntPtr.Zero != destination))
            return;
          Marshal.Copy(source2, 0, destination, length);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(int rows, int columns, int[] realData)
      : this(rows, columns, realData, (int[]) null, true, true)
    {
    }

    public MWNumericArray(
      int rows,
      int columns,
      int[] realData,
      bool makeDouble,
      bool rowMajorData)
      : this(rows, columns, realData, (int[]) null, makeDouble, rowMajorData)
    {
    }

    public MWNumericArray(int rows, int columns, int[] realData, int[] imaginaryData)
      : this(rows, columns, realData, imaginaryData, true, true)
    {
    }

    public MWNumericArray(
      int rows,
      int columns,
      int[] realData,
      int[] imaginaryData,
      bool makeDouble,
      bool rowMajorData)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        MWNumericArray.validateInput(rows * columns, (Array) realData, (Array) imaginaryData);
        int[] dimensions = new int[2]{ rows, columns };
        int length = realData.Length;
        MWArrayComplexity complexityFlag = imaginaryData != null ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
        if (!makeDouble)
        {
          int[] source1 = rowMajorData ? new int[realData.Length] : realData;
          int[] source2 = imaginaryData;
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Int32, complexityFlag), MWArrayType.Numeric, dimensions.Length, dimensions, complexityFlag == MWArrayComplexity.Complex);
          if (rowMajorData)
          {
            source2 = imaginaryData != null ? new int[realData.Length] : (int[]) null;
            for (int index1 = 0; index1 < rows; ++index1)
            {
              for (int index2 = 0; index2 < columns; ++index2)
              {
                source1[index2 * rows + index1] = realData[index1 * columns + index2];
                if (source2 != null)
                  source2[index2 * rows + index1] = imaginaryData[index1 * columns + index2];
              }
            }
          }
          IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
          IntPtr destination = imaginaryData != null ? MWNumericArray.mxGetImagData(this.MXArrayHandle) : IntPtr.Zero;
          if (IntPtr.Zero != data)
            Marshal.Copy(source1, 0, data, length);
          if (imaginaryData == null || !(IntPtr.Zero != destination))
            return;
          Marshal.Copy(source2, 0, destination, length);
        }
        else
        {
          double[] source1 = new double[length];
          double[] source2 = imaginaryData != null ? new double[imaginaryData.Length] : (double[]) null;
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, complexityFlag), MWArrayType.Numeric, dimensions.Length, dimensions, complexityFlag == MWArrayComplexity.Complex);
          if (!rowMajorData)
          {
            for (int index = 0; index < length; ++index)
            {
              source1[index] = (double) realData[index];
              if (source2 != null)
                source2[index] = (double) imaginaryData[index];
            }
          }
          else
          {
            for (int index1 = 0; index1 < rows; ++index1)
            {
              for (int index2 = 0; index2 < columns; ++index2)
              {
                source1[index2 * rows + index1] = (double) realData[index1 * columns + index2];
                if (source2 != null)
                  source2[index2 * rows + index1] = (double) imaginaryData[index1 * columns + index2];
              }
            }
          }
          IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
          IntPtr destination = imaginaryData != null ? MWNumericArray.mxGetImagData(this.MXArrayHandle) : IntPtr.Zero;
          if (IntPtr.Zero != data)
            Marshal.Copy(source1, 0, data, length);
          if (imaginaryData == null || !(IntPtr.Zero != destination))
            return;
          Marshal.Copy(source2, 0, destination, length);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(int rows, int columns, long[] realData)
      : this(rows, columns, realData, (long[]) null, true, true)
    {
    }

    public MWNumericArray(
      int rows,
      int columns,
      long[] realData,
      bool makeDouble,
      bool rowMajorData)
      : this(rows, columns, realData, (long[]) null, makeDouble, rowMajorData)
    {
    }

    public MWNumericArray(int rows, int columns, long[] realData, long[] imaginaryData)
      : this(rows, columns, realData, imaginaryData, true, true)
    {
    }

    public MWNumericArray(
      int rows,
      int columns,
      long[] realData,
      long[] imaginaryData,
      bool makeDouble,
      bool rowMajorData)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        MWNumericArray.validateInput(rows * columns, (Array) realData, (Array) imaginaryData);
        int[] dimensions = new int[2]{ rows, columns };
        int length = realData.Length;
        MWArrayComplexity complexityFlag = imaginaryData != null ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
        if (!makeDouble)
        {
          long[] source1 = rowMajorData ? new long[realData.Length] : realData;
          long[] source2 = imaginaryData;
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Int64, complexityFlag), MWArrayType.Numeric, dimensions.Length, dimensions, complexityFlag == MWArrayComplexity.Complex);
          if (rowMajorData)
          {
            source2 = imaginaryData != null ? new long[realData.Length] : (long[]) null;
            for (int index1 = 0; index1 < rows; ++index1)
            {
              for (int index2 = 0; index2 < columns; ++index2)
              {
                source1[index2 * rows + index1] = realData[index1 * columns + index2];
                if (source2 != null)
                  source2[index2 * rows + index1] = imaginaryData[index1 * columns + index2];
              }
            }
          }
          IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
          IntPtr destination = imaginaryData != null ? MWNumericArray.mxGetImagData(this.MXArrayHandle) : IntPtr.Zero;
          if (IntPtr.Zero != data)
            Marshal.Copy(source1, 0, data, length);
          if (imaginaryData == null || !(IntPtr.Zero != destination))
            return;
          Marshal.Copy(source2, 0, destination, length);
        }
        else
        {
          double[] source1 = new double[length];
          double[] source2 = imaginaryData != null ? new double[imaginaryData.Length] : (double[]) null;
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, complexityFlag), MWArrayType.Numeric, dimensions.Length, dimensions, complexityFlag == MWArrayComplexity.Complex);
          if (!rowMajorData)
          {
            for (int index = 0; index < length; ++index)
            {
              source1[index] = (double) realData[index];
              if (source2 != null)
                source2[index] = (double) imaginaryData[index];
            }
          }
          else
          {
            for (int index1 = 0; index1 < rows; ++index1)
            {
              for (int index2 = 0; index2 < columns; ++index2)
              {
                source1[index2 * rows + index1] = (double) realData[index1 * columns + index2];
                if (source2 != null)
                  source2[index2 * rows + index1] = (double) imaginaryData[index1 * columns + index2];
              }
            }
          }
          IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
          IntPtr destination = imaginaryData != null ? MWNumericArray.mxGetImagData(this.MXArrayHandle) : IntPtr.Zero;
          if (IntPtr.Zero != data)
            Marshal.Copy(source1, 0, data, length);
          if (imaginaryData == null || !(IntPtr.Zero != destination))
            return;
          Marshal.Copy(source2, 0, destination, length);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(int rows, int columns, float[] realData)
      : this(rows, columns, realData, (float[]) null, true, true)
    {
    }

    public MWNumericArray(
      int rows,
      int columns,
      float[] realData,
      bool makeDouble,
      bool rowMajorData)
      : this(rows, columns, realData, (float[]) null, makeDouble, rowMajorData)
    {
    }

    public MWNumericArray(int rows, int columns, float[] realData, float[] imaginaryData)
      : this(rows, columns, realData, imaginaryData, true, true)
    {
    }

    public MWNumericArray(
      int rows,
      int columns,
      float[] realData,
      float[] imaginaryData,
      bool makeDouble,
      bool rowMajorData)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        MWNumericArray.validateInput(rows * columns, (Array) realData, (Array) imaginaryData);
        int[] dimensions = new int[2]{ rows, columns };
        int length = realData.Length;
        MWArrayComplexity complexityFlag = imaginaryData != null ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
        if (!makeDouble)
        {
          float[] source1 = rowMajorData ? new float[realData.Length] : realData;
          float[] source2 = imaginaryData;
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Single, complexityFlag), MWArrayType.Numeric, dimensions.Length, dimensions, complexityFlag == MWArrayComplexity.Complex);
          if (rowMajorData)
          {
            source2 = imaginaryData != null ? new float[realData.Length] : (float[]) null;
            for (int index1 = 0; index1 < rows; ++index1)
            {
              for (int index2 = 0; index2 < columns; ++index2)
              {
                source1[index2 * rows + index1] = realData[index1 * columns + index2];
                if (source2 != null)
                  source2[index2 * rows + index1] = imaginaryData[index1 * columns + index2];
              }
            }
          }
          IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
          IntPtr destination = imaginaryData != null ? MWNumericArray.mxGetImagData(this.MXArrayHandle) : IntPtr.Zero;
          if (IntPtr.Zero != data)
            Marshal.Copy(source1, 0, data, length);
          if (imaginaryData == null || !(IntPtr.Zero != destination))
            return;
          Marshal.Copy(source2, 0, destination, length);
        }
        else
        {
          double[] source1 = new double[length];
          double[] source2 = imaginaryData != null ? new double[imaginaryData.Length] : (double[]) null;
          this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, complexityFlag), MWArrayType.Numeric, dimensions.Length, dimensions, complexityFlag == MWArrayComplexity.Complex);
          if (!rowMajorData)
          {
            for (int index = 0; index < length; ++index)
            {
              source1[index] = (double) realData[index];
              if (source2 != null)
                source2[index] = (double) imaginaryData[index];
            }
          }
          else
          {
            for (int index1 = 0; index1 < rows; ++index1)
            {
              for (int index2 = 0; index2 < columns; ++index2)
              {
                source1[index2 * rows + index1] = (double) realData[index1 * columns + index2];
                if (source2 != null)
                  source2[index2 * rows + index1] = (double) imaginaryData[index1 * columns + index2];
              }
            }
          }
          IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
          IntPtr destination = imaginaryData != null ? MWNumericArray.mxGetImagData(this.MXArrayHandle) : IntPtr.Zero;
          if (IntPtr.Zero != data)
            Marshal.Copy(source1, 0, data, length);
          if (imaginaryData == null || !(IntPtr.Zero != destination))
            return;
          Marshal.Copy(source2, 0, destination, length);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(int rows, int columns, double[] realData)
      : this(rows, columns, realData, (double[]) null, true)
    {
    }

    public MWNumericArray(int rows, int columns, double[] realData, bool rowMajorData)
      : this(rows, columns, realData, (double[]) null, rowMajorData)
    {
    }

    public MWNumericArray(int rows, int columns, double[] realData, double[] imaginaryData)
      : this(rows, columns, realData, imaginaryData, true)
    {
    }

    public MWNumericArray(
      int rows,
      int columns,
      double[] realData,
      double[] imaginaryData,
      bool rowMajorData)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        MWNumericArray.validateInput(rows * columns, (Array) realData, (Array) imaginaryData);
        int[] dimensions = new int[2]{ rows, columns };
        int length = realData.Length;
        MWArrayComplexity complexityFlag = imaginaryData != null ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
        double[] source1 = rowMajorData ? new double[realData.Length] : realData;
        double[] source2 = imaginaryData;
        this.SetMXArray(MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, complexityFlag), MWArrayType.Numeric, dimensions.Length, dimensions, complexityFlag == MWArrayComplexity.Complex);
        if (rowMajorData)
        {
          source2 = imaginaryData != null ? new double[realData.Length] : (double[]) null;
          for (int index1 = 0; index1 < rows; ++index1)
          {
            for (int index2 = 0; index2 < columns; ++index2)
            {
              source1[index2 * rows + index1] = realData[index1 * columns + index2];
              if (source2 != null)
                source2[index2 * rows + index1] = imaginaryData[index1 * columns + index2];
            }
          }
        }
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        IntPtr destination = imaginaryData != null ? MWNumericArray.mxGetImagData(this.MXArrayHandle) : IntPtr.Zero;
        if (IntPtr.Zero != data)
          Marshal.Copy(source1, 0, data, length);
        if (imaginaryData == null || !(IntPtr.Zero != destination))
          return;
        Marshal.Copy(source2, 0, destination, length);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWNumericArray(Array array)
    {
      if (JaggedArray.IsJagged(array))
      {
        JaggedArray jaggedArray = new JaggedArray(array);
        int[] matlabDimension = MWArray.NetDimensionToMATLABDimension(jaggedArray.GetDimensions());
        Array flatArray = jaggedArray.GetFlatArray();
        lock (MWArray.mxSync)
        {
          IntPtr zero = IntPtr.Zero;
          MWNumericType elementType = (MWNumericType) MWNumericArray.systemTypeToNumericType[(object) jaggedArray.GetElementType().Name];
          MWSafeHandle numericArray = MWNumericArray.mxCreateNumericArray(matlabDimension.Length, matlabDimension, elementType, MWArrayComplexity.Real);
          IntPtr data = MWArray.mxGetData(numericArray);
          MWMarshal.MarshalManagedFlatArrayToUnmanaged(flatArray, data);
          this.SetMXArray(numericArray, MWArrayType.Numeric);
        }
      }
      else
        this.FastBuildNumericArray(array, (Array) null, true, true);
    }

    public MWNumericArray(Array realData, bool makeDouble, bool rowMajorData)
      : this(realData, (Array) null, makeDouble, rowMajorData)
    {
    }

    public MWNumericArray(Array realData, Array imaginaryData)
      : this(realData, imaginaryData, true, true)
    {
    }

    public MWNumericArray(Array realData, Array imaginaryData, bool makeDouble, bool rowMajorData)
    {
      this.FastBuildNumericArray(realData, imaginaryData, makeDouble, rowMajorData);
      this.array_Type = MWArrayType.Numeric;
    }

    internal MWNumericArray(MWSafeHandle hMXArray)
      : base(hMXArray)
    {
      this.array_Type = MWArrayType.Numeric;
    }

    internal MWNumericArray(MWArrayComponent arrayComponent, int rows, int columns)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        MWArrayComplexity complexityFlag = arrayComponent == MWArrayComponent.Real ? MWArrayComplexity.Real : MWArrayComplexity.Complex;
        int[] dimensions = new int[2]{ rows, columns };
        this.SetMXArray(MWNumericArray.mxCreateDoubleMatrix(rows, columns, complexityFlag), MWArrayType.Numeric, dimensions.Length, dimensions, complexityFlag == MWArrayComplexity.Complex);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    internal MWNumericArray(double start, double step, double end)
    {
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        int num = disposing ? 1 : 0;
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    public MWNumericArray this[params int[] indices]
    {
      get
      {
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (MWNumericArray) MWArray.GetTypedArray(this.ArrayIndexer((MWArray) this, indices), IntPtr.Zero);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
      set
      {
        if (MWArrayType.Numeric != value.array_Type)
          throw new InvalidCastException(MWArray.resourceManager.GetString("MWErrorDataArrayType"));
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          this.ArrayIndexer((MWArray) value, (MWArray) this, indices);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public MWNumericArray this[MWArrayComponent component, params int[] indices]
    {
      get
      {
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          IntPtr[] indices1 = new IntPtr[indices.Length];
          for (int index = 0; index < indices.Length; ++index)
            indices1[index] = (IntPtr) indices[index];
          MWSafeHandle hMXArraySrcElem;
          if (component == MWArrayComponent.Real)
          {
            if (MWNumericArray.mclMXArrayGetReal(out hMXArraySrcElem, this.MXArrayHandle, (IntPtr) indices.Length, indices1) != 0)
              throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidIndex"));
          }
          else if (MWNumericArray.mclMXArrayGetImag(out hMXArraySrcElem, this.MXArrayHandle, (IntPtr) indices.Length, indices1) != 0)
            throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidIndex"));
          return (MWNumericArray) MWArray.GetTypedArray(hMXArraySrcElem, IntPtr.Zero);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
      set
      {
        if (MWArrayType.Numeric != value.array_Type)
          throw new InvalidCastException(MWArray.resourceManager.GetString("MWErrorDataArrayType"));
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          IntPtr[] indices1 = new IntPtr[indices.Length];
          for (int index = 0; index < indices.Length; ++index)
            indices1[index] = (IntPtr) indices[index];
          if (component == MWArrayComponent.Real)
          {
            if (MWNumericArray.mclMXArraySetReal(this.MXArrayHandle, value.MXArrayHandle, (IntPtr) indices.Length, indices1) != 0)
              throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidArray"));
          }
          else if (MWNumericArray.mclMXArraySetImag(this.MXArrayHandle, value.MXArrayHandle, (IntPtr) indices.Length, indices1) != 0)
            throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidArray"));
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public static implicit operator MWNumericArray(byte scalar)
    {
      return new MWNumericArray(scalar, false);
    }

    public static implicit operator MWNumericArray(short scalar)
    {
      return new MWNumericArray(scalar, false);
    }

    public static implicit operator MWNumericArray(int scalar)
    {
      return new MWNumericArray(scalar);
    }

    public static implicit operator MWNumericArray(long scalar)
    {
      return new MWNumericArray(scalar, false);
    }

    public static implicit operator MWNumericArray(float scalar)
    {
      return new MWNumericArray(scalar, false);
    }

    public static implicit operator MWNumericArray(double scalar)
    {
      return new MWNumericArray(scalar);
    }

    public static implicit operator MWNumericArray(byte[] values)
    {
      return new MWNumericArray(1, values.Length, values, (byte[]) null, false, false);
    }

    public static implicit operator MWNumericArray(short[] values)
    {
      return new MWNumericArray(1, values.Length, values, (short[]) null, false, false);
    }

    public static implicit operator MWNumericArray(int[] values)
    {
      return new MWNumericArray(1, values.Length, values);
    }

    public static implicit operator MWNumericArray(long[] values)
    {
      return new MWNumericArray(1, values.Length, values, (long[]) null, false, false);
    }

    public static implicit operator MWNumericArray(float[] values)
    {
      return new MWNumericArray(1, values.Length, values, (float[]) null, false, false);
    }

    public static implicit operator MWNumericArray(double[] values)
    {
      return new MWNumericArray(1, values.Length, values);
    }

    public static implicit operator MWNumericArray(Array realData)
    {
      return new MWNumericArray(realData, (Array) null, false, true);
    }

    public static explicit operator byte(MWNumericArray array)
    {
      array.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        if (9 != MWArray.mxGetClassID(array.MXArrayHandle))
          throw new ApplicationException(MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType"));
        return checked ((byte) MWArray.mxGetScalar(array.MXArrayHandle));
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public static explicit operator short(MWNumericArray array)
    {
      array.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        if (10 != MWArray.mxGetClassID(array.MXArrayHandle))
          throw new ApplicationException(MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType"));
        return checked ((short) MWArray.mxGetScalar(array.MXArrayHandle));
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public static explicit operator int(MWNumericArray array)
    {
      array.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int classId = MWArray.mxGetClassID(array.MXArrayHandle);
        if (12 != classId && 6 != classId)
          throw new ApplicationException(MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType"));
        return checked ((int) MWArray.mxGetScalar(array.MXArrayHandle));
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public static explicit operator long(MWNumericArray array)
    {
      array.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        if (14 != MWArray.mxGetClassID(array.MXArrayHandle))
          throw new ApplicationException(MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType"));
        return checked ((long) MWArray.mxGetScalar(array.MXArrayHandle));
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public static explicit operator float(MWNumericArray array)
    {
      array.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        if (7 != MWArray.mxGetClassID(array.MXArrayHandle))
          throw new ApplicationException(MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType"));
        return (float) MWArray.mxGetScalar(array.MXArrayHandle);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public static explicit operator double(MWNumericArray array)
    {
      array.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        if (6 != MWArray.mxGetClassID(array.MXArrayHandle))
          throw new ApplicationException(MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType"));
        return MWArray.mxGetScalar(array.MXArrayHandle);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public byte ToScalarByte()
    {
      return (byte) this;
    }

    public short ToScalarShort()
    {
      return (short) this;
    }

    public int ToScalarInteger()
    {
      return (int) this;
    }

    public long ToScalarLong()
    {
      return (long) this;
    }

    public float ToScalarFloat()
    {
      return (float) this;
    }

    public double ToScalarDouble()
    {
      return (double) this;
    }

    protected MWNumericArray(SerializationInfo serializationInfo, StreamingContext context)
      : base(serializationInfo, context)
    {
    }

    public static MWNumericArray Empty
    {
      get
      {
        return (MWNumericArray) MWNumericArray._Empty.Clone();
      }
    }

    public static double FloatingPointAccuracy
    {
      get
      {
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (double) new MWNumericArray(MWNumericArray.mxGetEps());
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    private static MWNumericArray _Inf
    {
      get
      {
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return new MWNumericArray(MWNumericArray.mxGetInf());
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    private static MWNumericArray _NaN
    {
      get
      {
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return new MWNumericArray(MWNumericArray.mxGetNaN());
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public MWNumericType NumericType
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (MWNumericType) MWArray.mxGetClassID(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsByte
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWNumericArray.mxIsUint8(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsComplex
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWNumericArray.mxIsComplex(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsDouble
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWNumericArray.mxIsDouble(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsFloat
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWNumericArray.mxIsSingle(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsInteger
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWNumericArray.mxIsInt32(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsLong
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWNumericArray.mxIsInt64(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsInfinity
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          if (1 != MWArray.mxGetNumberOfElements(this.MXArrayHandle))
            throw new Exception(MWArray.resourceManager.GetString("MWErrorNotScalar"));
          return (byte) 1 == MWNumericArray.mxIsInf((double) this);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsNaN
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          if (1 != MWArray.mxGetNumberOfElements(this.MXArrayHandle))
            throw new Exception(MWArray.resourceManager.GetString("MWErrorNotScalar"));
          return (byte) 1 == MWNumericArray.mxIsNaN((double) this);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsShort
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWNumericArray.mxIsInt16(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    private static void validateInput(int specifiedSize, Array realData, Array imaginaryData)
    {
      if (realData == null)
        throw new ArgumentNullException(nameof (realData));
      if (realData.LongLength != (long) specifiedSize)
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorDataArraySize"), nameof (realData));
      MWNumericArray.validateRealAndImaginaryInput(realData, imaginaryData);
    }

    private static void validateInput(Array realData, Array imaginaryData)
    {
      if (realData == null)
        throw new ArgumentNullException(nameof (realData));
      Type elementType = realData.GetType().GetElementType();
      if (typeof (double) != elementType && typeof (int) != elementType && (typeof (byte) != elementType && typeof (short) != elementType) && (typeof (long) != elementType && typeof (float) != elementType))
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorDataArrayType"));
      MWNumericArray.validateRealAndImaginaryInput(realData, imaginaryData);
    }

    private static void validateRealAndImaginaryInput(Array realData, Array imaginaryData)
    {
      if (imaginaryData == null)
        return;
      if (realData.GetType().GetElementType() != imaginaryData.GetType().GetElementType())
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorDataArrayType"), nameof (imaginaryData));
      if (realData.Length != imaginaryData.Length)
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorDataArraySizeMismatch"));
      int rank = realData.Rank;
      if (rank != imaginaryData.Rank)
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorDataArrayRankMismatch"));
      for (int dimension = 0; dimension < rank; ++dimension)
      {
        if (realData.GetLength(dimension) != imaginaryData.GetLength(dimension))
          throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorRealImaginaryDimensionMismatch"));
      }
    }

    public static MWNumericArray MakeSparse(
      int rows,
      int columns,
      MWArrayComplexity complexity,
      int nonZeroMax)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        IntPtr pMXArray;
        if (MWNumericArray.mclGetNumericSparse(out pMXArray, (IntPtr) 0, (IntPtr[]) null, (IntPtr) 0, (IntPtr[]) null, (IntPtr) 0, (double[]) null, (double[]) null, (IntPtr) rows, (IntPtr) columns, (IntPtr) nonZeroMax, MWArray.MATLABArrayType.Double, complexity) != 0)
          return (MWNumericArray) null;
        MWSafeHandle hMXArray;
        if (MWArray.mclArrayHandle2mxArray(out hMXArray, pMXArray) != 0)
          throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidArray"));
        return new MWNumericArray(hMXArray);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public static MWNumericArray MakeSparse(
      int[] rowIndex,
      int[] columnIndex,
      double[] realData)
    {
      return MWNumericArray.MakeSparse(rowIndex, columnIndex, realData, (double[]) null);
    }

    public static MWNumericArray MakeSparse(
      int[] rowIndex,
      int[] columnIndex,
      double[] realData,
      double[] imaginaryData)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int length = rowIndex.Length;
        if (length != columnIndex.Length)
          throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidIndices"));
        int num1 = 0;
        int num2 = 0;
        for (int index = 0; index < length; ++index)
        {
          num1 = Math.Max(num1, rowIndex[index]);
          num2 = Math.Max(num2, columnIndex[index]);
        }
        return MWNumericArray.MakeSparse(num1, num2, rowIndex, columnIndex, realData, imaginaryData);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public static MWNumericArray MakeSparse(
      int rows,
      int columns,
      int[] rowIndex,
      int[] columnIndex,
      double[] realData)
    {
      return MWNumericArray.MakeSparse(rows, columns, rowIndex, columnIndex, realData, (double[]) null);
    }

    public static MWNumericArray MakeSparse(
      int rows,
      int columns,
      int[] rowIndex,
      int[] columnIndex,
      double[] realData,
      int nonZeroMax)
    {
      return MWNumericArray.MakeSparse(rows, columns, rowIndex, columnIndex, realData, (double[]) null, nonZeroMax);
    }

    public static MWNumericArray MakeSparse(
      int rows,
      int columns,
      int[] rowIndex,
      int[] columnIndex,
      double[] realData,
      double[] imaginaryData)
    {
      return MWNumericArray.MakeSparse(rows, columns, rowIndex, columnIndex, realData, imaginaryData, rowIndex.Length);
    }

    public static MWNumericArray MakeSparse(
      int rows,
      int columns,
      int[] rowIndex,
      int[] columnIndex,
      double[] realData,
      double[] imaginaryData,
      int nonZeroMax)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        IntPtr pMXArray = IntPtr.Zero;
        MWArrayComplexity complexityFlag = imaginaryData == null ? MWArrayComplexity.Real : MWArrayComplexity.Complex;
        IntPtr[] rowindex = new IntPtr[rowIndex.Length];
        IntPtr[] columnindex = new IntPtr[columnIndex.Length];
        for (int index = 0; index < rowIndex.Length; ++index)
        {
          rowindex[index] = (IntPtr) rowIndex[index];
          columnindex[index] = (IntPtr) columnIndex[index];
        }
        if (MWNumericArray.mclGetNumericSparse(out pMXArray, (IntPtr) rowIndex.Length, rowindex, (IntPtr) columnIndex.Length, columnindex, (IntPtr) realData.Length, realData, imaginaryData, (IntPtr) rows, (IntPtr) columns, (IntPtr) nonZeroMax, MWArray.MATLABArrayType.Double, complexityFlag) != 0)
          return (MWNumericArray) null;
        MWSafeHandle hMXArray;
        if (MWArray.mclArrayHandle2mxArray(out hMXArray, pMXArray) != 0)
          throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidArray"));
        return new MWNumericArray(hMXArray);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    internal Array MarshalToFullArray<T>(
      int rows,
      int[,] rowIndices,
      int cols,
      int[,] colIndices,
      int numSparseElements,
      Array vectorData)
    {
      Array instance = Array.CreateInstance(typeof (T), rows, cols);
      for (int index = 0; index < numSparseElements; ++index)
      {
        T obj = ((T[]) vectorData)[index];
        instance.SetValue((object) obj, rowIndices[0, index] - 1, colIndices[0, index] - 1);
      }
      return instance;
    }

    internal Array ToArray(MWArrayComponent component, bool sparseIndex)
    {
      this.CheckDisposed();
      lock (MWArray.mxSync)
      {
        IntPtr num1 = component == MWArrayComponent.Real ? MWArray.mxGetData(this.MXArrayHandle) : MWNumericArray.mxGetImagData(this.MXArrayHandle);
        if (num1 == IntPtr.Zero && component == MWArrayComponent.Imaginary)
          throw new InvalidDataException("No imaginary data elements in the array.");
        if (MWArray.mxIsSparse(this.MXArrayHandle) != (byte) 0)
        {
          int num2 = 0;
          int num3 = 0;
          MWSafeHandle hMXArrayRows;
          MWSafeHandle hMXArrayCols;
          if (MWIndexArray.mclMXArrayGetIndexArrays(out hMXArrayRows, out hMXArrayCols, this.MXArrayHandle) != 0)
            throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidIndex"));
          MWNumericArray mwNumericArray1 = new MWNumericArray(hMXArrayRows);
          MWNumericArray mwNumericArray2 = new MWNumericArray(hMXArrayCols);
          int numberOfElements = mwNumericArray1.NumberOfElements;
          int[,] rowIndices;
          int[,] colIndices;
          if (MWNumericType.UInt64 != mwNumericArray1.NumericType)
          {
            rowIndices = (int[,]) mwNumericArray1.ToArray(MWArrayComponent.Real, true);
            colIndices = (int[,]) mwNumericArray2.ToArray(MWArrayComponent.Real, true);
            for (int index = 0; index < numberOfElements; ++index)
            {
              num2 = Math.Max(num2, rowIndices[0, index]);
              num3 = Math.Max(num3, colIndices[0, index]);
            }
          }
          else
          {
            long[,] array1 = (long[,]) mwNumericArray1.ToArray(MWArrayComponent.Real, true);
            long[,] array2 = (long[,]) mwNumericArray2.ToArray(MWArrayComponent.Real, true);
            rowIndices = new int[array1.GetLength(0), array1.GetLength(1)];
            colIndices = new int[array2.GetLength(0), array2.GetLength(1)];
            for (int index = 0; index < numberOfElements; ++index)
            {
              num2 = Math.Max(num2, rowIndices[0, index] = checked ((int) array1[0, index]));
              num3 = Math.Max(num3, colIndices[0, index] = checked ((int) array2[0, index]));
            }
          }
          switch (this.NumericType)
          {
            case MWNumericType.Double:
              Array instance1 = Array.CreateInstance(typeof (double), numberOfElements);
              Marshal.Copy(num1, (double[]) instance1, 0, numberOfElements);
              return this.MarshalToFullArray<double>(num2, rowIndices, num3, colIndices, numberOfElements, instance1);
            case MWNumericType.Single:
              Array instance2 = Array.CreateInstance(typeof (float), numberOfElements);
              Marshal.Copy(num1, (float[]) instance2, 0, numberOfElements);
              return this.MarshalToFullArray<float>(num2, rowIndices, num3, colIndices, numberOfElements, instance2);
            case MWNumericType.UInt8:
              Array instance3 = Array.CreateInstance(typeof (byte), numberOfElements);
              Marshal.Copy(num1, (byte[]) instance3, 0, numberOfElements);
              return this.MarshalToFullArray<byte>(num2, rowIndices, num3, colIndices, numberOfElements, instance3);
            case MWNumericType.Int16:
              Array instance4 = Array.CreateInstance(typeof (short), numberOfElements);
              Marshal.Copy(num1, (short[]) instance4, 0, numberOfElements);
              return this.MarshalToFullArray<short>(num2, rowIndices, num3, colIndices, numberOfElements, instance4);
            case MWNumericType.Int32:
              Array instance5 = Array.CreateInstance(typeof (int), numberOfElements);
              Marshal.Copy(num1, (int[]) instance5, 0, numberOfElements);
              return this.MarshalToFullArray<int>(num2, rowIndices, num3, colIndices, numberOfElements, instance5);
            case MWNumericType.Int64:
              Array instance6 = Array.CreateInstance(typeof (long), numberOfElements);
              Marshal.Copy(num1, (long[]) instance6, 0, numberOfElements);
              return this.MarshalToFullArray<long>(num2, rowIndices, num3, colIndices, numberOfElements, instance6);
            default:
              throw new ApplicationException(MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType"));
          }
        }
        else
        {
          MWNumericType index = this.NumericType;
          int[] netDimension = MWArray.MATLABDimensionToNetDimension(this.Dimensions);
          if (sparseIndex)
          {
            if (MWNumericType.UInt32 == index)
              index = MWNumericType.Int32;
            if (MWNumericType.UInt64 == index)
              index = MWNumericType.Int64;
          }
          Array instance = Array.CreateInstance(MWNumericArray.numericTypeToSystemType[index], netDimension);
          MWMarshal.MarshalUnmanagedColumnMajorToManagedRowMajor(num1, instance);
          return instance;
        }
      }
    }

    public override object Clone()
    {
      this.CheckDisposed();
      MWNumericArray mwNumericArray = (MWNumericArray) this.MemberwiseClone();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        mwNumericArray.SetMXArray(MWArray.mxDuplicateArray(this.MXArrayHandle), MWArrayType.Numeric);
        return (object) mwNumericArray;
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public override string ToString()
    {
      return base.ToString();
    }

    public override bool Equals(object obj)
    {
      if (!(obj is MWNumericArray))
        return false;
      return this.Equals(obj as MWNumericArray);
    }

    private bool Equals(MWNumericArray obj)
    {
      return base.Equals((object) obj);
    }

    bool IEquatable<MWNumericArray>.Equals(MWNumericArray other)
    {
      return this.Equals(other);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public Array ToVector(MWArrayComponent component)
    {
      this.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int numberOfElements = this.NumberOfElements;
        MWNumericType numericType = this.NumericType;
        IntPtr source = component == MWArrayComponent.Real ? MWArray.mxGetData(this.MXArrayHandle) : MWNumericArray.mxGetImagData(this.MXArrayHandle);
        switch (numericType)
        {
          case MWNumericType.Double:
            double[] destination1 = new double[numberOfElements];
            Marshal.Copy(source, destination1, 0, numberOfElements);
            return (Array) destination1;
          case MWNumericType.Single:
            float[] destination2 = new float[numberOfElements];
            Marshal.Copy(source, destination2, 0, numberOfElements);
            return (Array) destination2;
          case MWNumericType.UInt8:
            byte[] destination3 = new byte[numberOfElements];
            Marshal.Copy(source, destination3, 0, numberOfElements);
            return (Array) destination3;
          case MWNumericType.Int16:
            short[] destination4 = new short[numberOfElements];
            Marshal.Copy(source, destination4, 0, numberOfElements);
            return (Array) destination4;
          case MWNumericType.Int32:
            int[] destination5 = new int[numberOfElements];
            Marshal.Copy(source, destination5, 0, numberOfElements);
            return (Array) destination5;
          case MWNumericType.Int64:
            long[] destination6 = new long[numberOfElements];
            Marshal.Copy(source, destination6, 0, numberOfElements);
            return (Array) destination6;
          default:
            throw new ApplicationException(MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType"));
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public Array ToEmptyVector()
    {
      this.CheckDisposed();
      lock (MWArray.mxSync)
      {
        MWNumericType numericType = this.NumericType;
        return Array.CreateInstance(MWNumericArray.numericTypeToSystemType[numericType], 0);
      }
    }

    public override Array ToArray()
    {
      if (this.IsComplex)
        throw new InvalidDataException("Data type unsupported by MATLAB .NET Assembly for output");
      return this.ToArray(MWArrayComponent.Real);
    }

    public Array ToArray(MWArrayComponent component)
    {
      return this.ToArray(component, false);
    }

    private void BuildNumericArray(
      Array realData,
      Array imaginaryData,
      bool makeDouble,
      bool rowMajorData)
    {
      if (realData == null)
        throw new ArgumentNullException(nameof (realData));
      Type elementType1 = realData.GetType().GetElementType();
      if (typeof (double) != elementType1 && typeof (int) != elementType1 && (typeof (byte) != elementType1 && typeof (short) != elementType1) && (typeof (long) != elementType1 && typeof (float) != elementType1))
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorDataArrayType"));
      if (imaginaryData != null)
      {
        if (elementType1 != imaginaryData.GetType().GetElementType())
          throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorDataArrayType"), nameof (imaginaryData));
        if (realData.Length != imaginaryData.Length)
          throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorDataArraySizeMismatch"));
      }
      int rank = realData.Rank;
      int num1 = imaginaryData == null ? 0 : imaginaryData.Rank;
      MWArrayComplexity complexityFlag = num1 != 0 ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
      if (imaginaryData != null && rank != num1)
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorDimensionMismatch"));
      int[] dimensions = new int[Math.Max(rank, 2)];
      int length = realData.Length;
      int num2 = dimensions[0] = 1 < rank ? realData.GetLength(rank - 2) : 1;
      int num3 = dimensions[1] = realData.GetLength(rank - 1);
      for (int dimension = 0; dimension < rank - 2; ++dimension)
        dimensions[rank - dimension - 1] = realData.GetLength(dimension);
      IEnumerator enumerator1 = realData.GetEnumerator();
      IEnumerator enumerator2 = imaginaryData?.GetEnumerator();
      Type elementType2 = makeDouble ? typeof (double) : elementType1;
      Array instance = Array.CreateInstance(elementType2, length);
      Array array = imaginaryData != null ? Array.CreateInstance(elementType2, length) : (Array) null;
      if (!rowMajorData)
      {
        for (int index = 0; index < length; ++index)
        {
          enumerator1.MoveNext();
          instance.SetValue(enumerator1.Current, index);
          if (imaginaryData != null)
          {
            enumerator2.MoveNext();
            array.SetValue(enumerator2.Current, index);
          }
        }
      }
      else
      {
        for (int index1 = 0; index1 < length; index1 += num2 * num3)
        {
          for (int index2 = 0; index2 < num2; ++index2)
          {
            for (int index3 = 0; index3 < num3; ++index3)
            {
              enumerator1.MoveNext();
              instance.SetValue(enumerator1.Current, index3 * num2 + index2 + index1);
              if (imaginaryData != null)
              {
                enumerator2.MoveNext();
                array.SetValue(enumerator2.Current, index3 * num2 + index2 + index1);
              }
            }
          }
        }
      }
      try
      {
        Monitor.Enter(MWArray.mxSync);
        IntPtr zero1 = IntPtr.Zero;
        IntPtr zero2 = IntPtr.Zero;
        MWNumericType elementType3 = (MWNumericType) MWNumericArray.systemTypeToNumericType[(object) elementType2.Name];
        MWSafeHandle numericArray = MWNumericArray.mxCreateNumericArray(dimensions.Length, dimensions, elementType3, complexityFlag);
        IntPtr data = MWArray.mxGetData(numericArray);
        IntPtr destination = imaginaryData != null ? MWNumericArray.mxGetImagData(numericArray) : IntPtr.Zero;
        switch (elementType3)
        {
          case MWNumericType.Double:
            if (IntPtr.Zero != data)
              Marshal.Copy((double[]) instance, 0, data, length);
            if (IntPtr.Zero != destination)
            {
              Marshal.Copy((double[]) array, 0, destination, length);
              break;
            }
            break;
          case MWNumericType.Single:
            if (IntPtr.Zero != data)
              Marshal.Copy((float[]) instance, 0, data, length);
            if (IntPtr.Zero != destination)
            {
              Marshal.Copy((float[]) array, 0, destination, length);
              break;
            }
            break;
          case MWNumericType.UInt8:
            if (IntPtr.Zero != data)
              Marshal.Copy((byte[]) instance, 0, data, length);
            if (IntPtr.Zero != destination)
            {
              Marshal.Copy((byte[]) array, 0, destination, length);
              break;
            }
            break;
          case MWNumericType.Int16:
            if (IntPtr.Zero != data)
              Marshal.Copy((short[]) instance, 0, data, length);
            if (IntPtr.Zero != destination)
            {
              Marshal.Copy((short[]) array, 0, destination, length);
              break;
            }
            break;
          case MWNumericType.Int32:
            if (IntPtr.Zero != data)
              Marshal.Copy((int[]) instance, 0, data, length);
            if (IntPtr.Zero != destination)
            {
              Marshal.Copy((int[]) array, 0, destination, length);
              break;
            }
            break;
          case MWNumericType.Int64:
            if (IntPtr.Zero != data)
              Marshal.Copy((long[]) instance, 0, data, length);
            if (IntPtr.Zero != destination)
            {
              Marshal.Copy((long[]) array, 0, destination, length);
              break;
            }
            break;
          default:
            throw new ApplicationException(MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType"));
        }
        this.SetMXArray(numericArray, MWArrayType.Numeric);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    private unsafe void FastBuildNumericArray(
      Array realData,
      Array imaginaryData,
      bool makeDouble,
      bool rowMajorData)
    {
      MWNumericArray.validateInput(realData, imaginaryData);
      int rank = realData.Rank;
      if (rank == 1)
        rowMajorData = false;
      int[] dimensions = new int[rank];
      for (int dimension = 0; dimension < rank; ++dimension)
        dimensions[dimension] = realData.GetLength(dimension);
      int[] matlabDimension = MWArray.NetDimensionToMATLABDimension(dimensions);
      MWArrayComplexity complexityFlag = imaginaryData != null ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
      lock (MWArray.mxSync)
      {
        Type type = makeDouble ? typeof (double) : realData.GetType().GetElementType();
        MWNumericType elementType = (MWNumericType) MWNumericArray.systemTypeToNumericType[(object) type.Name];
        MWSafeHandle numericArray = MWNumericArray.mxCreateNumericArray(matlabDimension.Length, matlabDimension, elementType, complexityFlag);
        IntPtr data = MWArray.mxGetData(numericArray);
        if (makeDouble)
        {
          if (rowMajorData)
          {
            MWMarshal.MarshalManagedRowMajorToUnmanagedColumnMajor(realData, (double*) data.ToPointer());
            if (imaginaryData != null)
            {
              IntPtr imagData = MWNumericArray.mxGetImagData(numericArray);
              MWMarshal.MarshalManagedRowMajorToUnmanagedColumnMajor(imaginaryData, (double*) imagData.ToPointer());
            }
          }
          else
          {
            MWMarshal.MarshalManagedColumnMajorToUnmanagedColumnMajor(realData, (double*) data.ToPointer());
            if (imaginaryData != null)
            {
              IntPtr imagData = MWNumericArray.mxGetImagData(numericArray);
              MWMarshal.MarshalManagedColumnMajorToUnmanagedColumnMajor(imaginaryData, (double*) imagData.ToPointer());
            }
          }
        }
        else if (rowMajorData)
        {
          MWMarshal.MarshalManagedRowMajorToUnmanagedColumnMajor(realData, data);
          if (imaginaryData != null)
          {
            IntPtr imagData = MWNumericArray.mxGetImagData(numericArray);
            MWMarshal.MarshalManagedRowMajorToUnmanagedColumnMajor(imaginaryData, imagData);
          }
        }
        else
        {
          MWMarshal.MarshalManagedColumnMajorToUnmanagedColumnMajor(realData, data);
          if (imaginaryData != null)
          {
            IntPtr imagData = MWNumericArray.mxGetImagData(numericArray);
            MWMarshal.MarshalManagedColumnMajorToUnmanagedColumnMajor(imaginaryData, imagData);
          }
        }
        this.SetMXArray(numericArray, MWArrayType.Numeric);
      }
    }

    private object CastToNativeScalar()
    {
      switch (this.NumericType)
      {
        case MWNumericType.Double:
          return (object) (double) this;
        case MWNumericType.Single:
          return (object) (float) this;
        case MWNumericType.UInt8:
          return (object) (byte) this;
        case MWNumericType.Int16:
          return (object) (short) this;
        case MWNumericType.Int32:
          return (object) (int) this;
        case MWNumericType.Int64:
          return (object) (long) this;
        default:
          return (object) null;
      }
    }

    internal override object ConvertToType(Type t)
    {
      if (t == typeof (MWArray) || t == typeof (MWNumericArray))
        return (object) this;
      if (this.IsValidConversion(t))
      {
        if (t.IsPrimitive)
          return Convert.ChangeType(this.CastToNativeScalar(), t);
        if (t.IsArray)
        {
          if (!JaggedArray.IsJagged(t))
          {
            Array sourceArray = t.GetArrayRank() != 1 ? this.ToArray() : (!this.IsEmpty ? this.ToVector(MWArrayComponent.Real) : this.ToEmptyVector());
            if (t == MWNumericArray.numericTypeToSystemType[this.NumericType])
              return (object) sourceArray;
            int[] numArray = new int[sourceArray.Rank];
            for (int dimension = 0; dimension < sourceArray.Rank; ++dimension)
              numArray[dimension] = sourceArray.GetUpperBound(dimension) + 1;
            Array instance = Array.CreateInstance(t.GetElementType(), numArray);
            Array.Copy(sourceArray, instance, sourceArray.Length);
            return (object) instance;
          }
          int numberOfElements = MWArray.mxGetNumberOfElements(this.MXArrayHandle);
          IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
          Array instance1 = Array.CreateInstance(JaggedArray.GetElementType(t), numberOfElements);
          Array flatArray = instance1;
          MWMarshal.MarshalUnmanagedToManagedFlatArray(data, flatArray);
          int[] netDimension = MWArray.MATLABDimensionToNetDimension(this.Dimensions);
          return (object) JaggedArray.GetJaggedArrayFromFlatArray(instance1, netDimension);
        }
      }
      else if (this.IsEmpty)
      {
        if (t.IsArray)
        {
          int[] numArray = new int[t.GetArrayRank()];
          for (int index = 0; index < t.GetArrayRank(); ++index)
            numArray[index] = 0;
          return (object) Array.CreateInstance(t.GetElementType(), numArray);
        }
        if (t == typeof (char))
          return (object) char.MinValue;
        if (t == typeof (string))
          return (object) string.Empty;
        return Activator.CreateInstance(t);
      }
      throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDataConversion"), (Exception) new ApplicationException("Cannot convert from MWNumericArray of type: " + MWNumericArray.numericTypeToSystemType[this.NumericType].FullName + MWArray.RankToString(this.NumberofDimensions) + " to " + t.FullName));
    }

    internal override bool IsValidConversion(Type t)
    {
      Type index = MWNumericArray.numericTypeToSystemType[this.NumericType];
      Type[] type = MWArray.typeMap[index];
      if (type == null)
        return false;
      Type elementType = t;
      if (t.IsArray)
      {
        if (!JaggedArray.IsJagged(t))
        {
          int arrayRank = t.GetArrayRank();
          if (arrayRank == this.NumberofDimensions)
          {
            elementType = t.GetElementType();
          }
          else
          {
            if (arrayRank != 1 || !this.IsVector() && !this.IsEmpty)
              return false;
            elementType = t.GetElementType();
          }
        }
        else if (JaggedArray.GetRank(t) == this.NumberofDimensions && JaggedArray.GetElementType(t) == index)
          return true;
      }
      else if (!this.IsScalar() && !this.IsEmpty)
        return false;
      return Array.Exists<Type>(type, (Predicate<Type>) (c => c == elementType));
    }
  }
}
