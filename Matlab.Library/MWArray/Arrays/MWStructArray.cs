// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Arrays.MWStructArray
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using MathWorks.MATLAB.NET.Utility;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace MathWorks.MATLAB.NET.Arrays
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public class MWStructArray : MWArray, IEquatable<MWStructArray>
  {
    private static readonly MWStructArray _Empty = new MWStructArray();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxAddField_proxy")]
    internal static extern int mxAddField([In] MWSafeHandle hMXArray, [In] string fieldName);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclCreateSharedCopy_proxy")]
    internal static extern IntPtr mclCreateSharedCopy([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxCalcSingleSubscript_700_proxy")]
    internal static extern int mxCalcSingleSubscript(
      [In] MWSafeHandle hMXArray,
      [In] int numSubscripts,
      [In] int[] subscripts);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxCreateStructArray_700_proxy")]
    internal static extern MWSafeHandle mxCreateStructArray(
      [In] int numDimensions,
      [In] int[] dimensions,
      [In] int numFields,
      [In] string[] fieldNames);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetField_700_proxy")]
    internal static extern IntPtr mxGetField(
      [In] MWSafeHandle hMXArray,
      [In] int index,
      [In] string fieldName);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetFieldNameByNumber_proxy")]
    internal static extern IntPtr mxGetFieldNameByNumber(
      [In] MWSafeHandle hMXArray,
      [In] int fieldNumber);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetFieldNumber_proxy")]
    internal static extern int mxGetFieldNumber([In] MWSafeHandle hMXArray, [In] string fieldName);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetNumberOfFields_proxy")]
    internal static extern int mxGetNumberOfFields([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxRemoveField_proxy")]
    internal static extern void mxRemoveField([In] MWSafeHandle hMXArray, [In] int fieldNumber);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxSetField_700_proxy")]
    internal static extern void mxSetField(
      [In] MWSafeHandle hMXArray,
      [In] int index,
      [In] string fieldName,
      [In] MWSafeHandle fieldValue);

    public MWStructArray()
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2];
        this.SetMXArray(MWStructArray.mxCreateStructArray(0, (int[]) null, 0, (string[]) null), MWArrayType.Structure, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWStructArray(int rows, int columns, string[] fieldNames)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int[] dimensions = new int[2]{ rows, columns };
        this.SetMXArray(MWStructArray.mxCreateStructArray(dimensions.Length, dimensions, fieldNames.Length, fieldNames), MWArrayType.Structure, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWStructArray(int[] dimensions, string[] fieldNames)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        this.SetMXArray(MWStructArray.mxCreateStructArray(dimensions.Length, dimensions, fieldNames.Length, fieldNames), MWArrayType.Structure, dimensions.Length, dimensions);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWStructArray(params MWArray[] fieldDefs)
    {
      int length1 = fieldDefs.Length;
      if (length1 % 2 != 0)
        throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorNotNameValuePair"), nameof (fieldDefs));
      int length2 = length1 / 2;
      string[] fieldNames = new string[length2];
      MWArray[] mwArrayArray = new MWArray[length2];
      for (int index1 = 0; index1 < length2; ++index1)
      {
        int index2 = index1 * 2;
        fieldNames[index1] = ((MWCharArray) fieldDefs[index2]).ToString();
        mwArrayArray[index1] = fieldDefs[index2 + 1];
      }
      try
      {
        Monitor.Enter(MWArray.mxSync);
        this.SetMXArray(MWStructArray.mxCreateStructArray(1, new int[1]
        {
          1
        }, fieldNames.Length, fieldNames), MWArrayType.Structure, 1, new int[1]
        {
          1
        });
        for (int index = 0; index < length2; ++index)
          this.SetField(fieldNames[index], mwArrayArray[index]);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    internal MWStructArray(MWSafeHandle hMXArray)
      : base(hMXArray)
    {
      this.array_Type = MWArrayType.Structure;
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

    public MWArray this[string fieldName, params int[] indices]
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          int[] subscripts = new int[indices.Length];
          for (int index = 0; index < indices.Length; ++index)
            subscripts[index] = indices[index] - 1;
          int index1 = MWStructArray.mxCalcSingleSubscript(this.MXArrayHandle, subscripts.Length, subscripts);
          if (index1 >= MWArray.mxGetNumberOfElements(this.MXArrayHandle))
            throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDimensions"));
          MWSafeHandle hMXArray = new MWSafeHandle(MWStructArray.mxGetField(this.MXArrayHandle, index1, fieldName), false);
          if (!hMXArray.IsInvalid)
            return MWArray.GetTypedArray(new MWSafeHandle(MWStructArray.mclCreateSharedCopy(hMXArray), true), IntPtr.Zero);
          if (!this.IsField(fieldName))
            throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorFieldNotFound"));
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
            for (int index2 = 0; index2 < indices.Length; ++index2)
            {
              if (indices[index2] > dimensions[index2])
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
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          int[] subscripts = new int[indices.Length];
          for (int index = 0; index < indices.Length; ++index)
            subscripts[index] = indices[index] - 1;
          int index1 = MWStructArray.mxCalcSingleSubscript(this.MXArrayHandle, subscripts.Length, subscripts);
          if (index1 >= MWArray.mxGetNumberOfElements(this.MXArrayHandle))
            throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDimensions"));
          MWSafeHandle mwSafeHandle = new MWSafeHandle(MWStructArray.mxGetField(this.MXArrayHandle, index1, fieldName), true);
          MWSafeHandle fieldValue = new MWSafeHandle(MWStructArray.mclCreateSharedCopy(value.MXArrayHandle), false);
          MWStructArray.mxSetField(this.MXArrayHandle, index1, fieldName, fieldValue);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public MWArray this[string fieldName]
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

    protected MWStructArray(SerializationInfo serializationInfo, StreamingContext context)
      : base(serializationInfo, context)
    {
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
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          int numberOfFields = MWStructArray.mxGetNumberOfFields(this.MXArrayHandle);
          string[] strArray = new string[numberOfFields];
          for (int fieldNumber = 0; fieldNumber < numberOfFields; ++fieldNumber)
          {
            IntPtr fieldNameByNumber = MWStructArray.mxGetFieldNameByNumber(this.MXArrayHandle, fieldNumber);
            strArray[fieldNumber] = Marshal.PtrToStringAnsi(fieldNameByNumber);
          }
          return strArray;
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public int NumberOfFields
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return MWStructArray.mxGetNumberOfFields(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public override string ToString()
    {
      return base.ToString();
    }

    public override object Clone()
    {
      MWStructArray mwStructArray = (MWStructArray) this.MemberwiseClone();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        mwStructArray.SetMXArray(MWArray.mxDuplicateArray(this.MXArrayHandle), MWArrayType.Structure);
        return (object) mwStructArray;
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public override bool Equals(object obj)
    {
      if (obj is MWStructArray)
        return this.Equals(obj as MWStructArray);
      return false;
    }

    public bool Equals(MWStructArray other)
    {
      return base.Equals((object) other);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public MWArray GetField(string fieldName, int index)
    {
      this.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        if (index >= MWArray.mxGetNumberOfElements(this.MXArrayHandle))
          throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDimensions"));
        MWSafeHandle hMXArray = new MWSafeHandle(MWStructArray.mxGetField(this.MXArrayHandle, index, fieldName), false);
        if (!hMXArray.IsInvalid)
          return MWArray.GetTypedArray(new MWSafeHandle(MWStructArray.mclCreateSharedCopy(hMXArray), true), IntPtr.Zero);
        if (!this.IsField(fieldName))
          throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorFieldNotFound"));
        if (index < 0 || index > MWArray.mxGetNumberOfElements(this.MXArrayHandle))
          throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDimensions"));
        return (MWArray) MWNumericArray.Empty;
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWArray GetField(string fieldName)
    {
      return this.GetField(fieldName, 0);
    }

    public bool IsField(string fieldName)
    {
      this.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        return -1 != MWStructArray.mxGetFieldNumber(this.MXArrayHandle, fieldName);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWStructArray RemoveField(string fieldName)
    {
      this.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        int fieldNumber = MWStructArray.mxGetFieldNumber(this.MXArrayHandle, fieldName);
        if (-1 == fieldNumber)
          throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorFieldNotFound"), fieldName);
        int numberOfElements = MWArray.mxGetNumberOfElements(this.MXArrayHandle);
        for (int index = 0; index < numberOfElements; ++index)
        {
          MWSafeHandle mwSafeHandle = new MWSafeHandle(MWStructArray.mxGetField(this.MXArrayHandle, index, fieldName), true);
        }
        MWStructArray.mxRemoveField(this.MXArrayHandle, fieldNumber);
        return this;
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public void SetField(string fieldName, MWArray fieldValue)
    {
      this.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        if (!this.IsField(fieldName) && -1 == MWStructArray.mxAddField(this.MXArrayHandle, fieldName))
          throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorBadFieldName"), fieldName);
        MWSafeHandle mwSafeHandle = new MWSafeHandle(MWStructArray.mxGetField(this.MXArrayHandle, 0, fieldName), true);
        MWSafeHandle fieldValue1 = new MWSafeHandle(MWStructArray.mclCreateSharedCopy(fieldValue.MXArrayHandle), false);
        MWStructArray.mxSetField(this.MXArrayHandle, 0, fieldName, fieldValue1);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
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
        int length = dimensions1.Length + 1;
        int[] dimensions2 = new int[length];
        int[] subscripts = new int[length];
        int num1 = dimensions2[length - 2] = dimensions1[0];
        int num2 = dimensions2[length - 1] = dimensions1[1];
        int num3 = dimensions2[0] = this.FieldNames.Length;
        for (int index = 2; index < length - 1; ++index)
          dimensions2[length - (index + 1)] = dimensions1[index];
        Array instance = Array.CreateInstance(typeof (object), dimensions2);
        for (int index1 = 0; index1 < num3; ++index1)
        {
          for (int index2 = 0; index2 < numberOfElements; index2 += num1 * num2)
          {
            for (int index3 = 0; index3 < num1; ++index3)
            {
              for (int index4 = 0; index4 < num2; ++index4)
              {
                instance.SetValue((object) this.GetField(this.FieldNames[index1], index4 * num1 + index3 + index2).ToArray(), subscripts);
                subscripts = this.GetNextSubscript(dimensions2, subscripts, 0);
              }
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
      if (t == typeof (MWArray) || t == typeof (MWStructArray))
        return (object) this;
      if (t == typeof (Hashtable) && this.IsScalar())
        return this.GetHashtable();
      if (t.IsValueType && !t.IsPrimitive && (this.IsScalar() || this.IsEmpty))
        return this.GetNetScalarStruct(t);
      if (t.IsArray && t.GetArrayRank() == 2 && (!t.GetElementType().IsPrimitive && this.NumberofDimensions == 2))
        return this.GetNetStructMatrix(t);
      if (t.IsArray && t.GetArrayRank() == 1 && !t.GetElementType().IsPrimitive && (this.IsVector() || this.IsEmpty))
        return this.GetNetStructVector(t);
      throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDataConversion"), (Exception) new ApplicationException("Cannot convert from MWStructArray to " + t.FullName));
    }

    private object GetHashtable()
    {
      Hashtable hashtable = new Hashtable();
      foreach (string fieldName in this.FieldNames)
      {
        MWArray field = this.GetField(fieldName);
        if (field.IsStructArray)
          hashtable[(object) fieldName] = field.ConvertToType(typeof (Hashtable));
        else if (field.IsCellArray)
          hashtable[(object) fieldName] = field.ConvertToType(typeof (Array));
        else
          hashtable[(object) fieldName] = (object) field.ToArray();
      }
      return (object) hashtable;
    }

    private object GetNetScalarStruct(Type t)
    {
      FieldInfo[] fields = t.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
      object instance = Activator.CreateInstance(t);
      if (this.IsEmpty)
        return instance;
      foreach (FieldInfo validateField in this.ValidateFields(t, fields))
        validateField.SetValue(instance, this[validateField.Name].ConvertToType(validateField.FieldType));
      return instance;
    }

    private object GetNetStructMatrix(Type t)
    {
      FieldInfo[] fields = t.GetElementType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
      int[] dimensions = this.Dimensions;
      int[] columnMajorIndices = MWArray.GetColumnMajorIndices(dimensions[0], dimensions[1]);
      Array instance1 = Array.CreateInstance(t.GetElementType(), dimensions[0], dimensions[1]);
      if (this.IsEmpty)
        return (object) instance1;
      FieldInfo[] fieldInfoArray = this.ValidateFields(t.GetElementType(), fields);
      int index = 0;
      for (int index1 = 0; index1 < dimensions[0]; ++index1)
      {
        for (int index2 = 0; index2 < dimensions[1]; ++index2)
        {
          object instance2 = Activator.CreateInstance(t.GetElementType());
          foreach (FieldInfo fieldInfo in fieldInfoArray)
            fieldInfo.SetValue(instance2, this[fieldInfo.Name, new int[1]
            {
              columnMajorIndices[index]
            }].ConvertToType(fieldInfo.FieldType));
          instance1.SetValue(instance2, index1, index2);
          ++index;
        }
      }
      return (object) instance1;
    }

    private object GetNetStructVector(Type t)
    {
      FieldInfo[] fields = t.GetElementType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
      int numberOfElements = this.NumberOfElements;
      Array instance1 = Array.CreateInstance(t.GetElementType(), numberOfElements);
      if (this.IsEmpty)
        return (object) instance1;
      FieldInfo[] fieldInfoArray = this.ValidateFields(t.GetElementType(), fields);
      for (int index = 0; index < numberOfElements; ++index)
      {
        object instance2 = Activator.CreateInstance(t.GetElementType());
        foreach (FieldInfo fieldInfo in fieldInfoArray)
          fieldInfo.SetValue(instance2, this[fieldInfo.Name, new int[1]
          {
            index + 1
          }].ConvertToType(fieldInfo.FieldType));
        instance1.SetValue(instance2, index);
      }
      return (object) instance1;
    }

    private FieldInfo[] ValidateFields(Type t, FieldInfo[] netFields)
    {
      string message = MWArray.resourceManager.GetString("MWErrorInvalidDataConversion");
      string str = "Cannot convert from MWStructArray to " + t.FullName + ".\n";
      string[] fieldNames = this.FieldNames;
      int length1 = fieldNames.Length;
      int length2 = netFields.Length;
      FieldInfo[] fieldInfoArray = new FieldInfo[length1];
      bool[] flagArray = new bool[length2];
      for (int index1 = 0; index1 < length1; ++index1)
      {
        bool flag = true;
        for (int index2 = 0; index2 < length2; ++index2)
        {
          if (!flagArray[index2] && string.Compare(fieldNames[index1], netFields[index2].Name, StringComparison.Ordinal) == 0)
          {
            fieldInfoArray[index1] = netFields[index2];
            flagArray[index2] = true;
            flag = false;
            break;
          }
        }
        if (flag)
          throw new ArgumentException(message, (Exception) new ApplicationException(str + "Provided type does not have the same fields as the returned MATLAB struct."));
      }
      return fieldInfoArray;
    }
  }
}
