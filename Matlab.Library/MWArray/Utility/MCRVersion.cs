// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.MCRVersion
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System;

namespace MathWorks.MATLAB.NET.Utility
{
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
  public class MCRVersion : Attribute
  {
    public readonly string Major;
    public readonly string Minor;
    public readonly string Update;

    public MCRVersion(string major, string minor, string update)
    {
      this.Major = major;
      this.Minor = minor;
      this.Update = update;
    }
  }
}
