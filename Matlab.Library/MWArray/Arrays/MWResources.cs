// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Arrays.MWResources
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using System.Reflection;
using System.Resources;

namespace MathWorks.MATLAB.NET.Arrays
{
  internal class MWResources
  {
    private static ResourceManager resourceManager;

    static MWResources()
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      MWResources.resourceManager = new ResourceManager(executingAssembly.GetManifestResourceNames()[0].TrimEnd("resources".ToCharArray()).TrimEnd(".".ToCharArray()), executingAssembly);
    }

    public static ResourceManager getResourceManager()
    {
      return MWResources.resourceManager;
    }
  }
}
