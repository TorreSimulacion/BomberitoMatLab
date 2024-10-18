using MathWorks.MATLAB.NET.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace MathWorks.MATLAB.NET.Arrays
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MWNumericArray : MWIndexArray, IEquatable<MWNumericArray>
	{
		public static readonly MWNumericArray Inf;

		public static readonly MWNumericArray NaN;

		private static readonly MWNumericArray _Empty;

		private static IDictionary systemTypeToNumericType;

		private static Dictionary<MWNumericType, Type> numericTypeToSystemType;

		public new MWNumericArray this[params int[] indices]
		{
			get
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return (MWNumericArray)MWArray.GetTypedArray(ArrayIndexer(this, indices));
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
			set
			{
				if (MWArrayType.Numeric != value.array_Type)
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

		public MWNumericArray this[MWArrayComponent component, params int[] indices]
		{
			get
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					IntPtr[] array = new IntPtr[indices.Length];
					for (int i = 0; i < indices.Length; i++)
					{
						array[i] = (IntPtr)indices[i];
					}
					MWSafeHandle hMXArraySrcElem;
					if (component == MWArrayComponent.Real)
					{
						if (mclMXArrayGetReal(out hMXArraySrcElem, MXArrayHandle, (IntPtr)indices.Length, array) != 0)
						{
							string @string = MWArray.resourceManager.GetString("MWErrorInvalidIndex");
							throw new Exception(@string);
						}
					}
					else if (mclMXArrayGetImag(out hMXArraySrcElem, MXArrayHandle, (IntPtr)indices.Length, array) != 0)
					{
						string string2 = MWArray.resourceManager.GetString("MWErrorInvalidIndex");
						throw new Exception(string2);
					}
					return (MWNumericArray)MWArray.GetTypedArray(hMXArraySrcElem);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
			set
			{
				if (MWArrayType.Numeric != value.array_Type)
				{
					string @string = MWArray.resourceManager.GetString("MWErrorDataArrayType");
					throw new InvalidCastException(@string);
				}
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					IntPtr[] array = new IntPtr[indices.Length];
					for (int i = 0; i < indices.Length; i++)
					{
						array[i] = (IntPtr)indices[i];
					}
					if (component == MWArrayComponent.Real)
					{
						if (mclMXArraySetReal(MXArrayHandle, value.MXArrayHandle, (IntPtr)indices.Length, array) != 0)
						{
							string string2 = MWArray.resourceManager.GetString("MWErrorInvalidArray");
							throw new Exception(string2);
						}
					}
					else if (mclMXArraySetImag(MXArrayHandle, value.MXArrayHandle, (IntPtr)indices.Length, array) != 0)
					{
						string string3 = MWArray.resourceManager.GetString("MWErrorInvalidArray");
						throw new Exception(string3);
					}
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public static MWNumericArray Empty => (MWNumericArray)_Empty.Clone();

		public static double FloatingPointAccuracy
		{
			get
			{
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return (double)new MWNumericArray(mxGetEps());
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		private static MWNumericArray _Inf
		{
			get
			{
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return new MWNumericArray(mxGetInf());
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		private static MWNumericArray _NaN
		{
			get
			{
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return new MWNumericArray(mxGetNaN());
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public MWNumericType NumericType
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return (MWNumericType)MWArray.mxGetClassID(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public bool IsByte
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return 1 == mxIsUint8(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public bool IsComplex
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return 1 == mxIsComplex(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public bool IsDouble
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return 1 == mxIsDouble(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public bool IsFloat
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return 1 == mxIsSingle(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public bool IsInteger
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return 1 == mxIsInt32(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public bool IsLong
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return 1 == mxIsInt64(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public bool IsInfinity
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					if (1 != MWArray.mxGetNumberOfElements(MXArrayHandle))
					{
						string @string = MWArray.resourceManager.GetString("MWErrorNotScalar");
						throw new Exception(@string);
					}
					return 1 == mxIsInf((double)this);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public bool IsNaN
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					if (1 != MWArray.mxGetNumberOfElements(MXArrayHandle))
					{
						string @string = MWArray.resourceManager.GetString("MWErrorNotScalar");
						throw new Exception(@string);
					}
					return 1 == mxIsNaN((double)this);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public bool IsShort
		{
			get
			{
				CheckDisposed();
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return 1 == mxIsInt16(MXArrayHandle);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxCreateDoubleMatrix_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern MWSafeHandle mxCreateDoubleMatrix([In] int row, [In] int column, [In] MWArrayComplexity complexityFlag);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxCreateNumericArray_700_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern MWSafeHandle mxCreateNumericArray([In] int rank, [In] int[] dimensions, [In] MWNumericType elementType, [In] MWArrayComplexity complexityFlag);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxFastZeros_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern MWSafeHandle mxFastZeros([In] MWArrayComplexity complexityFlag, [In] int rows, [In] int columns);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetEps_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern double mxGetEps();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetImagData_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern IntPtr mxGetImagData([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetInf_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern double mxGetInf();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetNaN_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern double mxGetNaN();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetPi_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern IntPtr mxGetPi([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxGetPr_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern IntPtr mxGetPr([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsComplex_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsComplex([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsFinite_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern byte mxIsFinite([In] double value);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsInf_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern byte mxIsInf([In] double value);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsNaN_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern byte mxIsNaN([In] double value);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsUint8_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsUint8([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsInt16_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsInt16([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsInt32_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsInt32([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsInt64_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsInt64([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsSingle_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsSingle([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mxIsDouble_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern byte mxIsDouble([In] MWSafeHandle hMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "array_handle_imag_proxy")]
		[SuppressUnmanagedCodeSecurity]
		protected static extern IntPtr array_handle_imag([In] IntPtr pMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "array_handle_real_proxy")]
		[SuppressUnmanagedCodeSecurity]
		protected static extern IntPtr array_handle_real([In] IntPtr pMXArray);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMXArrayGetReal_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mclMXArrayGetReal(out MWSafeHandle hMXArraySrcElem, [In] MWSafeHandle hMXArraySrcArray, [In] IntPtr numIndices, [In] IntPtr[] indices);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMXArrayGetImag_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mclMXArrayGetImag(out MWSafeHandle hMXArraySrcElem, [In] MWSafeHandle hMXArraySrcArray, [In] IntPtr numIndices, [In] IntPtr[] indices);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMXArraySetReal_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mclMXArraySetReal([In] MWSafeHandle hMXArrayTrg, [In] MWSafeHandle hMXArraySrcElem, [In] IntPtr numIndices, [In] IntPtr[] indices);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMXArraySetImag_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mclMXArraySetImag([In] MWSafeHandle hMXArrayTrg, [In] MWSafeHandle hMXArraySrcElem, [In] IntPtr numIndices, [In] IntPtr[] indices);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclGetNumericSparse_proxy")]
		[SuppressUnmanagedCodeSecurity]
		internal static extern int mclGetNumericSparse(out IntPtr pMXArray, [In] IntPtr rowIndexSize, [In] IntPtr[] rowindex, [In] IntPtr colIndexSize, [In] IntPtr[] columnindex, [In] IntPtr dataSize, [In] double[] realData, [In] double[] imaginaryData, [In] IntPtr rows, [In] IntPtr columns, [In] IntPtr nonZeroMax, [In] MATLABArrayType arrayType, [In] MWArrayComplexity complexityFlag);

		static MWNumericArray()
		{
			Inf = _Inf;
			NaN = _NaN;
			_Empty = new MWNumericArray(MWArrayComponent.Real, 0, 0);
			systemTypeToNumericType = null;
			numericTypeToSystemType = null;
			systemTypeToNumericType = new Hashtable(6);
			systemTypeToNumericType.Add(typeof(byte).Name, MWNumericType.UInt8);
			systemTypeToNumericType.Add(typeof(short).Name, MWNumericType.Int16);
			systemTypeToNumericType.Add(typeof(int).Name, MWNumericType.Int32);
			systemTypeToNumericType.Add(typeof(long).Name, MWNumericType.Int64);
			systemTypeToNumericType.Add(typeof(float).Name, MWNumericType.Single);
			systemTypeToNumericType.Add(typeof(double).Name, MWNumericType.Double);
			numericTypeToSystemType = new Dictionary<MWNumericType, Type>();
			numericTypeToSystemType.Add(MWNumericType.UInt8, typeof(byte));
			numericTypeToSystemType.Add(MWNumericType.Int16, typeof(short));
			numericTypeToSystemType.Add(MWNumericType.Int32, typeof(int));
			numericTypeToSystemType.Add(MWNumericType.Int64, typeof(long));
			numericTypeToSystemType.Add(MWNumericType.Single, typeof(float));
			numericTypeToSystemType.Add(MWNumericType.Double, typeof(double));
		}

		public MWNumericArray()
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2];
				int[] array2 = array;
				SetMXArray(mxCreateNumericArray(array2.Length, array2, MWNumericType.Double, MWArrayComplexity.Real), MWArrayType.Numeric, array2.Length, array2);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(MWNumericType dataType)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[1];
				int[] array2 = array;
				SetMXArray(mxCreateNumericArray(array2.Length, array2, dataType, MWArrayComplexity.Real), MWArrayType.Numeric, array2.Length, array2);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(byte scalar)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				double value = (int)scalar;
				int[] array = new int[2]
				{
					1,
					1
				};
				SetMXArray(MWIndexArray.mxCreateDoubleScalar(value), MWArrayType.Numeric, array.Length, array);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(byte scalar, bool makeDouble)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				if (makeDouble)
				{
					double value = (int)scalar;
					SetMXArray(MWIndexArray.mxCreateDoubleScalar(value), MWArrayType.Numeric, array.Length, array);
				}
				else
				{
					byte[] source = new byte[1]
					{
						scalar
					};
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.UInt8, MWArrayComplexity.Real), MWArrayType.Numeric, array.Length, array);
					IntPtr destination = MWArray.mxGetData(MXArrayHandle);
					Marshal.Copy(source, 0, destination, 1);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(byte realValue, byte imaginaryValue)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				double[] source = new double[1]
				{
					(int)realValue
				};
				double[] source2 = new double[1]
				{
					(int)imaginaryValue
				};
				SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Double, MWArrayComplexity.Complex), MWArrayType.Numeric, array.Length, array, isComplex: true);
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				IntPtr destination2 = mxGetImagData(MXArrayHandle);
				Marshal.Copy(source, 0, destination, 1);
				Marshal.Copy(source2, 0, destination2, 1);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(byte realValue, byte imaginaryValue, bool makeDouble)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				MWNumericType elementType = makeDouble ? MWNumericType.Double : MWNumericType.UInt8;
				SetMXArray(mxCreateNumericArray(array.Length, array, elementType, MWArrayComplexity.Complex), MWArrayType.Numeric, array.Length, array, isComplex: true);
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				IntPtr destination2 = mxGetImagData(MXArrayHandle);
				if (makeDouble)
				{
					double[] source = new double[1]
					{
						(int)realValue
					};
					double[] source2 = new double[1]
					{
						(int)imaginaryValue
					};
					Marshal.Copy(source, 0, destination, 1);
					Marshal.Copy(source2, 0, destination2, 1);
				}
				else
				{
					byte[] source3 = new byte[1]
					{
						realValue
					};
					byte[] source4 = new byte[1]
					{
						imaginaryValue
					};
					Marshal.Copy(source3, 0, destination, 1);
					Marshal.Copy(source4, 0, destination2, 1);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(short scalar)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				double value = scalar;
				int[] array = new int[2]
				{
					1,
					1
				};
				SetMXArray(MWIndexArray.mxCreateDoubleScalar(value), MWArrayType.Numeric, array.Length, array);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(short scalar, bool makeDouble)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				if (makeDouble)
				{
					double value = scalar;
					SetMXArray(MWIndexArray.mxCreateDoubleScalar(value), MWArrayType.Numeric, array.Length, array);
				}
				else
				{
					short[] source = new short[1]
					{
						scalar
					};
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Int16, MWArrayComplexity.Real), MWArrayType.Numeric, array.Length, array);
					IntPtr destination = MWArray.mxGetData(MXArrayHandle);
					Marshal.Copy(source, 0, destination, 1);
				}
				array_Type = MWArrayType.Numeric;
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(short realValue, short imaginaryValue)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[1]
				{
					1
				};
				double[] source = new double[1]
				{
					realValue
				};
				double[] source2 = new double[1]
				{
					imaginaryValue
				};
				SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Double, MWArrayComplexity.Complex), MWArrayType.Numeric, array.Length, array, isComplex: true);
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				IntPtr destination2 = mxGetImagData(MXArrayHandle);
				Marshal.Copy(source, 0, destination, 1);
				Marshal.Copy(source2, 0, destination2, 1);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(short realValue, short imaginaryValue, bool makeDouble)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[1]
				{
					1
				};
				MWNumericType elementType = makeDouble ? MWNumericType.Double : MWNumericType.Int16;
				SetMXArray(mxCreateNumericArray(array.Length, array, elementType, MWArrayComplexity.Complex), MWArrayType.Numeric, array.Length, array, isComplex: true);
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				IntPtr destination2 = mxGetImagData(MXArrayHandle);
				if (makeDouble)
				{
					double[] source = new double[1]
					{
						realValue
					};
					double[] source2 = new double[1]
					{
						imaginaryValue
					};
					Marshal.Copy(source, 0, destination, 1);
					Marshal.Copy(source2, 0, destination2, 1);
				}
				else
				{
					short[] source3 = new short[1]
					{
						realValue
					};
					short[] source4 = new short[1]
					{
						imaginaryValue
					};
					Marshal.Copy(source3, 0, destination, 1);
					Marshal.Copy(source4, 0, destination2, 1);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(int scalar)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				double value = scalar;
				int[] array = new int[2]
				{
					1,
					1
				};
				SetMXArray(MWIndexArray.mxCreateDoubleScalar(value), MWArrayType.Numeric, array.Length, array);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(int scalar, bool makeDouble)
		{
			int[] array = new int[2]
			{
				1,
				1
			};
			try
			{
				Monitor.Enter(MWArray.mxSync);
				if (makeDouble)
				{
					double value = scalar;
					SetMXArray(MWIndexArray.mxCreateDoubleScalar(value), MWArrayType.Numeric, array.Length, array);
				}
				else
				{
					int[] source = new int[1]
					{
						scalar
					};
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Int32, MWArrayComplexity.Real), MWArrayType.Numeric, array.Length, array);
					IntPtr destination = MWArray.mxGetData(MXArrayHandle);
					Marshal.Copy(source, 0, destination, 1);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(int realValue, int imaginaryValue)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[1]
				{
					1
				};
				double[] source = new double[1]
				{
					realValue
				};
				double[] source2 = new double[1]
				{
					imaginaryValue
				};
				SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Double, MWArrayComplexity.Complex), MWArrayType.Numeric, array.Length, array, isComplex: true);
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				IntPtr destination2 = mxGetImagData(MXArrayHandle);
				Marshal.Copy(source, 0, destination, 1);
				Marshal.Copy(source2, 0, destination2, 1);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(int realValue, int imaginaryValue, bool makeDouble)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[1]
				{
					1
				};
				MWNumericType elementType = makeDouble ? MWNumericType.Double : MWNumericType.Int32;
				SetMXArray(mxCreateNumericArray(array.Length, array, elementType, MWArrayComplexity.Complex), MWArrayType.Numeric, array.Length, array, isComplex: true);
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				IntPtr destination2 = mxGetImagData(MXArrayHandle);
				if (makeDouble)
				{
					double[] source = new double[1]
					{
						realValue
					};
					double[] source2 = new double[1]
					{
						imaginaryValue
					};
					Marshal.Copy(source, 0, destination, 1);
					Marshal.Copy(source2, 0, destination2, 1);
				}
				else
				{
					int[] source3 = new int[1]
					{
						realValue
					};
					int[] source4 = new int[1]
					{
						imaginaryValue
					};
					Marshal.Copy(source3, 0, destination, 1);
					Marshal.Copy(source4, 0, destination2, 1);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(long scalar)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				double value = scalar;
				int[] array = new int[2]
				{
					1,
					1
				};
				SetMXArray(MWIndexArray.mxCreateDoubleScalar(value), MWArrayType.Numeric, array.Length, array);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(long scalar, bool makeDouble)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				if (makeDouble)
				{
					double value = scalar;
					SetMXArray(MWIndexArray.mxCreateDoubleScalar(value), MWArrayType.Numeric, array.Length, array);
				}
				else
				{
					long[] source = new long[1]
					{
						scalar
					};
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Int64, MWArrayComplexity.Real), MWArrayType.Numeric, array.Length, array);
					IntPtr destination = MWArray.mxGetData(MXArrayHandle);
					Marshal.Copy(source, 0, destination, 1);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(long realValue, long imaginaryValue)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				double[] source = new double[1]
				{
					realValue
				};
				double[] source2 = new double[1]
				{
					imaginaryValue
				};
				SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Double, MWArrayComplexity.Complex), MWArrayType.Numeric, array.Length, array, isComplex: true);
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				IntPtr destination2 = mxGetImagData(MXArrayHandle);
				Marshal.Copy(source, 0, destination, 1);
				Marshal.Copy(source2, 0, destination2, 1);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(long realValue, long imaginaryValue, bool makeDouble)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				MWNumericType elementType = makeDouble ? MWNumericType.Double : MWNumericType.Int64;
				SetMXArray(mxCreateNumericArray(array.Length, array, elementType, MWArrayComplexity.Complex), MWArrayType.Numeric, array.Length, array, isComplex: true);
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				IntPtr destination2 = mxGetImagData(MXArrayHandle);
				if (makeDouble)
				{
					double[] source = new double[1]
					{
						realValue
					};
					double[] source2 = new double[1]
					{
						imaginaryValue
					};
					Marshal.Copy(source, 0, destination, 1);
					Marshal.Copy(source2, 0, destination2, 1);
				}
				else
				{
					long[] source3 = new long[1]
					{
						realValue
					};
					long[] source4 = new long[1]
					{
						imaginaryValue
					};
					Marshal.Copy(source3, 0, destination, 1);
					Marshal.Copy(source4, 0, destination2, 1);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(float scalar)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				double value = scalar;
				int[] array = new int[2]
				{
					1,
					1
				};
				SetMXArray(MWIndexArray.mxCreateDoubleScalar(value), MWArrayType.Numeric, array.Length, array);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(float scalar, bool makeDouble)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				if (makeDouble)
				{
					double value = scalar;
					SetMXArray(MWIndexArray.mxCreateDoubleScalar(value), MWArrayType.Numeric, array.Length, array);
				}
				else
				{
					float[] source = new float[1]
					{
						scalar
					};
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Single, MWArrayComplexity.Real), MWArrayType.Numeric, array.Length, array);
					IntPtr destination = MWArray.mxGetData(MXArrayHandle);
					Marshal.Copy(source, 0, destination, 1);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(float realValue, float imaginaryValue)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[1]
				{
					1
				};
				double[] source = new double[1]
				{
					realValue
				};
				double[] source2 = new double[1]
				{
					imaginaryValue
				};
				SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Double, MWArrayComplexity.Complex), MWArrayType.Numeric, array.Length, array, isComplex: true);
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				IntPtr destination2 = mxGetImagData(MXArrayHandle);
				Marshal.Copy(source, 0, destination, 1);
				Marshal.Copy(source2, 0, destination2, 1);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(float realValue, float imaginaryValue, bool makeDouble)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				MWNumericType elementType = makeDouble ? MWNumericType.Double : MWNumericType.Single;
				SetMXArray(mxCreateNumericArray(array.Length, array, elementType, MWArrayComplexity.Complex), MWArrayType.Numeric, array.Length, array, isComplex: true);
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				IntPtr destination2 = mxGetImagData(MXArrayHandle);
				if (makeDouble)
				{
					double[] source = new double[1]
					{
						realValue
					};
					double[] source2 = new double[1]
					{
						imaginaryValue
					};
					Marshal.Copy(source, 0, destination, 1);
					Marshal.Copy(source2, 0, destination2, 1);
				}
				else
				{
					float[] source3 = new float[1]
					{
						realValue
					};
					float[] source4 = new float[1]
					{
						imaginaryValue
					};
					Marshal.Copy(source3, 0, destination, 1);
					Marshal.Copy(source4, 0, destination2, 1);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(double scalar)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				SetMXArray(MWIndexArray.mxCreateDoubleScalar(scalar), MWArrayType.Numeric, array.Length, array);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(double realValue, double imaginaryValue)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int[] array = new int[2]
				{
					1,
					1
				};
				double[] source = new double[1]
				{
					realValue
				};
				double[] source2 = new double[1]
				{
					imaginaryValue
				};
				SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Double, MWArrayComplexity.Complex), MWArrayType.Numeric, array.Length, array, isComplex: true);
				IntPtr destination = MWArray.mxGetData(MXArrayHandle);
				IntPtr destination2 = mxGetImagData(MXArrayHandle);
				Marshal.Copy(source, 0, destination, 1);
				Marshal.Copy(source2, 0, destination2, 1);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(params int[] dimensions)
		{
			if (dimensions == null)
			{
				throw new ArgumentNullException("dimensions");
			}
			try
			{
				Monitor.Enter(MWArray.mxSync);
				SetMXArray(mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, MWArrayComplexity.Real), MWArrayType.Numeric, dimensions.Length, dimensions);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(MWArrayComplexity complexity, params int[] dimensions)
		{
			if (dimensions == null)
			{
				throw new ArgumentNullException("dimensions");
			}
			try
			{
				Monitor.Enter(MWArray.mxSync);
				SetMXArray(mxCreateNumericArray(dimensions.Length, dimensions, MWNumericType.Double, complexity), MWArrayType.Numeric, dimensions.Length, dimensions, (complexity == MWArrayComplexity.Complex) ? true : false);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(MWArrayComplexity complexity, MWNumericType dataType, params int[] dimensions)
		{
			if (dimensions == null)
			{
				throw new ArgumentNullException("dimensions");
			}
			try
			{
				Monitor.Enter(MWArray.mxSync);
				SetMXArray(mxCreateNumericArray(dimensions.Length, dimensions, dataType, complexity), MWArrayType.Numeric, dimensions.Length, dimensions, (complexity == MWArrayComplexity.Complex) ? true : false);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(int rows, int columns, byte[] realData)
			: this(rows, columns, realData, null, makeDouble: true, rowMajorData: true)
		{
		}

		public MWNumericArray(int rows, int columns, byte[] realData, bool makeDouble, bool rowMajorData)
			: this(rows, columns, realData, null, makeDouble, rowMajorData)
		{
		}

		public MWNumericArray(int rows, int columns, byte[] realData, byte[] imaginaryData)
			: this(rows, columns, realData, imaginaryData, makeDouble: true, rowMajorData: true)
		{
		}

		public MWNumericArray(int rows, int columns, byte[] realData, byte[] imaginaryData, bool makeDouble, bool rowMajorData)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				validateInput(rows * columns, realData, imaginaryData);
				int[] array = new int[2]
				{
					rows,
					columns
				};
				int num = realData.Length;
				MWArrayComplexity mWArrayComplexity = (imaginaryData != null) ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
				if (!makeDouble)
				{
					byte[] array2 = rowMajorData ? new byte[num] : realData;
					byte[] array3 = imaginaryData;
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.UInt8, mWArrayComplexity), MWArrayType.Numeric, array.Length, array, (mWArrayComplexity == MWArrayComplexity.Complex) ? true : false);
					if (rowMajorData)
					{
						array3 = ((imaginaryData != null) ? new byte[num] : null);
						for (int i = 0; i < rows; i++)
						{
							for (int j = 0; j < columns; j++)
							{
								array2[j * rows + i] = realData[i * columns + j];
								if (array3 != null)
								{
									array3[j * rows + i] = imaginaryData[i * columns + j];
								}
							}
						}
					}
					IntPtr intPtr = MWArray.mxGetData(MXArrayHandle);
					IntPtr intPtr2 = (imaginaryData != null) ? mxGetImagData(MXArrayHandle) : IntPtr.Zero;
					if (IntPtr.Zero != intPtr)
					{
						Marshal.Copy(array2, 0, intPtr, num);
					}
					if (imaginaryData != null && IntPtr.Zero != intPtr2)
					{
						Marshal.Copy(array3, 0, intPtr2, num);
					}
				}
				else
				{
					double[] array4 = new double[num];
					double[] array5 = (imaginaryData != null) ? new double[num] : null;
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Double, mWArrayComplexity), MWArrayType.Numeric, array.Length, array, (mWArrayComplexity == MWArrayComplexity.Complex) ? true : false);
					if (!rowMajorData)
					{
						for (int k = 0; k < num; k++)
						{
							array4[k] = (int)realData[k];
							if (array5 != null)
							{
								array5[k] = (int)imaginaryData[k];
							}
						}
					}
					else
					{
						for (int l = 0; l < rows; l++)
						{
							for (int m = 0; m < columns; m++)
							{
								array4[m * rows + l] = (int)realData[l * columns + m];
								if (array5 != null)
								{
									array5[m * rows + l] = (int)imaginaryData[l * columns + m];
								}
							}
						}
					}
					IntPtr intPtr3 = MWArray.mxGetData(MXArrayHandle);
					IntPtr intPtr4 = (imaginaryData != null) ? mxGetImagData(MXArrayHandle) : IntPtr.Zero;
					if (IntPtr.Zero != intPtr3)
					{
						Marshal.Copy(array4, 0, intPtr3, num);
					}
					if (imaginaryData != null && IntPtr.Zero != intPtr4)
					{
						Marshal.Copy(array5, 0, intPtr4, num);
					}
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(int rows, int columns, short[] realData)
			: this(rows, columns, realData, null, makeDouble: true, rowMajorData: true)
		{
		}

		public MWNumericArray(int rows, int columns, short[] realData, bool makeDouble, bool rowMajorData)
			: this(rows, columns, realData, null, makeDouble, rowMajorData)
		{
		}

		public MWNumericArray(int rows, int columns, short[] realData, short[] imaginaryData)
			: this(rows, columns, realData, imaginaryData, makeDouble: true, rowMajorData: true)
		{
		}

		public MWNumericArray(int rows, int columns, short[] realData, short[] imaginaryData, bool makeDouble, bool rowMajorData)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				validateInput(rows * columns, realData, imaginaryData);
				int[] array = new int[2]
				{
					rows,
					columns
				};
				int num = realData.Length;
				MWArrayComplexity mWArrayComplexity = (imaginaryData != null) ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
				if (!makeDouble)
				{
					short[] array2 = rowMajorData ? new short[realData.Length] : realData;
					short[] array3 = imaginaryData;
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Int16, mWArrayComplexity), MWArrayType.Numeric, array.Length, array, (mWArrayComplexity == MWArrayComplexity.Complex) ? true : false);
					if (rowMajorData)
					{
						array3 = ((imaginaryData != null) ? new short[realData.Length] : null);
						for (int i = 0; i < rows; i++)
						{
							for (int j = 0; j < columns; j++)
							{
								array2[j * rows + i] = realData[i * columns + j];
								if (array3 != null)
								{
									array3[j * rows + i] = imaginaryData[i * columns + j];
								}
							}
						}
					}
					IntPtr intPtr = MWArray.mxGetData(MXArrayHandle);
					IntPtr intPtr2 = (imaginaryData != null) ? mxGetImagData(MXArrayHandle) : IntPtr.Zero;
					if (IntPtr.Zero != intPtr)
					{
						Marshal.Copy(array2, 0, intPtr, num);
					}
					if (imaginaryData != null && IntPtr.Zero != intPtr2)
					{
						Marshal.Copy(array3, 0, intPtr2, num);
					}
				}
				else
				{
					double[] array4 = new double[num];
					double[] array5 = (imaginaryData != null) ? new double[imaginaryData.Length] : null;
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Double, mWArrayComplexity), MWArrayType.Numeric, array.Length, array, (mWArrayComplexity == MWArrayComplexity.Complex) ? true : false);
					if (!rowMajorData)
					{
						for (int k = 0; k < num; k++)
						{
							array4[k] = realData[k];
							if (array5 != null)
							{
								array5[k] = imaginaryData[k];
							}
						}
					}
					else
					{
						for (int l = 0; l < rows; l++)
						{
							for (int m = 0; m < columns; m++)
							{
								array4[m * rows + l] = realData[l * columns + m];
								if (array5 != null)
								{
									array5[m * rows + l] = imaginaryData[l * columns + m];
								}
							}
						}
					}
					IntPtr intPtr3 = MWArray.mxGetData(MXArrayHandle);
					IntPtr intPtr4 = (imaginaryData != null) ? mxGetImagData(MXArrayHandle) : IntPtr.Zero;
					if (IntPtr.Zero != intPtr3)
					{
						Marshal.Copy(array4, 0, intPtr3, num);
					}
					if (imaginaryData != null && IntPtr.Zero != intPtr4)
					{
						Marshal.Copy(array5, 0, intPtr4, num);
					}
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(int rows, int columns, int[] realData)
			: this(rows, columns, realData, null, makeDouble: true, rowMajorData: true)
		{
		}

		public MWNumericArray(int rows, int columns, int[] realData, bool makeDouble, bool rowMajorData)
			: this(rows, columns, realData, null, makeDouble, rowMajorData)
		{
		}

		public MWNumericArray(int rows, int columns, int[] realData, int[] imaginaryData)
			: this(rows, columns, realData, imaginaryData, makeDouble: true, rowMajorData: true)
		{
		}

		public MWNumericArray(int rows, int columns, int[] realData, int[] imaginaryData, bool makeDouble, bool rowMajorData)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				validateInput(rows * columns, realData, imaginaryData);
				int[] array = new int[2]
				{
					rows,
					columns
				};
				int num = realData.Length;
				MWArrayComplexity mWArrayComplexity = (imaginaryData != null) ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
				if (!makeDouble)
				{
					int[] array2 = rowMajorData ? new int[realData.Length] : realData;
					int[] array3 = imaginaryData;
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Int32, mWArrayComplexity), MWArrayType.Numeric, array.Length, array, (mWArrayComplexity == MWArrayComplexity.Complex) ? true : false);
					if (rowMajorData)
					{
						array3 = ((imaginaryData != null) ? new int[realData.Length] : null);
						for (int i = 0; i < rows; i++)
						{
							for (int j = 0; j < columns; j++)
							{
								array2[j * rows + i] = realData[i * columns + j];
								if (array3 != null)
								{
									array3[j * rows + i] = imaginaryData[i * columns + j];
								}
							}
						}
					}
					IntPtr intPtr = MWArray.mxGetData(MXArrayHandle);
					IntPtr intPtr2 = (imaginaryData != null) ? mxGetImagData(MXArrayHandle) : IntPtr.Zero;
					if (IntPtr.Zero != intPtr)
					{
						Marshal.Copy(array2, 0, intPtr, num);
					}
					if (imaginaryData != null && IntPtr.Zero != intPtr2)
					{
						Marshal.Copy(array3, 0, intPtr2, num);
					}
				}
				else
				{
					double[] array4 = new double[num];
					double[] array5 = (imaginaryData != null) ? new double[imaginaryData.Length] : null;
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Double, mWArrayComplexity), MWArrayType.Numeric, array.Length, array, (mWArrayComplexity == MWArrayComplexity.Complex) ? true : false);
					if (!rowMajorData)
					{
						for (int k = 0; k < num; k++)
						{
							array4[k] = realData[k];
							if (array5 != null)
							{
								array5[k] = imaginaryData[k];
							}
						}
					}
					else
					{
						for (int l = 0; l < rows; l++)
						{
							for (int m = 0; m < columns; m++)
							{
								array4[m * rows + l] = realData[l * columns + m];
								if (array5 != null)
								{
									array5[m * rows + l] = imaginaryData[l * columns + m];
								}
							}
						}
					}
					IntPtr intPtr3 = MWArray.mxGetData(MXArrayHandle);
					IntPtr intPtr4 = (imaginaryData != null) ? mxGetImagData(MXArrayHandle) : IntPtr.Zero;
					if (IntPtr.Zero != intPtr3)
					{
						Marshal.Copy(array4, 0, intPtr3, num);
					}
					if (imaginaryData != null && IntPtr.Zero != intPtr4)
					{
						Marshal.Copy(array5, 0, intPtr4, num);
					}
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(int rows, int columns, long[] realData)
			: this(rows, columns, realData, null, makeDouble: true, rowMajorData: true)
		{
		}

		public MWNumericArray(int rows, int columns, long[] realData, bool makeDouble, bool rowMajorData)
			: this(rows, columns, realData, null, makeDouble, rowMajorData)
		{
		}

		public MWNumericArray(int rows, int columns, long[] realData, long[] imaginaryData)
			: this(rows, columns, realData, imaginaryData, makeDouble: true, rowMajorData: true)
		{
		}

		public MWNumericArray(int rows, int columns, long[] realData, long[] imaginaryData, bool makeDouble, bool rowMajorData)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				validateInput(rows * columns, realData, imaginaryData);
				int[] array = new int[2]
				{
					rows,
					columns
				};
				int num = realData.Length;
				MWArrayComplexity mWArrayComplexity = (imaginaryData != null) ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
				if (!makeDouble)
				{
					long[] array2 = rowMajorData ? new long[realData.Length] : realData;
					long[] array3 = imaginaryData;
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Int64, mWArrayComplexity), MWArrayType.Numeric, array.Length, array, (mWArrayComplexity == MWArrayComplexity.Complex) ? true : false);
					if (rowMajorData)
					{
						array3 = ((imaginaryData != null) ? new long[realData.Length] : null);
						for (int i = 0; i < rows; i++)
						{
							for (int j = 0; j < columns; j++)
							{
								array2[j * rows + i] = realData[i * columns + j];
								if (array3 != null)
								{
									array3[j * rows + i] = imaginaryData[i * columns + j];
								}
							}
						}
					}
					IntPtr intPtr = MWArray.mxGetData(MXArrayHandle);
					IntPtr intPtr2 = (imaginaryData != null) ? mxGetImagData(MXArrayHandle) : IntPtr.Zero;
					if (IntPtr.Zero != intPtr)
					{
						Marshal.Copy(array2, 0, intPtr, num);
					}
					if (imaginaryData != null && IntPtr.Zero != intPtr2)
					{
						Marshal.Copy(array3, 0, intPtr2, num);
					}
				}
				else
				{
					double[] array4 = new double[num];
					double[] array5 = (imaginaryData != null) ? new double[imaginaryData.Length] : null;
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Double, mWArrayComplexity), MWArrayType.Numeric, array.Length, array, (mWArrayComplexity == MWArrayComplexity.Complex) ? true : false);
					if (!rowMajorData)
					{
						for (int k = 0; k < num; k++)
						{
							array4[k] = realData[k];
							if (array5 != null)
							{
								array5[k] = imaginaryData[k];
							}
						}
					}
					else
					{
						for (int l = 0; l < rows; l++)
						{
							for (int m = 0; m < columns; m++)
							{
								array4[m * rows + l] = realData[l * columns + m];
								if (array5 != null)
								{
									array5[m * rows + l] = imaginaryData[l * columns + m];
								}
							}
						}
					}
					IntPtr intPtr3 = MWArray.mxGetData(MXArrayHandle);
					IntPtr intPtr4 = (imaginaryData != null) ? mxGetImagData(MXArrayHandle) : IntPtr.Zero;
					if (IntPtr.Zero != intPtr3)
					{
						Marshal.Copy(array4, 0, intPtr3, num);
					}
					if (imaginaryData != null && IntPtr.Zero != intPtr4)
					{
						Marshal.Copy(array5, 0, intPtr4, num);
					}
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(int rows, int columns, float[] realData)
			: this(rows, columns, realData, null, makeDouble: true, rowMajorData: true)
		{
		}

		public MWNumericArray(int rows, int columns, float[] realData, bool makeDouble, bool rowMajorData)
			: this(rows, columns, realData, null, makeDouble, rowMajorData)
		{
		}

		public MWNumericArray(int rows, int columns, float[] realData, float[] imaginaryData)
			: this(rows, columns, realData, imaginaryData, makeDouble: true, rowMajorData: true)
		{
		}

		public MWNumericArray(int rows, int columns, float[] realData, float[] imaginaryData, bool makeDouble, bool rowMajorData)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				validateInput(rows * columns, realData, imaginaryData);
				int[] array = new int[2]
				{
					rows,
					columns
				};
				int num = realData.Length;
				MWArrayComplexity mWArrayComplexity = (imaginaryData != null) ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
				if (!makeDouble)
				{
					float[] array2 = rowMajorData ? new float[realData.Length] : realData;
					float[] array3 = imaginaryData;
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Single, mWArrayComplexity), MWArrayType.Numeric, array.Length, array, (mWArrayComplexity == MWArrayComplexity.Complex) ? true : false);
					if (rowMajorData)
					{
						array3 = ((imaginaryData != null) ? new float[realData.Length] : null);
						for (int i = 0; i < rows; i++)
						{
							for (int j = 0; j < columns; j++)
							{
								array2[j * rows + i] = realData[i * columns + j];
								if (array3 != null)
								{
									array3[j * rows + i] = imaginaryData[i * columns + j];
								}
							}
						}
					}
					IntPtr intPtr = MWArray.mxGetData(MXArrayHandle);
					IntPtr intPtr2 = (imaginaryData != null) ? mxGetImagData(MXArrayHandle) : IntPtr.Zero;
					if (IntPtr.Zero != intPtr)
					{
						Marshal.Copy(array2, 0, intPtr, num);
					}
					if (imaginaryData != null && IntPtr.Zero != intPtr2)
					{
						Marshal.Copy(array3, 0, intPtr2, num);
					}
				}
				else
				{
					double[] array4 = new double[num];
					double[] array5 = (imaginaryData != null) ? new double[imaginaryData.Length] : null;
					SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Double, mWArrayComplexity), MWArrayType.Numeric, array.Length, array, (mWArrayComplexity == MWArrayComplexity.Complex) ? true : false);
					if (!rowMajorData)
					{
						for (int k = 0; k < num; k++)
						{
							array4[k] = realData[k];
							if (array5 != null)
							{
								array5[k] = imaginaryData[k];
							}
						}
					}
					else
					{
						for (int l = 0; l < rows; l++)
						{
							for (int m = 0; m < columns; m++)
							{
								array4[m * rows + l] = realData[l * columns + m];
								if (array5 != null)
								{
									array5[m * rows + l] = imaginaryData[l * columns + m];
								}
							}
						}
					}
					IntPtr intPtr3 = MWArray.mxGetData(MXArrayHandle);
					IntPtr intPtr4 = (imaginaryData != null) ? mxGetImagData(MXArrayHandle) : IntPtr.Zero;
					if (IntPtr.Zero != intPtr3)
					{
						Marshal.Copy(array4, 0, intPtr3, num);
					}
					if (imaginaryData != null && IntPtr.Zero != intPtr4)
					{
						Marshal.Copy(array5, 0, intPtr4, num);
					}
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(int rows, int columns, double[] realData)
			: this(rows, columns, realData, null, rowMajorData: true)
		{
		}

		public MWNumericArray(int rows, int columns, double[] realData, bool rowMajorData)
			: this(rows, columns, realData, null, rowMajorData)
		{
		}

		public MWNumericArray(int rows, int columns, double[] realData, double[] imaginaryData)
			: this(rows, columns, realData, imaginaryData, rowMajorData: true)
		{
		}

		public MWNumericArray(int rows, int columns, double[] realData, double[] imaginaryData, bool rowMajorData)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				validateInput(rows * columns, realData, imaginaryData);
				int[] array = new int[2]
				{
					rows,
					columns
				};
				int length = realData.Length;
				MWArrayComplexity mWArrayComplexity = (imaginaryData != null) ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
				double[] array2 = rowMajorData ? new double[realData.Length] : realData;
				double[] array3 = imaginaryData;
				SetMXArray(mxCreateNumericArray(array.Length, array, MWNumericType.Double, mWArrayComplexity), MWArrayType.Numeric, array.Length, array, (mWArrayComplexity == MWArrayComplexity.Complex) ? true : false);
				if (rowMajorData)
				{
					array3 = ((imaginaryData != null) ? new double[realData.Length] : null);
					for (int i = 0; i < rows; i++)
					{
						for (int j = 0; j < columns; j++)
						{
							array2[j * rows + i] = realData[i * columns + j];
							if (array3 != null)
							{
								array3[j * rows + i] = imaginaryData[i * columns + j];
							}
						}
					}
				}
				IntPtr intPtr = MWArray.mxGetData(MXArrayHandle);
				IntPtr intPtr2 = (imaginaryData != null) ? mxGetImagData(MXArrayHandle) : IntPtr.Zero;
				if (IntPtr.Zero != intPtr)
				{
					Marshal.Copy(array2, 0, intPtr, length);
				}
				if (imaginaryData != null && IntPtr.Zero != intPtr2)
				{
					Marshal.Copy(array3, 0, intPtr2, length);
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWNumericArray(Array array)
		{
			if (JaggedArray.IsJagged(array))
			{
				JaggedArray jaggedArray = new JaggedArray(array);
				int[] dimensions = jaggedArray.GetDimensions();
				int[] array2 = MWArray.NetDimensionToMATLABDimension(dimensions);
				Array flatArray = jaggedArray.GetFlatArray();
				lock (MWArray.mxSync)
				{
					IntPtr zero = IntPtr.Zero;
					MWNumericType elementType = (MWNumericType)systemTypeToNumericType[jaggedArray.GetElementType().Name];
					MWSafeHandle hMXArray = mxCreateNumericArray(array2.Length, array2, elementType, MWArrayComplexity.Real);
					zero = MWArray.mxGetData(hMXArray);
					MWMarshal.MarshalManagedFlatArrayToUnmanaged(flatArray, zero);
					SetMXArray(hMXArray, MWArrayType.Numeric);
				}
			}
			else
			{
				FastBuildNumericArray(array, null, makeDouble: true, rowMajorData: true);
			}
		}

		public MWNumericArray(Array realData, bool makeDouble, bool rowMajorData)
			: this(realData, null, makeDouble, rowMajorData)
		{
		}

		public MWNumericArray(Array realData, Array imaginaryData)
			: this(realData, imaginaryData, makeDouble: true, rowMajorData: true)
		{
		}

		public MWNumericArray(Array realData, Array imaginaryData, bool makeDouble, bool rowMajorData)
		{
			FastBuildNumericArray(realData, imaginaryData, makeDouble, rowMajorData);
			array_Type = MWArrayType.Numeric;
		}

		internal MWNumericArray(MWSafeHandle hMXArray)
			: base(hMXArray)
		{
			array_Type = MWArrayType.Numeric;
		}

		internal MWNumericArray(MWArrayComponent arrayComponent, int rows, int columns)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				MWArrayComplexity mWArrayComplexity = (arrayComponent != 0) ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
				int[] array = new int[2]
				{
					rows,
					columns
				};
				SetMXArray(mxCreateDoubleMatrix(rows, columns, mWArrayComplexity), MWArrayType.Numeric, array.Length, array, (mWArrayComplexity == MWArrayComplexity.Complex) ? true : false);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		internal MWNumericArray(double start, double step, double end)
		{
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

		public static implicit operator MWNumericArray(byte scalar)
		{
			return new MWNumericArray(scalar, makeDouble: false);
		}

		public static implicit operator MWNumericArray(short scalar)
		{
			return new MWNumericArray(scalar, makeDouble: false);
		}

		public static implicit operator MWNumericArray(int scalar)
		{
			return new MWNumericArray(scalar);
		}

		public static implicit operator MWNumericArray(long scalar)
		{
			return new MWNumericArray(scalar, makeDouble: false);
		}

		public static implicit operator MWNumericArray(float scalar)
		{
			return new MWNumericArray(scalar, makeDouble: false);
		}

		public static implicit operator MWNumericArray(double scalar)
		{
			return new MWNumericArray(scalar);
		}

		public static implicit operator MWNumericArray(byte[] values)
		{
			return new MWNumericArray(1, values.Length, values, null, makeDouble: false, rowMajorData: false);
		}

		public static implicit operator MWNumericArray(short[] values)
		{
			return new MWNumericArray(1, values.Length, values, null, makeDouble: false, rowMajorData: false);
		}

		public static implicit operator MWNumericArray(int[] values)
		{
			return new MWNumericArray(1, values.Length, values);
		}

		public static implicit operator MWNumericArray(long[] values)
		{
			return new MWNumericArray(1, values.Length, values, null, makeDouble: false, rowMajorData: false);
		}

		public static implicit operator MWNumericArray(float[] values)
		{
			return new MWNumericArray(1, values.Length, values, null, makeDouble: false, rowMajorData: false);
		}

		public static implicit operator MWNumericArray(double[] values)
		{
			return new MWNumericArray(1, values.Length, values);
		}

		public static implicit operator MWNumericArray(Array realData)
		{
			return new MWNumericArray(realData, null, makeDouble: false, rowMajorData: true);
		}

		public static explicit operator byte(MWNumericArray array)
		{
			array.CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				if (9 != MWArray.mxGetClassID(array.MXArrayHandle))
				{
					string @string = MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType");
					throw new ApplicationException(@string);
				}
				return checked((byte)MWArray.mxGetScalar(array.MXArrayHandle));
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public static explicit operator short(MWNumericArray array)
		{
			array.CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				if (10 != MWArray.mxGetClassID(array.MXArrayHandle))
				{
					string @string = MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType");
					throw new ApplicationException(@string);
				}
				return checked((short)MWArray.mxGetScalar(array.MXArrayHandle));
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public static explicit operator int(MWNumericArray array)
		{
			array.CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int num = MWArray.mxGetClassID(array.MXArrayHandle);
				if (12 != num && 6 != num)
				{
					string @string = MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType");
					throw new ApplicationException(@string);
				}
				return checked((int)MWArray.mxGetScalar(array.MXArrayHandle));
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public static explicit operator long(MWNumericArray array)
		{
			array.CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				if (14 != MWArray.mxGetClassID(array.MXArrayHandle))
				{
					string @string = MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType");
					throw new ApplicationException(@string);
				}
				return checked((long)MWArray.mxGetScalar(array.MXArrayHandle));
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public static explicit operator float(MWNumericArray array)
		{
			array.CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				if (7 != MWArray.mxGetClassID(array.MXArrayHandle))
				{
					string @string = MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType");
					throw new ApplicationException(@string);
				}
				return (float)MWArray.mxGetScalar(array.MXArrayHandle);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public static explicit operator double(MWNumericArray array)
		{
			array.CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				if (6 != MWArray.mxGetClassID(array.MXArrayHandle))
				{
					string @string = MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType");
					throw new ApplicationException(@string);
				}
				return MWArray.mxGetScalar(array.MXArrayHandle);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public byte ToScalarByte()
		{
			return (byte)this;
		}

		public short ToScalarShort()
		{
			return (short)this;
		}

		public int ToScalarInteger()
		{
			return (int)this;
		}

		public long ToScalarLong()
		{
			return (long)this;
		}

		public float ToScalarFloat()
		{
			return (float)this;
		}

		public double ToScalarDouble()
		{
			return (double)this;
		}

		protected MWNumericArray(SerializationInfo serializationInfo, StreamingContext context)
			: base(serializationInfo, context)
		{
		}

		private static void validateInput(int specifiedSize, Array realData, Array imaginaryData)
		{
			if (realData == null)
			{
				throw new ArgumentNullException("realData");
			}
			if (realData.LongLength != specifiedSize)
			{
				string @string = MWArray.resourceManager.GetString("MWErrorDataArraySize");
				throw new ArgumentException(@string, "realData");
			}
			validateRealAndImaginaryInput(realData, imaginaryData);
		}

		private static void validateInput(Array realData, Array imaginaryData)
		{
			if (realData == null)
			{
				throw new ArgumentNullException("realData");
			}
			Type elementType = realData.GetType().GetElementType();
			if (typeof(double) != elementType && typeof(int) != elementType && typeof(byte) != elementType && typeof(short) != elementType && typeof(long) != elementType && typeof(float) != elementType)
			{
				string @string = MWArray.resourceManager.GetString("MWErrorDataArrayType");
				throw new ArgumentException(@string);
			}
			validateRealAndImaginaryInput(realData, imaginaryData);
		}

		private static void validateRealAndImaginaryInput(Array realData, Array imaginaryData)
		{
			if (imaginaryData == null)
			{
				return;
			}
			if (realData.GetType().GetElementType() != imaginaryData.GetType().GetElementType())
			{
				string @string = MWArray.resourceManager.GetString("MWErrorDataArrayType");
				throw new ArgumentException(@string, "imaginaryData");
			}
			if (realData.Length != imaginaryData.Length)
			{
				string string2 = MWArray.resourceManager.GetString("MWErrorDataArraySizeMismatch");
				throw new ArgumentException(string2);
			}
			int rank = realData.Rank;
			if (rank != imaginaryData.Rank)
			{
				string string3 = MWArray.resourceManager.GetString("MWErrorDataArrayRankMismatch");
				throw new ArgumentException(string3);
			}
			int num = 0;
			while (true)
			{
				if (num < rank)
				{
					if (realData.GetLength(num) != imaginaryData.GetLength(num))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			string string4 = MWArray.resourceManager.GetString("MWErrorRealImaginaryDimensionMismatch");
			throw new ArgumentException(string4);
		}

		public static MWNumericArray MakeSparse(int rows, int columns, MWArrayComplexity complexity, int nonZeroMax)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				if (mclGetNumericSparse(out IntPtr pMXArray, (IntPtr)0, null, (IntPtr)0, null, (IntPtr)0, null, null, (IntPtr)rows, (IntPtr)columns, (IntPtr)nonZeroMax, MATLABArrayType.Double, complexity) == 0)
				{
					if (MWArray.mclArrayHandle2mxArray(out MWSafeHandle hMXArray, pMXArray) != 0)
					{
						string @string = MWArray.resourceManager.GetString("MWErrorInvalidArray");
						throw new Exception(@string);
					}
					return new MWNumericArray(hMXArray);
				}
				return null;
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public static MWNumericArray MakeSparse(int[] rowIndex, int[] columnIndex, double[] realData)
		{
			return MakeSparse(rowIndex, columnIndex, realData, null);
		}

		public static MWNumericArray MakeSparse(int[] rowIndex, int[] columnIndex, double[] realData, double[] imaginaryData)
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
				return MakeSparse(num2, num3, rowIndex, columnIndex, realData, imaginaryData);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public static MWNumericArray MakeSparse(int rows, int columns, int[] rowIndex, int[] columnIndex, double[] realData)
		{
			return MakeSparse(rows, columns, rowIndex, columnIndex, realData, null);
		}

		public static MWNumericArray MakeSparse(int rows, int columns, int[] rowIndex, int[] columnIndex, double[] realData, int nonZeroMax)
		{
			return MakeSparse(rows, columns, rowIndex, columnIndex, realData, null, nonZeroMax);
		}

		public static MWNumericArray MakeSparse(int rows, int columns, int[] rowIndex, int[] columnIndex, double[] realData, double[] imaginaryData)
		{
			return MakeSparse(rows, columns, rowIndex, columnIndex, realData, imaginaryData, rowIndex.Length);
		}

		public static MWNumericArray MakeSparse(int rows, int columns, int[] rowIndex, int[] columnIndex, double[] realData, double[] imaginaryData, int nonZeroMax)
		{
			try
			{
				Monitor.Enter(MWArray.mxSync);
				IntPtr pMXArray = IntPtr.Zero;
				MWArrayComplexity complexityFlag = (imaginaryData != null) ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
				IntPtr[] array = new IntPtr[rowIndex.Length];
				IntPtr[] array2 = new IntPtr[columnIndex.Length];
				for (int i = 0; i < rowIndex.Length; i++)
				{
					array[i] = (IntPtr)rowIndex[i];
					array2[i] = (IntPtr)columnIndex[i];
				}
				if (mclGetNumericSparse(out pMXArray, (IntPtr)rowIndex.Length, array, (IntPtr)columnIndex.Length, array2, (IntPtr)realData.Length, realData, imaginaryData, (IntPtr)rows, (IntPtr)columns, (IntPtr)nonZeroMax, MATLABArrayType.Double, complexityFlag) == 0)
				{
					if (MWArray.mclArrayHandle2mxArray(out MWSafeHandle hMXArray, pMXArray) != 0)
					{
						string @string = MWArray.resourceManager.GetString("MWErrorInvalidArray");
						throw new Exception(@string);
					}
					return new MWNumericArray(hMXArray);
				}
				return null;
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		internal Array MarshalToFullArray<T>(int rows, int[,] rowIndices, int cols, int[,] colIndices, int numSparseElements, Array vectorData)
		{
			Array array = Array.CreateInstance(typeof(T), rows, cols);
			for (int i = 0; i < numSparseElements; i++)
			{
				T val = ((T[])vectorData)[i];
				array.SetValue(val, rowIndices[0, i] - 1, colIndices[0, i] - 1);
			}
			return array;
		}

		internal Array ToArray(MWArrayComponent component, bool sparseIndex)
		{
			CheckDisposed();
			lock (MWArray.mxSync)
			{
				IntPtr intPtr = (component == MWArrayComponent.Real) ? MWArray.mxGetData(MXArrayHandle) : mxGetImagData(MXArrayHandle);
				if (intPtr == IntPtr.Zero && component == MWArrayComponent.Imaginary)
				{
					throw new InvalidDataException("No imaginary data elements in the array.");
				}
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
					switch (NumericType)
					{
					case MWNumericType.UInt8:
					{
						Array array10 = Array.CreateInstance(typeof(byte), numberOfElements);
						Marshal.Copy(intPtr, (byte[])array10, 0, numberOfElements);
						return MarshalToFullArray<byte>(num, array, num2, array2, numberOfElements, array10);
					}
					case MWNumericType.Int16:
					{
						Array array9 = Array.CreateInstance(typeof(short), numberOfElements);
						Marshal.Copy(intPtr, (short[])array9, 0, numberOfElements);
						return MarshalToFullArray<short>(num, array, num2, array2, numberOfElements, array9);
					}
					case MWNumericType.Int32:
					{
						Array array8 = Array.CreateInstance(typeof(int), numberOfElements);
						Marshal.Copy(intPtr, (int[])array8, 0, numberOfElements);
						return MarshalToFullArray<int>(num, array, num2, array2, numberOfElements, array8);
					}
					case MWNumericType.Int64:
					{
						Array array7 = Array.CreateInstance(typeof(long), numberOfElements);
						Marshal.Copy(intPtr, (long[])array7, 0, numberOfElements);
						return MarshalToFullArray<long>(num, array, num2, array2, numberOfElements, array7);
					}
					case MWNumericType.Single:
					{
						Array array6 = Array.CreateInstance(typeof(float), numberOfElements);
						Marshal.Copy(intPtr, (float[])array6, 0, numberOfElements);
						return MarshalToFullArray<float>(num, array, num2, array2, numberOfElements, array6);
					}
					case MWNumericType.Double:
					{
						Array array5 = Array.CreateInstance(typeof(double), numberOfElements);
						Marshal.Copy(intPtr, (double[])array5, 0, numberOfElements);
						return MarshalToFullArray<double>(num, array, num2, array2, numberOfElements, array5);
					}
					default:
					{
						string string2 = MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType");
						throw new ApplicationException(string2);
					}
					}
				}
				MWNumericType mWNumericType = NumericType;
				int[] lengths = MWArray.MATLABDimensionToNetDimension(Dimensions);
				if (sparseIndex)
				{
					if (MWNumericType.UInt32 == mWNumericType)
					{
						mWNumericType = MWNumericType.Int32;
					}
					if (MWNumericType.UInt64 == mWNumericType)
					{
						mWNumericType = MWNumericType.Int64;
					}
				}
				Type elementType = numericTypeToSystemType[mWNumericType];
				Array array11 = Array.CreateInstance(elementType, lengths);
				MWMarshal.MarshalUnmanagedColumnMajorToManagedRowMajor(intPtr, array11);
				return array11;
			}
		}

		public override object Clone()
		{
			CheckDisposed();
			MWNumericArray mWNumericArray = (MWNumericArray)MemberwiseClone();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				mWNumericArray.SetMXArray(MWArray.mxDuplicateArray(MXArrayHandle), MWArrayType.Numeric);
				return mWNumericArray;
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
			if (!(obj is MWNumericArray))
			{
				return false;
			}
			return Equals(obj as MWNumericArray);
		}

		private bool Equals(MWNumericArray obj)
		{
			return base.Equals((object)obj);
		}

		bool IEquatable<MWNumericArray>.Equals(MWNumericArray other)
		{
			return Equals(other);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public Array ToVector(MWArrayComponent component)
		{
			CheckDisposed();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				int numberOfElements = base.NumberOfElements;
				MWNumericType numericType = NumericType;
				IntPtr source = (component == MWArrayComponent.Real) ? MWArray.mxGetData(MXArrayHandle) : mxGetImagData(MXArrayHandle);
				switch (numericType)
				{
				case MWNumericType.UInt8:
				{
					byte[] array6 = new byte[numberOfElements];
					Marshal.Copy(source, array6, 0, numberOfElements);
					return array6;
				}
				case MWNumericType.Int16:
				{
					short[] array5 = new short[numberOfElements];
					Marshal.Copy(source, array5, 0, numberOfElements);
					return array5;
				}
				case MWNumericType.Int32:
				{
					int[] array4 = new int[numberOfElements];
					Marshal.Copy(source, array4, 0, numberOfElements);
					return array4;
				}
				case MWNumericType.Int64:
				{
					long[] array3 = new long[numberOfElements];
					Marshal.Copy(source, array3, 0, numberOfElements);
					return array3;
				}
				case MWNumericType.Single:
				{
					float[] array2 = new float[numberOfElements];
					Marshal.Copy(source, array2, 0, numberOfElements);
					return array2;
				}
				case MWNumericType.Double:
				{
					double[] array = new double[numberOfElements];
					Marshal.Copy(source, array, 0, numberOfElements);
					return array;
				}
				default:
				{
					string @string = MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType");
					throw new ApplicationException(@string);
				}
				}
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public Array ToEmptyVector()
		{
			CheckDisposed();
			lock (MWArray.mxSync)
			{
				MWNumericType numericType = NumericType;
				Type elementType = numericTypeToSystemType[numericType];
				return Array.CreateInstance(elementType, 0);
			}
		}

		public override Array ToArray()
		{
			if (IsComplex)
			{
				throw new InvalidDataException("Data type unsupported by MATLAB .NET Assembly for output");
			}
			return ToArray(MWArrayComponent.Real);
		}

		public Array ToArray(MWArrayComponent component)
		{
			return ToArray(component, sparseIndex: false);
		}

		private void BuildNumericArray(Array realData, Array imaginaryData, bool makeDouble, bool rowMajorData)
		{
			if (realData == null)
			{
				throw new ArgumentNullException("realData");
			}
			Type elementType = realData.GetType().GetElementType();
			if (typeof(double) != elementType && typeof(int) != elementType && typeof(byte) != elementType && typeof(short) != elementType && typeof(long) != elementType && typeof(float) != elementType)
			{
				string @string = MWArray.resourceManager.GetString("MWErrorDataArrayType");
				throw new ArgumentException(@string);
			}
			if (imaginaryData != null)
			{
				if (elementType != imaginaryData.GetType().GetElementType())
				{
					string string2 = MWArray.resourceManager.GetString("MWErrorDataArrayType");
					throw new ArgumentException(string2, "imaginaryData");
				}
				if (realData.Length != imaginaryData.Length)
				{
					string string3 = MWArray.resourceManager.GetString("MWErrorDataArraySizeMismatch");
					throw new ArgumentException(string3);
				}
			}
			int rank = realData.Rank;
			int num = imaginaryData?.Rank ?? 0;
			MWArrayComplexity complexityFlag = (num != 0) ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
			if (imaginaryData != null && rank != num)
			{
				string string4 = MWArray.resourceManager.GetString("MWErrorDimensionMismatch");
				throw new ArgumentException(string4);
			}
			int[] array = new int[Math.Max(rank, 2)];
			int length = realData.Length;
			int num2 = array[0] = ((1 >= rank) ? 1 : realData.GetLength(rank - 2));
			int num3 = array[1] = realData.GetLength(rank - 1);
			for (int i = 0; i < rank - 2; i++)
			{
				array[rank - i - 1] = realData.GetLength(i);
			}
			Array array2 = null;
			Array array3 = null;
			IEnumerator enumerator = realData.GetEnumerator();
			IEnumerator enumerator2 = imaginaryData?.GetEnumerator();
			elementType = (makeDouble ? typeof(double) : elementType);
			array2 = Array.CreateInstance(elementType, length);
			array3 = ((imaginaryData != null) ? Array.CreateInstance(elementType, length) : null);
			if (!rowMajorData)
			{
				for (int j = 0; j < length; j++)
				{
					enumerator.MoveNext();
					array2.SetValue(enumerator.Current, j);
					if (imaginaryData != null)
					{
						enumerator2.MoveNext();
						array3.SetValue(enumerator2.Current, j);
					}
				}
			}
			else
			{
				for (int k = 0; k < length; k += num2 * num3)
				{
					for (int l = 0; l < num2; l++)
					{
						for (int m = 0; m < num3; m++)
						{
							enumerator.MoveNext();
							array2.SetValue(enumerator.Current, m * num2 + l + k);
							if (imaginaryData != null)
							{
								enumerator2.MoveNext();
								array3.SetValue(enumerator2.Current, m * num2 + l + k);
							}
						}
					}
				}
			}
			try
			{
				Monitor.Enter(MWArray.mxSync);
				IntPtr zero = IntPtr.Zero;
				IntPtr zero2 = IntPtr.Zero;
				MWNumericType mWNumericType = (MWNumericType)systemTypeToNumericType[elementType.Name];
				MWSafeHandle hMXArray = mxCreateNumericArray(array.Length, array, mWNumericType, complexityFlag);
				zero = MWArray.mxGetData(hMXArray);
				zero2 = ((imaginaryData != null) ? mxGetImagData(hMXArray) : IntPtr.Zero);
				switch (mWNumericType)
				{
				case MWNumericType.UInt8:
					if (IntPtr.Zero != zero)
					{
						Marshal.Copy((byte[])array2, 0, zero, length);
					}
					if (IntPtr.Zero != zero2)
					{
						Marshal.Copy((byte[])array3, 0, zero2, length);
					}
					break;
				case MWNumericType.Int16:
					if (IntPtr.Zero != zero)
					{
						Marshal.Copy((short[])array2, 0, zero, length);
					}
					if (IntPtr.Zero != zero2)
					{
						Marshal.Copy((short[])array3, 0, zero2, length);
					}
					break;
				case MWNumericType.Int32:
					if (IntPtr.Zero != zero)
					{
						Marshal.Copy((int[])array2, 0, zero, length);
					}
					if (IntPtr.Zero != zero2)
					{
						Marshal.Copy((int[])array3, 0, zero2, length);
					}
					break;
				case MWNumericType.Int64:
					if (IntPtr.Zero != zero)
					{
						Marshal.Copy((long[])array2, 0, zero, length);
					}
					if (IntPtr.Zero != zero2)
					{
						Marshal.Copy((long[])array3, 0, zero2, length);
					}
					break;
				case MWNumericType.Single:
					if (IntPtr.Zero != zero)
					{
						Marshal.Copy((float[])array2, 0, zero, length);
					}
					if (IntPtr.Zero != zero2)
					{
						Marshal.Copy((float[])array3, 0, zero2, length);
					}
					break;
				case MWNumericType.Double:
					if (IntPtr.Zero != zero)
					{
						Marshal.Copy((double[])array2, 0, zero, length);
					}
					if (IntPtr.Zero != zero2)
					{
						Marshal.Copy((double[])array3, 0, zero2, length);
					}
					break;
				default:
				{
					string string5 = MWArray.resourceManager.GetString("MWErrorInvalidArrayDataType");
					throw new ApplicationException(string5);
				}
				}
				SetMXArray(hMXArray, MWArrayType.Numeric);
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		private unsafe void FastBuildNumericArray(Array realData, Array imaginaryData, bool makeDouble, bool rowMajorData)
		{
			validateInput(realData, imaginaryData);
			int rank = realData.Rank;
			if (rank == 1)
			{
				rowMajorData = false;
			}
			int[] array = new int[rank];
			for (int i = 0; i < rank; i++)
			{
				array[i] = realData.GetLength(i);
			}
			int[] array2 = MWArray.NetDimensionToMATLABDimension(array);
			MWArrayComplexity complexityFlag = (imaginaryData != null) ? MWArrayComplexity.Complex : MWArrayComplexity.Real;
			lock (MWArray.mxSync)
			{
				Type type = makeDouble ? typeof(double) : realData.GetType().GetElementType();
				MWNumericType elementType = (MWNumericType)systemTypeToNumericType[type.Name];
				MWSafeHandle hMXArray = mxCreateNumericArray(array2.Length, array2, elementType, complexityFlag);
				IntPtr dest = MWArray.mxGetData(hMXArray);
				if (makeDouble)
				{
					if (rowMajorData)
					{
						MWMarshal.MarshalManagedRowMajorToUnmanagedColumnMajor(realData, (double*)dest.ToPointer());
						if (imaginaryData != null)
						{
							MWMarshal.MarshalManagedRowMajorToUnmanagedColumnMajor(imaginaryData, (double*)mxGetImagData(hMXArray).ToPointer());
						}
					}
					else
					{
						MWMarshal.MarshalManagedColumnMajorToUnmanagedColumnMajor(realData, (double*)dest.ToPointer());
						if (imaginaryData != null)
						{
							MWMarshal.MarshalManagedColumnMajorToUnmanagedColumnMajor(imaginaryData, (double*)mxGetImagData(hMXArray).ToPointer());
						}
					}
				}
				else if (rowMajorData)
				{
					MWMarshal.MarshalManagedRowMajorToUnmanagedColumnMajor(realData, dest);
					if (imaginaryData != null)
					{
						IntPtr dest2 = mxGetImagData(hMXArray);
						MWMarshal.MarshalManagedRowMajorToUnmanagedColumnMajor(imaginaryData, dest2);
					}
				}
				else
				{
					MWMarshal.MarshalManagedColumnMajorToUnmanagedColumnMajor(realData, dest);
					if (imaginaryData != null)
					{
						IntPtr dest3 = mxGetImagData(hMXArray);
						MWMarshal.MarshalManagedColumnMajorToUnmanagedColumnMajor(imaginaryData, dest3);
					}
				}
				SetMXArray(hMXArray, MWArrayType.Numeric);
			}
		}

		private object CastToNativeScalar()
		{
			switch (NumericType)
			{
			case MWNumericType.UInt8:
				return (byte)this;
			case MWNumericType.Int16:
				return (short)this;
			case MWNumericType.Int32:
				return (int)this;
			case MWNumericType.Int64:
				return (long)this;
			case MWNumericType.Single:
				return (float)this;
			case MWNumericType.Double:
				return (double)this;
			default:
				return null;
			}
		}

		internal override object ConvertToType(Type t)
		{
			if (t == typeof(MWArray) || t == typeof(MWNumericArray))
			{
				return this;
			}
			Array array = null;
			if (IsValidConversion(t))
			{
				if (t.IsPrimitive)
				{
					return Convert.ChangeType(CastToNativeScalar(), t);
				}
				if (t.IsArray)
				{
					if (!JaggedArray.IsJagged(t))
					{
						array = ((t.GetArrayRank() != 1) ? ToArray() : ((!IsEmpty) ? ToVector(MWArrayComponent.Real) : ToEmptyVector()));
						if (t == numericTypeToSystemType[NumericType])
						{
							return array;
						}
						int[] array2 = new int[array.Rank];
						for (int i = 0; i < array.Rank; i++)
						{
							array2[i] = array.GetUpperBound(i) + 1;
						}
						Array array3 = Array.CreateInstance(t.GetElementType(), array2);
						Array.Copy(array, array3, array.Length);
						return array3;
					}
					int length = MWArray.mxGetNumberOfElements(MXArrayHandle);
					IntPtr src = MWArray.mxGetData(MXArrayHandle);
					array = Array.CreateInstance(JaggedArray.GetElementType(t), length);
					MWMarshal.MarshalUnmanagedToManagedFlatArray(src, array);
					int[] dimensions = MWArray.MATLABDimensionToNetDimension(Dimensions);
					return JaggedArray.GetJaggedArrayFromFlatArray(array, dimensions);
				}
			}
			else if (IsEmpty)
			{
				if (t.IsArray)
				{
					int[] array4 = new int[t.GetArrayRank()];
					for (int j = 0; j < t.GetArrayRank(); j++)
					{
						array4[j] = 0;
					}
					return Array.CreateInstance(t.GetElementType(), array4);
				}
				if (t == typeof(char))
				{
					return '\0';
				}
				if (t == typeof(string))
				{
					return string.Empty;
				}
				return Activator.CreateInstance(t);
			}
			string @string = MWArray.resourceManager.GetString("MWErrorInvalidDataConversion");
			string fullName = numericTypeToSystemType[NumericType].FullName;
			string message = "Cannot convert from MWNumericArray of type: " + fullName + MWArray.RankToString(base.NumberofDimensions) + " to " + t.FullName;
			throw new ArgumentException(@string, new ApplicationException(message));
		}

		internal override bool IsValidConversion(Type t)
		{
			Type type = numericTypeToSystemType[NumericType];
			Type[] array = MWArray.typeMap[type];
			if (array == null)
			{
				return false;
			}
			if (t.IsArray)
			{
				if (!JaggedArray.IsJagged(t))
				{
					int arrayRank = t.GetArrayRank();
					if (arrayRank == base.NumberofDimensions)
					{
						t = t.GetElementType();
					}
					else
					{
						if (arrayRank != 1)
						{
							return false;
						}
						if (!IsVector() && !IsEmpty)
						{
							return false;
						}
						t = t.GetElementType();
					}
				}
				else if (JaggedArray.GetRank(t) == base.NumberofDimensions && JaggedArray.GetElementType(t) == type)
				{
					return true;
				}
			}
			else if (!IsScalar() && !IsEmpty)
			{
				return false;
			}
			return Array.Exists(array, delegate(Type c)
			{
				if (c == t)
				{
					return true;
				}
				return false;
			});
		}
	}
}
