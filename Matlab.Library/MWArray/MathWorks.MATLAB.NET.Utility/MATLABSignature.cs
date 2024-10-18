using System;

namespace MathWorks.MATLAB.NET.Utility
{
	[AttributeUsage(AttributeTargets.Method)]
	public class MATLABSignature : Attribute
	{
		public readonly string Name;

		public readonly int Inputs;

		public readonly int Outputs;

		public readonly bool HasVarArgIn;

		public MATLABSignature(string name, int inputs, int outputs, int hasvarargin)
		{
			Name = name;
			Inputs = inputs;
			Outputs = outputs;
			HasVarArgIn = Convert.ToBoolean(hasvarargin);
		}
	}
}
