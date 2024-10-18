// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Arrays.MWArray
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using MathWorks.MATLAB.NET.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading;

namespace MathWorks.MATLAB.NET.Arrays
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential)]
  public class MWArray : ICloneable, ISerializable, IDisposable
  {
    private MWSafeHandle _hMXArray = new MWSafeHandle();
    private static bool nativeGCEnabled = true;
    private static long nativeGCBlockSize = 10000000;
    internal static StringBuilder formattedOutputString = (StringBuilder) null;
    internal static object mxSync = (object) new object[0];
    internal static Dictionary<Type, Type[]> typeMap = new Dictionary<Type, Type[]>();
    internal static ResourceManager resourceManager;
    internal MWArrayType array_Type;
    internal long NumElements;
    internal long ElementSize;

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclmcrInitialize2_proxy")]
    private static extern bool mclmcrInitialize2(int primaryMode);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxCreateString_proxy")]
    internal static extern MWSafeHandle mxCreateString([In] IntPtr pString);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMxDeserialize_proxy")]
    internal static extern MWSafeHandle mxDeserialize(
      [In] IntPtr pSerializedBuffer,
      [In] int size);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxDuplicateArray_proxy")]
    internal static extern MWSafeHandle mxDuplicateArray([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxFree_proxy")]
    internal static extern void mxFree([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetClassID_proxy")]
    internal static extern int mxGetClassID([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetData_proxy")]
    internal static extern IntPtr mxGetData([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetDimensions_700_proxy")]
    internal static extern IntPtr mxGetDimensions([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetElementSize_proxy")]
    internal static extern int mxGetElementSize([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetM_proxy")]
    internal static extern int mxGetM([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetN_proxy")]
    internal static extern int mxGetN([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetNumberOfDimensions_700_proxy")]
    internal static extern int mxGetNumberOfDimensions([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetNumberOfElements_proxy")]
    internal static extern int mxGetNumberOfElements([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxGetScalar_proxy")]
    internal static extern double mxGetScalar([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMxIsA_proxy")]
    internal static extern byte mxIsA([In] IntPtr pMCR, [In] MWSafeHandle hMXArray, [In] string typeName);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMxIsRef_proxy")]
    internal static extern byte mxIsRef([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMxRefIsA_proxy")]
    internal static extern byte mxRefIsA([In] MWSafeHandle hMXArray, [In] string typeName);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMxReleaseRef_proxy")]
    internal static extern IntPtr mxReleaseRef([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsObject_proxy")]
    internal static extern byte mxIsObject([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsOpaque_proxy")]
    internal static extern byte mxIsOpaque([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsCell_proxy")]
    internal static extern byte mxIsCell([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsChar_proxy")]
    internal static extern byte mxIsChar([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsEmpty_proxy")]
    internal static extern byte mxIsEmpty([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsLogical_proxy")]
    internal static extern byte mxIsLogical([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsNumeric_proxy")]
    internal static extern byte mxIsNumeric([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsSparse_proxy")]
    internal static extern byte mxIsSparse([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mxIsStruct_proxy")]
    internal static extern byte mxIsStruct([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMxSerialize_proxy")]
    internal static extern MWSafeHandle mxSerialize([In] MWSafeHandle hMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "array_handle_get_int_proxy")]
    internal static extern IntPtr array_handle_get_int(
      [In] IntPtr pMXArray,
      [In] IntPtr numIndices,
      [In] IntPtr[] indices);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclArrayHandle2mxArray_proxy")]
    internal static extern int mclArrayHandle2mxArray(
      out MWSafeHandle hMXArray,
      [In] IntPtr pArrayHandle);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclcppArrayToString_proxy")]
    private static extern int mclcppArrayToString([In] MWSafeHandle hMXArray, out string formattedString);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclIsIdentical_proxy")]
    internal static extern byte mclIsIdentical([In] MWSafeHandle hMXArray1, [In] MWSafeHandle hMXArray2);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclmxArray2ArrayHandle_proxy")]
    internal static extern int mclmxArray2ArrayHandle(out IntPtr pArrayHandle, [In] IntPtr pMXArray);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "array_handle_set_proxy")]
    internal static extern int array_handle_set([In] IntPtr pArrayHandle, [In] IntPtr pArrayHandleValue);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "array_handle_set_logical_proxy")]
    private static extern int array_handle_set_logical(
      [In] IntPtr pArrayHandle,
      [In] MWSafeHandle hMXArray,
      [In] IntPtr len);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMXArrayGet_proxy")]
    internal static extern int mclMXArrayGet(
      out MWSafeHandle hMXArraySrcElem,
      [In] MWSafeHandle hMXArray,
      [In] IntPtr numIndices,
      [In] IntPtr[] indices);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMXArraySet_proxy")]
    private static extern int mclMXArraySet(
      [In] MWSafeHandle hMXArrayTrg,
      [In] MWSafeHandle hMXArraySrcElem,
      [In] IntPtr numIndices,
      [In] IntPtr[] indices);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMXArraySetLogical_proxy")]
    private static extern int mclMXArraySetLogical(
      [In] MWSafeHandle hMXArrayTrg,
      [In] MWSafeHandle hMXArraySrcElem,
      [In] IntPtr numIndices,
      [In] IntPtr[] indices);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("libut.dll")]
    private static extern int ut_hash_pointer([Out] int hashCode, [In] MWSafeHandle hMXArray);

    static MWArray()
    {
      try
      {
        MWArray.PopulateTypeMap();
        Monitor.Enter(MWArray.mxSync);
        int num = MWMCR.MCRAppInitialized ? 1 : 0;
        Assembly entryAssembly = Assembly.GetEntryAssembly();
        NativeGCAttribute nativeGcAttribute = (Assembly) null != entryAssembly ? (NativeGCAttribute) Attribute.GetCustomAttribute(entryAssembly, typeof (NativeGCAttribute)) : (NativeGCAttribute) null;
        if (nativeGcAttribute != null && (MWArray.nativeGCEnabled = nativeGcAttribute.GCEnabled) && 0 < nativeGcAttribute.GCBlockSize)
          MWArray.nativeGCBlockSize = (long) ((double) nativeGcAttribute.GCBlockSize * 10000000.0);
        MWArray.resourceManager = MWResources.getResourceManager();
        MWArray.mclmcrInitialize2(3);
        MWArray.formattedOutputString = new StringBuilder(1024);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    internal MWArray()
    {
    }

    internal MWArray(MWSafeHandle hMXArray)
    {
      this.SetMXArray(hMXArray, MWArrayType.Array);
    }

    public static void DisposeArray(object _object)
    {
      if (_object == null)
        return;
      if (_object is IDisposable)
      {
        ((IDisposable) _object).Dispose();
      }
      else
      {
        if (!(_object is MWArray[]))
          return;
        foreach (object _object1 in (MWArray[]) _object)
          MWArray.DisposeArray(_object1);
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        if (this._hMXArray == null || this.IsDisposed || !MWMCR.MCRAppInitialized)
          return;
        int num = disposing ? 1 : 0;
        this._hMXArray.Dispose();
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWArray this[params int[] indices]
    {
      get
      {
        switch (this.array_Type)
        {
          case MWArrayType.Numeric:
            return (MWArray) ((MWNumericArray) this)[indices];
          case MWArrayType.Logical:
            return (MWArray) ((MWLogicalArray) this)[indices];
          case MWArrayType.Cell:
            return ((MWCellArray) this)[indices];
          case MWArrayType.NativeObjArray:
            throw new Exception(MWArray.resourceManager.GetString("MWErrorNotSupported"));
          case MWArrayType.Structure:
            throw new Exception(MWArray.resourceManager.GetString("MWErrorNotSupported"));
          default:
            return MWArray.GetTypedArray(this.ArrayIndexer(this, indices), IntPtr.Zero);
        }
      }
      set
      {
        switch (this.array_Type)
        {
          case MWArrayType.Numeric:
            if (this.array_Type != value.array_Type)
              throw new InvalidCastException(MWArray.resourceManager.GetString("MWErrorDataArrayType"));
            ((MWNumericArray) this)[indices] = (MWNumericArray) value;
            break;
          case MWArrayType.Logical:
            if (this.array_Type != value.array_Type)
              throw new InvalidCastException(MWArray.resourceManager.GetString("MWErrorDataArrayType"));
            ((MWLogicalArray) this)[indices] = (MWLogicalArray) value;
            break;
          case MWArrayType.Cell:
            ((MWCellArray) this)[indices] = value;
            break;
          case MWArrayType.NativeObjArray:
            if (this.array_Type != value.array_Type)
              throw new InvalidCastException(MWArray.resourceManager.GetString("MWErrorDataArrayType"));
            ((MWObjectArray) this)[indices] = (object) (MWObjectArray) value;
            break;
          case MWArrayType.Structure:
            throw new Exception(MWArray.resourceManager.GetString("MWErrorNotSupported"));
          default:
            this.ArrayIndexer(value, this, indices);
            break;
        }
      }
    }

    public static implicit operator MWArray(double scalar)
    {
      return (MWArray) new MWNumericArray(scalar);
    }

    public static implicit operator MWArray(string value)
    {
      return (MWArray) new MWCharArray(value);
    }

    internal MWArray(SerializationInfo serializationInfo, StreamingContext context)
    {
      IntPtr num = IntPtr.Zero;
      try
      {
        Monitor.Enter(MWArray.mxSync);
        byte[] source = (byte[]) serializationInfo.GetValue("serializedState", typeof (byte[]));
        this.array_Type = (MWArrayType) serializationInfo.GetValue(nameof (array_Type), typeof (MWArrayType));
        int length = source.Length;
        num = Marshal.AllocCoTaskMem(length);
        Marshal.Copy(source, 0, num, length);
        this.SetMXArray(MWArray.mxDeserialize(num, length), this.array_Type);
      }
      finally
      {
        if (IntPtr.Zero != num)
          Marshal.FreeCoTaskMem(num);
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      MWSafeHandle hMXArray = (MWSafeHandle) null;
      IntPtr zero = IntPtr.Zero;
      try
      {
        Monitor.Enter(MWArray.mxSync);
        hMXArray = MWArray.mxSerialize(this._hMXArray);
        int numberOfElements = MWArray.mxGetNumberOfElements(hMXArray);
        IntPtr data = MWArray.mxGetData(hMXArray);
        byte[] numArray = new byte[numberOfElements];
        byte[] destination = numArray;
        int length = numberOfElements;
        Marshal.Copy(data, destination, 0, length);
        info.AddValue("serializedState", (object) numArray);
        info.AddValue("array_Type", (object) this.array_Type);
      }
      finally
      {
        if (hMXArray != null && !hMXArray.IsInvalid)
          hMXArray.Dispose();
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public static bool NativeGCEnabled
    {
      get
      {
        return MWArray.nativeGCEnabled;
      }
      set
      {
        MWArray.nativeGCEnabled = value;
      }
    }

    public static long NativeGCBlockSize
    {
      get
      {
        return MWArray.nativeGCBlockSize;
      }
    }

    public MWArrayType ArrayType
    {
      get
      {
        this.CheckDisposed();
        return this.array_Type;
      }
    }

    public virtual int[] Dimensions
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          MWSafeHandle mxArrayHandle = this.MXArrayHandle;
          int numberOfDimensions = MWArray.mxGetNumberOfDimensions(mxArrayHandle);
          int[] destination = new int[numberOfDimensions];
          Marshal.Copy(MWArray.mxGetDimensions(mxArrayHandle), destination, 0, numberOfDimensions);
          return destination;
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public virtual bool IsDisposed
    {
      get
      {
        if (MWArrayType.NativeObjArray == this.array_Type)
          return this.IsDisposed;
        return this._hMXArray.IsInvalid;
      }
    }

    public virtual bool IsEmpty
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWArray.mxIsEmpty(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsCellArray
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWArray.mxIsCell(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsCharArray
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWArray.mxIsChar(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsLogicalArray
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWArray.mxIsLogical(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsNumericArray
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWArray.mxIsNumeric(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public bool IsStructArray
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (byte) 1 == MWArray.mxIsStruct(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public int NumberofDimensions
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return MWArray.mxGetNumberOfDimensions(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public int NumberOfElements
    {
      get
      {
        this.CheckDisposed();
        MWSafeHandle mxArrayHandle = this.MXArrayHandle;
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return MWArray.mxIsSparse(mxArrayHandle) == (byte) 0 ? MWArray.mxGetNumberOfElements(mxArrayHandle) : 0;
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    internal virtual MWSafeHandle MXArrayHandle
    {
      get
      {
        MWSafeHandle mwSafeHandle = this._hMXArray;
        if (MWArrayType.NativeObjArray == this.array_Type)
          mwSafeHandle = this.MXArrayHandle;
        return mwSafeHandle;
      }
    }

    internal MWArray.MATLABArrayType MATLABType
    {
      get
      {
        this.CheckDisposed();
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return (MWArray.MATLABArrayType) MWArray.mxGetClassID(this.MXArrayHandle);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    internal static MWArray GetTypedArray(MWSafeHandle hMXArray, IntPtr mcrInstance = default (IntPtr))
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        MWArray mwArray = (MWArray) null;
        if (hMXArray.IsInvalid)
          return (MWArray) MWNumericArray.Empty;
        switch (MWArray.mxGetClassID(hMXArray))
        {
          case 0:
          case 16:
          case 17:
          case 18:
            mwArray = new MWArray(hMXArray);
            break;
          case 1:
            mwArray = (MWArray) new MWCellArray(hMXArray);
            break;
          case 2:
            mwArray = (MWArray) new MWStructArray(hMXArray);
            break;
          case 3:
            mwArray = (MWArray) new MWLogicalArray(hMXArray);
            break;
          case 4:
            mwArray = (MWArray) new MWCharArray(hMXArray);
            break;
          case 5:
            mwArray = (MWArray) new MWIndexArray(hMXArray);
            break;
          case 6:
          case 7:
          case 9:
          case 10:
          case 12:
          case 14:
            mwArray = (MWArray) new MWNumericArray(hMXArray);
            break;
          case 8:
          case 11:
          case 13:
          case 15:
            throw new ArgumentOutOfRangeException("numArgsOut", MWArray.resourceManager.GetString("MWErrorInvalidReturnType"));
          default:
            if (MWArray.mxIsRef(hMXArray) != (byte) 0 && (MWArray.mxRefIsA(hMXArray, "System.Object") != (byte) 0 || MWArray.mxRefIsA(hMXArray, "int32") != (byte) 0))
            {
              mwArray = (MWArray) new MWObjectArray(new MWSafeHandle(MWArray.mxReleaseRef(hMXArray)));
              hMXArray.SetHandleAsInvalid();
              break;
            }
            if (MWArray.mxIsA(mcrInstance, hMXArray, "System.Object") != (byte) 0 || MWArray.mxIsA(mcrInstance, hMXArray, "int32") != (byte) 0)
            {
              mwArray = (MWArray) new MWObjectArray(hMXArray);
              break;
            }
            if (MWArray.mxIsObject(hMXArray) != (byte) 0 || MWArray.mxIsOpaque(hMXArray) != (byte) 0)
            {
              mwArray = new MWArray(hMXArray);
              break;
            }
            break;
        }
        return mwArray;
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    internal static bool IsMWArray(object objIn)
    {
      Type type = objIn.GetType();
      return type == typeof (MWArray) || type.IsSubclassOf(typeof (MWArray));
    }

    internal static MWArray ConvertObjectToMWArray(object objIn)
    {
      MWArray mwArray1;
      if (objIn != null)
      {
        if (MWArray.IsMWArray(objIn))
          mwArray1 = (MWArray) objIn;
        else if (objIn.GetType() == typeof (string))
          mwArray1 = (MWArray) new MWCharArray((string) objIn);
        else if (objIn.GetType().IsValueType && objIn.GetType().IsPrimitive)
          mwArray1 = MWArray.GetMWArrayFromValueType(objIn);
        else if (objIn.GetType().IsArray && MWArray.IsArrayOfSupportedType(objIn))
        {
          string arrType = MWArray.ArrayElementTypeName(objIn);
          mwArray1 = MWArray.GetMWArrayFromArrayType((Array) objIn, arrType);
        }
        else if (objIn.GetType().IsArray && MWArray.IsJaggedArrayOfSupportedType((Array) objIn))
          mwArray1 = MWArray.GetMWArrayFromJaggedArrayType((Array) objIn);
        else if (objIn.GetType().IsValueType && !objIn.GetType().IsPrimitive)
          mwArray1 = (MWArray) MWArray.GetMWStructArrayFromNETStructs((ValueType) objIn);
        else if (objIn.GetType().IsArray && objIn.GetType().GetElementType().IsValueType && !objIn.GetType().GetElementType().IsPrimitive)
          mwArray1 = (MWArray) MWArray.GetMWStructArrayFromNETStructs((Array) objIn);
        else if (objIn.GetType() == typeof (MathWorks.MATLAB.NET.Arrays.native.MWStructArray))
        {
          MathWorks.MATLAB.NET.Arrays.native.MWStructArray mwStructArray1 = (MathWorks.MATLAB.NET.Arrays.native.MWStructArray) objIn;
          MWStructArray mwStructArray2 = new MWStructArray(mwStructArray1.Dimensions, mwStructArray1.FieldNames);
          for (int index1 = 1; index1 <= mwStructArray1.NumberOfElements; ++index1)
          {
            for (int index2 = 0; index2 < mwStructArray1.NumberOfFields; ++index2)
            {
              object objIn1 = mwStructArray1[mwStructArray1.FieldNames[index2], new int[1]
              {
                index1
              }];
              if (objIn1 != null)
              {
                MWArray mwArray2 = MWArray.ConvertObjectToMWArray(objIn1);
                mwStructArray2[mwStructArray1.FieldNames[index2], new int[1]
                {
                  index1
                }] = mwArray2;
              }
            }
          }
          mwArray1 = (MWArray) mwStructArray2;
        }
        else if (objIn.GetType() == typeof (MathWorks.MATLAB.NET.Arrays.native.MWCellArray))
        {
          MathWorks.MATLAB.NET.Arrays.native.MWCellArray mwCellArray1 = (MathWorks.MATLAB.NET.Arrays.native.MWCellArray) objIn;
          MWCellArray mwCellArray2 = new MWCellArray(mwCellArray1.Dimensions);
          for (int index = 1; index <= mwCellArray1.NumberOfElements; ++index)
          {
            object objIn1 = mwCellArray1.get(index);
            if (objIn1 != null)
            {
              MWArray mwArray2 = MWArray.ConvertObjectToMWArray(objIn1);
              mwCellArray2.ArrayIndexer(mwArray2, (MWArray) mwCellArray2, index);
            }
          }
          mwArray1 = (MWArray) mwCellArray2;
        }
        else if (objIn.GetType() == typeof (Hashtable) || MWArray.IsDictionaryOfSupportedType(objIn))
          mwArray1 = (MWArray) MWArray.GetMWStructArrayFromIDictionary((IDictionary) objIn);
        else if (objIn.GetType() == typeof (ArrayList))
          mwArray1 = MWArray.GetMWCellArrayFromArrayList((ArrayList) objIn);
        else if (AppDomain.CurrentDomain.IsDefaultAppDomain())
        {
          mwArray1 = (MWArray) new MWObjectArray(objIn);
        }
        else
        {
          if (AppDomain.CurrentDomain.IsDefaultAppDomain() || !objIn.GetType().IsSerializable)
            throw new InvalidDataException("Input data type unsupported by MATLAB .NET Assembly");
          mwArray1 = (MWArray) new MWObjectArray(objIn);
        }
      }
      else
        mwArray1 = (MWArray) new MWNumericArray();
      return mwArray1;
    }

    private static MWArray GetMWArrayFromJaggedArrayType(Array array)
    {
      if (MWArray.IsCTSNumericType(JaggedArray.GetElementType(array.GetType())))
        return (MWArray) new MWNumericArray(array);
      throw new InvalidDataException("Input data type unsupported by MATLAB .NET Assembly");
    }

    private static MWArray GetMWCellArrayFromArrayList(ArrayList objIn)
    {
      int count = objIn.Count;
      MWCellArray mwCellArray;
      if (count == 0)
      {
        mwCellArray = new MWCellArray();
      }
      else
      {
        mwCellArray = new MWCellArray(1, count);
        int num = 1;
        foreach (object objIn1 in objIn)
          mwCellArray[new int[1]{ num++ }] = MWArray.ConvertObjectToMWArray(objIn1);
      }
      return (MWArray) mwCellArray;
    }

    private static bool IsDictionaryOfSupportedType(object objIn)
    {
      Type type = objIn.GetType();
      if (type.GetInterface("IDictionary") != (Type) null)
      {
        Type[] genericArguments = type.GetGenericArguments();
        if (genericArguments.Length == 2 && genericArguments[0] == typeof (string) && (MWArray.IsSupportedType(genericArguments[1]) || type.IsArray && MWArray.IsArrayOfSupportedType(objIn)))
          return true;
      }
      return false;
    }

    internal static MWStructArray GetMWStructArrayFromIDictionary(IDictionary objIn)
    {
      if (objIn.Count == 0)
        throw new InvalidDataException("Cannot convert an empty Hashtable");
      string[] fieldNames = new string[objIn.Count];
      int num1 = 0;
      foreach (object key in (IEnumerable) objIn.Keys)
        fieldNames[num1++] = (string) key;
      MWStructArray mwStructArray = new MWStructArray(1, 1, fieldNames);
      int num2 = 0;
      foreach (object objIn1 in (IEnumerable) objIn.Values)
        mwStructArray[fieldNames[num2++]] = MWArray.ConvertObjectToMWArray(objIn1);
      return mwStructArray;
    }

    internal static MWStructArray GetMWStructArrayFromNETStructs(ValueType structIn)
    {
      FieldInfo[] fields = structIn.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
      if (fields.Length == 0)
        throw new InvalidDataException(string.Format("Type {0} does not contain any declared public fields", (object) structIn.GetType().FullName));
      string[] fieldNames = new string[fields.Length];
      int num1 = 0;
      foreach (FieldInfo fieldInfo in fields)
        fieldNames[num1++] = fieldInfo.Name;
      MWStructArray mwStructArray = new MWStructArray(1, 1, fieldNames);
      int num2 = 0;
      foreach (FieldInfo fieldInfo in fields)
        mwStructArray[fieldNames[num2++]] = MWArray.ConvertObjectToMWArray(fieldInfo.GetValue((object) structIn));
      return mwStructArray;
    }

    internal static MWStructArray GetMWStructArrayFromNETStructs(Array structArrIn)
    {
      if (structArrIn.Rank > 2)
        throw new InvalidDataException(string.Format("Cannot convert an Array of {0} with rank more than 2", (object) structArrIn.GetType().GetElementType().FullName));
      FieldInfo[] fields = structArrIn.GetType().GetElementType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
      int num1;
      int length;
      if (structArrIn.Rank == 1)
      {
        num1 = 1;
        length = structArrIn.GetLength(0);
      }
      else
      {
        num1 = structArrIn.GetLength(0);
        length = structArrIn.GetLength(1);
      }
      string[] fieldNames = new string[fields.Length];
      int num2 = 0;
      foreach (FieldInfo fieldInfo in fields)
        fieldNames[num2++] = fieldInfo.Name;
      MWStructArray mwStructArray = new MWStructArray(num1, length, fieldNames);
      int[] columnMajorIndices = MWArray.GetColumnMajorIndices(num1, length);
      int index = 0;
      foreach (object obj in structArrIn)
      {
        foreach (FieldInfo fieldInfo in fields)
          mwStructArray[fieldInfo.Name, new int[1]
          {
            columnMajorIndices[index]
          }] = MWArray.ConvertObjectToMWArray(fieldInfo.GetValue(obj));
        ++index;
      }
      return mwStructArray;
    }

    internal static int[] GetColumnMajorIndices(int row, int col)
    {
      int[] numArray = new int[row * col];
      int num = 0;
      for (int index1 = 0; index1 < row; ++index1)
      {
        for (int index2 = 0; index2 < col; ++index2)
          numArray[num++] = index2 * row + index1 + 1;
      }
      return numArray;
    }

    internal static void RecursiveFieldCopy(
      ref int[] elementIdx,
      int dimIdx,
      ref MWStructArray outArray,
      ref Array inArray,
      ref FieldInfo[] infoFields)
    {
      if (dimIdx < outArray.NumberofDimensions - 1)
      {
        for (int index = 0; index < inArray.GetLength(dimIdx); ++index)
        {
          MWArray.RecursiveFieldCopy(ref elementIdx, dimIdx + 1, ref outArray, ref inArray, ref infoFields);
          ++elementIdx[dimIdx];
        }
        elementIdx[dimIdx + 1] = 0;
      }
      else
      {
        for (int index = 0; index < inArray.GetLength(dimIdx); ++index)
        {
          foreach (FieldInfo fieldInfo in infoFields)
            outArray[fieldInfo.Name, elementIdx] = MWArray.ConvertObjectToMWArray(fieldInfo.GetValue(inArray.GetValue(elementIdx)));
          ++elementIdx[dimIdx];
        }
        elementIdx[dimIdx] = 0;
      }
    }

    internal static MWArray GetMWArrayFromValueType(object objIn)
    {
      switch (objIn.GetType().FullName)
      {
        case "System.Boolean":
          return (MWArray) new MWLogicalArray((bool) objIn);
        case "System.Byte":
          return (MWArray) new MWNumericArray((byte) objIn, false);
        case "System.Char":
          return (MWArray) new MWCharArray((char) objIn);
        case "System.Double":
          return (MWArray) new MWNumericArray((double) objIn);
        case "System.Int16":
          return (MWArray) new MWNumericArray((short) objIn, false);
        case "System.Int32":
          return (MWArray) new MWNumericArray((int) objIn, false);
        case "System.Int64":
          return (MWArray) new MWNumericArray((long) objIn, false);
        case "System.Single":
          return (MWArray) new MWNumericArray((float) objIn, false);
        default:
          throw new InvalidDataException("Data type unsupported by MATLAB .NET Assembly");
      }
    }

    internal static MWArray GetMWArrayFromArrayType(Array arrIn, string arrType)
    {
      switch (arrType)
      {
        case "System.Boolean":
          return (MWArray) new MWLogicalArray(arrIn);
        case "System.Byte":
          return (MWArray) new MWNumericArray(arrIn, false, true);
        case "System.Char":
          return (MWArray) new MWCharArray(arrIn);
        case "System.Double":
          return (MWArray) new MWNumericArray(arrIn);
        case "System.Int16":
          return (MWArray) new MWNumericArray(arrIn, false, true);
        case "System.Int32":
          return (MWArray) new MWNumericArray(arrIn, false, true);
        case "System.Int64":
          return (MWArray) new MWNumericArray(arrIn, false, true);
        case "System.Single":
          return (MWArray) new MWNumericArray(arrIn, false, true);
        case "System.String":
          return (MWArray) new MWCharArray(arrIn);
        default:
          throw new InvalidDataException("Data type unsupported by MATLAB .NET Assembly for input");
      }
    }

    internal static bool IsArrayOfSupportedType(object objIn)
    {
      Type elementType = objIn.GetType().GetElementType();
      if (elementType.IsArray)
        return false;
      string fullName = elementType.FullName;
      bool flag = false;
      if (fullName.Contains("System.Double") || fullName.Contains("System.Single") || (fullName.Contains("System.Int64") || fullName.Contains("System.Int32")) || (fullName.Contains("System.Int16") || fullName.Contains("System.Byte") || (fullName.Contains("System.Boolean") || fullName.Contains("System.String"))) || fullName.Contains("System.Char"))
        flag = true;
      return flag;
    }

    private static bool IsCTSNumericType(Type t)
    {
      return t == typeof (byte) || t == typeof (short) || (t == typeof (int) || t == typeof (long)) || (t == typeof (float) || t == typeof (double));
    }

    internal static bool IsJaggedArrayOfSupportedType(Array objIn)
    {
      if (!JaggedArray.IsJagged(objIn))
        return false;
      return MWArray.IsCTSNumericType(JaggedArray.GetElementType(objIn.GetType()));
    }

    internal static bool IsSupportedType(Type t)
    {
      string fullName = t.FullName;
      bool flag = false;
      if (fullName.Contains("System.Double") || fullName.Contains("System.Single") || (fullName.Contains("System.Int64") || fullName.Contains("System.Int32")) || (fullName.Contains("System.Int16") || fullName.Contains("System.Byte") || (fullName.Contains("System.Boolean") || fullName.Contains("System.String"))) || fullName.Contains("System.Char"))
        flag = true;
      return flag;
    }

    internal static string ArrayElementTypeName(object objIn)
    {
      string str = "";
      string fullName = objIn.GetType().FullName;
      if (fullName.Contains("System.Double"))
        str = "System.Double";
      else if (fullName.Contains("System.Single"))
        str = "System.Single";
      else if (fullName.Contains("System.Int64"))
        str = "System.Int64";
      else if (fullName.Contains("System.Int32"))
        str = "System.Int32";
      else if (fullName.Contains("System.Int16"))
        str = "System.Int16";
      else if (fullName.Contains("System.Byte"))
        str = "System.Byte";
      else if (fullName.Contains("System.Boolean"))
        str = "System.Boolean";
      else if (fullName.Contains("System.String"))
        str = "System.String";
      else if (fullName.Contains("System.Char"))
        str = "System.Char";
      return str;
    }

    internal MWSafeHandle SetMXArray(
      MWSafeHandle hMXArray,
      MWArrayType arrayType,
      int numDimensions,
      int[] dimensions)
    {
      this.SetMXArray(hMXArray, arrayType, numDimensions, dimensions, false);
      return this._hMXArray;
    }

    internal MWSafeHandle SetMXArray(
      MWSafeHandle hMXArray,
      MWArrayType arrayType,
      int numDimensions,
      int[] dimensions,
      bool isComplex)
    {
      if (hMXArray.IsInvalid)
        throw new OutOfMemoryException(MWArray.resourceManager.GetString("MWErrorAllocatingMXArray"));
      this._hMXArray = hMXArray;
      this.array_Type = arrayType;
      this.NumElements = 1L;
      foreach (long dimension in dimensions)
        this.NumElements *= dimension;
      if (isComplex)
        this.NumElements *= 2L;
      try
      {
        Monitor.Enter(MWArray.mxSync);
        this.ElementSize = (long) MWArray.mxGetElementSize(this._hMXArray);
        if (MWArray.nativeGCEnabled)
        {
          long num = this.ElementSize * this.NumElements;
          if (0L < num)
            this._hMXArray.UnmanagedBytesAllocated = num;
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
      return this._hMXArray;
    }

    internal MWSafeHandle SetMXArray(MWSafeHandle hMXArray, MWArrayType arrayType)
    {
      if (hMXArray.IsInvalid)
        throw new OutOfMemoryException(MWArray.resourceManager.GetString("MWErrorAllocatingMXArray"));
      this._hMXArray = hMXArray;
      this.array_Type = arrayType;
      try
      {
        Monitor.Enter(MWArray.mxSync);
        this.NumElements = (long) this.NumberOfElements;
        this.ElementSize = (long) MWArray.mxGetElementSize(this._hMXArray);
        if (MWArray.nativeGCEnabled)
        {
          long num = this.ElementSize * this.NumElements;
          if (0L < num)
            this._hMXArray.UnmanagedBytesAllocated = num;
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
      return this._hMXArray;
    }

    internal void CheckDisposed()
    {
      if (this.IsDisposed)
        throw new ObjectDisposedException("MathWorks.MATLAB.Arrays.MWArray", MWArray.resourceManager.GetString("MWErrorObjectDisposed"));
    }

    internal void DestroyMXArray()
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        if (!MWArray.nativeGCEnabled)
          return;
        long bytesAllocated = this.ElementSize * this.NumElements;
        if (this._hMXArray == null || this._hMXArray.IsInvalid || 0L >= bytesAllocated)
          return;
        this._hMXArray.Dispose();
        GC.RemoveMemoryPressure(bytesAllocated);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    internal MWSafeHandle DetachMXArray()
    {
      MWSafeHandle hMxArray = this._hMXArray;
      this._hMXArray.Dispose();
      return hMxArray;
    }

    internal MWSafeHandle ArrayIndexer(MWArray srcArray, params int[] indices)
    {
      try
      {
        Monitor.Enter(MWArray.mxSync);
        IntPtr[] indices1 = new IntPtr[indices.Length];
        for (int index = 0; index < indices.Length; ++index)
          indices1[index] = (IntPtr) indices[index];
        MWSafeHandle hMXArraySrcElem;
        if (MWArray.mclMXArrayGet(out hMXArraySrcElem, srcArray._hMXArray, (IntPtr) indices.Length, indices1) != 0)
          throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidIndex"));
        return hMXArraySrcElem;
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    internal void ArrayIndexer(MWArray valueArray, MWArray targetArray, params int[] indices)
    {
      IntPtr[] indices1 = new IntPtr[indices.Length];
      for (int index = 0; index < indices.Length; ++index)
        indices1[index] = (IntPtr) indices[index];
      try
      {
        Monitor.Enter(MWArray.mxSync);
        switch (targetArray.ArrayType)
        {
          case MWArrayType.Numeric:
          case MWArrayType.Character:
          case MWArrayType.Cell:
            if (MWArray.mclMXArraySet(targetArray._hMXArray, valueArray._hMXArray, (IntPtr) indices.Length, indices1) == 0)
              break;
            throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidIndex"));
          case MWArrayType.Logical:
            if (MWArray.mclMXArraySetLogical(targetArray._hMXArray, valueArray._hMXArray, (IntPtr) indices.Length, indices1) == 0)
              break;
            throw new Exception(MWArray.resourceManager.GetString("MWErrorInvalidIndex"));
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null || !(obj is MWArray))
        return false;
      if (MWArrayType.NativeObjArray == this.array_Type)
        return ((MWObjectArray) this).Equals(obj);
      MWArray mwArray = (MWArray) obj;
      this.CheckDisposed();
      mwArray.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        return (byte) 1 == MWArray.mclIsIdentical(this._hMXArray, mwArray._hMXArray);
      }
      catch
      {
        return false;
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public override int GetHashCode()
    {
      this.CheckDisposed();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        return MWArray.ut_hash_pointer(0, this.MXArrayHandle);
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public override string ToString()
    {
      this.CheckDisposed();
      string formattedString = "[]";
      try
      {
        Monitor.Enter(MWArray.mxSync);
        if (!this.IsEmpty)
        {
          char ch1 = '\n';
          char ch2 = ' ';
          if (MWArray.mclcppArrayToString(this.MXArrayHandle, out formattedString) != 0)
            throw new ApplicationException(MWArray.resourceManager.GetString("MWErrorFormatError"));
          formattedString = formattedString.TrimStart(ch1);
          formattedString = formattedString.TrimEnd(ch1);
          if (-1 == formattedString.IndexOf(ch1))
          {
            formattedString = formattedString.TrimStart(ch2);
            formattedString = formattedString.TrimEnd(ch2);
          }
        }
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
      return formattedString;
    }

    public virtual object Clone()
    {
      this.CheckDisposed();
      if (MWArrayType.NativeObjArray == this.array_Type)
        return this.Clone();
      MWArray mwArray = (MWArray) this.MemberwiseClone();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        this.SetMXArray(MWArray.mxDuplicateArray(this._hMXArray), MWArrayType.Array);
        return (object) mwArray;
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public virtual Array ToArray()
    {
      throw new InvalidDataException("Data type unsupported by MATLAB .NET Assembly for conversion to array");
    }

    internal virtual bool IsValidConversion(Type t)
    {
      throw new NotImplementedException();
    }

    protected int[] GetNextSubscript(int[] dimensions, int[] subscripts, int index)
    {
      int index1 = dimensions.Length - (index + 1);
      if (subscripts[index1] < dimensions[index1] - 1)
      {
        ++subscripts[index1];
        return subscripts;
      }
      subscripts[index1] = 0;
      if (index1 != 0)
        return this.GetNextSubscript(dimensions, subscripts, index + 1);
      return subscripts;
    }

    internal static T[] ConvertMatrixToVector<T>(T[,] src)
    {
      T[] objArray = new T[src.Length];
      int num = 0;
      int length1 = src.GetLength(0);
      int length2 = src.GetLength(1);
      for (int index1 = 0; index1 < length1; ++index1)
      {
        for (int index2 = 0; index2 < length2; ++index2)
          objArray[num++] = src[index1, index2];
      }
      return objArray;
    }

    internal static void PopulateTypeMap()
    {
      MWArray.typeMap.Add(typeof (byte), new Type[6]
      {
        typeof (byte),
        typeof (short),
        typeof (int),
        typeof (long),
        typeof (float),
        typeof (double)
      });
      MWArray.typeMap.Add(typeof (short), new Type[5]
      {
        typeof (short),
        typeof (int),
        typeof (long),
        typeof (float),
        typeof (double)
      });
      MWArray.typeMap.Add(typeof (int), new Type[4]
      {
        typeof (int),
        typeof (long),
        typeof (float),
        typeof (double)
      });
      MWArray.typeMap.Add(typeof (long), new Type[3]
      {
        typeof (long),
        typeof (float),
        typeof (double)
      });
      MWArray.typeMap.Add(typeof (float), new Type[2]
      {
        typeof (float),
        typeof (double)
      });
      MWArray.typeMap.Add(typeof (double), new Type[1]
      {
        typeof (double)
      });
    }

    internal virtual object ConvertToType(Type t)
    {
      throw new NotImplementedException();
    }

    public static object[] ConvertToNativeTypes(MWArray[] src, Type[] specifiedTypes)
    {
      int length = src.Length;
      object[] objArray = new object[length];
      lock (MWArray.mxSync)
      {
        for (int index = 0; index < length; ++index)
        {
          src[index].CheckDisposed();
          objArray[index] = src[index].ConvertToType(specifiedTypes[index]);
        }
        return objArray;
      }
    }

    internal bool IsScalar()
    {
      if (this.NumberofDimensions == 2)
        return this.NumberOfElements == 1;
      return false;
    }

    internal bool IsVector()
    {
      if (this.NumberofDimensions != 2)
        return false;
      if (this.Dimensions[0] != 1)
        return this.Dimensions[1] == 1;
      return true;
    }

    internal Array AllocateNativeArray(Type t, out int[] nativeArrayDims)
    {
      lock (MWArray.mxSync)
      {
        int[] dimensions = this.Dimensions;
        int length = dimensions.Length;
        nativeArrayDims = new int[length];
        nativeArrayDims[length - 2] = dimensions[0];
        nativeArrayDims[length - 1] = dimensions[1];
        for (int index = 2; index < length; ++index)
          nativeArrayDims[length - (index + 1)] = dimensions[index];
        return Array.CreateInstance(t, nativeArrayDims);
      }
    }

    internal static int[] NetDimensionToMATLABDimension(int[] dimensions)
    {
      int length = dimensions.Length;
      int[] numArray = new int[Math.Max(length, 2)];
      numArray[0] = length > 1 ? dimensions[length - 2] : 1;
      numArray[1] = dimensions[length - 1];
      for (int index = 0; index < length - 2; ++index)
        numArray[length - index - 1] = dimensions[index];
      return numArray;
    }

    internal static int[] MATLABDimensionToNetDimension(int[] dimensions)
    {
      int length = dimensions.Length;
      int[] numArray = new int[length];
      numArray[length - 2] = dimensions[0];
      numArray[length - 1] = dimensions[1];
      for (int index = 0; index < length - 2; ++index)
        numArray[index] = dimensions[length - index - 1];
      return numArray;
    }

    internal static string RankToString(int rank)
    {
      StringBuilder stringBuilder = new StringBuilder("[", rank + 1);
      stringBuilder.Length = rank + 1;
      for (int index = 1; index < rank; ++index)
        stringBuilder[index] = ',';
      stringBuilder[rank] = ']';
      return stringBuilder.ToString();
    }

    internal enum MATLABArrayType
    {
      Unknown,
      Cell,
      Struct,
      Logical,
      Char,
      Index,
      Double,
      Single,
      Int8,
      UInt8,
      Int16,
      UInt16,
      Int32,
      UInt32,
      Int64,
      UInt64,
      Function,
      Opaque,
      Object,
    }
  }
}
