// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Arrays.native.MWArray
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

namespace MathWorks.MATLAB.NET.Arrays.native
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public class MWArray : ICloneable
  {
    internal static ResourceManager resourceManager;
    private int[] dims;
    private int numOfElem;
    private int maxValidIndex;
    private object[] flatArray;
    internal MWArrayType array_Type;
    private int[] axisWtArr;

    static MWArray()
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      MWArray.resourceManager = new ResourceManager(executingAssembly.GetManifestResourceNames()[0].TrimEnd("resources".ToCharArray()).TrimEnd(".".ToCharArray()), executingAssembly);
    }

    internal MWArray()
    {
      this.dims = new int[2];
      this.flatArray = new object[0];
      this.axisWtArr = new int[0];
      this.numOfElem = 0;
      this.MaxValidIndex = 0;
    }

    internal MWArray(int rows, int cols)
      : this(new int[2]{ rows, cols })
    {
    }

    internal MWArray(int[] inDims)
    {
      this.setDims(inDims);
      this.setNumOfElem(inDims);
      this.createAxisWeightArray();
      this.initNativeArray(this.numOfElem);
    }

    public MWArrayType ArrayType
    {
      get
      {
        return this.array_Type;
      }
    }

    public int[] Dimensions
    {
      get
      {
        return (int[]) this.dims.Clone();
      }
    }

    public bool IsCellArray
    {
      get
      {
        return this.array_Type == MWArrayType.Cell;
      }
    }

    public bool IsCharArray
    {
      get
      {
        return this.array_Type == MWArrayType.Character;
      }
    }

    public bool IsLogicalArray
    {
      get
      {
        return this.array_Type == MWArrayType.Logical;
      }
    }

    public bool IsNumericArray
    {
      get
      {
        return this.array_Type == MWArrayType.Numeric;
      }
    }

    public bool IsStructArray
    {
      get
      {
        return this.array_Type == MWArrayType.Structure;
      }
    }

    public bool IsEmpty
    {
      get
      {
        return this.NumberOfElements == 0;
      }
    }

    public int NumberofDimensions
    {
      get
      {
        return this.dims.Length;
      }
    }

    public int NumberOfElements
    {
      get
      {
        return this.numOfElem;
      }
      protected set
      {
        this.numOfElem = value;
      }
    }

    private int MaxValidIndex
    {
      get
      {
        return this.maxValidIndex;
      }
      set
      {
        this.maxValidIndex = value;
      }
    }

    public override bool Equals(object obj)
    {
      MWArray mwArray = (MWArray) obj;
      if (object.Equals((object) mwArray.Dimensions, (object) this.Dimensions))
        return object.Equals((object) mwArray.flatArray, (object) this.flatArray);
      return false;
    }

    public override int GetHashCode()
    {
      int num = 17;
      if (!this.IsEmpty)
      {
        foreach (int dim in this.dims)
          num = num * 7 + dim;
        if (this.flatArray.Length != 0)
        {
          foreach (object flat in this.flatArray)
            num = flat == null ? num : num * 7 + flat.GetHashCode();
        }
      }
      if (num >= 0)
        return num;
      return num * -1;
    }

    public override string ToString()
    {
      if (this.IsEmpty)
        return "[]";
      string dimsStr = this.getDimsStr();
      string str = "";
      switch (this.array_Type)
      {
        case MWArrayType.Cell:
          str = " cell";
          break;
        case MWArrayType.Structure:
          str = " struct";
          break;
      }
      return dimsStr + str + " array";
    }

    public virtual object Clone()
    {
      MWArray target = (MWArray) this.MemberwiseClone();
      this.deepCopy(target);
      return (object) target;
    }

    protected void setDims(int[] inDims)
    {
      string message = MWArray.resourceManager.GetString("MWErrorOneBasedIndexing");
      if (inDims.Length > 1)
      {
        this.dims = new int[inDims.Length];
        for (int index = 0; index < inDims.Length; ++index)
        {
          if (inDims[index] < 0)
            throw new ArgumentException(message);
          this.dims[index] = inDims[index];
        }
      }
      else if (inDims.Length == 1)
      {
        if (inDims[0] < 0)
          throw new ArgumentException(message);
        this.dims = new int[2]{ 1, inDims[0] };
      }
      else
        this.dims = new int[2];
    }

    internal object get(int index)
    {
      string message = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
      if (index < 1 || index > this.MaxValidIndex)
        throw new ArgumentException(message);
      return this.flatArray[index - 1];
    }

    protected object get(int[] index)
    {
      if (index.Length == 1)
        return this.get(index[0]);
      return this.get(this.getOneBasedIndexForArray(index));
    }

    protected void set(int index, object val)
    {
      string message = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
      if (index < 1 || index > this.MaxValidIndex)
        throw new ArgumentException(message);
      if (val is ICloneable)
        this.flatArray[index - 1] = ((ICloneable) val).Clone();
      else
        this.flatArray[index - 1] = val;
    }

    protected void set(int[] index, object val)
    {
      if (index.Length == 1)
        this.set(index[0], val);
      else
        this.set(this.getOneBasedIndexForArray(index), val);
    }

    protected void setNumOfElem(int[] inDims)
    {
      if (inDims.Length == 0)
      {
        this.numOfElem = 0;
      }
      else
      {
        int num = 1;
        foreach (int inDim in inDims)
          num *= inDim;
        this.numOfElem = num;
      }
    }

    protected void createAxisWeightArray()
    {
      this.axisWtArr = new int[this.dims.Length];
      if (this.dims.Length >= 2)
      {
        this.axisWtArr[0] = this.dims[1];
        this.axisWtArr[1] = this.dims[0];
      }
      if (this.dims.Length >= 3)
        this.axisWtArr[2] = this.axisWtArr[0] * this.axisWtArr[1];
      for (int index = 3; index < this.dims.Length; ++index)
        this.axisWtArr[index] = this.axisWtArr[index - 1] * this.dims[index - 1];
    }

    protected void initNativeArray(int num)
    {
      this.flatArray = new object[num];
      this.MaxValidIndex = num;
    }

    protected int getOneBasedIndexForArray(int[] index)
    {
      string message = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
      if (index.Length != this.dims.Length)
        throw new ArgumentException(message);
      for (int index1 = 0; index1 < index.Length; ++index1)
      {
        if (index[index1] < 1 || index[index1] > this.dims[index1])
          throw new ArgumentException(message);
      }
      int pageBasedIndex = this.getPageBasedIndex(index[0], index[1]);
      if (index.Length > 2)
      {
        for (int index1 = index.Length - 1; index1 > 1; --index1)
          pageBasedIndex += (index[index1] - 1) * this.axisWtArr[index1];
      }
      return pageBasedIndex;
    }

    protected void setFlatArr(object[] newArr)
    {
      this.flatArray = newArr;
      this.MaxValidIndex = this.flatArray.Length;
    }

    protected string getDimsStr()
    {
      string str = "";
      for (int index = 0; index < this.dims.Length; ++index)
      {
        str += (string) (object) this.dims[index];
        if (index != this.dims.Length - 1)
          str += "x";
      }
      return str;
    }

    protected void deepCopy(MWArray target)
    {
      target.dims = (int[]) this.dims.Clone();
      target.axisWtArr = (int[]) this.axisWtArr.Clone();
      target.flatArray = (object[]) this.flatArray.Clone();
    }

    protected void resizeFlatArray(int newSize)
    {
      Array.Resize<object>(ref this.flatArray, newSize);
    }

    private int getPageBasedIndex(int r, int c)
    {
      --r;
      --c;
      return c * this.dims[0] + r + 1;
    }
  }
}
