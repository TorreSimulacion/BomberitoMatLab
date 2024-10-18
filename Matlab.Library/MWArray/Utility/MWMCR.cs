// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.MWMCR
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace MathWorks.MATLAB.NET.Utility
{
  public class MWMCR : IDisposable
  {
    private static MWMCR.CallBackDelegate _OutDelegate = new MWMCR.CallBackDelegate(MWMCR.writeToOut);
    private static MWMCR.CallBackDelegate _ErrDelegate = new MWMCR.CallBackDelegate(MWMCR.writeToErr);
    private static bool callOncePerAppDomain = false;
    private static Queue mcrInstances = new Queue();
    private static int maxArgsOut = 5;
    private static int maxArgsIn = 10;
    private static IntPtr[] plhs = new IntPtr[MWMCR.maxArgsOut];
    private static IntPtr[] prhs = new IntPtr[MWMCR.maxArgsIn];
    private static ResourceManager mcrResourceManager = (ResourceManager) null;
    public static bool MCRAppInitialized = false;
    private static GCHandle gchOut;
    private static GCHandle gchErr;
    private static MathWorks.MATLAB.NET.Arrays.MWArray[] argsOut;
    private IntPtr mcrInstance;
    private bool disposed;

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclFeval_proxy")]
    private static extern byte mclFeval(
      [In] IntPtr pMCR,
      [In] string functionName,
      [In] int nlhs,
      [In] IntPtr[] plhs,
      [In] int nrhs,
      [In] IntPtr[] prhs);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclImpersonationFeval_proxy")]
    private static extern byte mclImpersonationFeval(
      [In] IntPtr pMCR,
      [In] string functionName,
      [In] int nlhs,
      [In] IntPtr[] plhs,
      [In] int nrhs,
      [In] IntPtr[] prhs,
      IntPtr impersonationToken);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "stopImpersonationOnMCRThread_proxy")]
    private static extern void stopImpersonationOnMCRThread([In] IntPtr pMCR);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclGetDotNetComponentType_proxy")]
    private static extern int mclGetDotNetComponentType();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclGetLastErrorMessage_proxy")]
    private static extern IntPtr mclGetLastErrorMessage();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclGetMCCTargetType_proxy")]
    private static extern int mclGetMCCTargetType(byte isLibrary);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclInitializeApplication_730_proxy")]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool mclInitializeApplication(
      [In] string[] startupOptions,
      [In] IntPtr startupOptionsCount);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclInitializeComponentInstanceNonEmbeddedStandalone_proxy")]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool mclInitializeComponentInstanceNonEmbeddedStandalone(
      out IntPtr pMcrInst,
      [In] string pathToComponent,
      [In] string componentName,
      [In] int targetType,
      [In] IntPtr errorHandler,
      [In] IntPtr printHandler);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclInitializeComponentInstanceWithCallbk_proxy")]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool mclInitializeComponentInstanceWithCallbk(
      out IntPtr pMcrInst,
      [In] IntPtr errorHandler,
      [In] IntPtr printHandler,
      [In] IntPtr readCtfStreamFcn,
      [In] long ctfStreamSize);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclmcrInitialize2_proxy")]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool mclmcrInitialize2(int primaryMode);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclTerminateApplication_proxy")]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool mclTerminateApplication();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclTerminateInstance_proxy")]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool mclTerminateInstance(ref IntPtr pMcrInst);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclWaitForFiguresToDie_proxy")]
    private static extern void mclWaitForFiguresToDie([In] IntPtr hMCRInstance);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclGetStackTrace_proxy")]
    private static extern int mclGetStackTrace(ref IntPtr stack);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclFreeStackTrace_proxy")]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool mclFreeStackTrace(ref IntPtr stack, int nStackDepth);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclGetTempFileName_proxy")]
    private static extern IntPtr mclGetTempFileName([In] string tempFileName);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclIsMCRInitialized_proxy")]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool mclIsMCRInitialized();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclIsJVMEnabled_proxy")]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool mclIsJVMEnabled();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclGetLogFileName_proxy")]
    private static extern IntPtr mclGetLogFileName();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclIsNoDisplaySet_proxy")]
    [return: MarshalAs(UnmanagedType.I1)]
    private static extern bool mclIsNoDisplaySet();

    [SuppressUnmanagedCodeSecurity]
    [DllImport("mclmcrrt9_6.dll", EntryPoint = "mclMxDestroyArray_proxy")]
    protected static extern void mclMxDestroyArray([In] IntPtr pMCR, [In] IntPtr pMXArray);

    static MWMCR()
    {
      MWMCR.mcrResourceManager = MWResources.getResourceManager();
      MWMCR.gchOut = GCHandle.Alloc((object) MWMCR._OutDelegate);
      MWMCR.gchErr = GCHandle.Alloc((object) MWMCR._ErrDelegate);
      if (!AppDomain.CurrentDomain.IsDefaultAppDomain())
        AppDomain.CurrentDomain.DomainUnload += new EventHandler(MWMCR.unloadAppDomain);
      AppDomain.CurrentDomain.ProcessExit += (EventHandler) ((sender, eventArgs) => MWMCR.processExiting((Exception) null));
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        if (MWMCR.MCRAppInitialized)
          return;
        if (!MWMCR.mclmcrInitialize2(3))
          throw new Exception("Trouble initializing libraries required by .NET Assembly.\n");
        if (!MWMCR.mclIsMCRInitialized())
        {
          List<string> stringList = new List<string>();
          Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
          bool flag = false;
          foreach (Assembly element in assemblies)
          {
            NOJVMAttribute customAttribute1 = (NOJVMAttribute) Attribute.GetCustomAttribute(element, typeof (NOJVMAttribute));
            LOGFILEAttribute customAttribute2 = (LOGFILEAttribute) Attribute.GetCustomAttribute(element, typeof (LOGFILEAttribute));
            MWMCROptionAttribute[] customAttributes = (MWMCROptionAttribute[]) Attribute.GetCustomAttributes(element, typeof (MWMCROptionAttribute));
            if (flag && (customAttribute1 != null || customAttribute2 != null || customAttributes.Length != 0))
              throw new Exception("Start-up options discovered in multiple assembiles.\n");
            if (customAttribute1 != null && customAttribute1.JVMDisabled)
              stringList.Add("-nojvm");
            if (customAttribute2 != null)
            {
              stringList.Add("-logfile");
              stringList.Add(customAttribute2.LogfileName);
            }
            if (customAttributes.Length != 0)
            {
              foreach (MWMCROptionAttribute mwmcrOptionAttribute in customAttributes)
                stringList.Add(mwmcrOptionAttribute.MWMCROption);
            }
            if (customAttribute1 != null || customAttribute2 != null || customAttributes.Length != 0)
              flag = true;
          }
          MWMCR.InitializeApplication(stringList.ToArray());
        }
        MWMCR.MCRAppInitialized = true;
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
    }

    public MWMCR(string componentName, string componentPath, bool isLibrary)
    {
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        string str = componentPath + "\\" + componentName + ".ctf";
        if (componentName == null || componentPath == null || !File.Exists(str))
          throw new FileNotFoundException(MWMCR.mcrResourceManager.GetString("MWErrorCTFFileNotFound"), str);
        IntPtr pointerForDelegate1 = Marshal.GetFunctionPointerForDelegate((Delegate) MWMCR._OutDelegate);
        IntPtr pointerForDelegate2 = Marshal.GetFunctionPointerForDelegate((Delegate) MWMCR._ErrDelegate);
        int mccTargetType = MWMCR.mclGetMCCTargetType(isLibrary ? (byte) 1 : (byte) 0);
        if (!MWMCR.mclInitializeComponentInstanceNonEmbeddedStandalone(out this.mcrInstance, componentPath, componentName, mccTargetType, pointerForDelegate2, pointerForDelegate1))
          throw new ApplicationException(MWMCR.mcrResourceManager.GetString("MWErrorMCRInitialize") + "\n" + MWMCR.LastErrorMessage);
        MWMCR.mcrInstances.Enqueue((object) this);
      }
      catch (Exception ex)
      {
        throw new Exception(MWMCR.mcrResourceManager.GetString("MWErrorMCRInitialize"), ex);
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
      this.setBuilderUserData();
    }

    public MWMCR(
      string componentId,
      string componentPath,
      Stream embeddedCtfStream,
      bool isLibrary)
    {
      if (embeddedCtfStream == null)
        throw new ApplicationException(MWMCR.mcrResourceManager.GetString("MWErrorCTFNotEmbedded"));
      GCHandle gcHandle;
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        IntPtr pointerForDelegate1 = Marshal.GetFunctionPointerForDelegate((Delegate) MWMCR._OutDelegate);
        IntPtr pointerForDelegate2 = Marshal.GetFunctionPointerForDelegate((Delegate) MWMCR._ErrDelegate);
        MWMCR.mclGetMCCTargetType(isLibrary ? (byte) 1 : (byte) 0);
        MWCTFStreamReader mwctfStreamReader = new MWCTFStreamReader(embeddedCtfStream);
        gcHandle = GCHandle.Alloc((object) mwctfStreamReader);
        int num = MWMCR.mclInitializeComponentInstanceWithCallbk(out this.mcrInstance, pointerForDelegate2, pointerForDelegate1, mwctfStreamReader.CtfStreamReadFcn, embeddedCtfStream.Length) ? 1 : 0;
        MWMCR.mcrInstances.Enqueue((object) this);
        if (num == 0)
          throw new ApplicationException(MWMCR.mcrResourceManager.GetString("MWErrorMCRInitialize") + "\n" + MWMCR.LastErrorMessage);
      }
      catch (Exception ex)
      {
        throw new Exception(MWMCR.mcrResourceManager.GetString("MWErrorMCRInitialize"), ex);
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
      this.setBuilderUserData();
      try
      {
        gcHandle.Free();
      }
      catch
      {
      }
    }

    ~MWMCR()
    {
      this.Dispose(false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        this.disposed = true;
        int num = disposing ? 1 : 0;
        if (!MWMCR.MCRAppInitialized || !(IntPtr.Zero != this.mcrInstance))
          return;
        if (!MWMCR.mclTerminateInstance(ref this.mcrInstance))
          throw new Exception(MWMCR.mcrResourceManager.GetString("MWErrorMCRTermination") + "\n" + MWMCR.LastErrorMessage);
        this.mcrInstance = IntPtr.Zero;
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
    }

    internal static string LastErrorMessage
    {
      get
      {
        IntPtr zero = IntPtr.Zero;
        try
        {
          Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
          string stringAnsi = Marshal.PtrToStringAnsi(MWMCR.mclGetLastErrorMessage());
          return string.Empty == stringAnsi ? "segv - SEVERE ERROR" : stringAnsi;
        }
        finally
        {
          Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        }
      }
    }

    internal IntPtr MCRInstance
    {
      get
      {
        return this.mcrInstance;
      }
    }

    private static bool InitializeApplication(params string[] startupOptions)
    {
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        if (MWMCR.IsMCRInitialized())
          throw new Exception("MWMCR.InitializeApplication should be called before initializing a .NET Assembly component.\n" + "MATLAB Runtime is already initialized with following options:\n" + "JVM enabled : " + MWMCR.IsMCRJVMEnabled().ToString() + "\nLogfile name : " + "\"" + MWMCR.GetMCRLogFileName() + "\"");
        if (MWMCR.mclInitializeApplication(startupOptions, new IntPtr(startupOptions.Length)))
          return true;
        throw new Exception("The MATLAB Runtime instance could not be initialized" + "\n" + MWMCR.LastErrorMessage);
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
    }

    public static bool TerminateApplication()
    {
      return true;
    }

    public static bool TerminateApplicationEx()
    {
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        if (MWMCR.MCRAppInitialized)
        {
          if (!MWMCR.mclTerminateApplication())
            throw new Exception(MWMCR.mcrResourceManager.GetString("MWErrorMCRTermination") + "\n" + MWMCR.LastErrorMessage);
          MWMCR.MCRAppInitialized = false;
        }
        return true;
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
    }

    private static void processExiting(Exception exception)
    {
      if (exception != null)
        return;
      MWMCR.TerminateApplicationEx();
    }

    internal static int writeToOut(string s)
    {
      Console.WriteLine(s);
      return s.Length;
    }

    internal static int writeToErr(string s)
    {
      Console.Error.WriteLine(s);
      return s.Length;
    }

    public void WaitForFiguresToDie()
    {
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        MWMCR.mclWaitForFiguresToDie(this.mcrInstance);
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
    }

    private void setBuilderUserData()
    {
      this.EvaluateFunction(0, "setmcruserdata", (MathWorks.MATLAB.NET.Arrays.MWArray) "builder", (MathWorks.MATLAB.NET.Arrays.MWArray) "net");
    }

    private MathWorks.MATLAB.NET.Arrays.MWArray[] EvaluateFunction(
      string functionName,
      int numArgsOut,
      int numArgsIn,
      MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn)
    {
      if (numArgsOut < 0)
        throw new ArgumentOutOfRangeException(nameof (numArgsOut), MWMCR.mcrResourceManager.GetString("MWErrorNegativeArg"));
      if (numArgsIn < 0)
        throw new ArgumentOutOfRangeException(nameof (numArgsIn), MWMCR.mcrResourceManager.GetString("MWErrorNegativeArg"));
      if (argsIn == null)
      {
        if (numArgsIn != 0)
          throw new ArgumentOutOfRangeException(nameof (argsIn), MWMCR.mcrResourceManager.GetString("MWErrorEvalFunctionArg"));
      }
      else
      {
        MathWorks.MATLAB.NET.Arrays.MWArray.formattedOutputString = new StringBuilder(1024);
        if (numArgsIn > argsIn.Length)
          throw new ArgumentOutOfRangeException(nameof (argsIn), MWMCR.mcrResourceManager.GetString("MWErrorEvalFunctionArg"));
        foreach (MathWorks.MATLAB.NET.Arrays.MWArray mwArray in argsIn)
        {
          if (mwArray == null)
            throw new ArgumentNullException(MWMCR.mcrResourceManager.GetString("MWErrorInvalidNullArgument"), new Exception());
          mwArray.CheckDisposed();
        }
      }
      if (numArgsIn > MWMCR.maxArgsIn)
      {
        MWMCR.maxArgsIn = numArgsIn;
        MWMCR.prhs = new IntPtr[MWMCR.maxArgsIn];
      }
      IntPtr[] numArray = new IntPtr[numArgsIn];
      if (numArgsOut > MWMCR.maxArgsOut)
      {
        MWMCR.maxArgsOut = numArgsOut;
        MWMCR.plhs = new IntPtr[MWMCR.maxArgsOut];
      }
      IntPtr zero = IntPtr.Zero;
      byte num1 = 1;
      WindowsIdentity current = null;
      RuntimeHelpers.PrepareConstrainedRegions();
      try
      {
        for (int index = 0; index < numArgsIn; ++index)
        {
          if (argsIn[index].ArrayType == MathWorks.MATLAB.NET.Arrays.MWArrayType.NativeObjArray && !MWMCR.callOncePerAppDomain)
          {
            MWMCR.callOncePerAppDomain = true;
            int num2 = (int) MWMCR.mclFeval(this.mcrInstance, "eval", 0, (IntPtr[]) null, 1, new IntPtr[1]
            {
              new MWCharArray("x=System.String('');clear x").MXArrayHandle.DangerousGetHandle()
            });
          }
          MWMCR.prhs[index] = argsIn[index].MXArrayHandle.DangerousGetHandle();
          if (argsIn[index].ArrayType == MathWorks.MATLAB.NET.Arrays.MWArrayType.NativeObjArray)
            numArray[index] = MWMCR.prhs[index];
        }
        num1 = current != null ? MWMCR.mclImpersonationFeval(this.mcrInstance, functionName, numArgsOut, MWMCR.plhs, numArgsIn, MWMCR.prhs, current.Token) : MWMCR.mclFeval(this.mcrInstance, functionName, numArgsOut, MWMCR.plhs, numArgsIn, MWMCR.prhs);
        if (num1 == (byte) 0)
          return (MathWorks.MATLAB.NET.Arrays.MWArray[]) null;
        MWMCR.argsOut = new MathWorks.MATLAB.NET.Arrays.MWArray[numArgsOut];
        for (int index = 0; index < numArgsOut; ++index)
          MWMCR.argsOut[index] = MathWorks.MATLAB.NET.Arrays.MWArray.GetTypedArray(new MWSafeHandle(MWMCR.plhs[index]), this.mcrInstance);
        return MWMCR.argsOut;
      }
      finally
      {
        foreach (IntPtr pMXArray in numArray)
        {
          if (pMXArray != IntPtr.Zero)
            MWMCR.mclMxDestroyArray(this.mcrInstance, pMXArray);
        }
        if (current != null)
          MWMCR.stopImpersonationOnMCRThread(this.mcrInstance);
        if (num1 == (byte) 0)
        {
          string lastErrorMessage = MWMCR.LastErrorMessage;
          string mlError = "\n\n... MWMCR::EvaluateFunction error ... \n" + (string.Empty == lastErrorMessage ? "segv - SEVERE ERROR" : lastErrorMessage) + ".";
          string[] mlStack = (string[]) null;
          int stackTrace = MWMCR.mclGetStackTrace(ref zero);
          if (stackTrace > 0 && zero != IntPtr.Zero)
          {
            IntPtr[] destination = new IntPtr[stackTrace];
            Marshal.Copy(zero, destination, 0, stackTrace);
            mlStack = new string[stackTrace];
            for (int index = 0; index < stackTrace; ++index)
              mlStack[index] = Marshal.PtrToStringAnsi(destination[index]);
            MWMCR.mclFreeStackTrace(ref zero, stackTrace);
          }
          throw new Exception(this.combineErrorMessages(mlError, mlStack));
        }
      }
    }

    private string combineErrorMessages(string mlError, string[] mlStack)
    {
      string str1 = mlError + "\n\n";
      if (mlStack != null)
      {
        string str2 = str1 + "... Matlab M-code Stack Trace ...\n";
        foreach (string ml in mlStack)
          str2 = str2 + "    at\n" + ml + "\n";
        str1 = str2 + "\n";
      }
      return str1;
    }

    private MathWorks.MATLAB.NET.Arrays.MWArray[] UnpackArgArray(object[] argArray)
    {
      if (argArray == null)
        return (MathWorks.MATLAB.NET.Arrays.MWArray[]) null;
      int length1 = argArray.Length;
      object[] objArray = (object[]) null;
      if (length1 > 0 && typeof (object[]) == argArray[length1 - 1].GetType())
        objArray = argArray[length1 - 1] as object[];
      int length2 = objArray != null ? length1 + objArray.Length - 1 : length1;
      int num = objArray == null ? length1 : length1 - 1;
      MathWorks.MATLAB.NET.Arrays.MWArray[] mwArrayArray = new MathWorks.MATLAB.NET.Arrays.MWArray[length2];
      int index1;
      for (index1 = 0; index1 < num; ++index1)
        mwArrayArray[index1] = MathWorks.MATLAB.NET.Arrays.MWArray.ConvertObjectToMWArray(argArray[index1]);
      if (objArray != null)
      {
        for (int index2 = 0; index2 < objArray.Length; ++index2)
        {
          mwArrayArray[index1] = MathWorks.MATLAB.NET.Arrays.MWArray.ConvertObjectToMWArray(objArray[index2]);
          ++index1;
        }
      }
      return mwArrayArray;
    }

    private MathWorks.MATLAB.NET.Arrays.MWArray[] UnpackArgArray(
      object[] argArray,
      object[] varArgArray)
    {
      MathWorks.MATLAB.NET.Arrays.MWArray[] mwArrayArray = new MathWorks.MATLAB.NET.Arrays.MWArray[argArray.Length + varArgArray.Length];
      int num = 0;
      foreach (object objIn in argArray)
        mwArrayArray[num++] = MathWorks.MATLAB.NET.Arrays.MWArray.ConvertObjectToMWArray(objIn);
      foreach (object varArg in varArgArray)
        mwArrayArray[num++] = MathWorks.MATLAB.NET.Arrays.MWArray.ConvertObjectToMWArray(varArg);
      return mwArrayArray;
    }

    public object EvaluateFunction(string functionName, params object[] argsIn)
    {
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn1 = this.UnpackArgArray(argsIn);
        int numArgsIn = argsIn1 == null ? 0 : argsIn1.Length;
        MathWorks.MATLAB.NET.Arrays.MWArray in_arg = this.EvaluateFunction(functionName, 1, numArgsIn, argsIn1)[0];
        object obj = (object) null;
        if (in_arg != null)
          obj = this.marshalMWArrayToObject(in_arg);
        return obj;
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
    }

    public MathWorks.MATLAB.NET.Arrays.MWArray EvaluateFunction(
      string functionName,
      params MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn)
    {
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        int numArgsIn = argsIn == null ? 0 : argsIn.Length;
        return this.EvaluateFunction(functionName, 1, numArgsIn, argsIn)?[0];
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
    }

    public MathWorks.MATLAB.NET.Arrays.MWArray[] EvaluateFunction(
      string functionName,
      int numArgsOut,
      MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn)
    {
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        int numArgsIn = argsIn != null ? argsIn.Length : 0;
        return this.EvaluateFunction(functionName, numArgsOut, numArgsIn, argsIn);
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
    }

    public MathWorks.MATLAB.NET.Arrays.MWArray[] EvaluateFunction(
      int numArgsOut,
      string functionName,
      params MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn)
    {
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        int numArgsIn = argsIn != null ? argsIn.Length : 0;
        return this.EvaluateFunction(functionName, numArgsOut, numArgsIn, argsIn);
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
    }

    public object[] EvaluateFunction(int numArgsOut, string functionName, params object[] argsIn)
    {
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn1 = this.UnpackArgArray(argsIn);
        int numArgsIn = argsIn1 == null ? 0 : argsIn1.Length;
        MathWorks.MATLAB.NET.Arrays.MWArray[] function = this.EvaluateFunction(functionName, numArgsOut, numArgsIn, argsIn1);
        object[] objArray = new object[numArgsOut];
        for (int index = 0; index < numArgsOut; ++index)
          objArray[index] = this.marshalMWArrayToObject(function[index]);
        return objArray;
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
    }

    public void EvaluateFunction(
      string functionName,
      int numArgsOut,
      ref MathWorks.MATLAB.NET.Arrays.MWArray[] argsOut,
      MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn)
    {
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        int numArgsIn = argsIn == null ? 0 : argsIn.Length;
        argsOut = this.EvaluateFunction(functionName, numArgsOut, numArgsIn, argsIn);
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
    }

    public void EvaluateFunctionForTypeSafeCall(
      string functionName,
      int numArgsOut,
      ref object[] argsOut,
      object[] argsIn,
      params object[] varArgsIn)
    {
      try
      {
        Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
        MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn1 = this.UnpackArgArray(argsIn, varArgsIn);
        int numArgsIn = argsIn1 == null ? 0 : argsIn1.Length;
        MathWorks.MATLAB.NET.Arrays.MWArray[] function = this.EvaluateFunction(functionName, numArgsOut, numArgsIn, argsIn1);
        argsOut = new object[numArgsOut];
        for (int index = 0; index < numArgsOut; ++index)
          argsOut[index] = (object) function[index];
      }
      finally
      {
        Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
      }
    }

    private object marshalMWArrayToObject(MathWorks.MATLAB.NET.Arrays.MWArray in_arg)
    {
      object obj1;
      switch (in_arg.ArrayType)
      {
        case MathWorks.MATLAB.NET.Arrays.MWArrayType.Cell:
          MathWorks.MATLAB.NET.Arrays.MWCellArray mwCellArray1 = (MathWorks.MATLAB.NET.Arrays.MWCellArray) in_arg;
          MathWorks.MATLAB.NET.Arrays.native.MWCellArray mwCellArray2 = new MathWorks.MATLAB.NET.Arrays.native.MWCellArray(mwCellArray1.Dimensions);
          for (int index = 1; index <= mwCellArray1.NumberOfElements; ++index)
          {
            MathWorks.MATLAB.NET.Arrays.MWArray in_arg1 = mwCellArray1[new int[1]
            {
              index
            }];
            if (in_arg1 != null)
            {
              object obj2 = this.marshalMWArrayToObject(in_arg1);
              mwCellArray2[new int[1]{ index }] = obj2;
            }
          }
          obj1 = (object) mwCellArray2;
          break;
        case MathWorks.MATLAB.NET.Arrays.MWArrayType.Structure:
          MathWorks.MATLAB.NET.Arrays.MWStructArray mwStructArray1 = (MathWorks.MATLAB.NET.Arrays.MWStructArray) in_arg;
          MathWorks.MATLAB.NET.Arrays.native.MWStructArray mwStructArray2 = new MathWorks.MATLAB.NET.Arrays.native.MWStructArray(mwStructArray1.Dimensions, mwStructArray1.FieldNames);
          for (int index1 = 1; index1 <= mwStructArray1.NumberOfElements; ++index1)
          {
            for (int index2 = 0; index2 < mwStructArray1.NumberOfFields; ++index2)
            {
              MathWorks.MATLAB.NET.Arrays.MWArray in_arg1 = mwStructArray1[mwStructArray1.FieldNames[index2], new int[1]
              {
                index1
              }];
              if (in_arg1 != null)
              {
                object obj2 = this.marshalMWArrayToObject(in_arg1);
                mwStructArray2[mwStructArray1.FieldNames[index2], new int[1]
                {
                  index1
                }] = obj2;
              }
            }
          }
          obj1 = (object) mwStructArray2;
          break;
        default:
          obj1 = (object) in_arg.ToArray();
          break;
      }
      return obj1;
    }

    private static void unloadAppDomain(object sender, EventArgs e)
    {
      while (MWMCR.mcrInstances.Count > 0)
        ((MWMCR) MWMCR.mcrInstances.Dequeue()).Dispose();
      MWMCR.gchOut.Free();
      MWMCR.gchErr.Free();
    }

    public static string GetMCRLogFileName()
    {
      return Marshal.PtrToStringAnsi(MWMCR.mclGetLogFileName());
    }

    public static bool IsMCRInitialized()
    {
      return MWMCR.mclIsMCRInitialized();
    }

    public static bool IsMCRJVMEnabled()
    {
      return MWMCR.mclIsJVMEnabled();
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int CallBackDelegate(string data);
  }
}
