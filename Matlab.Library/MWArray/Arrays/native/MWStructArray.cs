// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Arrays.native.MWStructArray
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System;
using System.Runtime.InteropServices;

namespace MathWorks.MATLAB.NET.Arrays.native
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public class MWStructArray : MWArray, ICloneable, IEquatable<MWStructArray>
  {
    private static readonly MWStructArray _Empty = new MWStructArray();
    private string[] fieldNames;

    public MWStructArray()
    {
      this.fieldNames = new string[0];
      this.array_Type = MWArrayType.Structure;
    }

    public MWStructArray(int rows, int cols, string[] fNames)
      : this(new int[2]{ rows, cols }, fNames)
    {
    }

    public MWStructArray(int[] inDims, string[] fNames)
      : base(inDims)
    {
      this.fieldNames = (string[]) fNames.Clone();
      this.initNativeArray(this.fieldNames.Length * this.NumberOfElements);
      this.array_Type = MWArrayType.Structure;
    }

    public MWStructArray(params object[] fieldDefs)
      : base(new int[2]{ 1, 1 })
    {
      int length1 = fieldDefs.Length;
      if (length1 % 2 != 0)
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorNotNameValuePair"), nameof (fieldDefs));
      int length2 = length1 / 2;
      this.fieldNames = new string[length2];
      this.initNativeArray(this.fieldNames.Length);
      for (int index1 = 0; index1 < length2; ++index1)
      {
        int index2 = index1 * 2;
        if (fieldDefs[index2].GetType() != typeof (string) || ((string) fieldDefs[index2]).Length == 0)
          throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorFieldNotString"));
        this.fieldNames[index1] = (string) fieldDefs[index2];
        this.set(index1 + 1, fieldDefs[index2 + 1]);
      }
      this.array_Type = MWArrayType.Structure;
    }

    public object this[string fieldName, params int[] indices]
    {
      get
      {
        if (indices.Length == 1)
          return this.get(fieldName, indices[0]);
        return this.get(fieldName, indices);
      }
      set
      {
        if (indices.Length == 1)
          this.set(fieldName, indices[0], value);
        else
          this.set(fieldName, indices, value);
      }
    }

    public object this[string fieldName]
    {
      get
      {
        return this.GetField(fieldName);
      }
      set
      {
        this.SetField(fieldName, value);
      }
    }

    public static MWStructArray Empty
    {
      get
      {
        return (MWStructArray) MWStructArray._Empty.Clone();
      }
    }

    public string[] FieldNames
    {
      get
      {
        return (string[]) this.fieldNames.Clone();
      }
    }

    public int NumberOfFields
    {
      get
      {
        return this.fieldNames.Length;
      }
    }

    public override string ToString()
    {
      if (this.IsEmpty)
        return "[]";
      string str1 = base.ToString() + " with ";
      string str2;
      if (this.fieldNames.Length == 0)
      {
        str2 = str1 + "no fields.";
      }
      else
      {
        str2 = str1 + "fields:\n";
        foreach (string fieldName in this.fieldNames)
        {
          str2 = str2 + "\t" + fieldName;
          if (!fieldName.Equals(this.fieldNames[this.fieldNames.Length - 1]))
            str2 += "\n";
        }
      }
      return str2;
    }

    public override object Clone()
    {
      MWStructArray mwStructArray = new MWStructArray(this.Dimensions, this.FieldNames);
      this.deepCopy((MWArray) mwStructArray);
      return (object) mwStructArray;
    }

    public override bool Equals(object obj)
    {
      if (obj is MWStructArray)
        return this.Equals(obj as MWStructArray);
      return false;
    }

    public bool Equals(MWStructArray obj)
    {
      MWStructArray mwStructArray = obj;
      if (base.Equals((object) mwStructArray))
        return object.Equals((object) mwStructArray.fieldNames, (object) this.fieldNames);
      return false;
    }

    public override int GetHashCode()
    {
      int num = base.GetHashCode();
      if (this.fieldNames.Length != 0)
      {
        foreach (string fieldName in this.fieldNames)
          num = num * 7 + fieldName.GetHashCode();
      }
      if (num >= 0)
        return num;
      return num * -1;
    }

    public object GetField(string fieldName, int index)
    {
      return this.get(fieldName, index);
    }

    public object GetField(string fieldName)
    {
      return this.GetField(fieldName, 1);
    }

    public void SetField(string fieldName, object fieldValue)
    {
      this.set(fieldName, 1, fieldValue);
    }

    public bool IsField(string fieldName)
    {
      return this.fieldIndex(fieldName) != -1;
    }

    public MWStructArray RemoveField(string fName)
    {
      if (!this.IsField(fName))
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorFieldNotFound"), fName);
      string[] strArray = new string[this.fieldNames.Length - 1];
      int index1 = 0;
      int index2 = 0;
      for (; index1 < this.fieldNames.Length; ++index1)
      {
        if (!this.fieldNames[index1].Equals(fName))
        {
          strArray[index2] = this.fieldNames[index1];
          ++index2;
        }
      }
      object[] newArr = new object[(this.NumberOfFields - 1) * this.NumberOfElements];
      int index3 = 0;
      for (int index4 = 0; index4 < this.NumberOfElements; ++index4)
      {
        foreach (string fieldName in this.fieldNames)
        {
          if (!fieldName.Equals(fName))
          {
            newArr[index3] = this[fieldName, new int[1]
            {
              index4 + 1
            }];
            ++index3;
          }
        }
      }
      this.setFlatArr(newArr);
      this.fieldNames = strArray;
      return this;
    }

    private object get(string fName, int index)
    {
      return this.get(this.structIndexToOneBasedIndex(fName, index));
    }

    private object get(string fName, int[] idxArr)
    {
      int basedIndexForArray = this.getOneBasedIndexForArray(idxArr);
      return this.get(fName, basedIndexForArray);
    }

    private void set(string fName, int index, object val)
    {
      if (this.isValidOneBasedIndex(index) && !this.IsField(fName))
      {
        string[] strArray = new string[this.NumberOfFields + 1];
        for (int index1 = 0; index1 < this.NumberOfFields; ++index1)
          strArray[index1] = this.fieldNames[index1];
        strArray[this.NumberOfFields] = fName;
        object[] newArr = new object[strArray.Length * this.NumberOfElements];
        int num = 0;
        int index2 = 0;
        for (; num < this.NumberOfElements; ++num)
        {
          foreach (string index1 in strArray)
          {
            if (!index1.Equals(fName))
              newArr[index2] = this[index1, new int[1]
              {
                num + 1
              }];
            ++index2;
          }
        }
        this.setFlatArr(newArr);
        this.fieldNames = strArray;
      }
      this.set(this.structIndexToOneBasedIndex(fName, index), val);
    }

    private void set(string fName, int[] idxArr, object val)
    {
      int basedIndexForArray = this.getOneBasedIndexForArray(idxArr);
      this.set(fName, basedIndexForArray, val);
    }

    private int structIndexToOneBasedIndex(string fName, int index)
    {
      string message = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
      if (!this.isValidOneBasedIndex(index))
        throw new ArgumentException(message);
      int num = this.fieldIndex(fName);
      if (num == -1)
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorBadFieldName"));
      return (index - 1) * this.NumberOfFields + (num + 1);
    }

    private int fieldIndex(string fieldName)
    {
      int num = -1;
      for (int index = 0; index < this.fieldNames.Length; ++index)
      {
        if (this.fieldNames[index].Equals(fieldName))
        {
          num = index;
          break;
        }
      }
      return num;
    }

    private bool isValidOneBasedIndex(int index)
    {
      return index >= 1 && index <= this.NumberOfElements;
    }
  }
}
