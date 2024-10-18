// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.MWCTFStreamReader
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MathWorks.MATLAB.NET.Utility
{
  public class MWCTFStreamReader
  {
    private BinaryReader ctfStreamReader;
    private IntPtr readCtfStreamFcn;
    private MWCTFStreamReader.ReadCtfStreamDelegate _readCtfStream;

    public MWCTFStreamReader(Stream embeddedCtfStream)
    {
      embeddedCtfStream.Position = 0L;
      this._readCtfStream = new MWCTFStreamReader.ReadCtfStreamDelegate(this.readCtfStream);
      this.readCtfStreamFcn = Marshal.GetFunctionPointerForDelegate((Delegate) this._readCtfStream);
      this.ctfStreamReader = new BinaryReader(embeddedCtfStream);
    }

    internal int readCtfStream(IntPtr ctfByte, int readSize)
    {
      try
      {
        byte[] source = this.ctfStreamReader.ReadBytes(readSize);
        Marshal.Copy(source, 0, ctfByte, source.Length);
        return source.Length;
      }
      catch (EndOfStreamException ex)
      {
        return -1;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public IntPtr CtfStreamReadFcn
    {
      get
      {
        return this.readCtfStreamFcn;
      }
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int ReadCtfStreamDelegate(IntPtr ctfByte, int size);
  }
}
