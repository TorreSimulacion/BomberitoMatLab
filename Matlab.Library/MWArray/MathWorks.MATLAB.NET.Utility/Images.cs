using MathWorks.MATLAB.NET.Arrays;
using System.Drawing;
using System.Drawing.Imaging;

namespace MathWorks.MATLAB.NET.Utility
{
	public class Images
	{
		public static Image renderArrayData(MWArray rgbData)
		{
			MWNumericArray mWNumericArray = (MWNumericArray)rgbData;
			byte[,,] array = (byte[,,])mWNumericArray.ToArray(MWArrayComponent.Real);
			int length = array.GetLength(1);
			int length2 = array.GetLength(2);
			Bitmap bitmap = new Bitmap(length2, length, PixelFormat.Format24bppRgb);
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					int red = array[0, i, j];
					int green = array[1, i, j];
					int blue = array[2, i, j];
					Color color = Color.FromArgb(red, green, blue);
					bitmap.SetPixel(j, i, color);
				}
			}
			return bitmap;
		}
	}
}
