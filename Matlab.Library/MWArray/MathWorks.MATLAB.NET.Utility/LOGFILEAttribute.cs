using System;

namespace MathWorks.MATLAB.NET.Utility
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	public class LOGFILEAttribute : Attribute
	{
		private string fName;

		public string LogfileName => fName;

		public LOGFILEAttribute(string fileName)
		{
			string environmentVariable = Environment.GetEnvironmentVariable("MW_LOGFILE");
			if (environmentVariable != null)
			{
				fName = environmentVariable;
			}
			else
			{
				fName = fileName;
			}
		}
	}
}
