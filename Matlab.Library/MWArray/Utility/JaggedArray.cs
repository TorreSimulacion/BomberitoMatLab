// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.JaggedArray
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Resources;

namespace MathWorks.MATLAB.NET.Utility
{
  internal class JaggedArray
  {
    internal static ResourceManager resourceManager = MWResources.getResourceManager();
    private Array array_;

    public JaggedArray(Array array)
    {
      if (!JaggedArray.IsJagged(array) || !JaggedArray.IsRectangular(array))
        throw new ArgumentException(JaggedArray.resourceManager.GetString("MWErrorNotRectangularJaggedArray"));
      this.array_ = array;
    }

    private static bool IsRectangular(Array array)
    {
      return JaggedArray.IsRectangular(array, JaggedArray.GetRank(array));
    }

    private static bool IsRectangular(Array array, int rank)
    {
      if (rank <= 1)
        return true;
      int length1 = array.Length;
      int length2 = ((Array) array.GetValue(0)).Length;
      for (int index = 1; index < length1; ++index)
      {
        if (((Array) array.GetValue(index)).Length != length2)
          return false;
      }
      for (int index = 0; index < length1; ++index)
      {
        if (!JaggedArray.IsRectangular((Array) array.GetValue(index), rank - 1))
          return false;
      }
      return true;
    }

    public int GetRank()
    {
      return JaggedArray.GetRank(this.array_);
    }

    private static int GetRank(Array array)
    {
      if (!array.GetType().GetElementType().IsArray)
        return 1;
      return 1 + JaggedArray.GetRank((Array) array.GetValue(0));
    }

    public static bool IsJagged(Array array)
    {
      return array.GetType().GetElementType().IsArray;
    }

    public static int GetRank(Type t)
    {
      if (!t.IsArray)
        return 0;
      return 1 + JaggedArray.GetRank(t.GetElementType());
    }

    public static bool IsJagged(Type t)
    {
      return t.GetElementType().IsArray;
    }

    public static Type GetElementType(Type t)
    {
      int rank = JaggedArray.GetRank(t);
      for (int index = 0; index < rank; ++index)
        t = t.GetElementType();
      return t;
    }

    private int GetDimension(int dim)
    {
      if (dim < 0 || dim >= this.GetRank())
        throw new IndexOutOfRangeException("Dimension:" + (object) dim + " does not exist in array.");
      Array array = this.array_;
      for (int index = 0; index < dim; ++index)
        array = (Array) array.GetValue(0);
      return array.Length;
    }

    public int[] GetDimensions()
    {
      int rank = this.GetRank();
      int[] numArray = new int[rank];
      for (int dim = 0; dim < rank; ++dim)
        numArray[dim] = this.GetDimension(dim);
      return numArray;
    }

    public Array GetFlatArray()
    {
      int rank = this.GetRank();
      this.GetElementType();
      if (rank == 2)
        return this.GetFlatArrayFrom2D(this);
      if (rank == 3)
        return this.GetFlatArrayFrom3D(this);
      return this.GetFlatArrayFromND(this);
    }

    private Array GetFlatArrayFromND(JaggedArray src)
    {
      if (src.GetRank() == 3)
        return this.GetFlatArrayFrom3D(src);
      int dimension = src.GetDimension(0);
      Array instance = Array.CreateInstance(src.GetElementType().MakeArrayType(), src.ArrayData.Length);
      for (int index = 0; index < dimension; ++index)
        instance.SetValue((object) this.GetFlatArrayFromND(new JaggedArray((Array) src.ArrayData.GetValue(index))), index);
      return JaggedArray.Stitch1DArrays(instance);
    }

    private Array GetFlatArrayFrom2D(JaggedArray src)
    {
      int[] dimensions = new int[2]
      {
        src.GetDimension(0),
        src.GetDimension(1)
      };
      Type elementType = src.GetElementType();
      if (elementType == typeof (byte))
        return (Array) this.Flatten2D<byte>((byte[][]) src.ArrayData, dimensions);
      if (elementType == typeof (short))
        return (Array) this.Flatten2D<short>((short[][]) src.ArrayData, dimensions);
      if (elementType == typeof (int))
        return (Array) this.Flatten2D<int>((int[][]) src.ArrayData, dimensions);
      if (elementType == typeof (long))
        return (Array) this.Flatten2D<long>((long[][]) src.ArrayData, dimensions);
      if (elementType == typeof (float))
        return (Array) this.Flatten2D<float>((float[][]) src.ArrayData, dimensions);
      if (elementType == typeof (double))
        return (Array) this.Flatten2D<double>((double[][]) src.ArrayData, dimensions);
      if (elementType == typeof (char))
        return (Array) this.Flatten2D<char>((char[][]) src.ArrayData, dimensions);
      if (elementType == typeof (bool))
        return (Array) this.Flatten2D<bool>((bool[][]) src.ArrayData, dimensions);
      throw new ArgumentException(JaggedArray.resourceManager.GetString("MWErrorUnsupportedArrayType") + elementType.FullName);
    }

    private Array GetFlatArrayFrom3D(JaggedArray src)
    {
      int[] dimensions = new int[3]
      {
        src.GetDimension(0),
        src.GetDimension(1),
        src.GetDimension(2)
      };
      Type elementType = src.GetElementType();
      if (elementType == typeof (byte))
        return (Array) this.Flatten3D<byte>((byte[][][]) src.ArrayData, dimensions);
      if (elementType == typeof (short))
        return (Array) this.Flatten3D<short>((short[][][]) src.ArrayData, dimensions);
      if (elementType == typeof (int))
        return (Array) this.Flatten3D<int>((int[][][]) src.ArrayData, dimensions);
      if (elementType == typeof (long))
        return (Array) this.Flatten3D<long>((long[][][]) src.ArrayData, dimensions);
      if (elementType == typeof (float))
        return (Array) this.Flatten3D<float>((float[][][]) src.ArrayData, dimensions);
      if (elementType == typeof (double))
        return (Array) this.Flatten3D<double>((double[][][]) src.ArrayData, dimensions);
      if (elementType == typeof (char))
        return (Array) this.Flatten3D<char>((char[][][]) src.ArrayData, dimensions);
      if (elementType == typeof (bool))
        return (Array) this.Flatten3D<bool>((bool[][][]) src.ArrayData, dimensions);
      throw new ArgumentException(JaggedArray.resourceManager.GetString("MWErrorUnsupportedArrayType") + elementType.FullName);
    }

    private T[] Flatten2D<T>(T[][] src, int[] dimensions)
    {
      T[] objArray = new T[dimensions[0] * dimensions[1]];
      int num = 0;
      for (int index1 = 0; index1 < dimensions[1]; ++index1)
      {
        for (int index2 = 0; index2 < dimensions[0]; ++index2)
          objArray[num++] = src[index2][index1];
      }
      return objArray;
    }

    private T[] Flatten3D<T>(T[][][] src, int[] dimensions)
    {
      T[] objArray = new T[dimensions[0] * dimensions[1] * dimensions[2]];
      int num = 0;
      for (int index1 = 0; index1 < dimensions[0]; ++index1)
      {
        for (int index2 = 0; index2 < dimensions[2]; ++index2)
        {
          for (int index3 = 0; index3 < dimensions[1]; ++index3)
            objArray[num++] = src[index1][index3][index2];
        }
      }
      return objArray;
    }

    private static Array Stitch1DArrays(Array src)
    {
      Type type = src.GetType();
      if (type == typeof (byte[][]))
        return (Array) JaggedArray.Stitch<byte>((byte[][]) src);
      if (type == typeof (short[][]))
        return (Array) JaggedArray.Stitch<byte>((byte[][]) src);
      if (type == typeof (int[][]))
        return (Array) JaggedArray.Stitch<int>((int[][]) src);
      if (type == typeof (long[][]))
        return (Array) JaggedArray.Stitch<long>((long[][]) src);
      if (type == typeof (float))
        return (Array) JaggedArray.Stitch<float>((float[][]) src);
      if (type == typeof (double))
        return (Array) JaggedArray.Stitch<double>((double[][]) src);
      if (type == typeof (char))
        return (Array) JaggedArray.Stitch<char>((char[][]) src);
      if (type == typeof (bool))
        return (Array) JaggedArray.Stitch<bool>((bool[][]) src);
      throw new ArgumentException(JaggedArray.resourceManager.GetString("MWErrorUnsupportedArrayType") + type.FullName);
    }

    private static T[] Stitch<T>(T[][] src)
    {
      int length = 0;
      foreach (T[] objArray in src)
        length += objArray.Length;
      T[] objArray1 = new T[length];
      int index = 0;
      foreach (T[] objArray2 in src)
      {
        objArray2.CopyTo((Array) objArray1, index);
        index += objArray2.Length;
      }
      return objArray1;
    }

    public Type GetElementType()
    {
      Type type = this.array_.GetType();
      int rank = this.GetRank();
      for (int index = 0; index < rank; ++index)
        type = type.GetElementType();
      return type;
    }

    public static Array GetJaggedArrayFromFlatArray(Array flatArray, int[] dimensions)
    {
      switch (dimensions.Length)
      {
        case 2:
          return JaggedArray.Get2DJaggedArray(flatArray, dimensions[0], dimensions[1]);
        case 3:
          return JaggedArray.Get3DJaggedArray(flatArray, dimensions[1], dimensions[2], dimensions[0], 0);
        default:
          return JaggedArray.GetNDJaggedArray(flatArray, dimensions, 0, 0, dimensions[1] * dimensions[2] * dimensions[0]);
      }
    }

    private static Array GetNDJaggedArray(
      Array flatArray,
      int[] dimensions,
      int currentDim,
      int startingOffset,
      int pageStride)
    {
      int rank = dimensions.Length - currentDim;
      if (rank == 3)
        return JaggedArray.Get3DJaggedArray(flatArray, dimensions[currentDim + 1], dimensions[currentDim + 2], dimensions[currentDim], startingOffset);
      Type elementType = JaggedArray.MakeJaggedArrayType(flatArray.GetType().GetElementType(), rank);
      int dimension = dimensions[currentDim];
      int length = dimension;
      Array instance = Array.CreateInstance(elementType, length);
      for (int index = 0; index < dimension; ++index)
      {
        Array ndJaggedArray = JaggedArray.GetNDJaggedArray(flatArray, dimensions, currentDim + 1, startingOffset + index * pageStride, pageStride);
        instance.SetValue((object) ndJaggedArray, index);
      }
      return instance;
    }

    private static Array Get2DJaggedArray(Array flatArray, int row, int col)
    {
      Type elementType = flatArray.GetType().GetElementType();
      if (elementType == typeof (byte))
        return (Array) JaggedArray.Unflatten2D<byte>((byte[]) flatArray, row, col);
      if (elementType == typeof (short))
        return (Array) JaggedArray.Unflatten2D<short>((short[]) flatArray, row, col);
      if (elementType == typeof (int))
        return (Array) JaggedArray.Unflatten2D<int>((int[]) flatArray, row, col);
      if (elementType == typeof (long))
        return (Array) JaggedArray.Unflatten2D<long>((long[]) flatArray, row, col);
      if (elementType == typeof (float))
        return (Array) JaggedArray.Unflatten2D<float>((float[]) flatArray, row, col);
      if (elementType == typeof (double))
        return (Array) JaggedArray.Unflatten2D<double>((double[]) flatArray, row, col);
      if (elementType == typeof (char))
        return (Array) JaggedArray.Unflatten2D<char>((char[]) flatArray, row, col);
      if (elementType == typeof (bool))
        return (Array) JaggedArray.Unflatten2D<bool>((bool[]) flatArray, row, col);
      throw new ArgumentException(JaggedArray.resourceManager.GetString("MWErrorUnsupportedArrayType") + elementType.FullName);
    }

    private static T[][] Unflatten2D<T>(T[] flatArray, int row, int col)
    {
      T[][] objArray = new T[row][];
      for (int index = 0; index < row; ++index)
        objArray[index] = new T[col];
      for (int index1 = 0; index1 < row; ++index1)
      {
        for (int index2 = 0; index2 < col; ++index2)
          objArray[index1][index2] = flatArray[index2 * row + index1];
      }
      return objArray;
    }

    private static Array Get3DJaggedArray(
      Array flatArray,
      int row,
      int col,
      int page,
      int startingOffset)
    {
      Type elementType = flatArray.GetType().GetElementType();
      if (elementType == typeof (byte))
        return (Array) JaggedArray.Unflatten3D<byte>((byte[]) flatArray, row, col, page, startingOffset);
      if (elementType == typeof (short))
        return (Array) JaggedArray.Unflatten3D<short>((short[]) flatArray, row, col, page, startingOffset);
      if (elementType == typeof (int))
        return (Array) JaggedArray.Unflatten3D<int>((int[]) flatArray, row, col, page, startingOffset);
      if (elementType == typeof (long))
        return (Array) JaggedArray.Unflatten3D<long>((long[]) flatArray, row, col, page, startingOffset);
      if (elementType == typeof (float))
        return (Array) JaggedArray.Unflatten3D<float>((float[]) flatArray, row, col, page, startingOffset);
      if (elementType == typeof (double))
        return (Array) JaggedArray.Unflatten3D<double>((double[]) flatArray, row, col, page, startingOffset);
      if (elementType == typeof (char))
        return (Array) JaggedArray.Unflatten3D<char>((char[]) flatArray, row, col, page, startingOffset);
      if (elementType == typeof (bool))
        return (Array) JaggedArray.Unflatten3D<bool>((bool[]) flatArray, row, col, page, startingOffset);
      throw new ArgumentException(JaggedArray.resourceManager.GetString("MWErrorUnsupportedArrayType") + elementType.FullName);
    }

    private static T[][][] Unflatten3D<T>(
      T[] flatArray,
      int row,
      int col,
      int page,
      int startingOffset)
    {
      T[][][] objArray = new T[page][][];
      for (int index1 = 0; index1 < page; ++index1)
      {
        objArray[index1] = new T[row][];
        for (int index2 = 0; index2 < row; ++index2)
          objArray[index1][index2] = new T[col];
      }
      int num1 = row * col;
      int num2 = 0;
      int index3 = 0;
      while (index3 < page)
      {
        for (int index1 = 0; index1 < row; ++index1)
        {
          for (int index2 = 0; index2 < col; ++index2)
            objArray[index3][index1][index2] = flatArray[index2 * row + index1 + num2 + startingOffset];
        }
        ++index3;
        num2 += num1;
      }
      return objArray;
    }

    private static Array CreateJaggedArray(Type t, int[] dimensions, int currentDim)
    {
      int length = dimensions.Length;
      int dimension = dimensions[currentDim];
      if (length - currentDim <= 1)
        return Array.CreateInstance(t, dimension);
      Array instance = Array.CreateInstance(JaggedArray.MakeJaggedArrayType(t, length - currentDim), dimension);
      for (int index = 0; index < dimension; ++index)
        instance.SetValue((object) JaggedArray.CreateJaggedArray(t, dimensions, currentDim + 1), index);
      return instance;
    }

    private static Type MakeJaggedArrayType(Type t, int rank)
    {
      if (rank == 2)
        return t.MakeArrayType();
      return JaggedArray.MakeJaggedArrayType(t.MakeArrayType(), rank - 1);
    }

    public Array ArrayData
    {
      get
      {
        return this.array_;
      }
    }
  }
}
