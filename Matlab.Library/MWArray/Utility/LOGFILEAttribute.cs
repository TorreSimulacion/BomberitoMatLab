// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.LOGFILEAttribute
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System;

namespace MathWorks.MATLAB.NET.Utility
{
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
  public class LOGFILEAttribute : Attribute
  {
    private string fName;

    public LOGFILEAttribute(string fileName)
    {
      string environmentVariable = Environment.GetEnvironmentVariable("MW_LOGFILE");
      if (environmentVariable != null)
        this.fName = environmentVariable;
      else
        this.fName = fileName;
    }

    public string LogfileName
    {
      get
      {
        return this.fName;
      }
    }
  }
}
