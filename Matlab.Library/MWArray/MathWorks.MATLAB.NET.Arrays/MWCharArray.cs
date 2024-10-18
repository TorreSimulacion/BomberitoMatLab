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

		public new MWCharArray this[params int[] indices]
		{
			get
			{
				return (MWCharArray)MWArray.GetTypedArray(ArrayIndexer(this, indices));
			}
			set
			{
				ArrayIndexer(value, this, indices);
			}
		}

		public static MWCharArray Empty => (MWCharArray)_Empty.Clone();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxArrayToString_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern IntPtr mxArrayToString([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxCreateCharArray_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern MWSafeHandle mxCreateCharArray([In] int numberOfDimensions, [In] int[] dimensions);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetChars_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern IntPtr mxGetChars([In] MWSafeHandle hMXArray);

		public MWCharArray()
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2];
				int[] array2 = array;
				SetMXArray(mxCreateCharArray(array2.Length, array2), MWArrayType.Character, array2.Length, array2);
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
				SetMXArray(mxCreateCharArray(dimensions.Length, dimensions), MWArrayType.Character, dimensions.Length, dimensions);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWCharArray(string value)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				Monitor.Enter(MWArray.mxSync);
				intPtr = Marshal.StringToHGlobalAnsi(value);
				SetMXArray(MWArray.mxCreateString(intPtr), MWArrayType.Character);
			}
			finally
			{
				if (IntPtr.Zero != intPtr)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWCharArray(char value)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				Monitor.Enter(MWArray.mxSync);
				intPtr = Marshal.StringToHGlobalAnsi(value.ToString());
				SetMXArray(MWArray.mxCreateString(intPtr), MWArrayType.Character);
			}
			finally
			{
				if (IntPtr.Zero != intPtr)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWCharArray(char[] value)
		{
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				Monitor.Enter(MWArray.mxSync);
				intPtr = Marshal.StringToHGlobalAnsi(new string(value));
				SetMXArray(MWArray.mxCreateString(intPtr), MWArrayType.Character);
			}
			finally
			{
				if (IntPtr.Zero != intPtr)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWCharArray(string[] strings)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = null;
				if (strings == null)
				{
					int[] array2 = new int[1];
					array = array2;
				}
				else
				{
					int num = 0;
					foreach (string text in strings)
					{
						num = Math.Max(text.Length, num);
					}
					array = new int[2]
					{
						strings.Length,
						num
					};
				}
				int num2 = array[0];
				int num3 = array[1];
				SetMXArray(mxCreateCharArray(array.Length, array), MWArrayType.Character, array.Length, array);
				int num4 = num2 * num3;
				char[] array3 = new char[num4];
				for (int j = 0; j < num2; j++)
				{
					string text2 = strings[j].PadRight(num3);
					for (int k = 0; k < num3; k++)
					{
						text2.CopyTo(k, array3, k * num2 + j, 1);
					}
				}
				IntPtr intPtr = MWArray.mxGetData(MXArrayHandle);
				if (intPtr != IntPtr.Zero)
				{
					Marshal.Copy(array3, 0, intPtr, num4);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWCharArray(Array stringArray)
		{
			int num = 0;
			int rank = stringArray.Rank;
			int[] array = null;
			char[] array2 = null;
			if (stringArray != null && typeof(char) == stringArray.GetType().GetElementType())
			{
				MWCharArrayFromNetCharArray(stringArray);
				return;
			}
			if (stringArray != null && typeof(string) == stringArray.GetType().GetElementType())
			{
				int num2 = 0;
				string text = null;
				array = new int[rank + 1];
				IEnumerator enumerator = stringArray.GetEnumerator();
				for (int i = 0; i < stringArray.LongLength; i++)
				{
					enumerator.MoveNext();
					text = (string)enumerator.Current;
					if (text == null)
					{
						text = string.Empty;
					}
					num2 = Math.Max(text.Length, num2);
				}
				int num3 = array[0] = stringArray.GetLength(rank - 1);
				int num4 = array[1] = num2;
				for (int j = 0; j < rank - 1; j++)
				{
					array[rank - j] = stringArray.GetLength(j);
				}
				num = stringArray.Length * num2;
				array2 = new char[num];
				enumerator.Reset();
				for (int k = 0; k < num; k += num3 * num4)
				{
					for (int l = 0; l < num3; l++)
					{
						enumerator.MoveNext();
						text = (string)enumerator.Current;
						text = text.PadRight(num4);
						for (int m = 0; m < num4; m++)
						{
							text.CopyTo(m, array2, m * num3 + l + k, 1);
						}
					}
				}
				try
				{
					Monitor.Enter(MWArray.mxSync);
					SetMXArray(mxCreateCharArray(array.Length, array), MWArrayType.Character, array.Length, array);
					IntPtr intPtr = MWArray.mxGetData(MXArrayHandle);
					if (intPtr != IntPtr.Zero)
					{
						Marshal.Copy(array2, 0, intPtr, num);
					}
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
				return;
			}
			string @string = MWArray.resourceManager.GetString("MWErrorArrayStringType");
			throw new ArgumentException(@string, "stringArray");
		}

		internal MWCharArray(MWSafeHandle hMXArray)
			: base(hMXArray)
		{
			array_Type = MWArrayType.Character;
		}

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{
				try
				{
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}

		public static explicit operator char(MWCharArray scalar)
		{
			scalar.CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				IntPtr intPtr = mxGetChars(scalar.MXArrayHandle);
				if (intPtr == IntPtr.Zero)
				{
					return '\0';
				}
				char[] array = new char[1];
				Marshal.Copy(intPtr, array, 0, 1);
				return array[0];
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

		private unsafe void MWCharArrayFromNetCharArray(Array charArray)
		{
			int rank = charArray.Rank;
			int[] array = new int[rank];
			char[] array2 = null;
			for (int i = 0; i < rank; i++)
			{
				array[i] = charArray.GetLength(i);
			}
			int[] array3 = MWArray.NetDimensionToMATLABDimension(array);
			if (rank == 1)
			{
				array2 = (char[])charArray;
			}
			else
			{
				int length = charArray.Length;
				int num = array3[0];
				int num2 = array3[1];
				int num3 = num * num2;
				array2 = new char[length];
				GCHandle gCHandle = GCHandle.Alloc(charArray, GCHandleType.Pinned);
				GCHandle gCHandle2 = GCHandle.Alloc(array2, GCHandleType.Pinned);
				try
				{
					char* ptr = (char*)(void*)gCHandle.AddrOfPinnedObject();
					char* ptr2 = (char*)(void*)gCHandle2.AddrOfPinnedObject();
					for (int j = 0; j < length; j += num3)
					{
						for (int k = 0; k < num; k++)
						{
							for (int l = 0; l < num2; l++)
							{
								char* intPtr = ptr2 + (l * num + k + j);
								char* intPtr2 = ptr;
								ptr = intPtr2 + 1;
								*intPtr = *intPtr2;
							}
						}
					}
				}
				finally
				{
					gCHandle.Free();
					gCHandle2.Free();
				}
			}
			lock (MWArray.mxSync)
			{
				SetMXArray(mxCreateCharArray(array3.Length, array3), MWArrayType.Character, array3.Length, array3);
				IntPtr intPtr3 = MWArray.mxGetData(MXArrayHandle);
				if (intPtr3 != IntPtr.Zero)
				{
					Marshal.Copy(array2, 0, intPtr3, charArray.Length);
				}
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
				int num = dimensions.Length;
				int[] array = new int[num];
				int[] array2 = new int[num];
				int num2 = dimensions[0];
				int num3 = array[num - 1] = dimensions[1];
				array[num - 2] = num2;
				for (int i = 2; i < num; i++)
				{
					array[num - (i + 1)] = dimensions[i];
				}
				IntPtr intPtr = MWArray.mxGetData(MXArrayHandle);
				Array array3 = Array.CreateInstance(typeof(char), array);
				char[] array4 = new char[numberOfElements];
				if (IntPtr.Zero != intPtr)
				{
					Marshal.Copy(intPtr, array4, 0, numberOfElements);
				}
				for (int j = 0; j < numberOfElements; j += num2 * num3)
				{
					for (int k = 0; k < num2; k++)
					{
						for (int l = 0; l < num3; l++)
						{
							char c = array4[l * num2 + k + j];
							array3.SetValue(c, array2);
							array2 = GetNextSubscript(array, array2, 0);
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

		internal string CharArrayToString(Array src)
		{
			string text = null;
			if (src.Rank == 1)
			{
				return new string((char[])src);
			}
			if (src.Rank == 2 && src.GetUpperBound(0) == 0)
			{
				int length = src.Length;
				return new string(MWArray.ConvertMatrixToVector((char[,])src));
			}
			if (src.GetLength(0) == 0)
			{
				return string.Empty;
			}
			throw new ArgumentException("Not a valid conversion");
		}

		internal unsafe Array CharArrayToStringArray(Array src)
		{
			int num = src.Rank - 1;
			int[] array = new int[num];
			int[] array2 = new int[num];
			int length = src.GetLength(num);
			for (int i = 0; i < num; i++)
			{
				array[i] = src.GetLength(i);
			}
			Array array3 = Array.CreateInstance(typeof(string), array);
			GCHandle gCHandle = GCHandle.Alloc(src, GCHandleType.Pinned);
			try
			{
				char* value = (char*)(void*)gCHandle.AddrOfPinnedObject();
				int length2 = src.Length;
				for (int j = 0; j < length2; j += length)
				{
					string value2 = new string(value, j, length);
					array3.SetValue(value2, array2);
					array2 = GetNextSubscript(array, array2, 0);
				}
				return array3;
			}
			finally
			{
				gCHandle.Free();
			}
		}

		internal override object ConvertToType(Type t)
		{
			if (t == typeof(MWArray) || t == typeof(MWCharArray))
			{
				return this;
			}
			Array array = ToArray();
			if (t == typeof(string))
			{
				return CharArrayToString(array);
			}
			if (t.IsArray && t.GetElementType() == typeof(string) && t.GetArrayRank() == base.NumberofDimensions - 1)
			{
				return CharArrayToStringArray(array);
			}
			if (t == typeof(char) && (IsScalar() || IsEmpty))
			{
				return (char)this;
			}
			if (t.IsArray && t.GetArrayRank() == 1 && (IsVector() || IsEmpty))
			{
				return MWArray.ConvertMatrixToVector((char[,])array);
			}
			if (t.IsArray && t.GetArrayRank() == base.NumberofDimensions)
			{
				return array;
			}
			string @string = MWArray.resourceManager.GetString("MWErrorInvalidDataConversion");
			string message = "Cannot convert from MWCharArray" + MWArray.RankToString(base.NumberofDimensions) + " to " + t.FullName;
			throw new ArgumentException(@string, new ApplicationException(message));
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override object Clone()
		{
			MWCharArray mWCharArray = (MWCharArray)MemberwiseClone();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				mWCharArray.SetMXArray(MWArray.mxDuplicateArray(MXArrayHandle), MWArrayType.Character);
				return mWCharArray;
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
			{
				return Equals(obj as MWCharArray);
			}
			return false;
		}

		private bool Equals(MWCharArray other)
		{
			return base.Equals((object)other);
		}

		bool IEquatable<MWCharArray>.Equals(MWCharArray other)
		{
			return Equals(other);
		}
	}
}
