using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Arrays.native;
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
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int CallBackDelegate(string data);

		private static CallBackDelegate _OutDelegate;

		private static CallBackDelegate _ErrDelegate;

		private static GCHandle gchOut;

		private static GCHandle gchErr;

		private static bool callOncePerAppDomain;

		private static Queue mcrInstances;

		private static MathWorks.MATLAB.NET.Arrays.MWArray[] argsOut;

		private static int maxArgsOut;

		private static int maxArgsIn;

		private static IntPtr[] plhs;

		private static IntPtr[] prhs;

		private static ResourceManager mcrResourceManager;

		public static bool MCRAppInitialized;

		private IntPtr mcrInstance;

		private bool disposed;

		internal static string LastErrorMessage
		{
			get
			{
				IntPtr zero = IntPtr.Zero;
				try
				{
					Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
					zero = mclGetLastErrorMessage();
					string text = Marshal.PtrToStringAnsi(zero);
					return (string.Empty == text) ? "segv - SEVERE ERROR" : text;
				}
				finally
				{
					Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
				}
			}
		}

		internal IntPtr MCRInstance => mcrInstance;

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclFeval_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern byte mclFeval([In] IntPtr pMCR, [In] string functionName, [In] int nlhs, [In] IntPtr[] plhs, [In] int nrhs, [In] IntPtr[] prhs);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclImpersonationFeval_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern byte mclImpersonationFeval([In] IntPtr pMCR, [In] string functionName, [In] int nlhs, [In] IntPtr[] plhs, [In] int nrhs, [In] IntPtr[] prhs, IntPtr impersonationToken);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "stopImpersonationOnMCRThread_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern void stopImpersonationOnMCRThread();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclGetDotNetComponentType_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern int mclGetDotNetComponentType();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclGetLastErrorMessage_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern IntPtr mclGetLastErrorMessage();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclGetMCCTargetType_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern int mclGetMCCTargetType(byte isLibrary);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclInitializeApplication_800_proxy")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool mclInitializeApplication([In] string[] startupOptions, [In] IntPtr startupOptionsCount);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclInitializeComponentInstanceNonEmbeddedStandalone_proxy")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool mclInitializeComponentInstanceNonEmbeddedStandalone(out IntPtr pMcrInst, [In] string pathToComponent, [In] string componentName, [In] int targetType, [In] IntPtr errorHandler, [In] IntPtr printHandler);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclInitializeComponentInstanceWithCallbk_proxy")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool mclInitializeComponentInstanceWithCallbk(out IntPtr pMcrInst, [In] IntPtr errorHandler, [In] IntPtr printHandler, [In] IntPtr readCtfStreamFcn, [In] long ctfStreamSize);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclmcrInitialize2_proxy")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool mclmcrInitialize2(int primaryMode);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclTerminateApplication_proxy")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool mclTerminateApplication();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclTerminateInstance_proxy")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool mclTerminateInstance(ref IntPtr pMcrInst);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclWaitForFiguresToDie_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern void mclWaitForFiguresToDie([In] IntPtr hMCRInstance);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclGetStackTrace_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern int mclGetStackTrace(ref IntPtr stack);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclFreeStackTrace_proxy")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool mclFreeStackTrace(ref IntPtr stack, int nStackDepth);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclGetTempFileName_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern IntPtr mclGetTempFileName([In] string tempFileName);

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclIsMCRInitialized_proxy")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool mclIsMCRInitialized();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclIsJVMEnabled_proxy")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool mclIsJVMEnabled();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclGetLogFileName_proxy")]
		[SuppressUnmanagedCodeSecurity]
		private static extern IntPtr mclGetLogFileName();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclIsNoDisplaySet_proxy")]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.I1)]
		private static extern bool mclIsNoDisplaySet();

		[DllImport("mclmcrrt9_1.dll", EntryPoint = "mclMxDestroyArray_proxy")]
		[SuppressUnmanagedCodeSecurity]
		protected static extern void mclMxDestroyArray([In] IntPtr pMXArray, bool onInterpreterThread);

		static MWMCR()
		{
			_OutDelegate = writeToOut;
			_ErrDelegate = writeToErr;
			callOncePerAppDomain = false;
			mcrInstances = new Queue();
			maxArgsOut = 5;
			maxArgsIn = 10;
			plhs = new IntPtr[maxArgsOut];
			prhs = new IntPtr[maxArgsIn];
			mcrResourceManager = null;
			MCRAppInitialized = false;
			mcrResourceManager = MWResources.getResourceManager();
			gchOut = GCHandle.Alloc(_OutDelegate);
			gchErr = GCHandle.Alloc(_ErrDelegate);
			if (!AppDomain.CurrentDomain.IsDefaultAppDomain())
			{
				AppDomain.CurrentDomain.DomainUnload += unloadAppDomain;
			}
			try
			{
				Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
				if (!MCRAppInitialized)
				{
					if (!mclmcrInitialize2(3))
					{
						throw new Exception("Trouble initializing libraries required by .NET Assembly.\n");
					}
					if (!mclIsMCRInitialized())
					{
						List<string> list = new List<string>();
						AppDomain currentDomain = AppDomain.CurrentDomain;
						Assembly[] assemblies = currentDomain.GetAssemblies();
						bool flag = false;
						Assembly[] array = assemblies;
						foreach (Assembly element in array)
						{
							NOJVMAttribute nOJVMAttribute = (NOJVMAttribute)Attribute.GetCustomAttribute(element, typeof(NOJVMAttribute));
							LOGFILEAttribute lOGFILEAttribute = (LOGFILEAttribute)Attribute.GetCustomAttribute(element, typeof(LOGFILEAttribute));
							MWMCROptionAttribute[] array2 = (MWMCROptionAttribute[])Attribute.GetCustomAttributes(element, typeof(MWMCROptionAttribute));
							if (flag && (nOJVMAttribute != null || lOGFILEAttribute != null || array2.Length != 0))
							{
								throw new Exception("Start-up options discovered in multiple assembiles.\n");
							}
							if (nOJVMAttribute != null && nOJVMAttribute.JVMDisabled)
							{
								list.Add("-nojvm");
							}
							if (lOGFILEAttribute != null)
							{
								list.Add("-logfile");
								list.Add(lOGFILEAttribute.LogfileName);
							}
							if (array2.Length != 0)
							{
								MWMCROptionAttribute[] array3 = array2;
								foreach (MWMCROptionAttribute mWMCROptionAttribute in array3)
								{
									list.Add(mWMCROptionAttribute.MWMCROption);
								}
							}
							if (nOJVMAttribute != null || lOGFILEAttribute != null || array2.Length != 0)
							{
								flag = true;
							}
						}
						string[] startupOptions = list.ToArray();
						InitializeApplication(startupOptions);
					}
					MCRAppInitialized = true;
				}
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
				string text = componentPath + "\\" + componentName + ".ctf";
				bool flag = false;
				if (componentName == null || componentPath == null || !File.Exists(text))
				{
					string @string = mcrResourceManager.GetString("MWErrorCTFFileNotFound");
					throw new FileNotFoundException(@string, text);
				}
				IntPtr functionPointerForDelegate = Marshal.GetFunctionPointerForDelegate(_OutDelegate);
				IntPtr functionPointerForDelegate2 = Marshal.GetFunctionPointerForDelegate(_ErrDelegate);
				int targetType = mclGetMCCTargetType((byte)(isLibrary ? 1 : 0));
				if (!mclInitializeComponentInstanceNonEmbeddedStandalone(out mcrInstance, componentPath, componentName, targetType, functionPointerForDelegate2, functionPointerForDelegate))
				{
					string string2 = mcrResourceManager.GetString("MWErrorMCRInitialize");
					string2 = string2 + "\n" + LastErrorMessage;
					throw new ApplicationException(string2);
				}
				mcrInstances.Enqueue(this);
			}
			catch (Exception innerException)
			{
				string string3 = mcrResourceManager.GetString("MWErrorMCRInitialize");
				throw new Exception(string3, innerException);
			}
			finally
			{
				Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
			}
			setBuilderUserData();
		}

		public MWMCR(string componentId, string componentPath, Stream embeddedCtfStream, bool isLibrary)
		{
			if (embeddedCtfStream == null)
			{
				string @string = mcrResourceManager.GetString("MWErrorCTFNotEmbedded");
				throw new ApplicationException(@string);
			}
			GCHandle gCHandle;
			try
			{
				Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
				IntPtr functionPointerForDelegate = Marshal.GetFunctionPointerForDelegate(_OutDelegate);
				IntPtr functionPointerForDelegate2 = Marshal.GetFunctionPointerForDelegate(_ErrDelegate);
				mclGetMCCTargetType((byte)(isLibrary ? 1 : 0));
				MWCTFStreamReader mWCTFStreamReader = new MWCTFStreamReader(embeddedCtfStream);
				gCHandle = GCHandle.Alloc(mWCTFStreamReader);
				bool flag = mclInitializeComponentInstanceWithCallbk(out mcrInstance, functionPointerForDelegate2, functionPointerForDelegate, mWCTFStreamReader.CtfStreamReadFcn, embeddedCtfStream.Length);
				mcrInstances.Enqueue(this);
				if (!flag)
				{
					string string2 = mcrResourceManager.GetString("MWErrorMCRInitialize");
					string2 = string2 + "\n" + LastErrorMessage;
					throw new ApplicationException(string2);
				}
			}
			catch (Exception innerException)
			{
				string string3 = mcrResourceManager.GetString("MWErrorMCRInitialize");
				throw new Exception(string3, innerException);
			}
			finally
			{
				Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
			}
			setBuilderUserData();
			try
			{
				gCHandle.Free();
			}
			catch
			{
			}
		}

		~MWMCR()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				try
				{
					Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
					disposed = true;
					if (MCRAppInitialized && IntPtr.Zero != mcrInstance)
					{
						if (!mclTerminateInstance(ref mcrInstance))
						{
							string @string = mcrResourceManager.GetString("MWErrorMCRTermination");
							throw new Exception(@string + "\n" + LastErrorMessage);
						}
						mcrInstance = IntPtr.Zero;
					}
				}
				finally
				{
					Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
				}
			}
		}

		private static bool InitializeApplication(params string[] startupOptions)
		{
			try
			{
				Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
				if (IsMCRInitialized())
				{
					string str = "MWMCR.InitializeApplication should be called before initializing a .NET Assembly component.\n";
					str += "MATLAB Runtime is already initialized with following options:\n";
					str += "JVM enabled : ";
					str += IsMCRJVMEnabled();
					str += "\nLogfile name : ";
					str += "\"";
					str += GetMCRLogFileName();
					str += "\"";
					throw new Exception(str);
				}
				if (!mclInitializeApplication(startupOptions, new IntPtr(startupOptions.Length)))
				{
					string str2 = "The MATLAB Runtime instance could not be initialized";
					string lastErrorMessage = LastErrorMessage;
					throw new Exception(str2 + "\n" + lastErrorMessage);
				}
				return true;
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
				if (MCRAppInitialized)
				{
					if (!mclTerminateApplication())
					{
						string @string = mcrResourceManager.GetString("MWErrorMCRTermination");
						@string += "\n";
						@string += LastErrorMessage;
						throw new Exception(@string);
					}
					MCRAppInitialized = false;
				}
				return true;
			}
			finally
			{
				Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
			}
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
				mclWaitForFiguresToDie(mcrInstance);
			}
			finally
			{
				Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
			}
		}

		private void setBuilderUserData()
		{
			MathWorks.MATLAB.NET.Arrays.MWArray mWArray = "builder";
			MathWorks.MATLAB.NET.Arrays.MWArray mWArray2 = "net";
			EvaluateFunction(0, "setmcruserdata", mWArray, mWArray2);
		}

		private MathWorks.MATLAB.NET.Arrays.MWArray[] EvaluateFunction(string functionName, int numArgsOut, int numArgsIn, MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn)
		{
			if (numArgsOut < 0)
			{
				string @string = mcrResourceManager.GetString("MWErrorNegativeArg");
				throw new ArgumentOutOfRangeException("numArgsOut", @string);
			}
			if (numArgsIn < 0)
			{
				string string2 = mcrResourceManager.GetString("MWErrorNegativeArg");
				throw new ArgumentOutOfRangeException("numArgsIn", string2);
			}
			if (argsIn == null)
			{
				if (numArgsIn != 0)
				{
					string string3 = mcrResourceManager.GetString("MWErrorEvalFunctionArg");
					throw new ArgumentOutOfRangeException("argsIn", string3);
				}
			}
			else
			{
				MathWorks.MATLAB.NET.Arrays.MWArray.formattedOutputString = new StringBuilder(1024);
				if (numArgsIn > argsIn.Length)
				{
					string string4 = mcrResourceManager.GetString("MWErrorEvalFunctionArg");
					throw new ArgumentOutOfRangeException("argsIn", string4);
				}
				foreach (MathWorks.MATLAB.NET.Arrays.MWArray mWArray in argsIn)
				{
					if (mWArray == null)
					{
						string string5 = mcrResourceManager.GetString("MWErrorInvalidNullArgument");
						throw new ArgumentNullException(string5, new Exception());
					}
					mWArray.CheckDisposed();
				}
			}
			if (numArgsIn > maxArgsIn)
			{
				maxArgsIn = numArgsIn;
				prhs = new IntPtr[maxArgsIn];
			}
			IntPtr[] array = new IntPtr[numArgsIn];
			if (numArgsOut > maxArgsOut)
			{
				maxArgsOut = numArgsOut;
				plhs = new IntPtr[maxArgsOut];
			}
			IntPtr stack = IntPtr.Zero;
			int num = 0;
			byte b = 1;
			WindowsIdentity current = WindowsIdentity.GetCurrent();
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				for (int j = 0; j < numArgsIn; j++)
				{
					if (argsIn[j].ArrayType == MathWorks.MATLAB.NET.Arrays.MWArrayType.NativeObjArray && !callOncePerAppDomain)
					{
						callOncePerAppDomain = true;
						MWCharArray mWCharArray = new MWCharArray("x=System.String('');clear x");
						mclFeval(mcrInstance, "eval", 0, null, 1, new IntPtr[1]
						{
							mWCharArray.MXArrayHandle.DangerousGetHandle()
						});
					}
					prhs[j] = argsIn[j].MXArrayHandle.DangerousGetHandle();
					if (argsIn[j].ArrayType == MathWorks.MATLAB.NET.Arrays.MWArrayType.NativeObjArray)
					{
						array[j] = prhs[j];
					}
				}
				b = ((current != null) ? mclImpersonationFeval(mcrInstance, functionName, numArgsOut, plhs, numArgsIn, prhs, current.Token) : mclFeval(mcrInstance, functionName, numArgsOut, plhs, numArgsIn, prhs));
				if (b != 0)
				{
					argsOut = new MathWorks.MATLAB.NET.Arrays.MWArray[numArgsOut];
					for (int k = 0; k < numArgsOut; k++)
					{
						argsOut[k] = MathWorks.MATLAB.NET.Arrays.MWArray.GetTypedArray(new MWSafeHandle(plhs[k]));
					}
					return argsOut;
				}
				return null;
			}
			finally
			{
				IntPtr[] array2 = array;
				foreach (IntPtr intPtr in array2)
				{
					if (intPtr != IntPtr.Zero)
					{
						mclMxDestroyArray(intPtr, onInterpreterThread: true);
					}
				}
				if (current != null)
				{
					stopImpersonationOnMCRThread();
				}
				if (b == 0)
				{
					string lastErrorMessage = LastErrorMessage;
					lastErrorMessage = ((string.Empty == lastErrorMessage) ? "segv - SEVERE ERROR" : lastErrorMessage);
					string mlError = "\n\n... MWMCR::EvaluateFunction error ... \n" + lastErrorMessage + ".";
					string[] array3 = null;
					num = mclGetStackTrace(ref stack);
					if (num > 0 && stack != IntPtr.Zero)
					{
						IntPtr[] array4 = new IntPtr[num];
						Marshal.Copy(stack, array4, 0, num);
						array3 = new string[num];
						for (int m = 0; m < num; m++)
						{
							array3[m] = Marshal.PtrToStringAnsi(array4[m]);
						}
						mclFreeStackTrace(ref stack, num);
					}
					string message = combineErrorMessages(mlError, array3);
					throw new Exception(message);
				}
			}
		}

		private string combineErrorMessages(string mlError, string[] mlStack)
		{
			string text = mlError + "\n\n";
			if (mlStack != null)
			{
				text += "... Matlab M-code Stack Trace ...\n";
				foreach (string str in mlStack)
				{
					text += "    at\n";
					text += str;
					text += "\n";
				}
				text += "\n";
			}
			return text;
		}

		private MathWorks.MATLAB.NET.Arrays.MWArray[] UnpackArgArray(object[] argArray)
		{
			if (argArray == null)
			{
				return null;
			}
			int num = argArray.Length;
			object[] array = null;
			if (num > 0 && typeof(object[]) == argArray[num - 1].GetType())
			{
				array = (argArray[num - 1] as object[]);
			}
			int num2 = (array != null) ? (num + array.Length - 1) : num;
			int num3 = (array == null) ? num : (num - 1);
			MathWorks.MATLAB.NET.Arrays.MWArray[] array2 = new MathWorks.MATLAB.NET.Arrays.MWArray[num2];
			int num4 = 0;
			for (num4 = 0; num4 < num3; num4++)
			{
				array2[num4] = MathWorks.MATLAB.NET.Arrays.MWArray.ConvertObjectToMWArray(argArray[num4]);
			}
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array2[num4] = MathWorks.MATLAB.NET.Arrays.MWArray.ConvertObjectToMWArray(array[i]);
					num4++;
				}
			}
			return array2;
		}

		private MathWorks.MATLAB.NET.Arrays.MWArray[] UnpackArgArray(object[] argArray, object[] varArgArray)
		{
			int num = argArray.Length + varArgArray.Length;
			MathWorks.MATLAB.NET.Arrays.MWArray[] array = new MathWorks.MATLAB.NET.Arrays.MWArray[num];
			int num2 = 0;
			foreach (object objIn in argArray)
			{
				array[num2++] = MathWorks.MATLAB.NET.Arrays.MWArray.ConvertObjectToMWArray(objIn);
			}
			foreach (object objIn2 in varArgArray)
			{
				array[num2++] = MathWorks.MATLAB.NET.Arrays.MWArray.ConvertObjectToMWArray(objIn2);
			}
			return array;
		}

		public object EvaluateFunction(string functionName, params object[] argsIn)
		{
			try
			{
				Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
				MathWorks.MATLAB.NET.Arrays.MWArray[] array = UnpackArgArray(argsIn);
				int numArgsIn = (array != null) ? array.Length : 0;
				MathWorks.MATLAB.NET.Arrays.MWArray[] array2 = EvaluateFunction(functionName, 1, numArgsIn, array);
				MathWorks.MATLAB.NET.Arrays.MWArray mWArray = array2[0];
				object result = null;
				if (mWArray != null)
				{
					result = marshalMWArrayToObject(mWArray);
				}
				return result;
			}
			finally
			{
				Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
			}
		}

		public MathWorks.MATLAB.NET.Arrays.MWArray EvaluateFunction(string functionName, params MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn)
		{
			try
			{
				Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
				int numArgsIn = (argsIn != null) ? argsIn.Length : 0;
				MathWorks.MATLAB.NET.Arrays.MWArray[] array = EvaluateFunction(functionName, 1, numArgsIn, argsIn);
				return (array != null) ? array[0] : null;
			}
			finally
			{
				Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
			}
		}

		public MathWorks.MATLAB.NET.Arrays.MWArray[] EvaluateFunction(string functionName, int numArgsOut, MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn)
		{
			try
			{
				Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
				int numArgsIn = (argsIn != null) ? argsIn.Length : 0;
				return EvaluateFunction(functionName, numArgsOut, numArgsIn, argsIn);
			}
			finally
			{
				Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
			}
		}

		public MathWorks.MATLAB.NET.Arrays.MWArray[] EvaluateFunction(int numArgsOut, string functionName, params MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn)
		{
			try
			{
				Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
				int numArgsIn = (argsIn != null) ? argsIn.Length : 0;
				return EvaluateFunction(functionName, numArgsOut, numArgsIn, argsIn);
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
				MathWorks.MATLAB.NET.Arrays.MWArray[] array = UnpackArgArray(argsIn);
				int numArgsIn = (array != null) ? array.Length : 0;
				MathWorks.MATLAB.NET.Arrays.MWArray[] array2 = EvaluateFunction(functionName, numArgsOut, numArgsIn, array);
				object[] array3 = new object[numArgsOut];
				for (int i = 0; i < numArgsOut; i++)
				{
					array3[i] = marshalMWArrayToObject(array2[i]);
				}
				return array3;
			}
			finally
			{
				Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
			}
		}

		public void EvaluateFunction(string functionName, int numArgsOut, ref MathWorks.MATLAB.NET.Arrays.MWArray[] argsOut, MathWorks.MATLAB.NET.Arrays.MWArray[] argsIn)
		{
			try
			{
				Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
				int numArgsIn = (argsIn != null) ? argsIn.Length : 0;
				argsOut = EvaluateFunction(functionName, numArgsOut, numArgsIn, argsIn);
			}
			finally
			{
				Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
			}
		}

		public void EvaluateFunctionForTypeSafeCall(string functionName, int numArgsOut, ref object[] argsOut, object[] argsIn, params object[] varArgsIn)
		{
			try
			{
				Monitor.Enter(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
				MathWorks.MATLAB.NET.Arrays.MWArray[] array = UnpackArgArray(argsIn, varArgsIn);
				int numArgsIn = (array != null) ? array.Length : 0;
				MathWorks.MATLAB.NET.Arrays.MWArray[] array2 = EvaluateFunction(functionName, numArgsOut, numArgsIn, array);
				argsOut = new object[numArgsOut];
				for (int i = 0; i < numArgsOut; i++)
				{
					argsOut[i] = array2[i];
				}
			}
			finally
			{
				Monitor.Exit(MathWorks.MATLAB.NET.Arrays.MWArray.mxSync);
			}
		}

		private object marshalMWArrayToObject(MathWorks.MATLAB.NET.Arrays.MWArray in_arg)
		{
			switch (in_arg.ArrayType)
			{
			case MathWorks.MATLAB.NET.Arrays.MWArrayType.Structure:
			{
				MathWorks.MATLAB.NET.Arrays.MWStructArray mWStructArray = (MathWorks.MATLAB.NET.Arrays.MWStructArray)in_arg;
				MathWorks.MATLAB.NET.Arrays.native.MWStructArray mWStructArray2 = new MathWorks.MATLAB.NET.Arrays.native.MWStructArray(mWStructArray.Dimensions, mWStructArray.FieldNames);
				for (int j = 1; j <= mWStructArray.NumberOfElements; j++)
				{
					for (int k = 0; k < mWStructArray.NumberOfFields; k++)
					{
						MathWorks.MATLAB.NET.Arrays.MWArray mWArray2 = mWStructArray[mWStructArray.FieldNames[k], new int[1]
						{
							j
						}];
						if (mWArray2 != null)
						{
							object value2 = marshalMWArrayToObject(mWArray2);
							mWStructArray2[mWStructArray.FieldNames[k], new int[1]
							{
								j
							}] = value2;
						}
					}
				}
				return mWStructArray2;
			}
			case MathWorks.MATLAB.NET.Arrays.MWArrayType.Cell:
			{
				MathWorks.MATLAB.NET.Arrays.MWCellArray mWCellArray = (MathWorks.MATLAB.NET.Arrays.MWCellArray)in_arg;
				MathWorks.MATLAB.NET.Arrays.native.MWCellArray mWCellArray2 = new MathWorks.MATLAB.NET.Arrays.native.MWCellArray(mWCellArray.Dimensions);
				for (int i = 1; i <= mWCellArray.NumberOfElements; i++)
				{
					MathWorks.MATLAB.NET.Arrays.MWArray mWArray = mWCellArray[new int[1]
					{
						i
					}];
					if (mWArray != null)
					{
						object value = marshalMWArrayToObject(mWArray);
						mWCellArray2[new int[1]
						{
							i
						}] = value;
					}
				}
				return mWCellArray2;
			}
			default:
				return in_arg.ToArray();
			}
		}

		private static void unloadAppDomain(object sender, EventArgs e)
		{
			while (mcrInstances.Count > 0)
			{
				((MWMCR)mcrInstances.Dequeue()).Dispose();
			}
			gchOut.Free();
			gchErr.Free();
		}

		public static string GetMCRLogFileName()
		{
			IntPtr ptr = mclGetLogFileName();
			return Marshal.PtrToStringAnsi(ptr);
		}

		public static bool IsMCRInitialized()
		{
			return mclIsMCRInitialized();
		}

		public static bool IsMCRJVMEnabled()
		{
			return mclIsJVMEnabled();
		}
	}
}
