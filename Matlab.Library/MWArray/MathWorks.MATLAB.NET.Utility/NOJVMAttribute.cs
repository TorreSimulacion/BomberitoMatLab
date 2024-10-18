using System;

namespace MathWorks.MATLAB.NET.Utility
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
	public class NOJVMAttribute : Attribute
	{
		private bool disableJVM;

		public bool JVMDisabled => disableJVM;

		public NOJVMAttribute(bool JVMDisable)
		{
			disableJVM = JVMDisable;
		}
	}
}
