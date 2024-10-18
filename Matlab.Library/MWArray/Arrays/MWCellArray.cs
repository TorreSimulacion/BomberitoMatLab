// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Arrays.MWCellArray
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using MathWorks.MATLAB.NET.Utility;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading;

namespace MathWorks.MATLAB.NET.Arrays
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public class MWCellArray : MWArray, IEquatable<MWCellArray>
  {
    private static readonly MWCellArray _Empty = new MWCellArray();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxCalcSingleSubscript_700_proxy")]
    private static extern int mxCalcSingleSubscript(
      [In] MWSafeHandle hMXArray,
      [In] int numSubscripts,
      [In] int[] subscripts);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxCreateCellArray_700_proxy")]
    private static extern MWSafeHandle mxCreateCellArray(
      [In] int numDimensions,
      [In] int[] dimensions);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetCell_700_proxy")]
    private static extern MWSafeHandle mxGetCell([In] MWSafeHandle hMXArray, [In] int index);

    public MWCellArray()
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2];
        this.SetMXArray(MWCellArray.mxCreateCellArray(dimensions.Length, dimensions), MWArrayType.Cell, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWCellArray(int rows, int columns)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ rows, columns };
        this.SetMXArray(MWCellArray.mxCreateCellArray(dimensions.Length, dimensions), MWArrayType.Cell, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWCellArray(params int[] dimensions)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        this.SetMXArray(MWCellArray.mxCreateCellArray(dimensions.Length, dimensions), MWArrayType.Cell, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWCellArray(MWNumericArray dimensions)
    {
      double[] vector = (double[]) dimensions.ToVector(MWArrayComponent.Real);
      int[] dimensions1 = new int[vector.Length];
      for (int index = 0; index < vector.Length; ++index)
        dimensions1[index] = checked ((int) vector[index]);
      try
      {
        Monitor.Enter(MWArray.mxSync);
        this.SetMXArray(MWCellArray.mxCreateCellArray(dimensions1.Length, dimensions1), MWArrayType.Cell, dimensions1.Length, dimensions1);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWCellArray(MWCharArray strings)
    {
      if (2 != strings.NumberofDimensions)
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorNotAMatrix"), nameof (strings));
      int[] dimensions1 = strings.Dimensions;
      int num1 = dimensions1[0];
      int num2 = dimensions1[1];
      try
      {
        Monitor.Enter(MWArray.mxSync);
        StringBuilder stringBuilder = new StringBuilder(num2);
        int[] dimensions2 = new int[2]{ num1, 1 };
        this.SetMXArray(MWCellArray.mxCreateCellArray(dimensions2.Length, dimensions2), MWArrayType.Cell);
        for (int index1 = 1; index1 <= num1; ++index1)
        {
          for (int index2 = 1; index2 <= num2; ++index2)
            stringBuilder.Insert(index2 - 1, (char) strings[new int[2]
            {
              index1,
              index2
            }]);
          this[new int[2]{ index1, 1 }] = (MWArray) new MWCharArray(stringBuilder.ToString());
          stringBuilder.Remove(0, num2);
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    internal MWCellArray(MWSafeHandle hMXArray)
      : base(hMXArray)
    {
      this.array_Type = MWArrayType.Cell;
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

    public new MWArray this[params int[] indices]
    {
      get
      {
        try
        {
          Monitor.Enter(MWArray.mxSync);
          MWSafeHandle hMXArray = this.ArrayIndexer((MWArray) this, indices);
          if (!hMXArray.IsInvalid)
            return MWArray.GetTypedArray(hMXArray, IntPtr.Zero);
          if (1 == indices.Length)
          {
            if (indices[0] < 0 || indices[0] > MWArray.mxGetNumberOfElements(this.MXArrayHandle))
              throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDimensions"));
          }
          else
          {
            int[] dimensions = this.Dimensions;
            if (dimensions.Length != indices.Length)
              throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDimensions"));
            for (int index = 0; index < indices.Length; ++index)
            {
              if (indices[index] > dimensions[index])
                throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDimensions"));
            }
          }
          return (MWArray) MWNumericArray.Empty;
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
      set
      {
        this.ArrayIndexer(value, (MWArray) this, indices);
      }
    }

    protected MWCellArray(SerializationInfo serializationInfo, StreamingContext context)
      : base(serializationInfo, context)
    {
    }

    public static MWCellArray Empty
    {
      get
      {
        return (MWCellArray) MWCellArray._Empty.Clone();
      }
    }

    public override string ToString()
    {
      return base.ToString();
    }

    public override object Clone()
    {
      MWCellArray mwCellArray = (MWCellArray) this.MemberwiseClone();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        mwCellArray.SetMXArray(MWArray.mxDuplicateArray(this.MXArrayHandle), MWArrayType.Cell);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
      return (object) mwCellArray;
    }

    public override bool Equals(object obj)
    {
      if (obj is MWCellArray)
        return this.Equals(obj as MWCellArray);
      return false;
    }

    private bool Equals(MWCellArray other)
    {
      return base.Equals((object) other);
    }

    bool IEquatable<MWCellArray>.Equals(MWCellArray other)
    {
      return this.Equals(other);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override Array ToArray()
    {
      this.CheckDisposed();
      int[] dimensions1 = this.Dimensions;
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int numberOfElements = this.NumberOfElements;
        int length = dimensions1.Length;
        int[] dimensions2 = new int[length];
        int[] subscripts = new int[length];
        int num1 = dimensions1[0];
        int num2 = dimensions2[length - 1] = dimensions1[1];
        dimensions2[length - 2] = num1;
        for (int index = 2; index < length; ++index)
          dimensions2[length - (index + 1)] = dimensions1[index];
        Array instance = Array.CreateInstance(typeof (object), dimensions2);
        for (int index1 = 0; index1 < numberOfElements; index1 += num1 * num2)
        {
          for (int index2 = 0; index2 < num1; ++index2)
          {
            for (int index3 = 0; index3 < num2; ++index3)
            {
              MWArray mwArray = this[new int[1]
              {
                index3 * num1 + index2 + index1 + 1
              }];
              instance.SetValue((object) mwArray.ToArray(), subscripts);
              subscripts = this.GetNextSubscript(dimensions2, subscripts, 0);
            }
          }
        }
        return instance;
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    internal override object ConvertToType(Type t)
    {
      if (t == typeof (MWArray) || t == typeof (MWCellArray))
        return (object) this;
      if (t == typeof (ArrayList) && this.IsVector())
      {
        ArrayList arrayList = new ArrayList();
        int numberOfElements = this.NumberOfElements;
        for (int index = 1; index <= numberOfElements; ++index)
        {
          MWArray mwArray = this[new int[1]{ index }];
          if (mwArray.IsStructArray)
            arrayList.Add((object) mwArray);
          else if (mwArray.IsCellArray)
            arrayList.Add(this.ConvertToType(typeof (Array)));
          else
            arrayList.Add((object) mwArray.ToArray());
        }
        return (object) arrayList;
      }
      if (!(t == typeof (Array)))
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDataConversion"), (Exception) new ApplicationException("Cannot convert from MWCellArray to " + t.FullName));
      int[] nativeArrayDims;
      Array array = this.AllocateNativeArray(typeof (object), out nativeArrayDims);
      int numberOfElements1 = this.NumberOfElements;
      int[] subscripts = new int[this.NumberofDimensions];
      int dimension1 = this.Dimensions[0];
      int dimension2 = this.Dimensions[1];
      for (int index1 = 0; index1 < numberOfElements1; index1 += dimension1 * dimension2)
      {
        for (int index2 = 0; index2 < dimension1; ++index2)
        {
          for (int index3 = 0; index3 < dimension2; ++index3)
          {
            MWArray mwArray = this[new int[1]
            {
              index3 * dimension1 + index2 + index1 + 1
            }];
            if (!mwArray.IsStructArray)
            {
              if (mwArray.IsCellArray)
                this.ConvertToType(typeof (Array));
              else
                mwArray.ToArray();
            }
            array.SetValue((object) mwArray, subscripts);
            subscripts = this.GetNextSubscript(nativeArrayDims, subscripts, 0);
          }
        }
      }
      return (object) array;
    }
  }
}
