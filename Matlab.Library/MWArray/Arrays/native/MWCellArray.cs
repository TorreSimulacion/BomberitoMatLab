// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Arrays.native.MWCellArray
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System;
using System.Runtime.InteropServices;

namespace MathWorks.MATLAB.NET.Arrays.native
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public class MWCellArray : MWArray, ICloneable, IEquatable<MWCellArray>
  {
    private static readonly MWCellArray _Empty = new MWCellArray();

    public MWCellArray()
    {
      this.array_Type = MWArrayType.Cell;
    }

    public MWCellArray(int rows, int columns)
      : base(rows, columns)
    {
      this.array_Type = MWArrayType.Cell;
    }

    public MWCellArray(params int[] dimensions)
      : base(dimensions)
    {
      this.array_Type = MWArrayType.Cell;
    }

    public object this[params int[] indices]
    {
      get
      {
        return this.get(indices);
      }
      set
      {
        this.set(indices, value);
      }
    }

    public static MWCellArray Empty
    {
      get
      {
        return (MWCellArray) MWCellArray._Empty.Clone();
      }
    }

    public override object Clone()
    {
      MWCellArray mwCellArray = (MWCellArray) this.MemberwiseClone();
      this.deepCopy((MWArray) mwCellArray);
      return (object) mwCellArray;
    }

    public override bool Equals(object obj)
    {
      if (obj is MWCellArray)
        return this.Equals(obj as MWCellArray);
      return false;
    }

    public bool Equals(MWCellArray other)
    {
      return base.Equals((object) other);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }
}
