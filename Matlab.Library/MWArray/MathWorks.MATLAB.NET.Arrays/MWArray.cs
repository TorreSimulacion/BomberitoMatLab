using MathWorks.MATLAB.NET.Arrays.native;
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
			Object
		}

		private static bool nativeGCEnabled;

		private static long nativeGCBlockSize;

		internal static ResourceManager resourceManager;

		internal static StringBuilder formattedOutputString;

		internal static object mxSync;

		internal static Dictionary<Type, Type[]> typeMap;

		private MWSafeHandle _hMXArray = new MWSafeHandle();

		internal MWArrayType array_Type;

		internal long NumElements;

		internal long ElementSize;

		public MWArray this[params int[] indices]
		{
			get
			{
				switch (array_Type)
				{
				case MWArrayType.Logical:
					return ((MWLogicalArray)this)[indices];
				case MWArrayType.Numeric:
					return ((MWNumericArray)this)[indices];
				case MWArrayType.Structure:
				{
					string string2 = resourceManager.GetString("MWErrorNotSupported");
					throw new Exception(string2);
				}
				case MWArrayType.Cell:
					return ((MWCellArray)this)[indices];
				case MWArrayType.NativeObjArray:
				{
					string @string = resourceManager.GetString("MWErrorNotSupported");
					throw new Exception(@string);
				}
				default:
					return GetTypedArray(ArrayIndexer(this, indices));
				}
			}
			set
			{
				switch (array_Type)
				{
				case MWArrayType.Logical:
					if (array_Type != value.array_Type)
					{
						string string3 = resourceManager.GetString("MWErrorDataArrayType");
						throw new InvalidCastException(string3);
					}
					((MWLogicalArray)this)[indices] = (MWLogicalArray)value;
					break;
				case MWArrayType.Numeric:
					if (array_Type != value.array_Type)
					{
						string string2 = resourceManager.GetString("MWErrorDataArrayType");
						throw new InvalidCastException(string2);
					}
					((MWNumericArray)this)[indices] = (MWNumericArray)value;
					break;
				case MWArrayType.Structure:
				{
					string string4 = resourceManager.GetString("MWErrorNotSupported");
					throw new Exception(string4);
				}
				case MWArrayType.Cell:
					((MWCellArray)this)[indices] = value;
					break;
				case MWArrayType.NativeObjArray:
					if (array_Type != value.array_Type)
					{
						string @string = resourceManager.GetString("MWErrorDataArrayType");
						throw new InvalidCastException(@string);
					}
					((MWObjectArray)this)[indices] = (MWObjectArray)value;
					break;
				default:
					ArrayIndexer(value, this, indices);
					break;
				}
			}
		}

		public static bool NativeGCEnabled
		{
			get
			{
				return nativeGCEnabled;
			}
			set
			{
				nativeGCEnabled = value;
			}
		}

		public static long NativeGCBlockSize => nativeGCBlockSize;

		public MWArrayType ArrayType
		{
			get
			{
				CheckDisposed();
				return array_Type;
			}
		}

		public virtual int[] Dimensions
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(mxSync);
					MWSafeHandle mXArrayHandle = MXArrayHandle;
					int num = mxGetNumberOfDimensions(mXArrayHandle);
					int[] array = new int[num];
					IntPtr source = mxGetDimensions(mXArrayHandle);
					Marshal.Copy(source, array, 0, num);
					return array;
				}
				finally
				{
					Monitor.Exit(mxSync);
				}
			}
		}

		public virtual bool IsDisposed
		{
			get
			{
				if (MWArrayType.NativeObjArray == array_Type)
				{
					return ((MWObjectArray)this).IsDisposed;
				}
				return _hMXArray.IsInvalid;
			}
		}

		public virtual bool IsEmpty
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(mxSync);
					return 1 == mxIsEmpty(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(mxSync);
				}
			}
		}

		public bool IsCellArray
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(mxSync);
					return 1 == mxIsCell(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(mxSync);
				}
			}
		}

		public bool IsCharArray
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(mxSync);
					return 1 == mxIsChar(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(mxSync);
				}
			}
		}

		public bool IsLogicalArray
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(mxSync);
					return 1 == mxIsLogical(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(mxSync);
				}
			}
		}

		public bool IsNumericArray
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(mxSync);
					return 1 == mxIsNumeric(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(mxSync);
				}
			}
		}

		public bool IsStructArray
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(mxSync);
					return 1 == mxIsStruct(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(mxSync);
				}
			}
		}

		public int NumberofDimensions
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(mxSync);
					return mxGetNumberOfDimensions(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(mxSync);
				}
			}
		}

		public int NumberOfElements
		{
			get
			{
				CheckDisposed();
				MWSafeHandle mXArrayHandle = MXArrayHandle;
				try
				{
					Monitor.Enter(mxSync);
					return (mxIsSparse(mXArrayHandle) == 0) ? mxGetNumberOfElements(mXArrayHandle) : 0;
				}
				finally
				{
					Monitor.Exit(mxSync);
				}
			}
		}

		internal virtual MWSafeHandle MXArrayHandle
		{
			get
			{
				MWSafeHandle result = _hMXArray;
				if (MWArrayType.NativeObjArray == array_Type)
				{
					result = ((MWObjectArray)this).MXArrayHandle;
				}
				return result;
			}
		}

		internal MATLABArrayType MATLABType
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(mxSync);
					return (MATLABArrayType)mxGetClassID(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(mxSync);
				}
			}
		}

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclmcrInitialize2_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern bool mclmcrInitialize2(int primaryMode);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxCreateString_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern MWSafeHandle mxCreateString([In] IntPtr pString);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMxDeserialize_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern MWSafeHandle mxDeserialize([In] IntPtr pSerializedBuffer, [In] int size);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxDuplicateArray_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern MWSafeHandle mxDuplicateArray([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxFree_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern void mxFree([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetClassID_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mxGetClassID([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetData_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern IntPtr mxGetData([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetDimensions_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern IntPtr mxGetDimensions([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetElementSize_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mxGetElementSize([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetM_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mxGetM([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetN_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mxGetN([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetNumberOfDimensions_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mxGetNumberOfDimensions([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetNumberOfElements_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mxGetNumberOfElements([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetScalar_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern double mxGetScalar([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMxIsA_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsA([In] MWSafeHandle hMXArray, [In] string typeName);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMxIsRef_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsRef([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMxRefIsA_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxRefIsA([In] MWSafeHandle hMXArray, [In] string typeName);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMxReleaseRef_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern IntPtr mxReleaseRef([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsObject_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsObject([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsOpaque_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsOpaque([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsCell_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsCell([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsChar_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsChar([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsEmpty_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsEmpty([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsLogical_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsLogical([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsNumeric_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsNumeric([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsSparse_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsSparse([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsStruct_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsStruct([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMxSerialize_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern MWSafeHandle mxSerialize([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "array_handle_get_int_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern IntPtr array_handle_get_int([In] IntPtr pMXArray, [In] IntPtr numIndices, [In] IntPtr[] indices);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclArrayHandle2mxArray_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mclArrayHandle2mxArray(out MWSafeHandle hMXArray, [In] IntPtr pArrayHandle);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclcppArrayToString_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern int mclcppArrayToString([In] MWSafeHandle hMXArray, out string formattedString);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclIsIdentical_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mclIsIdentical([In] MWSafeHandle hMXArray1, [In] MWSafeHandle hMXArray2);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclmxArray2ArrayHandle_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mclmxArray2ArrayHandle(out IntPtr pArrayHandle, [In] IntPtr pMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "array_handle_set_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int array_handle_set([In] IntPtr pArrayHandle, [In] IntPtr pArrayHandleValue);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "array_handle_set_logical_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern int array_handle_set_logical([In] IntPtr pArrayHandle, [In] MWSafeHandle hMXArray, [In] IntPtr len);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMXArrayGet_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mclMXArrayGet(out MWSafeHandle hMXArraySrcElem, [In] MWSafeHandle hMXArray, [In] IntPtr numIndices, [In] IntPtr[] indices);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMXArraySet_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern int mclMXArraySet([In] MWSafeHandle hMXArrayTrg, [In] MWSafeHandle hMXArraySrcElem, [In] IntPtr numIndices, [In] IntPtr[] indices);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMXArraySetLogical_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern int mclMXArraySetLogical([In] MWSafeHandle hMXArrayTrg, [In] MWSafeHandle hMXArraySrcElem, [In] IntPtr numIndices, [In] IntPtr[] indices);

		[DllImport("libut.dll")]
		[SuppressUnmanagedCodeSecurity]
		private static extern int ut_hash_pointer([Out] int hashCode, [In] MWSafeHandle hMXArray);

		static MWArray()
		{
			nativeGCEnabled = true;
			nativeGCBlockSize = 10000000L;
			formattedOutputString = null;
			mxSync = new object[0];
			typeMap = new Dictionary<Type, Type[]>();
			try
			{
				PopulateTypeMap();
				Monitor.Enter(mxSync);
				bool mCRAppInitialized = MWMCR.MCRAppInitialized;
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				NativeGCAttribute nativeGCAttribute = (entryAssembly != null) ? ((NativeGCAttribute)Attribute.GetCustomAttribute(entryAssembly, typeof(NativeGCAttribute))) : null;
				if (nativeGCAttribute != null && (nativeGCEnabled = nativeGCAttribute.GCEnabled) && 0 < nativeGCAttribute.GCBlockSize)
				{
					nativeGCBlockSize = (long)((double)nativeGCAttribute.GCBlockSize * 10000000.0);
				}
				resourceManager = MWResources.getResourceManager();
				mclmcrInitialize2(3);
				formattedOutputString = new StringBuilder(1024);
			}
			finally
			{
				Monitor.Exit(mxSync);
			}
		}

		internal MWArray()
		{
		}

		internal MWArray(MWSafeHandle hMXArray)
		{
			SetMXArray(hMXArray, MWArrayType.Array);
		}

		public static void DisposeArray(object _object)
		{
			if (_object == null)
			{
				return;
			}
			if (_object is IDisposable)
			{
				((IDisposable)_object).Dispose();
			}
			else if (_object is MWArray[])
			{
				MWArray[] array = (MWArray[])_object;
				MWArray[] array2 = array;
				foreach (object @object in array2)
				{
					DisposeArray(@object);
				}
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			try
			{
				Monitor.Enter(mxSync);
				if (_hMXArray != null && !IsDisposed && MWMCR.MCRAppInitialized)
				{
					_hMXArray.Dispose();
				}
			}
			finally
			{
				Monitor.Exit(mxSync);
			}
		}

		public static implicit operator MWArray(double scalar)
		{
			return new MWNumericArray(scalar);
		}

		public static implicit operator MWArray(string value)
		{
			return new MWCharArray(value);
		}

		internal MWArray(SerializationInfo serializationInfo, StreamingContext context)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				Monitor.Enter(mxSync);
				byte[] array = (byte[])serializationInfo.GetValue("serializedState", typeof(byte[]));
				array_Type = (MWArrayType)serializationInfo.GetValue("array_Type", typeof(MWArrayType));
				int num = array.Length;
				intPtr = Marshal.AllocCoTaskMem(num);
				Marshal.Copy(array, 0, intPtr, num);
				SetMXArray(mxDeserialize(intPtr, num), array_Type);
			}
			finally
			{
				if (IntPtr.Zero != intPtr)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
				Monitor.Exit(mxSync);
			}
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			MWSafeHandle mWSafeHandle = null;
			IntPtr zero = IntPtr.Zero;
			try
			{
				Monitor.Enter(mxSync);
				mWSafeHandle = mxSerialize(_hMXArray);
				int num = mxGetNumberOfElements(mWSafeHandle);
				zero = mxGetData(mWSafeHandle);
				byte[] array = new byte[num];
				Marshal.Copy(zero, array, 0, num);
				info.AddValue("serializedState", array);
				info.AddValue("array_Type", array_Type);
			}
			finally
			{
				if (mWSafeHandle != null && !mWSafeHandle.IsInvalid)
				{
					mWSafeHandle.Dispose();
				}
				Monitor.Exit(mxSync);
			}
		}

		internal static MWArray GetTypedArray(MWSafeHandle hMXArray)
		{
			try
			{
				Monitor.Enter(mxSync);
				MWArray result = null;
				if (hMXArray.IsInvalid)
				{
					return MWNumericArray.Empty;
				}
				switch (mxGetClassID(hMXArray))
				{
				case 0:
				case 16:
				case 17:
				case 18:
					result = new MWArray(hMXArray);
					break;
				case 1:
					result = new MWCellArray(hMXArray);
					break;
				case 3:
					result = new MWLogicalArray(hMXArray);
					break;
				case 4:
					result = new MWCharArray(hMXArray);
					break;
				case 5:
					result = new MWIndexArray(hMXArray);
					break;
				case 6:
				case 7:
				case 9:
				case 10:
				case 12:
				case 14:
					result = new MWNumericArray(hMXArray);
					break;
				case 8:
				case 11:
				case 13:
				case 15:
				{
					string @string = resourceManager.GetString("MWErrorInvalidReturnType");
					throw new ArgumentOutOfRangeException("numArgsOut", @string);
				}
				case 2:
					result = new MWStructArray(hMXArray);
					break;
				default:
					if (mxIsRef(hMXArray) != 0 && (mxRefIsA(hMXArray, "System.Object") != 0 || mxRefIsA(hMXArray, "int32") != 0))
					{
						result = new MWObjectArray(new MWSafeHandle(mxReleaseRef(hMXArray)));
						hMXArray.SetHandleAsInvalid();
					}
					else if (mxIsA(hMXArray, "System.Object") != 0 || mxIsA(hMXArray, "int32") != 0)
					{
						result = new MWObjectArray(hMXArray);
					}
					else if (mxIsObject(hMXArray) != 0 || mxIsOpaque(hMXArray) != 0)
					{
						result = new MWArray(hMXArray);
					}
					break;
				}
				return result;
			}
			finally
			{
				Monitor.Exit(mxSync);
			}
		}

		internal static bool IsMWArray(object objIn)
		{
			Type type = objIn.GetType();
			if (type == typeof(MWArray) || type.IsSubclassOf(typeof(MWArray)))
			{
				return true;
			}
			return false;
		}

		internal static MWArray ConvertObjectToMWArray(object objIn)
		{
			MWArray mWArray = null;
			if (objIn != null)
			{
				if (IsMWArray(objIn))
				{
					return (MWArray)objIn;
				}
				if (objIn.GetType() == typeof(string))
				{
					return new MWCharArray((string)objIn);
				}
				if (objIn.GetType().IsValueType && objIn.GetType().IsPrimitive)
				{
					return GetMWArrayFromValueType(objIn);
				}
				if (objIn.GetType().IsArray && IsArrayOfSupportedType(objIn))
				{
					string arrType = ArrayElementTypeName(objIn);
					return GetMWArrayFromArrayType((Array)objIn, arrType);
				}
				if (objIn.GetType().IsArray && IsJaggedArrayOfSupportedType((Array)objIn))
				{
					return GetMWArrayFromJaggedArrayType((Array)objIn);
				}
				if (objIn.GetType().IsValueType && !objIn.GetType().IsPrimitive)
				{
					return GetMWStructArrayFromNETStructs((ValueType)objIn);
				}
				if (objIn.GetType().IsArray && objIn.GetType().GetElementType().IsValueType && !objIn.GetType().GetElementType().IsPrimitive)
				{
					return GetMWStructArrayFromNETStructs((Array)objIn);
				}
				if (objIn.GetType() == typeof(MathWorks.MATLAB.NET.Arrays.native.MWStructArray))
				{
					MathWorks.MATLAB.NET.Arrays.native.MWStructArray mWStructArray = (MathWorks.MATLAB.NET.Arrays.native.MWStructArray)objIn;
					MWStructArray mWStructArray2 = new MWStructArray(mWStructArray.Dimensions, mWStructArray.FieldNames);
					for (int i = 1; i <= mWStructArray.NumberOfElements; i++)
					{
						for (int j = 0; j < mWStructArray.NumberOfFields; j++)
						{
							object obj = mWStructArray[mWStructArray.FieldNames[j], new int[1]
							{
								i
							}];
							if (obj != null)
							{
								MWArray value = ConvertObjectToMWArray(obj);
								mWStructArray2[mWStructArray.FieldNames[j], new int[1]
								{
									i
								}] = value;
							}
						}
					}
					return mWStructArray2;
				}
				if (objIn.GetType() == typeof(MathWorks.MATLAB.NET.Arrays.native.MWCellArray))
				{
					MathWorks.MATLAB.NET.Arrays.native.MWCellArray mWCellArray = (MathWorks.MATLAB.NET.Arrays.native.MWCellArray)objIn;
					MWCellArray mWCellArray2 = new MWCellArray(mWCellArray.Dimensions);
					for (int k = 1; k <= mWCellArray.NumberOfElements; k++)
					{
						object obj2 = mWCellArray.get(k);
						if (obj2 != null)
						{
							MWArray valueArray = ConvertObjectToMWArray(obj2);
							mWCellArray2.ArrayIndexer(valueArray, mWCellArray2, k);
						}
					}
					return mWCellArray2;
				}
				if (objIn.GetType() == typeof(Hashtable) || IsDictionaryOfSupportedType(objIn))
				{
					return GetMWStructArrayFromIDictionary((IDictionary)objIn);
				}
				if (objIn.GetType() == typeof(ArrayList))
				{
					return GetMWCellArrayFromArrayList((ArrayList)objIn);
				}
				if (AppDomain.CurrentDomain.IsDefaultAppDomain())
				{
					return new MWObjectArray(objIn);
				}
				if (!AppDomain.CurrentDomain.IsDefaultAppDomain() && objIn.GetType().IsSerializable)
				{
					return new MWObjectArray(objIn);
				}
				throw new InvalidDataException("Input data type unsupported by MATLAB .NET Assembly");
			}
			return new MWNumericArray();
		}

		private static MWArray GetMWArrayFromJaggedArrayType(Array array)
		{
			Type elementType = JaggedArray.GetElementType(array.GetType());
			if (IsCTSNumericType(elementType))
			{
				return new MWNumericArray(array);
			}
			throw new InvalidDataException("Input data type unsupported by MATLAB .NET Assembly");
		}

		private static MWArray GetMWCellArrayFromArrayList(ArrayList objIn)
		{
			MWCellArray mWCellArray = null;
			int count = objIn.Count;
			if (count == 0)
			{
				return new MWCellArray();
			}
			mWCellArray = new MWCellArray(1, count);
			int num = 1;
			foreach (object item in objIn)
			{
				mWCellArray[new int[1]
				{
					num++
				}] = ConvertObjectToMWArray(item);
			}
			return mWCellArray;
		}

		private static bool IsDictionaryOfSupportedType(object objIn)
		{
			Type type = objIn.GetType();
			if (type.GetInterface("IDictionary") != null)
			{
				Type[] genericArguments = type.GetGenericArguments();
				if (genericArguments.Length == 2 && genericArguments[0] == typeof(string) && (IsSupportedType(genericArguments[1]) || (type.IsArray && IsArrayOfSupportedType(objIn))))
				{
					return true;
				}
			}
			return false;
		}

		internal static MWStructArray GetMWStructArrayFromIDictionary(IDictionary objIn)
		{
			if (objIn.Count == 0)
			{
				throw new InvalidDataException("Cannot convert an empty Hashtable");
			}
			string[] array = new string[objIn.Count];
			int num = 0;
			foreach (object key in objIn.Keys)
			{
				array[num++] = (string)key;
			}
			MWStructArray mWStructArray = new MWStructArray(1, 1, array);
			num = 0;
			foreach (object value in objIn.Values)
			{
				mWStructArray[array[num++]] = ConvertObjectToMWArray(value);
			}
			return mWStructArray;
		}

		internal static MWStructArray GetMWStructArrayFromNETStructs(ValueType structIn)
		{
			FieldInfo[] fields = structIn.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			if (fields.Length == 0)
			{
				throw new InvalidDataException($"Type {structIn.GetType().FullName} does not contain any declared public fields");
			}
			string[] array = new string[fields.Length];
			int num = 0;
			FieldInfo[] array2 = fields;
			foreach (FieldInfo fieldInfo in array2)
			{
				array[num++] = fieldInfo.Name;
			}
			MWStructArray mWStructArray = new MWStructArray(1, 1, array);
			num = 0;
			FieldInfo[] array3 = fields;
			foreach (FieldInfo fieldInfo2 in array3)
			{
				mWStructArray[array[num++]] = ConvertObjectToMWArray(fieldInfo2.GetValue(structIn));
			}
			return mWStructArray;
		}

		internal static MWStructArray GetMWStructArrayFromNETStructs(Array structArrIn)
		{
			if (structArrIn.Rank > 2)
			{
				throw new InvalidDataException($"Cannot convert an Array of {structArrIn.GetType().GetElementType().FullName} with rank more than 2");
			}
			FieldInfo[] fields = structArrIn.GetType().GetElementType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			int num = 0;
			int num2 = 0;
			if (structArrIn.Rank == 1)
			{
				num = 1;
				num2 = structArrIn.GetLength(0);
			}
			else
			{
				num = structArrIn.GetLength(0);
				num2 = structArrIn.GetLength(1);
			}
			string[] array = new string[fields.Length];
			int num3 = 0;
			FieldInfo[] array2 = fields;
			foreach (FieldInfo fieldInfo in array2)
			{
				array[num3++] = fieldInfo.Name;
			}
			MWStructArray mWStructArray = new MWStructArray(num, num2, array);
			int[] columnMajorIndices = GetColumnMajorIndices(num, num2);
			int num5 = 0;
			foreach (object item in structArrIn)
			{
				FieldInfo[] array3 = fields;
				foreach (FieldInfo fieldInfo2 in array3)
				{
					mWStructArray[fieldInfo2.Name, new int[1]
					{
						columnMajorIndices[num5]
					}] = ConvertObjectToMWArray(fieldInfo2.GetValue(item));
				}
				num5++;
			}
			return mWStructArray;
		}

		internal static int[] GetColumnMajorIndices(int row, int col)
		{
			int[] array = new int[row * col];
			int num = 0;
			for (int i = 0; i < row; i++)
			{
				for (int j = 0; j < col; j++)
				{
					array[num++] = j * row + i + 1;
				}
			}
			return array;
		}

		internal static void RecursiveFieldCopy(ref int[] elementIdx, int dimIdx, ref MWStructArray outArray, ref Array inArray, ref FieldInfo[] infoFields)
		{
			if (dimIdx < outArray.NumberofDimensions - 1)
			{
				for (int i = 0; i < inArray.GetLength(dimIdx); i++)
				{
					RecursiveFieldCopy(ref elementIdx, dimIdx + 1, ref outArray, ref inArray, ref infoFields);
					elementIdx[dimIdx]++;
				}
				elementIdx[dimIdx + 1] = 0;
				return;
			}
			for (int j = 0; j < inArray.GetLength(dimIdx); j++)
			{
				FieldInfo[] array = infoFields;
				foreach (FieldInfo fieldInfo in array)
				{
					outArray[fieldInfo.Name, elementIdx] = ConvertObjectToMWArray(fieldInfo.GetValue(inArray.GetValue(elementIdx)));
				}
				elementIdx[dimIdx]++;
			}
			elementIdx[dimIdx] = 0;
		}

		internal static MWArray GetMWArrayFromValueType(object objIn)
		{
			MWArray mWArray = null;
			string fullName = objIn.GetType().FullName;
			switch (fullName)
			{
			case "System.Double":
				return new MWNumericArray((double)objIn);
			case "System.Single":
				return new MWNumericArray((float)objIn, makeDouble: false);
			case "System.Int64":
				return new MWNumericArray((long)objIn, makeDouble: false);
			case "System.Int32":
				return new MWNumericArray((int)objIn, makeDouble: false);
			case "System.Int16":
				return new MWNumericArray((short)objIn, makeDouble: false);
			case "System.Byte":
				return new MWNumericArray((byte)objIn, makeDouble: false);
			case "System.Boolean":
				return new MWLogicalArray((bool)objIn);
			case "System.Char":
				return new MWCharArray((char)objIn);
			default:
				throw new InvalidDataException("Data type unsupported by MATLAB .NET Assembly");
			}
		}

		internal static MWArray GetMWArrayFromArrayType(Array arrIn, string arrType)
		{
			MWArray mWArray = null;
			switch (arrType)
			{
			case "System.Double":
				return new MWNumericArray(arrIn);
			case "System.Single":
				return new MWNumericArray(arrIn, makeDouble: false, rowMajorData: true);
			case "System.Int64":
				return new MWNumericArray(arrIn, makeDouble: false, rowMajorData: true);
			case "System.Int32":
				return new MWNumericArray(arrIn, makeDouble: false, rowMajorData: true);
			case "System.Int16":
				return new MWNumericArray(arrIn, makeDouble: false, rowMajorData: true);
			case "System.Byte":
				return new MWNumericArray(arrIn, makeDouble: false, rowMajorData: true);
			case "System.Boolean":
				return new MWLogicalArray(arrIn);
			case "System.String":
				return new MWCharArray(arrIn);
			case "System.Char":
				return new MWCharArray(arrIn);
			default:
				throw new InvalidDataException("Data type unsupported by MATLAB .NET Assembly for input");
			}
		}

		internal static bool IsArrayOfSupportedType(object objIn)
		{
			Type elementType = objIn.GetType().GetElementType();
			if (elementType.IsArray)
			{
				return false;
			}
			string fullName = elementType.FullName;
			bool result = false;
			if (fullName.Contains("System.Double") || fullName.Contains("System.Single") || fullName.Contains("System.Int64") || fullName.Contains("System.Int32") || fullName.Contains("System.Int16") || fullName.Contains("System.Byte") || fullName.Contains("System.Boolean") || fullName.Contains("System.String") || fullName.Contains("System.Char"))
			{
				result = true;
			}
			return result;
		}

		private static bool IsCTSNumericType(Type t)
		{
			if (t == typeof(byte) || t == typeof(short) || t == typeof(int) || t == typeof(long) || t == typeof(float) || t == typeof(double))
			{
				return true;
			}
			return false;
		}

		internal static bool IsJaggedArrayOfSupportedType(Array objIn)
		{
			if (!JaggedArray.IsJagged(objIn))
			{
				return false;
			}
			Type elementType = JaggedArray.GetElementType(objIn.GetType());
			return IsCTSNumericType(elementType);
		}

		internal static bool IsSupportedType(Type t)
		{
			string fullName = t.FullName;
			bool result = false;
			if (fullName.Contains("System.Double") || fullName.Contains("System.Single") || fullName.Contains("System.Int64") || fullName.Contains("System.Int32") || fullName.Contains("System.Int16") || fullName.Contains("System.Byte") || fullName.Contains("System.Boolean") || fullName.Contains("System.String") || fullName.Contains("System.Char"))
			{
				result = true;
			}
			return result;
		}

		internal static string ArrayElementTypeName(object objIn)
		{
			string result = "";
			string fullName = objIn.GetType().FullName;
			if (fullName.Contains("System.Double"))
			{
				result = "System.Double";
			}
			else if (fullName.Contains("System.Single"))
			{
				result = "System.Single";
			}
			else if (fullName.Contains("System.Int64"))
			{
				result = "System.Int64";
			}
			else if (fullName.Contains("System.Int32"))
			{
				result = "System.Int32";
			}
			else if (fullName.Contains("System.Int16"))
			{
				result = "System.Int16";
			}
			else if (fullName.Contains("System.Byte"))
			{
				result = "System.Byte";
			}
			else if (fullName.Contains("System.Boolean"))
			{
				result = "System.Boolean";
			}
			else if (fullName.Contains("System.String"))
			{
				result = "System.String";
			}
			else if (fullName.Contains("System.Char"))
			{
				result = "System.Char";
			}
			return result;
		}

		internal MWSafeHandle SetMXArray(MWSafeHandle hMXArray, MWArrayType arrayType, int numDimensions, int[] dimensions)
		{
			SetMXArray(hMXArray, arrayType, numDimensions, dimensions, isComplex: false);
			return _hMXArray;
		}

		internal MWSafeHandle SetMXArray(MWSafeHandle hMXArray, MWArrayType arrayType, int numDimensions, int[] dimensions, bool isComplex)
		{
			if (hMXArray.IsInvalid)
			{
				string @string = resourceManager.GetString("MWErrorAllocatingMXArray");
				throw new OutOfMemoryException(@string);
			}
			_hMXArray = hMXArray;
			array_Type = arrayType;
			NumElements = 1L;
			foreach (int num in dimensions)
			{
				NumElements *= num;
			}
			if (isComplex)
			{
				NumElements *= 2L;
			}
			try
			{
				Monitor.Enter(mxSync);
				ElementSize = mxGetElementSize(_hMXArray);
				if (nativeGCEnabled)
				{
					long num2 = ElementSize * NumElements;
					if (0 < num2)
					{
						_hMXArray.UnmanagedBytesAllocated = num2;
					}
				}
			}
			finally
			{
				Monitor.Exit(mxSync);
			}
			return _hMXArray;
		}

		internal MWSafeHandle SetMXArray(MWSafeHandle hMXArray, MWArrayType arrayType)
		{
			if (hMXArray.IsInvalid)
			{
				string @string = resourceManager.GetString("MWErrorAllocatingMXArray");
				throw new OutOfMemoryException(@string);
			}
			_hMXArray = hMXArray;
			array_Type = arrayType;
			try
			{
				Monitor.Enter(mxSync);
				NumElements = NumberOfElements;
				ElementSize = mxGetElementSize(_hMXArray);
				if (nativeGCEnabled)
				{
					long num = ElementSize * NumElements;
					if (0 < num)
					{
						_hMXArray.UnmanagedBytesAllocated = num;
					}
				}
			}
			finally
			{
				Monitor.Exit(mxSync);
			}
			return _hMXArray;
		}

		internal void CheckDisposed()
		{
			if (IsDisposed)
			{
				string @string = resourceManager.GetString("MWErrorObjectDisposed");
				throw new ObjectDisposedException("MathWorks.MATLAB.Arrays.MWArray", @string);
			}
		}

		internal void DestroyMXArray()
		{
			try
			{
				Monitor.Enter(mxSync);
				if (nativeGCEnabled)
				{
					long num = ElementSize * NumElements;
					if (_hMXArray != null && !_hMXArray.IsInvalid && 0 < num)
					{
						_hMXArray.Dispose();
						GC.RemoveMemoryPressure(num);
					}
				}
			}
			finally
			{
				Monitor.Exit(mxSync);
			}
		}

		internal MWSafeHandle DetachMXArray()
		{
			MWSafeHandle hMXArray = _hMXArray;
			_hMXArray.Dispose();
			return hMXArray;
		}

		internal MWSafeHandle ArrayIndexer(MWArray srcArray, params int[] indices)
		{
			try
			{
				Monitor.Enter(mxSync);
				IntPtr[] array = new IntPtr[indices.Length];
				for (int i = 0; i < indices.Length; i++)
				{
					array[i] = (IntPtr)indices[i];
				}
				if (mclMXArrayGet(out MWSafeHandle hMXArraySrcElem, srcArray._hMXArray, (IntPtr)indices.Length, array) != 0)
				{
					string @string = resourceManager.GetString("MWErrorInvalidIndex");
					throw new Exception(@string);
				}
				return hMXArraySrcElem;
			}
			finally
			{
				Monitor.Exit(mxSync);
			}
		}

		internal void ArrayIndexer(MWArray valueArray, MWArray targetArray, params int[] indices)
		{
			IntPtr[] array = new IntPtr[indices.Length];
			for (int i = 0; i < indices.Length; i++)
			{
				array[i] = (IntPtr)indices[i];
			}
			try
			{
				Monitor.Enter(mxSync);
				switch (targetArray.ArrayType)
				{
				case MWArrayType.Logical:
					if (mclMXArraySetLogical(targetArray._hMXArray, valueArray._hMXArray, (IntPtr)indices.Length, array) != 0)
					{
						string string2 = resourceManager.GetString("MWErrorInvalidIndex");
						throw new Exception(string2);
					}
					break;
				case MWArrayType.Numeric:
				case MWArrayType.Character:
				case MWArrayType.Cell:
					if (mclMXArraySet(targetArray._hMXArray, valueArray._hMXArray, (IntPtr)indices.Length, array) != 0)
					{
						string @string = resourceManager.GetString("MWErrorInvalidIndex");
						throw new Exception(@string);
					}
					break;
				}
			}
			finally
			{
				Monitor.Exit(mxSync);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is MWArray))
			{
				return false;
			}
			if (MWArrayType.NativeObjArray == array_Type)
			{
				return ((MWObjectArray)this).Equals(obj);
			}
			MWArray mWArray = (MWArray)obj;
			CheckDisposed();
			mWArray.CheckDisposed();
			try
			{
				Monitor.Enter(mxSync);
				return 1 == mclIsIdentical(_hMXArray, mWArray._hMXArray);
			}
			catch
			{
				return false;
			}
			finally
			{
				Monitor.Exit(mxSync);
			}
		}

		public override int GetHashCode()
		{
			CheckDisposed();
			try
			{
				Monitor.Enter(mxSync);
				int hashCode = 0;
				return ut_hash_pointer(hashCode, MXArrayHandle);
			}
			finally
			{
				Monitor.Exit(mxSync);
			}
		}

		public override string ToString()
		{
			CheckDisposed();
			string formattedString = "[]";
			try
			{
				Monitor.Enter(mxSync);
				if (IsEmpty)
				{
					return formattedString;
				}
				char c = '\n';
				char c2 = ' ';
				if (mclcppArrayToString(MXArrayHandle, out formattedString) != 0)
				{
					string @string = resourceManager.GetString("MWErrorFormatError");
					throw new ApplicationException(@string);
				}
				formattedString = formattedString.TrimStart(c);
				formattedString = formattedString.TrimEnd(c);
				if (-1 == formattedString.IndexOf(c))
				{
					formattedString = formattedString.TrimStart(c2);
					return formattedString.TrimEnd(c2);
				}
				return formattedString;
			}
			finally
			{
				Monitor.Exit(mxSync);
			}
		}

		public virtual object Clone()
		{
			CheckDisposed();
			if (MWArrayType.NativeObjArray == array_Type)
			{
				return ((MWObjectArray)this).Clone();
			}
			MWArray result = (MWArray)MemberwiseClone();
			try
			{
				Monitor.Enter(mxSync);
				SetMXArray(mxDuplicateArray(_hMXArray), MWArrayType.Array);
				return result;
			}
			finally
			{
				Monitor.Exit(mxSync);
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
			int num = dimensions.Length;
			int num2 = num - (index + 1);
			if (subscripts[num2] < dimensions[num2] - 1)
			{
				subscripts[num2]++;
				return subscripts;
			}
			subscripts[num2] = 0;
			if (num2 != 0)
			{
				return GetNextSubscript(dimensions, subscripts, index + 1);
			}
			return subscripts;
		}

		internal static T[] ConvertMatrixToVector<T>(T[,] src)
		{
			T[] array = new T[src.Length];
			int num = 0;
			int length = src.GetLength(0);
			int length2 = src.GetLength(1);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					array[num++] = src[i, j];
				}
			}
			return array;
		}

		internal static void PopulateTypeMap()
		{
			typeMap.Add(typeof(byte), new Type[6]
			{
				typeof(byte),
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(float),
				typeof(double)
			});
			typeMap.Add(typeof(short), new Type[5]
			{
				typeof(short),
				typeof(int),
				typeof(long),
				typeof(float),
				typeof(double)
			});
			typeMap.Add(typeof(int), new Type[4]
			{
				typeof(int),
				typeof(long),
				typeof(float),
				typeof(double)
			});
			typeMap.Add(typeof(long), new Type[3]
			{
				typeof(long),
				typeof(float),
				typeof(double)
			});
			typeMap.Add(typeof(float), new Type[2]
			{
				typeof(float),
				typeof(double)
			});
			typeMap.Add(typeof(double), new Type[1]
			{
				typeof(double)
			});
		}

		internal virtual object ConvertToType(Type t)
		{
			throw new NotImplementedException();
		}

		public static object[] ConvertToNativeTypes(MWArray[] src, Type[] specifiedTypes)
		{
			int num = src.Length;
			object[] array = new object[num];
			lock (mxSync)
			{
				for (int i = 0; i < num; i++)
				{
					src[i].CheckDisposed();
					array[i] = src[i].ConvertToType(specifiedTypes[i]);
				}
				return array;
			}
		}

		internal bool IsScalar()
		{
			if (NumberofDimensions == 2)
			{
				return NumberOfElements == 1;
			}
			return false;
		}

		internal bool IsVector()
		{
			if (NumberofDimensions == 2)
			{
				if (Dimensions[0] != 1)
				{
					return Dimensions[1] == 1;
				}
				return true;
			}
			return false;
		}

		internal Array AllocateNativeArray(Type t, out int[] nativeArrayDims)
		{
			lock (mxSync)
			{
				int[] dimensions = Dimensions;
				int num = dimensions.Length;
				nativeArrayDims = new int[num];
				nativeArrayDims[num - 2] = dimensions[0];
				nativeArrayDims[num - 1] = dimensions[1];
				for (int i = 2; i < num; i++)
				{
					nativeArrayDims[num - (i + 1)] = dimensions[i];
				}
				return Array.CreateInstance(t, nativeArrayDims);
			}
		}

		internal static int[] NetDimensionToMATLABDimension(int[] dimensions)
		{
			int num = dimensions.Length;
			int[] array = new int[Math.Max(num, 2)];
			array[0] = ((num <= 1) ? 1 : dimensions[num - 2]);
			array[1] = dimensions[num - 1];
			for (int i = 0; i < num - 2; i++)
			{
				array[num - i - 1] = dimensions[i];
			}
			return array;
		}

		internal static int[] MATLABDimensionToNetDimension(int[] dimensions)
		{
			int num = dimensions.Length;
			int[] array = new int[num];
			array[num - 2] = dimensions[0];
			array[num - 1] = dimensions[1];
			for (int i = 0; i < num - 2; i++)
			{
				array[i] = dimensions[num - i - 1];
			}
			return array;
		}

		internal static string RankToString(int rank)
		{
			StringBuilder stringBuilder = new StringBuilder("[", rank + 1);
			stringBuilder.Length = rank + 1;
			for (int i = 1; i < rank; i++)
			{
				stringBuilder[i] = ',';
			}
			stringBuilder[rank] = ']';
			return stringBuilder.ToString();
		}
	}
}
