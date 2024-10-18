using System;

namespace MathWorks.MATLAB.NET.Utility
{
	internal class PlatformInfo
	{
		public static string getArchDir()
		{
			if (Is32BitProcess())
			{
				return "win32";
			}
			if (Is64BitProcess())
			{
				return "win64";
			}
			throw new Exception("Unsupported Windows platform");
		}

		public static bool Is32BitProcess()
		{
			return IntPtr.Size == 4;
		}

		public static bool Is64BitProcess()
		{
			return IntPtr.Size == 8;
		}
	}
}
