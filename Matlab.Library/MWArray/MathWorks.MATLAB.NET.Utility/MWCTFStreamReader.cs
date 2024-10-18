using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MathWorks.MATLAB.NET.Utility
{
	public class MWCTFStreamReader
	{
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int ReadCtfStreamDelegate(IntPtr ctfByte, int size);

		private BinaryReader ctfStreamReader;

		private IntPtr readCtfStreamFcn;

		private ReadCtfStreamDelegate _readCtfStream;

		public IntPtr CtfStreamReadFcn => readCtfStreamFcn;

		public MWCTFStreamReader(Stream embeddedCtfStream)
		{
			embeddedCtfStream.Position = 0L;
			_readCtfStream = readCtfStream;
			readCtfStreamFcn = Marshal.GetFunctionPointerForDelegate(_readCtfStream);
			ctfStreamReader = new BinaryReader(embeddedCtfStream);
		}

		internal int readCtfStream(IntPtr ctfByte, int readSize)
		{
			try
			{
				byte[] array = ctfStreamReader.ReadBytes(readSize);
				Marshal.Copy(array, 0, ctfByte, array.Length);
				return array.Length;
			}
			catch (EndOfStreamException)
			{
				return -1;
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
		}
	}
}
