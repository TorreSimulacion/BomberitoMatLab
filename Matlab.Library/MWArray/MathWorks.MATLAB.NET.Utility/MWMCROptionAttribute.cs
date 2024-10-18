using System;

namespace MathWorks.MATLAB.NET.Utility
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
	public class MWMCROptionAttribute : Attribute
	{
		private string option;

		public string MWMCROption => option;

		public MWMCROptionAttribute(string OptVal)
		{
			option = OptVal;
		}
	}
}
