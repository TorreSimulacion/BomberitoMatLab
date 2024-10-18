// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.MWMarshal
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MathWorks.MATLAB.NET.Utility
{
  internal class MWMarshal
  {
    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "memcpy_proxy")]
    private static extern int memcpy(IntPtr source, IntPtr destination, IntPtr size);

    private static int SizeOfType(Type t)
    {
      if (t == typeof (byte))
        return 1;
      if (t == typeof (short))
        return 2;
      if (t == typeof (int))
        return 4;
      if (t == typeof (long))
        return 8;
      if (t == typeof (float))
        return 4;
      if (t == typeof (double))
        return 8;
      return t == typeof (char) ? 2 : -1;
    }

    public static void MarshalManagedColumnMajorToUnmanagedColumnMajor(
      Array managedSrc,
      IntPtr dest)
    {
      if (dest == IntPtr.Zero)
        return;
      long num = (long) (managedSrc.Length * MWMarshal.SizeOfType(managedSrc.GetType().GetElementType()));
      GCHandle gcHandle = GCHandle.Alloc((object) managedSrc, GCHandleType.Pinned);
      IntPtr source = gcHandle.AddrOfPinnedObject();
      try
      {
        MWMarshal.memcpy(source, dest, (IntPtr) num);
      }
      finally
      {
        gcHandle.Free();
      }
    }

    public static unsafe void MarshalManagedColumnMajorToUnmanagedColumnMajor(
      Array managedSrc,
      double* destPtr)
    {
      if ((IntPtr) destPtr == IntPtr.Zero)
        return;
      Type elementType = managedSrc.GetType().GetElementType();
      int length = managedSrc.Length;
      GCHandle gcHandle = GCHandle.Alloc((object) managedSrc, GCHandleType.Pinned);
      IntPtr source = gcHandle.AddrOfPinnedObject();
      try
      {
        if (elementType == typeof (byte))
        {
          byte* numPtr = (byte*) (void*) source;
          for (int index = 0; index < length; ++index)
            *destPtr++ = (double) *numPtr++;
        }
        else if (elementType == typeof (short))
        {
          short* numPtr = (short*) (void*) source;
          for (int index = 0; index < length; ++index)
            *destPtr++ = (double) *numPtr++;
        }
        else if (elementType == typeof (int))
        {
          int* numPtr = (int*) (void*) source;
          for (int index = 0; index < length; ++index)
            *destPtr++ = (double) *numPtr++;
        }
        else if (elementType == typeof (long))
        {
          long* numPtr = (long*) (void*) source;
          for (int index = 0; index < length; ++index)
            *destPtr++ = (double) *numPtr++;
        }
        else if (elementType == typeof (float))
        {
          float* numPtr = (float*) (void*) source;
          for (int index = 0; index < length; ++index)
            *destPtr++ = (double) *numPtr++;
        }
        else
        {
          if (!(elementType == typeof (double)))
            return;
          MWMarshal.memcpy(source, (IntPtr) ((void*) destPtr), (IntPtr) (length * 8));
        }
      }
      finally
      {
        gcHandle.Free();
      }
    }

    public static unsafe void MarshalManagedRowMajorToUnmanagedColumnMajor(
      Array managedSrc,
      IntPtr dest)
    {
      if (dest == IntPtr.Zero)
        return;
      Type elementType = managedSrc.GetType().GetElementType();
      int length1 = managedSrc.Length;
      int length2 = managedSrc.GetLength(managedSrc.Rank - 2);
      int length3 = managedSrc.GetLength(managedSrc.Rank - 1);
      int num1 = length2 * length3;
      GCHandle gcHandle = GCHandle.Alloc((object) managedSrc, GCHandleType.Pinned);
      IntPtr num2 = gcHandle.AddrOfPinnedObject();
      try
      {
        if (elementType == typeof (byte))
        {
          byte* numPtr1 = (byte*) (void*) num2;
          byte* numPtr2 = (byte*) (void*) dest;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                numPtr2[index3 * length2 + index2 + index1] = *numPtr1++;
            }
          }
        }
        else if (elementType == typeof (short))
        {
          short* numPtr1 = (short*) (void*) num2;
          short* numPtr2 = (short*) (void*) dest;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                numPtr2[index3 * length2 + index2 + index1] = *numPtr1++;
            }
          }
        }
        else if (elementType == typeof (int))
        {
          int* numPtr1 = (int*) (void*) num2;
          int* numPtr2 = (int*) (void*) dest;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                numPtr2[index3 * length2 + index2 + index1] = *numPtr1++;
            }
          }
        }
        else if (elementType == typeof (long))
        {
          long* numPtr1 = (long*) (void*) num2;
          long* numPtr2 = (long*) (void*) dest;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                numPtr2[index3 * length2 + index2 + index1] = *numPtr1++;
            }
          }
        }
        else if (elementType == typeof (float))
        {
          float* numPtr1 = (float*) (void*) num2;
          float* numPtr2 = (float*) (void*) dest;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                numPtr2[index3 * length2 + index2 + index1] = *numPtr1++;
            }
          }
        }
        else
        {
          if (!(elementType == typeof (double)))
            return;
          double* numPtr1 = (double*) (void*) num2;
          double* numPtr2 = (double*) (void*) dest;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                numPtr2[index3 * length2 + index2 + index1] = *numPtr1++;
            }
          }
        }
      }
      finally
      {
        gcHandle.Free();
      }
    }

    public static unsafe void MarshalUnmanagedColumnMajorToManagedRowMajor(IntPtr src, Array dest)
    {
      if (src == IntPtr.Zero)
        return;
      Type elementType = dest.GetType().GetElementType();
      int length1 = dest.Length;
      int length2 = dest.GetLength(dest.Rank - 2);
      int length3 = dest.GetLength(dest.Rank - 1);
      int num1 = length2 * length3;
      GCHandle gcHandle = GCHandle.Alloc((object) dest, GCHandleType.Pinned);
      IntPtr num2 = gcHandle.AddrOfPinnedObject();
      try
      {
        if (elementType == typeof (byte))
        {
          byte* pointer = (byte*) num2.ToPointer();
          byte* numPtr = (byte*) (void*) src;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                *pointer++ = numPtr[index3 * length2 + index2 + index1];
            }
          }
        }
        else if (elementType == typeof (short))
        {
          short* pointer = (short*) num2.ToPointer();
          short* numPtr = (short*) (void*) src;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                *pointer++ = numPtr[index3 * length2 + index2 + index1];
            }
          }
        }
        else if (elementType == typeof (int))
        {
          int* pointer = (int*) num2.ToPointer();
          int* numPtr = (int*) (void*) src;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                *pointer++ = numPtr[index3 * length2 + index2 + index1];
            }
          }
        }
        else if (elementType == typeof (long))
        {
          long* pointer = (long*) num2.ToPointer();
          long* numPtr = (long*) (void*) src;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                *pointer++ = numPtr[index3 * length2 + index2 + index1];
            }
          }
        }
        else if (elementType == typeof (float))
        {
          float* pointer = (float*) num2.ToPointer();
          float* numPtr = (float*) (void*) src;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                *pointer++ = numPtr[index3 * length2 + index2 + index1];
            }
          }
        }
        else
        {
          if (!(elementType == typeof (double)))
            return;
          double* pointer = (double*) num2.ToPointer();
          double* numPtr = (double*) (void*) src;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                *pointer++ = numPtr[index3 * length2 + index2 + index1];
            }
          }
        }
      }
      finally
      {
        gcHandle.Free();
      }
    }

    public static unsafe void MarshalManagedRowMajorToUnmanagedColumnMajor(
      Array managedSrc,
      double* destPtr)
    {
      if ((IntPtr) destPtr == IntPtr.Zero)
        return;
      Type elementType = managedSrc.GetType().GetElementType();
      int length1 = managedSrc.Length;
      int length2 = managedSrc.GetLength(managedSrc.Rank - 2);
      int length3 = managedSrc.GetLength(managedSrc.Rank - 1);
      int num1 = length2 * length3;
      GCHandle gcHandle = GCHandle.Alloc((object) managedSrc, GCHandleType.Pinned);
      IntPtr num2 = gcHandle.AddrOfPinnedObject();
      try
      {
        if (elementType == typeof (byte))
        {
          byte* numPtr = (byte*) (void*) num2;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                destPtr[index3 * length2 + index2 + index1] = (double) *numPtr++;
            }
          }
        }
        else if (elementType == typeof (short))
        {
          short* numPtr = (short*) (void*) num2;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                destPtr[index3 * length2 + index2 + index1] = (double) *numPtr++;
            }
          }
        }
        else if (elementType == typeof (int))
        {
          int* numPtr = (int*) (void*) num2;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                destPtr[index3 * length2 + index2 + index1] = (double) *numPtr++;
            }
          }
        }
        else if (elementType == typeof (long))
        {
          long* numPtr = (long*) (void*) num2;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                destPtr[index3 * length2 + index2 + index1] = (double) *numPtr++;
            }
          }
        }
        else if (elementType == typeof (float))
        {
          float* numPtr = (float*) (void*) num2;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                destPtr[index3 * length2 + index2 + index1] = (double) *numPtr++;
            }
          }
        }
        else
        {
          if (!(elementType == typeof (double)))
            return;
          double* numPtr = (double*) (void*) num2;
          for (int index1 = 0; index1 < length1; index1 += num1)
          {
            for (int index2 = 0; index2 < length2; ++index2)
            {
              for (int index3 = 0; index3 < length3; ++index3)
                destPtr[index3 * length2 + index2 + index1] = *numPtr++;
            }
          }
        }
      }
      finally
      {
        gcHandle.Free();
      }
    }

    public static void MarshalManagedFlatArrayToUnmanaged(Array flatArray, IntPtr dest)
    {
      if (dest == IntPtr.Zero)
        return;
      Type elementType = flatArray.GetType().GetElementType();
      if (elementType == typeof (byte))
        Marshal.Copy((byte[]) flatArray, 0, dest, flatArray.Length);
      else if (elementType == typeof (short))
        Marshal.Copy((short[]) flatArray, 0, dest, flatArray.Length);
      else if (elementType == typeof (int))
        Marshal.Copy((int[]) flatArray, 0, dest, flatArray.Length);
      else if (elementType == typeof (long))
        Marshal.Copy((long[]) flatArray, 0, dest, flatArray.Length);
      else if (elementType == typeof (float))
      {
        Marshal.Copy((float[]) flatArray, 0, dest, flatArray.Length);
      }
      else
      {
        if (!(elementType == typeof (double)))
          return;
        Marshal.Copy((double[]) flatArray, 0, dest, flatArray.Length);
      }
    }

    public static void MarshalUnmanagedToManagedFlatArray(IntPtr src, Array flatArray)
    {
      if (src == IntPtr.Zero)
        return;
      Type elementType = flatArray.GetType().GetElementType();
      if (elementType == typeof (byte))
        Marshal.Copy(src, (byte[]) flatArray, 0, flatArray.Length);
      else if (elementType == typeof (short))
        Marshal.Copy(src, (short[]) flatArray, 0, flatArray.Length);
      else if (elementType == typeof (int))
        Marshal.Copy(src, (int[]) flatArray, 0, flatArray.Length);
      else if (elementType == typeof (long))
        Marshal.Copy(src, (long[]) flatArray, 0, flatArray.Length);
      else if (elementType == typeof (float))
      {
        Marshal.Copy(src, (float[]) flatArray, 0, flatArray.Length);
      }
      else
      {
        if (!(elementType == typeof (double)))
          return;
        Marshal.Copy(src, (double[]) flatArray, 0, flatArray.Length);
      }
    }
  }
}
