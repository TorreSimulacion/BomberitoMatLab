// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.NativeGCAttribute
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System;

namespace MathWorks.MATLAB.NET.Utility
{
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
  public class NativeGCAttribute : Attribute
  {
    private bool gcEnabled;
    private int gcBlockSize;

    public NativeGCAttribute(bool enableGCTrigger)
    {
      this.gcEnabled = enableGCTrigger;
    }

    public NativeGCAttribute(bool enableGCTrigger, int GCBlockSize)
    {
      this.gcEnabled = enableGCTrigger;
      this.gcBlockSize = GCBlockSize;
    }

    public bool GCEnabled
    {
      get
      {
        return this.gcEnabled;
      }
    }

    public int GCBlockSize
    {
      get
      {
        return this.gcBlockSize;
      }
      set
      {
        this.gcBlockSize = value;
      }
    }
  }
}
