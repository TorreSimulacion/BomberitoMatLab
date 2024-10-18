using System.Reflection;
using System.Resources;

namespace MathWorks.MATLAB.NET.Arrays
{
	internal class MWResources
	{
		private static ResourceManager resourceManager;

		static MWResources()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames[0];
			text = text.TrimEnd("resources".ToCharArray());
			text = text.TrimEnd(".".ToCharArray());
			resourceManager = new ResourceManager(text, executingAssembly);
		}

		public static ResourceManager getResourceManager()
		{
			return resourceManager;
		}
	}
}
