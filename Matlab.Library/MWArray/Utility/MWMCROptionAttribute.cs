// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.MWMCROptionAttribute
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System;

namespace MathWorks.MATLAB.NET.Utility
{
  [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
  public class MWMCROptionAttribute : Attribute
  {
    private string option;

    public MWMCROptionAttribute(string OptVal)
    {
      this.option = OptVal;
    }

    public string MWMCROption
    {
      get
      {
        return this.option;
      }
    }
  }
}
