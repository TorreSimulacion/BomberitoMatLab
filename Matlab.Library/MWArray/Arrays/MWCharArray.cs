// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Arrays.MWCharArray
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using MathWorks.MATLAB.NET.Utility;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace MathWorks.MATLAB.NET.Arrays
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public class MWCharArray : MWArray, IEquatable<MWCharArray>
  {
    private static readonly MWCharArray _Empty = new MWCharArray();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxArrayToString_proxy")]
    private static extern IntPtr mxArrayToString([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxCreateCharArray_700_proxy")]
    private static extern MWSafeHandle mxCreateCharArray(
      [In] int numberOfDimensions,
      [In] int[] dimensions);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetChars_proxy")]
    private static extern IntPtr mxGetChars([In] MWSafeHandle hMXArray);

    public MWCharArray()
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2];
        this.SetMXArray(MWCharArray.mxCreateCharArray(dimensions.Length, dimensions), MWArrayType.Character, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWCharArray(params int[] dimensions)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        this.SetMXArray(MWCharArray.mxCreateCharArray(dimensions.Length, dimensions), MWArrayType.Character, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWCharArray(string value)
    {
      IntPtr num = IntPtr.Zero;
      try
      {
        Monitor.Enter(MWArray.mxSync);
        num = Marshal.StringToHGlobalAnsi(value);
        this.SetMXArray(MWArray.mxCreateString(num), MWArrayType.Character);
      }
      finally
      {
        if (IntPtr.Zero != num)
          Marshal.FreeCoTaskMem(num);
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWCharArray(char value)
    {
      IntPtr num = IntPtr.Zero;
      try
      {
        Monitor.Enter(MWArray.mxSync);
        num = Marshal.StringToHGlobalAnsi(value.ToString());
        this.SetMXArray(MWArray.mxCreateString(num), MWArrayType.Character);
      }
      finally
      {
        if (IntPtr.Zero != num)
          Marshal.FreeCoTaskMem(num);
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWCharArray(char[] value)
    {
      IntPtr num = IntPtr.Zero;
      try
      {
        Monitor.Enter(MWArray.mxSync);
        num = Marshal.StringToHGlobalAnsi(new string(value));
        this.SetMXArray(MWArray.mxCreateString(num), MWArrayType.Character);
      }
      finally
      {
        if (IntPtr.Zero != num)
          Marshal.FreeCoTaskMem(num);
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWCharArray(string[] strings)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions;
        if (strings == null)
        {
          dimensions = new int[1];
        }
        else
        {
          int val2 = 0;
          foreach (string str in strings)
            val2 = Math.Max(str.Length, val2);
          dimensions = new int[2]{ strings.Length, val2 };
        }
        int num = dimensions[0];
        int totalWidth = dimensions[1];
        this.SetMXArray(MWCharArray.mxCreateCharArray(dimensions.Length, dimensions), MWArrayType.Character, dimensions.Length, dimensions);
        int length = num * totalWidth;
        char[] chArray = new char[length];
        for (int index = 0; index < num; ++index)
        {
          string str = strings[index].PadRight(totalWidth);
          for (int sourceIndex = 0; sourceIndex < totalWidth; ++sourceIndex)
            str.CopyTo(sourceIndex, chArray, sourceIndex * num + index, 1);
        }
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        if (!(data != IntPtr.Zero))
          return;
        Marshal.Copy(chArray, 0, data, length);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWCharArray(Array stringArray)
    {
      int rank = stringArray.Rank;
      if (stringArray != null && typeof (char) == stringArray.GetType().GetElementType())
      {
        this.MWCharArrayFromNetCharArray(stringArray);
      }
      else
      {
        if (stringArray == null || !(typeof (string) == stringArray.GetType().GetElementType()))
          throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorArrayStringType"), nameof (stringArray));
        int val2 = 0;
        int[] dimensions = new int[rank + 1];
        IEnumerator enumerator = stringArray.GetEnumerator();
        for (int index = 0; (long) index < stringArray.LongLength; ++index)
        {
          enumerator.MoveNext();
          val2 = Math.Max(((string) enumerator.Current ?? string.Empty).Length, val2);
        }
        int num = dimensions[0] = stringArray.GetLength(rank - 1);
        int totalWidth = dimensions[1] = val2;
        for (int dimension = 0; dimension < rank - 1; ++dimension)
          dimensions[rank - dimension] = stringArray.GetLength(dimension);
        int length = stringArray.Length * val2;
        char[] chArray = new char[length];
        enumerator.Reset();
        for (int index1 = 0; index1 < length; index1 += num * totalWidth)
        {
          for (int index2 = 0; index2 < num; ++index2)
          {
            enumerator.MoveNext();
            string str = ((string) enumerator.Current).PadRight(totalWidth);
            for (int sourceIndex = 0; sourceIndex < totalWidth; ++sourceIndex)
              str.CopyTo(sourceIndex, chArray, sourceIndex * num + index2 + index1, 1);
          }
        }
        try
        {
          Monitor.Enter(MWArray.mxSync);
          this.SetMXArray(MWCharArray.mxCreateCharArray(dimensions.Length, dimensions), MWArrayType.Character, dimensions.Length, dimensions);
          IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
          if (!(data != IntPtr.Zero))
            return;
          Marshal.Copy(chArray, 0, data, length);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    internal MWCharArray(MWSafeHandle hMXArray)
      : base(hMXArray)
    {
      this.array_Type = MWArrayType.Character;
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

    public MWCharArray this[params int[] indices]
    {
      get
      {
        return (MWCharArray) MWArray.GetTypedArray(this.ArrayIndexer((MWArray) this, indices), IntPtr.Zero);
      }
      set
      {
        this.ArrayIndexer((MWArray) value, (MWArray) this, indices);
      }
    }

    public static explicit operator char(MWCharArray scalar)
    {
      scalar.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        IntPtr chars = MWCharArray.mxGetChars(scalar.MXArrayHandle);
        if (chars == IntPtr.Zero)
          return char.MinValue;
        char[] destination = new char[1];
        Marshal.Copy(chars, destination, 0, 1);
        return destination[0];
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public static implicit operator MWCharArray(string value)
    {
      return new MWCharArray(value);
    }

    protected MWCharArray(SerializationInfo serializationInfo, StreamingContext context)
      : base(serializationInfo, context)
    {
    }

    public static MWCharArray Empty
    {
      get
      {
        return (MWCharArray) MWCharArray._Empty.Clone();
      }
    }

    private unsafe void MWCharArrayFromNetCharArray(Array charArray)
    {
      int rank = charArray.Rank;
      int[] dimensions = new int[rank];
      for (int dimension = 0; dimension < rank; ++dimension)
        dimensions[dimension] = charArray.GetLength(dimension);
      int[] matlabDimension = MWArray.NetDimensionToMATLABDimension(dimensions);
      char[] source;
      if (rank == 1)
      {
        source = (char[]) charArray;
      }
      else
      {
        int length = charArray.Length;
        int num1 = matlabDimension[0];
        int num2 = matlabDimension[1];
        int num3 = num1 * num2;
        source = new char[length];
        GCHandle gcHandle1 = GCHandle.Alloc((object) charArray, GCHandleType.Pinned);
        GCHandle gcHandle2 = GCHandle.Alloc((object) source, GCHandleType.Pinned);
        try
        {
          char* chPtr1 = (char*) (void*) gcHandle1.AddrOfPinnedObject();
          char* chPtr2 = (char*) (void*) gcHandle2.AddrOfPinnedObject();
          for (int index1 = 0; index1 < length; index1 += num3)
          {
            for (int index2 = 0; index2 < num1; ++index2)
            {
              for (int index3 = 0; index3 < num2; ++index3)
                chPtr2[index3 * num1 + index2 + index1] = *chPtr1++;
            }
          }
        }
        finally
        {
          gcHandle1.Free();
          gcHandle2.Free();
        }
      }
      lock (MWArray.mxSync)
      {
        this.SetMXArray(MWCharArray.mxCreateCharArray(matlabDimension.Length, matlabDimension), MWArrayType.Character, matlabDimension.Length, matlabDimension);
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        if (!(data != IntPtr.Zero))
          return;
        Marshal.Copy(source, 0, data, charArray.Length);
      }
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
        IntPtr data = MWArray.mxGetData(this.MXArrayHandle);
        Array instance = Array.CreateInstance(typeof (char), dimensions2);
        char[] destination = new char[numberOfElements];
        if (IntPtr.Zero != data)
          Marshal.Copy(data, destination, 0, numberOfElements);
        for (int index1 = 0; index1 < numberOfElements; index1 += num1 * num2)
        {
          for (int index2 = 0; index2 < num1; ++index2)
          {
            for (int index3 = 0; index3 < num2; ++index3)
            {
              char ch = destination[index3 * num1 + index2 + index1];
              instance.SetValue((object) ch, subscripts);
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

    internal string CharArrayToString(Array src)
    {
      if (src.Rank == 1)
        return new string((char[]) src);
      if (src.Rank == 2 && src.GetUpperBound(0) == 0)
      {
        char[] chArray = new char[src.Length];
        return new string(MWArray.ConvertMatrixToVector<char>((char[,]) src));
      }
      if (src.GetLength(0) == 0)
        return string.Empty;
      throw new ArgumentException("Not a valid conversion");
    }

    internal unsafe Array CharArrayToStringArray(Array src)
    {
      int dimension1 = src.Rank - 1;
      int[] dimensions = new int[dimension1];
      int[] subscripts = new int[dimension1];
      int length1 = src.GetLength(dimension1);
      for (int dimension2 = 0; dimension2 < dimension1; ++dimension2)
        dimensions[dimension2] = src.GetLength(dimension2);
      Array instance = Array.CreateInstance(typeof (string), dimensions);
      GCHandle gcHandle = GCHandle.Alloc((object) src, GCHandleType.Pinned);
      try
      {
        char* chPtr = (char*) (void*) gcHandle.AddrOfPinnedObject();
        int length2 = src.Length;
        for (int startIndex = 0; startIndex < length2; startIndex += length1)
        {
          string str = new string(chPtr, startIndex, length1);
          instance.SetValue((object) str, subscripts);
          subscripts = this.GetNextSubscript(dimensions, subscripts, 0);
        }
      }
      finally
      {
        gcHandle.Free();
      }
      return instance;
    }

    internal override object ConvertToType(Type t)
    {
      if (t == typeof (MWArray) || t == typeof (MWCharArray))
        return (object) this;
      Array array = this.ToArray();
      if (t == typeof (string))
        return (object) this.CharArrayToString(array);
      if (t.IsArray && t.GetElementType() == typeof (string) && t.GetArrayRank() == this.NumberofDimensions - 1)
        return (object) this.CharArrayToStringArray(array);
      if (t == typeof (char) && (this.IsScalar() || this.IsEmpty))
        return (object) (char) this;
      if (t.IsArray && t.GetArrayRank() == 1 && (this.IsVector() || this.IsEmpty))
        return (object) MWArray.ConvertMatrixToVector<char>((char[,]) array);
      if (t.IsArray && t.GetArrayRank() == this.NumberofDimensions)
        return (object) array;
      throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDataConversion"), (Exception) new ApplicationException("Cannot convert from MWCharArray" + MWArray.RankToString(this.NumberofDimensions) + " to " + t.FullName));
    }

    public override string ToString()
    {
      if (this.IsEmpty)
        return "";
      return base.ToString();
    }

    public override object Clone()
    {
      MWCharArray mwCharArray = (MWCharArray) this.MemberwiseClone();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        mwCharArray.SetMXArray(MWArray.mxDuplicateArray(this.MXArrayHandle), MWArrayType.Character);
        return (object) mwCharArray;
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

    public override bool Equals(object obj)
    {
      if (obj is MWCharArray)
        return this.Equals(obj as MWCharArray);
      return false;
    }

    private bool Equals(MWCharArray other)
    {
      return base.Equals((object) other);
    }

    bool IEquatable<MWCharArray>.Equals(MWCharArray other)
    {
      return this.Equals(other);
    }
  }
}
