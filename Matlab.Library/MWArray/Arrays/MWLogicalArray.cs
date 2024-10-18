// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Arrays.MWLogicalArray
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using MathWorks.MATLAB.NET.Utility;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace MathWorks.MATLAB.NET.Arrays
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public class MWLogicalArray : MWIndexArray, IEquatable<MWLogicalArray>
  {
    private static readonly MWLogicalArray _Empty = new MWLogicalArray();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxCreateLogicalArray_700_proxy")]
    private static extern MWSafeHandle mxCreateLogicalArray(
      [In] int numberOfDimensions,
      [In] int[] dimensions);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxCreateLogicalScalar_proxy")]
    internal static extern MWSafeHandle mxCreateLogicalScalar([In] byte logical);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxCreateLogicalMatrix_700_proxy")]
    internal static extern MWSafeHandle mxCreateLogicalMatrix([In] int numRows, [In] int numCols);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsLogicalScalarTrue_proxy")]
    internal static extern byte mxIsLogicalScalarTrue([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclGetLogicalSparse_proxy")]
    internal static extern int mclGetLogicalSparse(
      out IntPtr pMXArray,
      [In] IntPtr rowIndexSize,
      [In] IntPtr[] rowindex,
      [In] IntPtr colIndexSize,
      [In] IntPtr[] columnindex,
      [In] IntPtr dataSize,
      [In] byte[] logicalData,
      [In] IntPtr rows,
      [In] IntPtr columns,
      [In] IntPtr nonZeroMax);

    public MWLogicalArray()
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2];
        this.SetMXArray(MWLogicalArray.mxCreateLogicalMatrix(0, 0), MWArrayType.Logical, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWLogicalArray(int row, int column)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ row, column };
        this.SetMXArray(MWLogicalArray.mxCreateLogicalMatrix(row, column), MWArrayType.Logical, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWLogicalArray(params int[] dimensions)
    {
      if (dimensions == null)
        throw new ArgumentNullException(nameof (dimensions));
      try
      {
        Monitor.Enter(MWArray.mxSync);
        this.SetMXArray(MWLogicalArray.mxCreateLogicalArray(dimensions.Length, dimensions), MWArrayType.Logical, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWLogicalArray(bool logicalValue)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ 1, 1 };
        this.SetMXArray(logicalValue ? MWLogicalArray.mxCreateLogicalScalar((byte) 1) : MWLogicalArray.mxCreateLogicalScalar((byte) 0), MWArrayType.Logical, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWLogicalArray(int rows, int columns, bool[] boolArray)
    {
      int[] numArray = new int[2]{ rows, columns };
      this.CreateLogicalArray(rows, columns, boolArray, false);
    }

    public MWLogicalArray(int rows, int columns, bool[] boolArray, bool columnMajorOrder)
    {
      int[] numArray = new int[2]{ rows, columns };
      this.CreateLogicalArray(rows, columns, boolArray, columnMajorOrder);
    }

    public MWLogicalArray(Array boolArray)
    {
      if (boolArray != null)
      {
        if (!(typeof (bool) != boolArray.GetType().GetElementType()))
        {
          try
          {
            int rank = boolArray.Rank;
            int[] dimensions = new int[Math.Max(rank, 2)];
            int length = boolArray.Length;
            int num1 = dimensions[0] = 1 < rank ? boolArray.GetLength(rank - 2) : 1;
            int num2 = dimensions[1] = boolArray.GetLength(rank - 1);
            for (int dimension = 0; dimension < rank - 2; ++dimension)
              dimensions[rank - dimension - 1] = boolArray.GetLength(dimension);
            byte[] source = new byte[length];
            IEnumerator enumerator = boolArray.GetEnumerator();
            for (int index1 = 0; index1 < length; index1 += num1 * num2)
            {
              for (int index2 = 0; index2 < num1; ++index2)
              {
                for (int index3 = 0; index3 < num2; ++index3)
                {
                  enumerator.MoveNext();
                  source[index3 * num1 + index2 + index1] = (bool) enumerator.Current ? (byte) 1 : (byte) 0;
                }
              }
            }
            Monitor.Enter(MWArray.mxSync);
            this.SetMXArray(MWLogicalArray.mxCreateLogicalArray(dimensions.Length, dimensions), MWArrayType.Logical, dimensions.Length, dimensions);
            IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
            Marshal.Copy(source, 0, data, length);
            return;
          }
          finally
          {
            Monitor.Exit(MWArray.mxSync);
          }
        }
      }
      throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorDataArrayType"), nameof (boolArray));
    }

    internal MWLogicalArray(MWSafeHandle hMXArray)
      : base(hMXArray)
    {
      this.array_Type = MWArrayType.Logical;
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

    public MWLogicalArray this[params int[] indices]
    {
      get
      {
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (MWLogicalArray) MWArray.GetTypedArray(this.ArrayIndexer((MWArray) this, indices), IntPtr.Zero);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
      set
      {
        if (MWArrayType.Logical != value.array_Type)
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

    public static implicit operator bool(MWLogicalArray logicalArray)
    {
      logicalArray.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        return (byte) 1 == MWLogicalArray.mxIsLogicalScalarTrue(logicalArray.MXArrayHandle);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public static implicit operator MWLogicalArray(bool scalar)
    {
      return new MWLogicalArray(scalar);
    }

    protected MWLogicalArray(SerializationInfo serializationInfo, StreamingContext context)
      : base(serializationInfo, context)
    {
    }

    public static MWLogicalArray Empty
    {
      get
      {
        return (MWLogicalArray) MWLogicalArray._Empty.Clone();
      }
    }

    public bool IsScalar
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWLogicalArray.mxIsLogicalScalarTrue(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public static MWLogicalArray MakeSparse(int rows, int columns, int nonZeroMax)
    {
      return MWLogicalArray.MakeSparse(rows, columns, new int[1]
      {
        1
      }, new int[1]{ 1 }, new bool[1], nonZeroMax);
    }

    public static MWLogicalArray MakeSparse(
      int[] rowIndex,
      int[] columnIndex,
      bool[] dataArray)
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
        return MWLogicalArray.MakeSparse(num1, num2, rowIndex, columnIndex, dataArray);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public static MWLogicalArray MakeSparse(
      int rows,
      int columns,
      int[] rowIndex,
      int[] columnIndex,
      bool[] dataArray)
    {
      return MWLogicalArray.MakeSparse(rows, columns, rowIndex, columnIndex, dataArray, dataArray.Length);
    }

    public static MWLogicalArray MakeSparse(
      int rows,
      int columns,
      int[] rowIndex,
      int[] columnIndex,
      bool[] dataArray,
      int nonZeroMax)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        IntPtr pMXArray = IntPtr.Zero;
        int length = 0;
        byte[] logicalData = (byte[]) null;
        if (dataArray != null)
        {
          length = dataArray.Length;
          logicalData = new byte[length];
        }
        IntPtr[] rowindex = new IntPtr[rowIndex.Length];
        IntPtr[] columnindex = new IntPtr[columnIndex.Length];
        for (int index = 0; index < rowIndex.Length; ++index)
        {
          rowindex[index] = (IntPtr) rowIndex[index];
          columnindex[index] = (IntPtr) columnIndex[index];
        }
        for (int index = 0; index < length; ++index)
          logicalData[index] = dataArray[index] ? (byte) 1 : (byte) 0;
        if (MWLogicalArray.mclGetLogicalSparse(out pMXArray, (IntPtr) rowIndex.Length, rowindex, (IntPtr) columnIndex.Length, columnindex, (IntPtr) dataArray.Length, logicalData, (IntPtr) rows, (IntPtr) columns, (IntPtr) nonZeroMax) != 0)
          return (MWLogicalArray) null;
        MWSafeHandle hMXArray;
        if (MWArray.mclArrayHandle2mxArray(out hMXArray, pMXArray) != 0)
          throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidArray"));
        return new MWLogicalArray(hMXArray);
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
      if (obj is MWLogicalArray)
        return this.Equals(obj as MWLogicalArray);
      return false;
    }

    private bool Equals(MWLogicalArray other)
    {
      return base.Equals((object) other);
    }

    bool IEquatable<MWLogicalArray>.Equals(MWLogicalArray other)
    {
      return this.Equals(other);
    }

    public override object Clone()
    {
      MWLogicalArray mwLogicalArray = (MWLogicalArray) this.MemberwiseClone();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        mwLogicalArray.SetMXArray(MWArray.mxDuplicateArray(this.MXArrayHandle), MWArrayType.Logical);
        return (object) mwLogicalArray;
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

    public bool[] ToVector()
    {
      this.CheckDisposed();
      IntPtr source = IntPtr.Zero;
      double numberOfElements = (double) this.NumberOfElements;
      bool[] flagArray = new bool[checked ((int) numberOfElements)];
      byte[] destination = new byte[checked ((int) numberOfElements)];
      try
      {
        Monitor.Enter(MWArray.mxSync);
        source = MWArray.mxGetData(this.MXArrayHandle);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
      Marshal.Copy(source, destination, 0, checked ((int) numberOfElements));
      for (int index = 0; index < destination.Length; ++index)
        flagArray[index] = destination[index] != (byte) 0;
      return flagArray;
    }

    public override Array ToArray()
    {
      this.CheckDisposed();
      int[] dimensions1 = this.Dimensions;
      IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        if (MWArray.mxIsSparse(this.MXArrayHandle) != (byte) 0)
        {
          int num1 = 0;
          int num2 = 0;
          MWSafeHandle hMXArrayRows;
          MWSafeHandle hMXArrayCols;
          if (MWIndexArray.mclMXArrayGetIndexArrays(out hMXArrayRows, out hMXArrayCols, this.MXArrayHandle) != 0)
            throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidIndex"));
          MWNumericArray mwNumericArray1 = new MWNumericArray(hMXArrayRows);
          MWNumericArray mwNumericArray2 = new MWNumericArray(hMXArrayCols);
          int numberOfElements = mwNumericArray1.NumberOfElements;
          int[,] numArray1;
          int[,] numArray2;
          if (MWNumericType.UInt64 != mwNumericArray1.NumericType)
          {
            numArray1 = (int[,]) mwNumericArray1.ToArray(MWArrayComponent.Real, true);
            numArray2 = (int[,]) mwNumericArray2.ToArray(MWArrayComponent.Real, true);
            for (int index = 0; index < numberOfElements; ++index)
            {
              num1 = Math.Max(num1, numArray1[0, index]);
              num2 = Math.Max(num2, numArray2[0, index]);
            }
          }
          else
          {
            long[,] array1 = (long[,]) mwNumericArray1.ToArray(MWArrayComponent.Real, true);
            long[,] array2 = (long[,]) mwNumericArray2.ToArray(MWArrayComponent.Real, true);
            numArray1 = new int[array1.GetLength(0), array1.GetLength(1)];
            numArray2 = new int[array2.GetLength(0), array2.GetLength(1)];
            for (int index = 0; index < numberOfElements; ++index)
            {
              num1 = Math.Max(num1, numArray1[0, index] = checked ((int) array1[0, index]));
              num2 = Math.Max(num2, numArray2[0, index] = checked ((int) array2[0, index]));
            }
          }
          byte[] destination = new byte[num1 * num2];
          if (IntPtr.Zero != data)
            Marshal.Copy(data, destination, 0, numberOfElements);
          Array instance = Array.CreateInstance(typeof (bool), num1, num2);
          for (int index = 0; index < numberOfElements; ++index)
          {
            bool flag = (byte) 1 == destination[index];
            instance.SetValue((object) flag, numArray1[0, index] - 1, numArray2[0, index] - 1);
          }
          return instance;
        }
        int numberOfElements1 = this.NumberOfElements;
        int length = dimensions1.Length;
        int[] dimensions2 = new int[length];
        int[] subscripts = new int[length];
        int num3 = dimensions1[0];
        int num4 = dimensions2[length - 1] = dimensions1[1];
        dimensions2[length - 2] = num3;
        for (int index = 2; index < length; ++index)
          dimensions2[length - (index + 1)] = dimensions1[index];
        Array instance1 = Array.CreateInstance(typeof (bool), dimensions2);
        byte[] destination1 = new byte[numberOfElements1];
        if (IntPtr.Zero != data)
          Marshal.Copy(data, destination1, 0, numberOfElements1);
        for (int index1 = 0; index1 < numberOfElements1; index1 += num3 * num4)
        {
          for (int index2 = 0; index2 < num3; ++index2)
          {
            for (int index3 = 0; index3 < num4; ++index3)
            {
              bool flag = (byte) 1 == destination1[index3 * num3 + index2 + index1];
              instance1.SetValue((object) flag, subscripts);
              subscripts = this.GetNextSubscript(dimensions2, subscripts, 0);
            }
          }
        }
        return instance1;
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    private void CreateLogicalArray(int rows, int columns, bool[] boolArray, bool columnMajorData)
    {
      try
      {
        int length = rows * columns;
        if (boolArray == null || length != boolArray.Length)
          throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorDataArraySize"), nameof (boolArray));
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ rows, columns };
        this.SetMXArray(MWLogicalArray.mxCreateLogicalArray(dimensions.Length, dimensions), MWArrayType.Logical, dimensions.Length, dimensions);
        byte[] source = new byte[length];
        if (columnMajorData)
        {
          for (int index = 0; index < length; ++index)
            source[index] = boolArray[index] ? (byte) 1 : (byte) 0;
        }
        else
        {
          for (int index1 = 0; index1 < rows; ++index1)
          {
            for (int index2 = 0; index2 < columns; ++index2)
              source[index2 * rows + index1] = boolArray[index1 * columns + index2] ? (byte) 1 : (byte) 0;
          }
        }
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        Marshal.Copy(source, 0, data, length);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    internal override object ConvertToType(Type t)
    {
      if (t == typeof (MWArray) || t == typeof (MWLogicalArray))
        return (object) this;
      if (t == typeof (bool) && this.IsScalar())
        return (object) (bool) this;
      if (t.IsArray && t.GetArrayRank() == this.NumberofDimensions)
        return (object) this.ToArray();
      if (t.IsArray && t.GetArrayRank() == 1 && this.IsVector())
        return (object) this.ToVector();
      throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDataConversion"), (Exception) new ApplicationException("Cannot convert from MWLogicalArray" + MWArray.RankToString(this.NumberofDimensions) + " to " + t.FullName));
    }
  }
}
