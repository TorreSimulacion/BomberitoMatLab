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

		public MWArray this[string fieldName, params int[] indices]
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					int[] array = new int[indices.Length];
					for (int i = 0; i < indices.Length; i++)
					{
						array[i] = indices[i] - 1;
					}
					int num = mxCalcSingleSubscript(MXArrayHandle, array.Length, array);
					if (num >= MWArray.mxGetNumberOfElements(MXArrayHandle))
					{
						string @string = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
						throw new ArgumentException(@string);
					}
					MWSafeHandle mWSafeHandle = new MWSafeHandle(mxGetField(MXArrayHandle, num, fieldName), ownsHandle: false);
					if (!mWSafeHandle.IsInvalid)
					{
						MWSafeHandle hMXArray = new MWSafeHandle(mclCreateSharedCopy(mWSafeHandle), ownsHandle: true);
						return MWArray.GetTypedArray(hMXArray);
					}
					if (!IsField(fieldName))
					{
						string string2 = MWArray.resourceManager.GetString("MWErrorFieldNotFound");
						throw new ArgumentException(string2);
					}
					if (1 == indices.Length)
					{
						if (indices[0] < 0 || indices[0] > MWArray.mxGetNumberOfElements(MXArrayHandle))
						{
							string string3 = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
							throw new ArgumentException(string3);
						}
					}
					else
					{
						int[] dimensions = Dimensions;
						if (dimensions.Length != indices.Length)
						{
							string string4 = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
							throw new ArgumentException(string4);
						}
						for (int j = 0; j < indices.Length; j++)
						{
							if (indices[j] > dimensions[j])
							{
								string string5 = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
								throw new ArgumentException(string5);
							}
						}
					}
					return MWNumericArray.Empty;
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
			set
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					int[] array = new int[indices.Length];
					for (int i = 0; i < indices.Length; i++)
					{
						array[i] = indices[i] - 1;
					}
					int num = mxCalcSingleSubscript(MXArrayHandle, array.Length, array);
					if (num >= MWArray.mxGetNumberOfElements(MXArrayHandle))
					{
						string @string = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
						throw new ArgumentException(@string);
					}
					new MWSafeHandle(mxGetField(MXArrayHandle, num, fieldName), ownsHandle: true);
					MWSafeHandle fieldValue = new MWSafeHandle(mclCreateSharedCopy(value.MXArrayHandle), ownsHandle: false);
					mxSetField(MXArrayHandle, num, fieldName, fieldValue);
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
				return GetField(fieldName);
			}
			set
			{
				SetField(fieldName, value);
			}
		}

		public static MWStructArray Empty => (MWStructArray)_Empty.Clone();

		public string[] FieldNames
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					int num = mxGetNumberOfFields(MXArrayHandle);
					string[] array = new string[num];
					for (int i = 0; i < num; i++)
					{
						IntPtr ptr = mxGetFieldNameByNumber(MXArrayHandle, i);
						array[i] = Marshal.PtrToStringAnsi(ptr);
					}
					return array;
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
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return mxGetNumberOfFields(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxAddField_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mxAddField([In] MWSafeHandle hMXArray, [In] string fieldName);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclCreateSharedCopy_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern IntPtr mclCreateSharedCopy([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxCalcSingleSubscript_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mxCalcSingleSubscript([In] MWSafeHandle hMXArray, [In] int numSubscripts, [In] int[] subscripts);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxCreateStructArray_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern MWSafeHandle mxCreateStructArray([In] int numDimensions, [In] int[] dimensions, [In] int numFields, [In] string[] fieldNames);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetField_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern IntPtr mxGetField([In] MWSafeHandle hMXArray, [In] int index, [In] string fieldName);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetFieldNameByNumber_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern IntPtr mxGetFieldNameByNumber([In] MWSafeHandle hMXArray, [In] int fieldNumber);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetFieldNumber_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mxGetFieldNumber([In] MWSafeHandle hMXArray, [In] string fieldName);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetNumberOfFields_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mxGetNumberOfFields([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxRemoveField_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern void mxRemoveField([In] MWSafeHandle hMXArray, [In] int fieldNumber);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxSetField_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern void mxSetField([In] MWSafeHandle hMXArray, [In] int index, [In] string fieldName, [In] MWSafeHandle fieldValue);

		public MWStructArray()
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2];
				int[] array2 = array;
				SetMXArray(mxCreateStructArray(0, null, 0, null), MWArrayType.Structure, array2.Length, array2);
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
				int[] array = new int[2]
				{
					rows,
					columns
				};
				SetMXArray(mxCreateStructArray(array.Length, array, fieldNames.Length, fieldNames), MWArrayType.Structure, array.Length, array);
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
				SetMXArray(mxCreateStructArray(dimensions.Length, dimensions, fieldNames.Length, fieldNames), MWArrayType.Structure, dimensions.Length, dimensions);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWStructArray(params MWArray[] fieldDefs)
		{
			int num = fieldDefs.Length;
			if (num % 2 != 0)
			{
				string @string = MWArray.resourceManager.GetString("MWErrorNotNameValuePair");
				throw new ArgumentException(@string, "fieldDefs");
			}
			num /= 2;
			string[] array = new string[num];
			MWArray[] array2 = new MWArray[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = i * 2;
				array[i] = ((MWCharArray)fieldDefs[num2]).ToString();
				array2[i] = fieldDefs[num2 + 1];
			}
			try
			{
				Monitor.Enter(MWArray.mxSync);
				SetMXArray(mxCreateStructArray(1, new int[1]
				{
					1
				}, array.Length, array), MWArrayType.Structure, 1, new int[1]
				{
					1
				});
				for (int j = 0; j < num; j++)
				{
					SetField(array[j], array2[j]);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		internal MWStructArray(MWSafeHandle hMXArray)
			: base(hMXArray)
		{
			array_Type = MWArrayType.Structure;
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		protected MWStructArray(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override object Clone()
		{
			MWStructArray mWStructArray = (MWStructArray)MemberwiseClone();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				mWStructArray.SetMXArray(MWArray.mxDuplicateArray(MXArrayHandle), MWArrayType.Structure);
				return mWStructArray;
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is MWStructArray)
			{
				return Equals(obj as MWStructArray);
			}
			return false;
		}

		public bool Equals(MWStructArray other)
		{
			return base.Equals((object)other);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public MWArray GetField(string fieldName, int index)
		{
			CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				if (index >= MWArray.mxGetNumberOfElements(MXArrayHandle))
				{
					string @string = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
					throw new ArgumentException(@string);
				}
				MWSafeHandle mWSafeHandle = new MWSafeHandle(mxGetField(MXArrayHandle, index, fieldName), ownsHandle: false);
				if (!mWSafeHandle.IsInvalid)
				{
					MWSafeHandle hMXArray = new MWSafeHandle(mclCreateSharedCopy(mWSafeHandle), ownsHandle: true);
					return MWArray.GetTypedArray(hMXArray);
				}
				if (!IsField(fieldName))
				{
					string string2 = MWArray.resourceManager.GetString("MWErrorFieldNotFound");
					throw new ArgumentException(string2);
				}
				if (index < 0 || index > MWArray.mxGetNumberOfElements(MXArrayHandle))
				{
					string string3 = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
					throw new ArgumentException(string3);
				}
				return MWNumericArray.Empty;
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWArray GetField(string fieldName)
		{
			return GetField(fieldName, 0);
		}

		public bool IsField(string fieldName)
		{
			CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				return -1 != mxGetFieldNumber(MXArrayHandle, fieldName);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWStructArray RemoveField(string fieldName)
		{
			CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int num = mxGetFieldNumber(MXArrayHandle, fieldName);
				if (-1 == num)
				{
					string @string = MWArray.resourceManager.GetString("MWErrorFieldNotFound");
					throw new ArgumentException(@string, fieldName);
				}
				int num2 = MWArray.mxGetNumberOfElements(MXArrayHandle);
				for (int i = 0; i < num2; i++)
				{
					new MWSafeHandle(mxGetField(MXArrayHandle, i, fieldName), ownsHandle: true);
				}
				mxRemoveField(MXArrayHandle, num);
				return this;
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public void SetField(string fieldName, MWArray fieldValue)
		{
			CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				if (!IsField(fieldName) && -1 == mxAddField(MXArrayHandle, fieldName))
				{
					string @string = MWArray.resourceManager.GetString("MWErrorBadFieldName");
					throw new ArgumentException(@string, fieldName);
				}
				new MWSafeHandle(mxGetField(MXArrayHandle, 0, fieldName), ownsHandle: true);
				MWSafeHandle fieldValue2 = new MWSafeHandle(mclCreateSharedCopy(fieldValue.MXArrayHandle), ownsHandle: false);
				mxSetField(MXArrayHandle, 0, fieldName, fieldValue2);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public override Array ToArray()
		{
			CheckDisposed();
			int[] dimensions = Dimensions;
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int numberOfElements = base.NumberOfElements;
				int num = dimensions.Length + 1;
				int[] array = new int[num];
				int[] array2 = new int[num];
				int num2 = array[num - 2] = dimensions[0];
				int num3 = array[num - 1] = dimensions[1];
				int num4 = array[0] = FieldNames.Length;
				for (int i = 2; i < num - 1; i++)
				{
					array[num - (i + 1)] = dimensions[i];
				}
				Array array3 = Array.CreateInstance(typeof(object), array);
				for (int j = 0; j < num4; j++)
				{
					for (int k = 0; k < numberOfElements; k += num2 * num3)
					{
						for (int l = 0; l < num2; l++)
						{
							for (int m = 0; m < num3; m++)
							{
								array3.SetValue(GetField(FieldNames[j], m * num2 + l + k).ToArray(), array2);
								array2 = GetNextSubscript(array, array2, 0);
							}
						}
					}
				}
				return array3;
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		internal override object ConvertToType(Type t)
		{
			if (t == typeof(MWArray) || t == typeof(MWStructArray))
			{
				return this;
			}
			if (t == typeof(Hashtable) && IsScalar())
			{
				return GetHashtable();
			}
			if (t.IsValueType && !t.IsPrimitive && (IsScalar() || IsEmpty))
			{
				return GetNetScalarStruct(t);
			}
			if (t.IsArray && t.GetArrayRank() == 2 && !t.GetElementType().IsPrimitive && base.NumberofDimensions == 2)
			{
				return GetNetStructMatrix(t);
			}
			if (t.IsArray && t.GetArrayRank() == 1 && !t.GetElementType().IsPrimitive && (IsVector() || IsEmpty))
			{
				return GetNetStructVector(t);
			}
			string @string = MWArray.resourceManager.GetString("MWErrorInvalidDataConversion");
			string message = "Cannot convert from MWStructArray to " + t.FullName;
			throw new ArgumentException(@string, new ApplicationException(message));
		}

		private object GetHashtable()
		{
			Hashtable hashtable = new Hashtable();
			string[] fieldNames = FieldNames;
			foreach (string text in fieldNames)
			{
				MWArray field = GetField(text);
				if (field.IsStructArray)
				{
					hashtable[text] = field.ConvertToType(typeof(Hashtable));
				}
				else if (field.IsCellArray)
				{
					hashtable[text] = field.ConvertToType(typeof(Array));
				}
				else
				{
					hashtable[text] = field.ToArray();
				}
			}
			return hashtable;
		}

		private object GetNetScalarStruct(Type t)
		{
			FieldInfo[] fields = t.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			object obj = Activator.CreateInstance(t);
			if (IsEmpty)
			{
				return obj;
			}
			FieldInfo[] array = ValidateFields(t, fields);
			FieldInfo[] array2 = array;
			foreach (FieldInfo fieldInfo in array2)
			{
				fieldInfo.SetValue(obj, this[fieldInfo.Name].ConvertToType(fieldInfo.FieldType));
			}
			return obj;
		}

		private object GetNetStructMatrix(Type t)
		{
			FieldInfo[] fields = t.GetElementType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			int[] dimensions = Dimensions;
			int[] columnMajorIndices = MWArray.GetColumnMajorIndices(dimensions[0], dimensions[1]);
			Array array = Array.CreateInstance(t.GetElementType(), dimensions[0], dimensions[1]);
			if (IsEmpty)
			{
				return array;
			}
			FieldInfo[] array2 = ValidateFields(t.GetElementType(), fields);
			int num = 0;
			for (int i = 0; i < dimensions[0]; i++)
			{
				for (int j = 0; j < dimensions[1]; j++)
				{
					object obj = Activator.CreateInstance(t.GetElementType());
					FieldInfo[] array3 = array2;
					foreach (FieldInfo fieldInfo in array3)
					{
						fieldInfo.SetValue(obj, this[fieldInfo.Name, new int[1]
						{
							columnMajorIndices[num]
						}].ConvertToType(fieldInfo.FieldType));
					}
					array.SetValue(obj, i, j);
					num++;
				}
			}
			return array;
		}

		private object GetNetStructVector(Type t)
		{
			FieldInfo[] fields = t.GetElementType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			int numberOfElements = base.NumberOfElements;
			Array array = Array.CreateInstance(t.GetElementType(), numberOfElements);
			if (IsEmpty)
			{
				return array;
			}
			FieldInfo[] array2 = ValidateFields(t.GetElementType(), fields);
			for (int i = 0; i < numberOfElements; i++)
			{
				object obj = Activator.CreateInstance(t.GetElementType());
				FieldInfo[] array3 = array2;
				foreach (FieldInfo fieldInfo in array3)
				{
					fieldInfo.SetValue(obj, this[fieldInfo.Name, new int[1]
					{
						i + 1
					}].ConvertToType(fieldInfo.FieldType));
				}
				array.SetValue(obj, i);
			}
			return array;
		}

		private FieldInfo[] ValidateFields(Type t, FieldInfo[] netFields)
		{
			string @string = MWArray.resourceManager.GetString("MWErrorInvalidDataConversion");
			string str = "Cannot convert from MWStructArray to " + t.FullName + ".\n";
			string[] fieldNames = FieldNames;
			int num = fieldNames.Length;
			int num2 = netFields.Length;
			FieldInfo[] array = new FieldInfo[num];
			bool[] array2 = new bool[num2];
			bool flag = true;
			for (int i = 0; i < num; i++)
			{
				flag = true;
				for (int j = 0; j < num2; j++)
				{
					if (!array2[j] && string.Compare(fieldNames[i], netFields[j].Name, StringComparison.Ordinal) == 0)
					{
						array[i] = netFields[j];
						array2[j] = true;
						flag = false;
						break;
					}
				}
				if (flag)
				{
					throw new ArgumentException(@string, new ApplicationException(str + "Provided type does not have the same fields as the returned MATLAB struct."));
				}
			}
			return array;
		}
	}
}
