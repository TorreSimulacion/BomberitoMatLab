// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Arrays.MWObjectArray
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using MathWorks.MATLAB.NET.Utility;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;

namespace MathWorks.MATLAB.NET.Arrays
{
  [Serializable]
  public class MWObjectArray : MWArray, IEquatable<MWObjectArray>
  {
    private static Assembly dotnetcliAssembly = Assembly.LoadFrom(MWObjectArray.getPathToDotnetcli());
    private static Type SafeNETCliConversions;
    private static MethodInfo GetMxArrayFromObjectPtr;
    private static MethodInfo GetObjectFromMxArrayPtr;
    private object _object;

    static MWObjectArray()
    {
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(MWObjectArray.DotnetcliResolveEventHandler);
      MWObjectArray.SafeNETCliConversions = MWObjectArray.dotnetcliAssembly.GetType("dotnetcli.DeployedDataConversion");
      MWObjectArray.GetMxArrayFromObjectPtr = MWObjectArray.SafeNETCliConversions.GetMethod("GetMxArrayFromObject", BindingFlags.Static | BindingFlags.Public);
      MWObjectArray.GetObjectFromMxArrayPtr = MWObjectArray.SafeNETCliConversions.GetMethod("GetObjectFromMxArray", BindingFlags.Static | BindingFlags.Public);
    }

    internal MWObjectArray(MWSafeHandle hMXArray)
    {
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        Monitor.Enter(MWArray.mxSync);
        IntPtr handle = hMXArray.DangerousGetHandle();
        this._object = MWObjectArray.GetObjectFromMxArrayPtr.Invoke((object) null, new object[1]
        {
          (object) handle
        });
        this.array_Type = MWArrayType.NativeObjArray;
      }
      finally
      {
        Monitor.Exit(MWArray.mxSync);
      }
    }

    public MWObjectArray()
    {
      this._object = new object();
      this.array_Type = MWArrayType.NativeObjArray;
    }

    public MWObjectArray(object obj)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj), MWArray.resourceManager.GetString("MWErrorInvalidNullArgument"));
      this._object = obj;
      this.array_Type = MWArrayType.NativeObjArray;
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        int num = disposing ? 1 : 0;
      }
      finally
      {
        this._object = (object) null;
      }
    }

    public object this[params int[] indices]
    {
      get
      {
        if (2 < indices.Length)
          throw new ArgumentOutOfRangeException(MWArray.resourceManager.GetString("MWErrorInvalidDimensions"));
        foreach (int index in indices)
        {
          if (1 != index)
            throw new ArgumentOutOfRangeException(MWArray.resourceManager.GetString("MWErrorInvalidIndex"));
        }
        return this._object;
      }
      set
      {
        if (2 < indices.Length)
          throw new ArgumentOutOfRangeException(MWArray.resourceManager.GetString("MWErrorInvalidDimensions"));
        foreach (int index in indices)
        {
          if (1 != index)
            throw new ArgumentOutOfRangeException(MWArray.resourceManager.GetString("MWErrorInvalidIndex"));
        }
        this._object = value;
      }
    }

    public static bool operator ==(MWObjectArray objectArrayA, MWObjectArray objectArrayB)
    {
      if ((object) objectArrayA == null)
        return (object) objectArrayB == null;
      return objectArrayA.Equals(objectArrayB);
    }

    public static bool operator !=(MWObjectArray objectArrayA, MWObjectArray objectArrayB)
    {
      if ((object) objectArrayA == null)
        return objectArrayB != null;
      return !objectArrayA.Equals(objectArrayB);
    }

    public override bool Equals(object obj)
    {
      if ((object) (obj as MWObjectArray) == null)
        return false;
      return this.Equals(obj as MWObjectArray);
    }

    public bool Equals(MWObjectArray obj)
    {
      if ((object) obj == null)
        return false;
      return this._object == obj._object;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (!(this._object is ISerializable))
        throw new InvalidCastException(MWArray.resourceManager.GetString("MWNotSerializeable"));
      info.AddValue("nativeObject", this._object, this._object.GetType());
    }

    protected MWObjectArray(SerializationInfo serializationInfo, StreamingContext context)
    {
      this._object = new object();
      this._object = serializationInfo.GetValue("nativeObject", this._object.GetType());
      this.array_Type = MWArrayType.NativeObjArray;
    }

    public override bool IsDisposed
    {
      get
      {
        return this._object == null;
      }
    }

    public override bool IsEmpty
    {
      get
      {
        this.CheckDisposed();
        return MWObjectArray.Empty == this;
      }
    }

    internal override MWSafeHandle MXArrayHandle
    {
      get
      {
        try
        {
          Monitor.Enter(MWArray.mxSync);
          return new MWSafeHandle((IntPtr) MWObjectArray.GetMxArrayFromObjectPtr.Invoke((object) null, new object[1]
          {
            this._object
          }), false);
        }
        finally
        {
          Monitor.Exit(MWArray.mxSync);
        }
      }
    }

    public static MWObjectArray Empty
    {
      get
      {
        return new MWObjectArray();
      }
    }

    public object Object
    {
      get
      {
        return this._object;
      }
      set
      {
        this._object = value;
      }
    }

    public override object Clone()
    {
      this.CheckDisposed();
      MWObjectArray mwObjectArray = (MWObjectArray) this.MemberwiseClone();
      if (!this._object.GetType().IsValueType)
      {
        ICloneable cloneable = this._object as ICloneable;
        if (cloneable != null)
          mwObjectArray.Object = cloneable.Clone();
      }
      return (object) mwObjectArray;
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override string ToString()
    {
      this.CheckDisposed();
      string str1 = this._object.ToString();
      char ch1 = '\n';
      char ch2 = ' ';
      string str2 = str1.TrimStart(ch1).TrimEnd(ch1);
      if (-1 == str2.IndexOf(ch1))
        str2 = str2.TrimStart(ch2).TrimEnd(ch2);
      return str2;
    }

    internal override object ConvertToType(Type t)
    {
      if (t == this.Object.GetType() || t.IsAssignableFrom(this.Object.GetType()))
        return this.Object;
      throw new ArgumentException(MWArray.resourceManager.GetString("MWErrorInvalidDataConversion"), (Exception) new ApplicationException("Cannot convert from MWObjectArray containing object of type " + this.Object.GetType().FullName + " to " + t.FullName));
    }

    private static string getPathToDotnetcli()
    {
      string matlabRootForAssembly = MWObjectArray.getMatlabRootForAssembly();
      if (matlabRootForAssembly.Length <= 0)
        throw new Exception("Failed to find mclmcrrt9_6.dll required to load dotnetcli on system path : \n" + Environment.GetEnvironmentVariable("path"));
      string archDir = PlatformInfo.getArchDir();
      return Path.Combine(Path.Combine(matlabRootForAssembly, Path.Combine("bin", archDir)), "dotnetcli.dll");
    }

    private static string getMatlabRootForAssembly()
    {
      string str1 = "\"";
      string str2 = "";
      string path2 = "mclmcrrt9_6.dll";
      string[] strArray = Environment.GetEnvironmentVariable("path").Split(';');
      for (int index = 0; index < strArray.Length; ++index)
      {
        try
        {
          string path = !strArray[index].Contains(str1) ? Path.Combine(strArray[index], path2) : Path.Combine(strArray[index].Trim(str1.ToCharArray()), path2);
          string archDir = PlatformInfo.getArchDir();
          if (File.Exists(path))
          {
            if (path.Contains(Path.Combine("runtime", archDir)))
            {
              str2 = path.Substring(0, path.LastIndexOf("runtime"));
              break;
            }
          }
        }
        catch (ArgumentException ex)
        {
        }
      }
      return str2;
    }

    private static Assembly DotnetcliResolveEventHandler(
      object sender,
      ResolveEventArgs args)
    {
      return MWObjectArray.dotnetcliAssembly;
    }
  }
}
