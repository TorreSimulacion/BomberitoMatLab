// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.PlatformInfo
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System;

namespace MathWorks.MATLAB.NET.Utility
{
  internal class PlatformInfo
  {
    public static string getArchDir()
    {
      if (PlatformInfo.Is32BitProcess())
        return "win32";
      if (PlatformInfo.Is64BitProcess())
        return "win64";
      throw new Exception("Unsupported Windows platform");
    }

    public static bool Is32BitProcess()
    {
      return IntPtr.Size == 4;
    }

    public static bool Is64BitProcess()
    {
      return IntPtr.Size == 8;
    }
  }
}
