using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MathWorks.MATLAB.NET.Utility
{
	internal class MWMarshal
	{
		[DllImport("mclmcrrt9_1.dll", EntryPoint = "memcpy_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern int memcpy(IntPtr source, IntPtr destination, IntPtr size);

		private static int SizeOfType(Type t)
		{
			if (t == typeof(byte))
			{
				return 1;
			}
			if (t == typeof(short))
			{
				return 2;
			}
			if (t == typeof(int))
			{
				return 4;
			}
			if (t == typeof(long))
			{
				return 8;
			}
			if (t == typeof(float))
			{
				return 4;
			}
			if (t == typeof(double))
			{
				return 8;
			}
			if (t == typeof(char))
			{
				return 2;
			}
			return -1;
		}

		public static void MarshalManagedColumnMajorToUnmanagedColumnMajor(Array managedSrc, IntPtr dest)
		{
			if (!(dest == IntPtr.Zero))
			{
				long value = managedSrc.Length * SizeOfType(managedSrc.GetType().GetElementType());
				GCHandle gCHandle = GCHandle.Alloc(managedSrc, GCHandleType.Pinned);
				IntPtr source = gCHandle.AddrOfPinnedObject();
				try
				{
					memcpy(source, dest, (IntPtr)value);
				}
				finally
				{
					gCHandle.Free();
				}
			}
		}

		public unsafe static void MarshalManagedColumnMajorToUnmanagedColumnMajor(Array managedSrc, double* destPtr)
		{
			if (destPtr != null)
			{
				Type elementType = managedSrc.GetType().GetElementType();
				int length = managedSrc.Length;
				GCHandle gCHandle = GCHandle.Alloc(managedSrc, GCHandleType.Pinned);
				IntPtr intPtr = gCHandle.AddrOfPinnedObject();
				try
				{
					if (elementType == typeof(byte))
					{
						byte* ptr = (byte*)(void*)intPtr;
						for (int i = 0; i < length; i++)
						{
							double* intPtr2 = destPtr;
							destPtr = intPtr2 + 1;
							byte* intPtr3 = ptr;
							ptr = intPtr3 + 1;
							*intPtr2 = (int)(*intPtr3);
						}
					}
					else if (elementType == typeof(short))
					{
						short* ptr2 = (short*)(void*)intPtr;
						for (int j = 0; j < length; j++)
						{
							double* intPtr4 = destPtr;
							destPtr = intPtr4 + 1;
							short* intPtr5 = ptr2;
							ptr2 = intPtr5 + 1;
							*intPtr4 = *intPtr5;
						}
					}
					else if (elementType == typeof(int))
					{
						int* ptr3 = (int*)(void*)intPtr;
						for (int k = 0; k < length; k++)
						{
							double* intPtr6 = destPtr;
							destPtr = intPtr6 + 1;
							int* intPtr7 = ptr3;
							ptr3 = intPtr7 + 1;
							*intPtr6 = *intPtr7;
						}
					}
					else if (elementType == typeof(long))
					{
						long* ptr4 = (long*)(void*)intPtr;
						for (int l = 0; l < length; l++)
						{
							double* intPtr8 = destPtr;
							destPtr = intPtr8 + 1;
							long* intPtr9 = ptr4;
							ptr4 = intPtr9 + 1;
							*intPtr8 = *intPtr9;
						}
					}
					else if (elementType == typeof(float))
					{
						float* ptr5 = (float*)(void*)intPtr;
						for (int m = 0; m < length; m++)
						{
							double* intPtr10 = destPtr;
							destPtr = intPtr10 + 1;
							float* intPtr11 = ptr5;
							ptr5 = intPtr11 + 1;
							*intPtr10 = *intPtr11;
						}
					}
					else if (elementType == typeof(double))
					{
						memcpy(intPtr, (IntPtr)(void*)destPtr, (IntPtr)(length * 8));
					}
				}
				finally
				{
					gCHandle.Free();
				}
			}
		}

		public unsafe static void MarshalManagedRowMajorToUnmanagedColumnMajor(Array managedSrc, IntPtr dest)
		{
			if (!(dest == IntPtr.Zero))
			{
				Type elementType = managedSrc.GetType().GetElementType();
				int length = managedSrc.Length;
				int length2 = managedSrc.GetLength(managedSrc.Rank - 2);
				int length3 = managedSrc.GetLength(managedSrc.Rank - 1);
				int num = length2 * length3;
				GCHandle gCHandle = GCHandle.Alloc(managedSrc, GCHandleType.Pinned);
				IntPtr value = gCHandle.AddrOfPinnedObject();
				try
				{
					if (elementType == typeof(byte))
					{
						byte* ptr = (byte*)(void*)value;
						byte* ptr2 = (byte*)(void*)dest;
						for (int i = 0; i < length; i += num)
						{
							for (int j = 0; j < length2; j++)
							{
								for (int k = 0; k < length3; k++)
								{
									byte* intPtr = ptr2 + (k * length2 + j + i);
									byte* intPtr2 = ptr;
									ptr = intPtr2 + 1;
									*intPtr = *intPtr2;
								}
							}
						}
					}
					else if (elementType == typeof(short))
					{
						short* ptr3 = (short*)(void*)value;
						short* ptr4 = (short*)(void*)dest;
						for (int l = 0; l < length; l += num)
						{
							for (int m = 0; m < length2; m++)
							{
								for (int n = 0; n < length3; n++)
								{
									short* intPtr3 = ptr4 + (n * length2 + m + l);
									short* intPtr4 = ptr3;
									ptr3 = intPtr4 + 1;
									*intPtr3 = *intPtr4;
								}
							}
						}
					}
					else if (elementType == typeof(int))
					{
						int* ptr5 = (int*)(void*)value;
						int* ptr6 = (int*)(void*)dest;
						for (int num2 = 0; num2 < length; num2 += num)
						{
							for (int num3 = 0; num3 < length2; num3++)
							{
								for (int num4 = 0; num4 < length3; num4++)
								{
									int* intPtr5 = ptr6 + (num4 * length2 + num3 + num2);
									int* intPtr6 = ptr5;
									ptr5 = intPtr6 + 1;
									*intPtr5 = *intPtr6;
								}
							}
						}
					}
					else if (elementType == typeof(long))
					{
						long* ptr7 = (long*)(void*)value;
						long* ptr8 = (long*)(void*)dest;
						for (int num5 = 0; num5 < length; num5 += num)
						{
							for (int num6 = 0; num6 < length2; num6++)
							{
								for (int num7 = 0; num7 < length3; num7++)
								{
									long* intPtr7 = ptr8 + (num7 * length2 + num6 + num5);
									long* intPtr8 = ptr7;
									ptr7 = intPtr8 + 1;
									*intPtr7 = *intPtr8;
								}
							}
						}
					}
					else if (elementType == typeof(float))
					{
						float* ptr9 = (float*)(void*)value;
						float* ptr10 = (float*)(void*)dest;
						for (int num8 = 0; num8 < length; num8 += num)
						{
							for (int num9 = 0; num9 < length2; num9++)
							{
								for (int num10 = 0; num10 < length3; num10++)
								{
									float* intPtr9 = ptr10 + (num10 * length2 + num9 + num8);
									float* intPtr10 = ptr9;
									ptr9 = intPtr10 + 1;
									*intPtr9 = *intPtr10;
								}
							}
						}
					}
					else if (elementType == typeof(double))
					{
						double* ptr11 = (double*)(void*)value;
						double* ptr12 = (double*)(void*)dest;
						for (int num11 = 0; num11 < length; num11 += num)
						{
							for (int num12 = 0; num12 < length2; num12++)
							{
								for (int num13 = 0; num13 < length3; num13++)
								{
									double* intPtr11 = ptr12 + (num13 * length2 + num12 + num11);
									double* intPtr12 = ptr11;
									ptr11 = intPtr12 + 1;
									*intPtr11 = *intPtr12;
								}
							}
						}
					}
				}
				finally
				{
					gCHandle.Free();
				}
			}
		}

		public unsafe static void MarshalUnmanagedColumnMajorToManagedRowMajor(IntPtr src, Array dest)
		{
			if (!(src == IntPtr.Zero))
			{
				Type elementType = dest.GetType().GetElementType();
				int length = dest.Length;
				int length2 = dest.GetLength(dest.Rank - 2);
				int length3 = dest.GetLength(dest.Rank - 1);
				int num = length2 * length3;
				GCHandle gCHandle = GCHandle.Alloc(dest, GCHandleType.Pinned);
				IntPtr intPtr = gCHandle.AddrOfPinnedObject();
				try
				{
					if (elementType == typeof(byte))
					{
						byte* ptr = (byte*)intPtr.ToPointer();
						byte* ptr2 = (byte*)(void*)src;
						for (int i = 0; i < length; i += num)
						{
							for (int j = 0; j < length2; j++)
							{
								for (int k = 0; k < length3; k++)
								{
									byte* intPtr2 = ptr;
									ptr = intPtr2 + 1;
									*intPtr2 = ptr2[k * length2 + j + i];
								}
							}
						}
					}
					else if (elementType == typeof(short))
					{
						short* ptr3 = (short*)intPtr.ToPointer();
						short* ptr4 = (short*)(void*)src;
						for (int l = 0; l < length; l += num)
						{
							for (int m = 0; m < length2; m++)
							{
								for (int n = 0; n < length3; n++)
								{
									short* intPtr3 = ptr3;
									ptr3 = intPtr3 + 1;
									*intPtr3 = ptr4[n * length2 + m + l];
								}
							}
						}
					}
					else if (elementType == typeof(int))
					{
						int* ptr5 = (int*)intPtr.ToPointer();
						int* ptr6 = (int*)(void*)src;
						for (int num2 = 0; num2 < length; num2 += num)
						{
							for (int num3 = 0; num3 < length2; num3++)
							{
								for (int num4 = 0; num4 < length3; num4++)
								{
									int* intPtr4 = ptr5;
									ptr5 = intPtr4 + 1;
									*intPtr4 = ptr6[num4 * length2 + num3 + num2];
								}
							}
						}
					}
					else if (elementType == typeof(long))
					{
						long* ptr7 = (long*)intPtr.ToPointer();
						long* ptr8 = (long*)(void*)src;
						for (int num5 = 0; num5 < length; num5 += num)
						{
							for (int num6 = 0; num6 < length2; num6++)
							{
								for (int num7 = 0; num7 < length3; num7++)
								{
									long* intPtr5 = ptr7;
									ptr7 = intPtr5 + 1;
									*intPtr5 = ptr8[num7 * length2 + num6 + num5];
								}
							}
						}
					}
					else if (elementType == typeof(float))
					{
						float* ptr9 = (float*)intPtr.ToPointer();
						float* ptr10 = (float*)(void*)src;
						for (int num8 = 0; num8 < length; num8 += num)
						{
							for (int num9 = 0; num9 < length2; num9++)
							{
								for (int num10 = 0; num10 < length3; num10++)
								{
									float* intPtr6 = ptr9;
									ptr9 = intPtr6 + 1;
									*intPtr6 = ptr10[num10 * length2 + num9 + num8];
								}
							}
						}
					}
					else if (elementType == typeof(double))
					{
						double* ptr11 = (double*)intPtr.ToPointer();
						double* ptr12 = (double*)(void*)src;
						for (int num11 = 0; num11 < length; num11 += num)
						{
							for (int num12 = 0; num12 < length2; num12++)
							{
								for (int num13 = 0; num13 < length3; num13++)
								{
									double* intPtr7 = ptr11;
									ptr11 = intPtr7 + 1;
									*intPtr7 = ptr12[num13 * length2 + num12 + num11];
								}
							}
						}
					}
				}
				finally
				{
					gCHandle.Free();
				}
			}
		}

		public unsafe static void MarshalManagedRowMajorToUnmanagedColumnMajor(Array managedSrc, double* destPtr)
		{
			if (destPtr != null)
			{
				Type elementType = managedSrc.GetType().GetElementType();
				int length = managedSrc.Length;
				int length2 = managedSrc.GetLength(managedSrc.Rank - 2);
				int length3 = managedSrc.GetLength(managedSrc.Rank - 1);
				int num = length2 * length3;
				GCHandle gCHandle = GCHandle.Alloc(managedSrc, GCHandleType.Pinned);
				IntPtr value = gCHandle.AddrOfPinnedObject();
				try
				{
					if (elementType == typeof(byte))
					{
						byte* ptr = (byte*)(void*)value;
						for (int i = 0; i < length; i += num)
						{
							for (int j = 0; j < length2; j++)
							{
								for (int k = 0; k < length3; k++)
								{
									double* intPtr = destPtr + (k * length2 + j + i);
									byte* intPtr2 = ptr;
									ptr = intPtr2 + 1;
									*intPtr = (int)(*intPtr2);
								}
							}
						}
					}
					else if (elementType == typeof(short))
					{
						short* ptr2 = (short*)(void*)value;
						for (int l = 0; l < length; l += num)
						{
							for (int m = 0; m < length2; m++)
							{
								for (int n = 0; n < length3; n++)
								{
									double* intPtr3 = destPtr + (n * length2 + m + l);
									short* intPtr4 = ptr2;
									ptr2 = intPtr4 + 1;
									*intPtr3 = *intPtr4;
								}
							}
						}
					}
					else if (elementType == typeof(int))
					{
						int* ptr3 = (int*)(void*)value;
						for (int num2 = 0; num2 < length; num2 += num)
						{
							for (int num3 = 0; num3 < length2; num3++)
							{
								for (int num4 = 0; num4 < length3; num4++)
								{
									double* intPtr5 = destPtr + (num4 * length2 + num3 + num2);
									int* intPtr6 = ptr3;
									ptr3 = intPtr6 + 1;
									*intPtr5 = *intPtr6;
								}
							}
						}
					}
					else if (elementType == typeof(long))
					{
						long* ptr4 = (long*)(void*)value;
						for (int num5 = 0; num5 < length; num5 += num)
						{
							for (int num6 = 0; num6 < length2; num6++)
							{
								for (int num7 = 0; num7 < length3; num7++)
								{
									double* intPtr7 = destPtr + (num7 * length2 + num6 + num5);
									long* intPtr8 = ptr4;
									ptr4 = intPtr8 + 1;
									*intPtr7 = *intPtr8;
								}
							}
						}
					}
					else if (elementType == typeof(float))
					{
						float* ptr5 = (float*)(void*)value;
						for (int num8 = 0; num8 < length; num8 += num)
						{
							for (int num9 = 0; num9 < length2; num9++)
							{
								for (int num10 = 0; num10 < length3; num10++)
								{
									double* intPtr9 = destPtr + (num10 * length2 + num9 + num8);
									float* intPtr10 = ptr5;
									ptr5 = intPtr10 + 1;
									*intPtr9 = *intPtr10;
								}
							}
						}
					}
					else if (elementType == typeof(double))
					{
						double* ptr6 = (double*)(void*)value;
						for (int num11 = 0; num11 < length; num11 += num)
						{
							for (int num12 = 0; num12 < length2; num12++)
							{
								for (int num13 = 0; num13 < length3; num13++)
								{
									double* intPtr11 = destPtr + (num13 * length2 + num12 + num11);
									double* intPtr12 = ptr6;
									ptr6 = intPtr12 + 1;
									*intPtr11 = *intPtr12;
								}
							}
						}
					}
				}
				finally
				{
					gCHandle.Free();
				}
			}
		}

		public static void MarshalManagedFlatArrayToUnmanaged(Array flatArray, IntPtr dest)
		{
			if (!(dest == IntPtr.Zero))
			{
				Type elementType = flatArray.GetType().GetElementType();
				if (elementType == typeof(byte))
				{
					Marshal.Copy((byte[])flatArray, 0, dest, flatArray.Length);
				}
				else if (elementType == typeof(short))
				{
					Marshal.Copy((short[])flatArray, 0, dest, flatArray.Length);
				}
				else if (elementType == typeof(int))
				{
					Marshal.Copy((int[])flatArray, 0, dest, flatArray.Length);
				}
				else if (elementType == typeof(long))
				{
					Marshal.Copy((long[])flatArray, 0, dest, flatArray.Length);
				}
				else if (elementType == typeof(float))
				{
					Marshal.Copy((float[])flatArray, 0, dest, flatArray.Length);
				}
				else if (elementType == typeof(double))
				{
					Marshal.Copy((double[])flatArray, 0, dest, flatArray.Length);
				}
			}
		}

		public static void MarshalUnmanagedToManagedFlatArray(IntPtr src, Array flatArray)
		{
			if (!(src == IntPtr.Zero))
			{
				Type elementType = flatArray.GetType().GetElementType();
				if (elementType == typeof(byte))
				{
					Marshal.Copy(src, (byte[])flatArray, 0, flatArray.Length);
				}
				else if (elementType == typeof(short))
				{
					Marshal.Copy(src, (short[])flatArray, 0, flatArray.Length);
				}
				else if (elementType == typeof(int))
				{
					Marshal.Copy(src, (int[])flatArray, 0, flatArray.Length);
				}
				else if (elementType == typeof(long))
				{
					Marshal.Copy(src, (long[])flatArray, 0, flatArray.Length);
				}
				else if (elementType == typeof(float))
				{
					Marshal.Copy(src, (float[])flatArray, 0, flatArray.Length);
				}
				else if (elementType == typeof(double))
				{
					Marshal.Copy(src, (double[])flatArray, 0, flatArray.Length);
				}
			}
		}
	}
}
