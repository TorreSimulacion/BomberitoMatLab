using System;

namespace MathWorks.MATLAB.NET.Utility
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	public class MCRVersion : Attribute
	{
		public readonly string Major;

		public readonly string Minor;

		public readonly string Update;

		public MCRVersion(string major, string minor, string update)
		{
			Major = major;
			Minor = minor;
			Update = update;
		}
	}
}
