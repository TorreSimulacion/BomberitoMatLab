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
		private static Assembly dotnetcliAssembly;

		private static Type SafeNETCliConversions;

		private static MethodInfo GetMxArrayFromObjectPtr;

		private static MethodInfo GetObjectFromMxArrayPtr;

		private object _object;

		public new object this[params int[] indices]
		{
			get
			{
				if (2 < indices.Length)
				{
					string @string = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
					throw new ArgumentOutOfRangeException(@string);
				}
				foreach (int num in indices)
				{
					if (1 != num)
					{
						string string2 = MWArray.resourceManager.GetString("MWErrorInvalidIndex");
						throw new ArgumentOutOfRangeException(string2);
					}
				}
				return _object;
			}
			set
			{
				if (2 < indices.Length)
				{
					string @string = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
					throw new ArgumentOutOfRangeException(@string);
				}
				foreach (int num in indices)
				{
					if (1 != num)
					{
						string string2 = MWArray.resourceManager.GetString("MWErrorInvalidIndex");
						throw new ArgumentOutOfRangeException(string2);
					}
				}
				_object = value;
			}
		}

		public override bool IsDisposed => null == _object;

		public override bool IsEmpty
		{
			get
			{
				CheckDisposed();
				return Empty == this;
			}
		}

		internal override MWSafeHandle MXArrayHandle
		{
			get
			{
				try
				{
					Monitor.Enter(MWArray.mxSync);
					return new MWSafeHandle((IntPtr)GetMxArrayFromObjectPtr.Invoke(null, new object[1]
					{
						_object
					}), ownsHandle: false);
				}
				finally
				{
					Monitor.Exit(MWArray.mxSync);
				}
			}
		}

		public static MWObjectArray Empty => new MWObjectArray();

		public object Object
		{
			get
			{
				return _object;
			}
			set
			{
				_object = value;
			}
		}

		static MWObjectArray()
		{
			dotnetcliAssembly = Assembly.LoadFrom(getPathToDotnetcli());
			AppDomain.CurrentDomain.AssemblyResolve += DotnetcliResolveEventHandler;
			SafeNETCliConversions = dotnetcliAssembly.GetType("dotnetcli.DeployedDataConversion");
			GetMxArrayFromObjectPtr = SafeNETCliConversions.GetMethod("GetMxArrayFromObject", BindingFlags.Static | BindingFlags.Public);
			GetObjectFromMxArrayPtr = SafeNETCliConversions.GetMethod("GetObjectFromMxArray", BindingFlags.Static | BindingFlags.Public);
		}

		internal MWObjectArray(MWSafeHandle hMXArray)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.Enter(MWArray.mxSync);
				IntPtr intPtr = hMXArray.DangerousGetHandle();
				_object = GetObjectFromMxArrayPtr.Invoke(null, new object[1]
				{
					intPtr
				});
				array_Type = MWArrayType.NativeObjArray;
			}
			finally
			{
				Monitor.Exit(MWArray.mxSync);
			}
		}

		public MWObjectArray()
		{
			_object = new object();
			array_Type = MWArrayType.NativeObjArray;
		}

		public MWObjectArray(object obj)
		{
			if (obj == null)
			{
				string @string = MWArray.resourceManager.GetString("MWErrorInvalidNullArgument");
				throw new ArgumentNullException("obj", @string);
			}
			_object = obj;
			array_Type = MWArrayType.NativeObjArray;
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
			}
			finally
			{
				_object = null;
			}
		}

		public static bool operator ==(MWObjectArray objectArrayA, MWObjectArray objectArrayB)
		{
			return objectArrayA?.Equals(objectArrayB) ?? ((object)objectArrayB == null);
		}

		public static bool operator !=(MWObjectArray objectArrayA, MWObjectArray objectArrayB)
		{
			if ((object)objectArrayA == null)
			{
				return (object)objectArrayB != null;
			}
			return !objectArrayA.Equals(objectArrayB);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is MWObjectArray))
			{
				return false;
			}
			return Equals(obj as MWObjectArray);
		}

		public bool Equals(MWObjectArray obj)
		{
			if ((object)obj == null)
			{
				return false;
			}
			return object.ReferenceEquals(_object, obj._object);
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			ISerializable serializable = _object as ISerializable;
			if (serializable == null)
			{
				string @string = MWArray.resourceManager.GetString("MWNotSerializeable");
				throw new InvalidCastException(@string);
			}
			info.AddValue("nativeObject", _object, _object.GetType());
		}

		protected MWObjectArray(SerializationInfo serializationInfo, StreamingContext context)
		{
			_object = new object();
			_object = serializationInfo.GetValue("nativeObject", _object.GetType());
			array_Type = MWArrayType.NativeObjArray;
		}

		public override object Clone()
		{
			CheckDisposed();
			MWObjectArray mWObjectArray = (MWObjectArray)MemberwiseClone();
			if (!_object.GetType().IsValueType)
			{
				ICloneable cloneable = _object as ICloneable;
				if (cloneable != null)
				{
					mWObjectArray.Object = cloneable.Clone();
				}
			}
			return mWObjectArray;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			CheckDisposed();
			string text = _object.ToString();
			char c = '\n';
			char c2 = ' ';
			text = text.TrimStart(c);
			text = text.TrimEnd(c);
			if (-1 == text.IndexOf(c))
			{
				text = text.TrimStart(c2);
				text = text.TrimEnd(c2);
			}
			return text;
		}

		internal override object ConvertToType(Type t)
		{
			if (t == Object.GetType() || t.IsAssignableFrom(Object.GetType()))
			{
				return Object;
			}
			string @string = MWArray.resourceManager.GetString("MWErrorInvalidDataConversion");
			string message = "Cannot convert from MWObjectArray containing object of type " + Object.GetType().FullName + " to " + t.FullName;
			throw new ArgumentException(@string, new ApplicationException(message));
		}

		private static string getPathToDotnetcli()
		{
			string matlabRootForAssembly = getMatlabRootForAssembly();
			if (matlabRootForAssembly.Length > 0)
			{
				string archDir = PlatformInfo.getArchDir();
				string path = Path.Combine(matlabRootForAssembly, Path.Combine("bin", archDir));
				return Path.Combine(path, "dotnetcli.dll");
			}
			string message = "Failed to find mclmcrrt9_1.dll required to load dotnetcli on system path : \n" + Environment.GetEnvironmentVariable("path");
			throw new Exception(message);
		}

		private static string getMatlabRootForAssembly()
		{
			string text = "\"";
			string result = "";
			string path = "mclmcrrt9_1.dll";
			string environmentVariable = Environment.GetEnvironmentVariable("path");
			string[] array = environmentVariable.Split(';');
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					string text2 = (!array[i].Contains(text)) ? Path.Combine(array[i], path) : Path.Combine(array[i].Trim(text.ToCharArray()), path);
					string archDir = PlatformInfo.getArchDir();
					if (File.Exists(text2) && text2.Contains(Path.Combine("runtime", archDir)))
					{
						result = text2.Substring(0, text2.LastIndexOf("runtime"));
						return result;
					}
				}
				catch (ArgumentException)
				{
				}
			}
			return result;
		}

		private static Assembly DotnetcliResolveEventHandler(object sender, ResolveEventArgs args)
		{
			return dotnetcliAssembly;
		}
	}
}
