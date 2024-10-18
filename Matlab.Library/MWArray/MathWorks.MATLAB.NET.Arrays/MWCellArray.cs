using MathWorks.MATLAB.NET.Utility;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading;

namespace MathWorks.MATLAB.NET.Arrays
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MWCellArray : MWArray, IEquatable<MWCellArray>
	{
		private static readonly MWCellArray _Empty = new MWCellArray();

		public new MWArray this[params int[] indices]
		{
			get
			{
				try
				{
					Monitor.Enter(MWArray.mxSync);
					MWSafeHandle mWSafeHandle = ArrayIndexer(this, indices);
					if (mWSafeHandle.IsInvalid)
					{
						if (1 == indices.Length)
						{
							if (indices[0] < 0 || indices[0] > MWArray.mxGetNumberOfElements(MXArrayHandle))
							{
								string @string = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
								throw new ArgumentException(@string);
							}
						}
						else
						{
							int[] dimensions = Dimensions;
							if (dimensions.Length != indices.Length)
							{
								string string2 = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
								throw new ArgumentException(string2);
							}
							for (int i = 0; i < indices.Length; i++)
							{
								if (indices[i] > dimensions[i])
								{
									string string3 = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
									throw new ArgumentException(string3);
								}
							}
						}
						return MWNumericArray.Empty;
					}
					return MWArray.GetTypedArray(mWSafeHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
			set
			{
				ArrayIndexer(value, this, indices);
			}
		}

		public static MWCellArray Empty => (MWCellArray)_Empty.Clone();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxCalcSingleSubscript_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern int mxCalcSingleSubscript([In] MWSafeHandle hMXArray, [In] int numSubscripts, [In] int[] subscripts);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxCreateCellArray_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern MWSafeHandle mxCreateCellArray([In] int numDimensions, [In] int[] dimensions);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetCell_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern MWSafeHandle mxGetCell([In] MWSafeHandle hMXArray, [In] int index);

		public MWCellArray()
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2];
				int[] array2 = array;
				SetMXArray(mxCreateCellArray(array2.Length, array2), MWArrayType.Cell, array2.Length, array2);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWCellArray(int rows, int columns)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					rows,
					columns
				};
				SetMXArray(mxCreateCellArray(array.Length, array), MWArrayType.Cell, array.Length, array);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWCellArray(params int[] dimensions)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				SetMXArray(mxCreateCellArray(dimensions.Length, dimensions), MWArrayType.Cell, dimensions.Length, dimensions);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWCellArray(MWNumericArray dimensions)
		{
			double[] array = (double[])dimensions.ToVector(MWArrayComponent.Real);
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = checked((int)array[i]);
			}
			try
			{
				Monitor.Enter(MWArray.mxSync);
				SetMXArray(mxCreateCellArray(array2.Length, array2), MWArrayType.Cell, array2.Length, array2);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWCellArray(MWCharArray strings)
		{
			if (2 != strings.NumberofDimensions)
			{
				string @string = MWArray.resourceManager.GetString("MWErrorNotAMatrix");
				throw new ArgumentException(@string, "strings");
			}
			int[] dimensions = strings.Dimensions;
			int num = dimensions[0];
			int num2 = dimensions[1];
			try
			{
				Monitor.Enter(MWArray.mxSync);
				StringBuilder stringBuilder = new StringBuilder(num2);
				int[] array = new int[2]
				{
					num,
					1
				};
				SetMXArray(mxCreateCellArray(array.Length, array), MWArrayType.Cell);
				for (int i = 1; i <= num; i++)
				{
					for (int j = 1; j <= num2; j++)
					{
						stringBuilder.Insert(j - 1, (char)strings[new int[2]
						{
							i,
							j
						}]);
					}
					this[new int[2]
					{
						i,
						1
					}] = new MWCharArray(stringBuilder.ToString());
					stringBuilder.Remove(0, num2);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		internal MWCellArray(MWSafeHandle hMXArray)
			: base(hMXArray)
		{
			array_Type = MWArrayType.Cell;
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

		protected MWCellArray(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}

		public override string ToString()
		{
			return base.ToString();
		}

		public override object Clone()
		{
			MWCellArray mWCellArray = (MWCellArray)MemberwiseClone();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				mWCellArray.SetMXArray(MWArray.mxDuplicateArray(MXArrayHandle), MWArrayType.Cell);
				return mWCellArray;
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is MWCellArray)
			{
				return Equals(obj as MWCellArray);
			}
			return false;
		}

		private bool Equals(MWCellArray other)
		{
			return base.Equals((object)other);
		}

		bool IEquatable<MWCellArray>.Equals(MWCellArray other)
		{
			return Equals(other);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
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
				Array array3 = Array.CreateInstance(typeof(object), array);
				for (int j = 0; j < numberOfElements; j += num2 * num3)
				{
					for (int k = 0; k < num2; k++)
					{
						for (int l = 0; l < num3; l++)
						{
							MWArray mWArray = this[new int[1]
							{
								l * num2 + k + j + 1
							}];
							array3.SetValue(mWArray.ToArray(), array2);
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

		internal override object ConvertToType(Type t)
		{
			if (t == typeof(MWArray) || t == typeof(MWCellArray))
			{
				return this;
			}
			if (t == typeof(ArrayList) && IsVector())
			{
				ArrayList arrayList = new ArrayList();
				int numberOfElements = base.NumberOfElements;
				for (int i = 1; i <= numberOfElements; i++)
				{
					MWArray mWArray = this[new int[1]
					{
						i
					}];
					if (mWArray.IsStructArray)
					{
						arrayList.Add(mWArray);
					}
					else if (mWArray.IsCellArray)
					{
						arrayList.Add(ConvertToType(typeof(Array)));
					}
					else
					{
						arrayList.Add(mWArray.ToArray());
					}
				}
				return arrayList;
			}
			if (t == typeof(Array))
			{
				int[] nativeArrayDims;
				Array array = AllocateNativeArray(typeof(object), out nativeArrayDims);
				int numberOfElements2 = base.NumberOfElements;
				int[] array2 = new int[base.NumberofDimensions];
				int num = Dimensions[0];
				int num2 = Dimensions[1];
				for (int j = 0; j < numberOfElements2; j += num * num2)
				{
					for (int k = 0; k < num; k++)
					{
						for (int l = 0; l < num2; l++)
						{
							MWArray mWArray2 = this[new int[1]
							{
								l * num + k + j + 1
							}];
							if (!mWArray2.IsStructArray)
							{
								if (mWArray2.IsCellArray)
								{
									ConvertToType(typeof(Array));
								}
								else
								{
									mWArray2.ToArray();
								}
							}
							array.SetValue(mWArray2, array2);
							array2 = GetNextSubscript(nativeArrayDims, array2, 0);
						}
					}
				}
				return array;
			}
			string @string = MWArray.resourceManager.GetString("MWErrorInvalidDataConversion");
			string message = "Cannot convert from MWCellArray to " + t.FullName;
			throw new ArgumentException(@string, new ApplicationException(message));
		}
	}
}
