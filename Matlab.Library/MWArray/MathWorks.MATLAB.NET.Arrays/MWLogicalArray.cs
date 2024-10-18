using MathWorks.MATLAB.NET.Utility;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace MathWorks.MATLAB.NET.Arrays
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MWLogicalArray : MWIndexArray, IEquatable<MWLogicalArray>
	{
		private static readonly MWLogicalArray _Empty = new MWLogicalArray();

		public new MWLogicalArray this[params int[] indices]
		{
			get
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return (MWLogicalArray)MWArray.GetTypedArray(ArrayIndexer(this, indices));
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
			set
			{
				if (MWArrayType.Logical != value.array_Type)
				{
					string @string = MWArray.resourceManager.GetString("MWErrorDataArrayType");
					throw new InvalidCastException(@string);
				}
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					ArrayIndexer(value, this, indices);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public static MWLogicalArray Empty => (MWLogicalArray)_Empty.Clone();

		public new bool IsScalar
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return 1 == mxIsLogicalScalarTrue(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxCreateLogicalArray_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern MWSafeHandle mxCreateLogicalArray([In] int numberOfDimensions, [In] int[] dimensions);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxCreateLogicalScalar_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern MWSafeHandle mxCreateLogicalScalar([In] byte logical);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxCreateLogicalMatrix_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern MWSafeHandle mxCreateLogicalMatrix([In] int numRows, [In] int numCols);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsLogicalScalarTrue_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsLogicalScalarTrue([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclGetLogicalSparse_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mclGetLogicalSparse(out IntPtr pMXArray, [In] IntPtr rowIndexSize, [In] IntPtr[] rowindex, [In] IntPtr colIndexSize, [In] IntPtr[] columnindex, [In] IntPtr dataSize, [In] byte[] logicalData, [In] IntPtr rows, [In] IntPtr columns, [In] IntPtr nonZeroMax);

		public MWLogicalArray()
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2];
				int[] array2 = array;
				SetMXArray(mxCreateLogicalMatrix(0, 0), MWArrayType.Logical, array2.Length, array2);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWLogicalArray(int row, int column)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					row,
					column
				};
				SetMXArray(mxCreateLogicalMatrix(row, column), MWArrayType.Logical, array.Length, array);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWLogicalArray(params int[] dimensions)
		{
			if (dimensions == null)
			{
				throw new ArgumentNullException("dimensions");
			}
			try
			{
				Monitor.Enter(MWArray.mxSync);
				SetMXArray(mxCreateLogicalArray(dimensions.Length, dimensions), MWArrayType.Logical, dimensions.Length, dimensions);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWLogicalArray(bool logicalValue)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				SetMXArray(logicalValue ? mxCreateLogicalScalar(1) : mxCreateLogicalScalar(0), MWArrayType.Logical, array.Length, array);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWLogicalArray(int rows, int columns, bool[] boolArray)
		{
			CreateLogicalArray(rows, columns, boolArray, columnMajorData: false);
		}

		public MWLogicalArray(int rows, int columns, bool[] boolArray, bool columnMajorOrder)
		{
			CreateLogicalArray(rows, columns, boolArray, columnMajorOrder);
		}

		public MWLogicalArray(Array boolArray)
		{
			if (boolArray == null || typeof(bool) != boolArray.GetType().GetElementType())
			{
				string @string = MWArray.resourceManager.GetString("MWErrorDataArrayType");
				throw new ArgumentException(@string, "boolArray");
			}
			try
			{
				int rank = boolArray.Rank;
				int[] array = new int[Math.Max(rank, 2)];
				int length = boolArray.Length;
				int num = array[0] = ((1 >= rank) ? 1 : boolArray.GetLength(rank - 2));
				int num2 = array[1] = boolArray.GetLength(rank - 1);
				for (int i = 0; i < rank - 2; i++)
				{
					array[rank - i - 1] = boolArray.GetLength(i);
				}
				byte[] array2 = new byte[length];
				IEnumerator enumerator = boolArray.GetEnumerator();
				for (int j = 0; j < length; j += num * num2)
				{
					for (int k = 0; k < num; k++)
					{
						for (int l = 0; l < num2; l++)
						{
							enumerator.MoveNext();
							array2[l * num + k + j] = (byte)(((bool)enumerator.Current) ? 1 : 0);
						}
					}
				}
				Monitor.Enter(MWArray.mxSync);
				SetMXArray(mxCreateLogicalArray(array.Length, array), MWArrayType.Logical, array.Length, array);
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				Marshal.Copy(array2, 0, destination, length);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		internal MWLogicalArray(MWSafeHandle hMXArray)
			: base(hMXArray)
		{
			array_Type = MWArrayType.Logical;
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

		public static implicit operator bool(MWLogicalArray logicalArray)
		{
			logicalArray.CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				return 1 == mxIsLogicalScalarTrue(logicalArray.MXArrayHandle);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public static implicit operator MWLogicalArray(bool scalar)
		{
			return new MWLogicalArray(scalar);
		}

		protected MWLogicalArray(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}

		public static MWLogicalArray MakeSparse(int rows, int columns, int nonZeroMax)
		{
			int[] rowIndex = new int[1]
			{
				1
			};
			int[] columnIndex = new int[1]
			{
				1
			};
			bool[] dataArray = new bool[1];
			return MakeSparse(rows, columns, rowIndex, columnIndex, dataArray, nonZeroMax);
		}

		public static MWLogicalArray MakeSparse(int[] rowIndex, int[] columnIndex, bool[] dataArray)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int num = rowIndex.Length;
				if (num != columnIndex.Length)
				{
					string @string = MWArray.resourceManager.GetString("MWErrorInvalidIndices");
					throw new Exception(@string);
				}
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < num; i++)
				{
					num2 = Math.Max(num2, rowIndex[i]);
					num3 = Math.Max(num3, columnIndex[i]);
				}
				return MakeSparse(num2, num3, rowIndex, columnIndex, dataArray);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public static MWLogicalArray MakeSparse(int rows, int columns, int[] rowIndex, int[] columnIndex, bool[] dataArray)
		{
			return MakeSparse(rows, columns, rowIndex, columnIndex, dataArray, dataArray.Length);
		}

		public static MWLogicalArray MakeSparse(int rows, int columns, int[] rowIndex, int[] columnIndex, bool[] dataArray, int nonZeroMax)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				IntPtr pMXArray = IntPtr.Zero;
				int num = 0;
				byte[] array = null;
				if (dataArray != null)
				{
					num = dataArray.Length;
					array = new byte[num];
				}
				IntPtr[] array2 = new IntPtr[rowIndex.Length];
				IntPtr[] array3 = new IntPtr[columnIndex.Length];
				for (int i = 0; i < rowIndex.Length; i++)
				{
					array2[i] = (IntPtr)rowIndex[i];
					array3[i] = (IntPtr)columnIndex[i];
				}
				for (int j = 0; j < num; j++)
				{
					array[j] = (byte)(dataArray[j] ? 1 : 0);
				}
				if (mclGetLogicalSparse(out pMXArray, (IntPtr)rowIndex.Length, array2, (IntPtr)columnIndex.Length, array3, (IntPtr)dataArray.Length, array, (IntPtr)rows, (IntPtr)columns, (IntPtr)nonZeroMax) == 0)
				{
					if (MWArray.mclArrayHandle2mxArray(out MWSafeHandle hMXArray, pMXArray) != 0)
					{
						string @string = MWArray.resourceManager.GetString("MWErrorInvalidArray");
						throw new Exception(@string);
					}
					return new MWLogicalArray(hMXArray);
				}
				return null;
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj is MWLogicalArray)
			{
				return Equals(obj as MWLogicalArray);
			}
			return false;
		}

		private bool Equals(MWLogicalArray other)
		{
			return base.Equals((object)other);
		}

		bool IEquatable<MWLogicalArray>.Equals(MWLogicalArray other)
		{
			return Equals(other);
		}

		public override object Clone()
		{
			MWLogicalArray mWLogicalArray = (MWLogicalArray)MemberwiseClone();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				mWLogicalArray.SetMXArray(MWArray.mxDuplicateArray(MXArrayHandle), MWArrayType.Logical);
				return mWLogicalArray;
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

		public bool[] ToVector()
		{
			CheckDisposed();
			IntPtr source = IntPtr.Zero;
			double num = base.NumberOfElements;
			bool[] array;
			byte[] array2;
			checked
			{
				array = new bool[(int)num];
				array2 = new byte[(int)num];
				try
				{
					Monitor.Enter(MWArray.mxSync);
					source = MWArray.mxGetData(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
				Marshal.Copy(source, array2, 0, (int)num);
			}
			for (int i = 0; i < array2.Length; i++)
			{
				array[i] = ((array2[i] != 0) ? true : false);
			}
			return array;
		}

		public override Array ToArray()
		{
			CheckDisposed();
			int[] dimensions = Dimensions;
			IntPtr intPtr = MWArray.mxGetData(MXArrayHandle);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				if (MWArray.mxIsSparse(MXArrayHandle) != 0)
				{
					int[,] array = null;
					int[,] array2 = null;
					int num = 0;
					int num2 = 0;
					if (MWIndexArray.mclMXArrayGetIndexArrays(out MWSafeHandle hMXArrayRows, out MWSafeHandle hMXArrayCols, MXArrayHandle) != 0)
					{
						string @string = MWArray.resourceManager.GetString("MWErrorInvalidIndex");
						throw new Exception(@string);
					}
					MWNumericArray mWNumericArray = new MWNumericArray(hMXArrayRows);
					MWNumericArray mWNumericArray2 = new MWNumericArray(hMXArrayCols);
					int numberOfElements = mWNumericArray.NumberOfElements;
					if (MWNumericType.UInt64 != mWNumericArray.NumericType)
					{
						array = (int[,])mWNumericArray.ToArray(MWArrayComponent.Real, sparseIndex: true);
						array2 = (int[,])mWNumericArray2.ToArray(MWArrayComponent.Real, sparseIndex: true);
						for (int i = 0; i < numberOfElements; i++)
						{
							num = Math.Max(num, array[0, i]);
							num2 = Math.Max(num2, array2[0, i]);
						}
					}
					else
					{
						long[,] array3 = (long[,])mWNumericArray.ToArray(MWArrayComponent.Real, sparseIndex: true);
						long[,] array4 = (long[,])mWNumericArray2.ToArray(MWArrayComponent.Real, sparseIndex: true);
						array = new int[array3.GetLength(0), array3.GetLength(1)];
						array2 = new int[array4.GetLength(0), array4.GetLength(1)];
						for (int j = 0; j < numberOfElements; j++)
						{
							checked
							{
								num = Math.Max(num, array[0, j] = (int)array3[0, j]);
								num2 = Math.Max(num2, array2[0, j] = (int)array4[0, j]);
							}
						}
					}
					int num3 = num * num2;
					byte[] array5 = new byte[num3];
					if (IntPtr.Zero != intPtr)
					{
						Marshal.Copy(intPtr, array5, 0, numberOfElements);
					}
					Array array6 = Array.CreateInstance(typeof(bool), num, num2);
					for (int k = 0; k < numberOfElements; k++)
					{
						bool flag = 1 == array5[k];
						array6.SetValue(flag, array[0, k] - 1, array2[0, k] - 1);
					}
					return array6;
				}
				int numberOfElements2 = base.NumberOfElements;
				int num4 = dimensions.Length;
				int[] array7 = new int[num4];
				int[] array8 = new int[num4];
				int num5 = dimensions[0];
				int num6 = array7[num4 - 1] = dimensions[1];
				array7[num4 - 2] = num5;
				for (int l = 2; l < num4; l++)
				{
					array7[num4 - (l + 1)] = dimensions[l];
				}
				Array array9 = Array.CreateInstance(typeof(bool), array7);
				byte[] array10 = new byte[numberOfElements2];
				if (IntPtr.Zero != intPtr)
				{
					Marshal.Copy(intPtr, array10, 0, numberOfElements2);
				}
				for (int m = 0; m < numberOfElements2; m += num5 * num6)
				{
					for (int n = 0; n < num5; n++)
					{
						for (int num7 = 0; num7 < num6; num7++)
						{
							bool flag2 = 1 == array10[num7 * num5 + n + m];
							array9.SetValue(flag2, array8);
							array8 = GetNextSubscript(array7, array8, 0);
						}
					}
				}
				return array9;
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		private void CreateLogicalArray(int rows, int columns, bool[] boolArray, bool columnMajorData)
		{
			try
			{
				int num = rows * columns;
				if (boolArray == null || num != boolArray.Length)
				{
					string @string = MWArray.resourceManager.GetString("MWErrorDataArraySize");
					throw new ArgumentException(@string, "boolArray");
				}
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					rows,
					columns
				};
				SetMXArray(mxCreateLogicalArray(array.Length, array), MWArrayType.Logical, array.Length, array);
				byte[] array2 = new byte[num];
				if (columnMajorData)
				{
					for (int i = 0; i < num; i++)
					{
						array2[i] = (byte)(boolArray[i] ? 1 : 0);
					}
				}
				else
				{
					for (int j = 0; j < rows; j++)
					{
						for (int k = 0; k < columns; k++)
						{
							array2[k * rows + j] = (byte)(boolArray[j * columns + k] ? 1 : 0);
						}
					}
				}
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				Marshal.Copy(array2, 0, destination, num);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		internal override object ConvertToType(Type t)
		{
			if (t == typeof(MWArray) || t == typeof(MWLogicalArray))
			{
				return this;
			}
			if (t == typeof(bool) && IsScalar())
			{
				return (bool)this;
			}
			if (t.IsArray && t.GetArrayRank() == base.NumberofDimensions)
			{
				return ToArray();
			}
			if (t.IsArray && t.GetArrayRank() == 1 && IsVector())
			{
				return ToVector();
			}
			string @string = MWArray.resourceManager.GetString("MWErrorInvalidDataConversion");
			string message = "Cannot convert from MWLogicalArray" + MWArray.RankToString(base.NumberofDimensions) + " to " + t.FullName;
			throw new ArgumentException(@string, new ApplicationException(message));
		}
	}
}
