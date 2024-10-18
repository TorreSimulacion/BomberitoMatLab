// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.MWException
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System;

namespace MathWorks.MATLAB.NET.Utility
{
  [Serializable]
  public class MWException : ApplicationException
  {
    private bool disposed;
    private string[] mwStack;

    public MWException(string msg, string[] stack)
      : base(msg)
    {
      this.mwStack = stack;
    }

    ~MWException()
    {
      this.Dispose(false);
    }

    internal void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    internal virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing && this.mwStack != null)
      {
        Array.Clear((Array) this.mwStack, 0, this.mwStack.Length);
        this.mwStack = (string[]) null;
      }
      this.disposed = true;
    }

    public override string StackTrace
    {
      get
      {
        string str = "... Matlab M-code Stack Trace ...\n";
        foreach (string mw in this.mwStack)
          str = str + "    at\n" + mw + "\n";
        return str + "\n... .Application Stack Trace ...\n" + base.StackTrace;
      }
    }
  }
}
