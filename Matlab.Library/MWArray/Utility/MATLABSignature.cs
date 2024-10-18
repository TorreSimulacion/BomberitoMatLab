// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.MATLABSignature
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System;

namespace MathWorks.MATLAB.NET.Utility
{
  [AttributeUsage(AttributeTargets.Method)]
  public class MATLABSignature : Attribute
  {
    public readonly string Name;
    public readonly int Inputs;
    public readonly int Outputs;
    public readonly bool HasVarArgIn;

    public MATLABSignature(string name, int inputs, int outputs, int hasvarargin)
    {
      this.Name = name;
      this.Inputs = inputs;
      this.Outputs = outputs;
      this.HasVarArgIn = Convert.ToBoolean(hasvarargin);
    }
  }
}
