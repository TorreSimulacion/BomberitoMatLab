// Decompiled with JetBrains decompiler
// Type: MathWorks.MATLAB.NET.Utility.Images
// Assembly: MWArray, Version=2.19.0.0, Culture=neutral, PublicKeyToken=e1d84a0da19db86f
// MVID: B64B92B2-5671-4E6B-9407-FF93C672D333
// Assembly location: C:\Users\APAAPG\Documents\Visual Studio 2017\Projects\TestML\MWArray.dll

using MathWorks.MATLAB.NET.Arrays;
using System.Drawing;
using System.Drawing.Imaging;

namespace MathWorks.MATLAB.NET.Utility
{
  public class Images
  {
    public static Image renderArrayData(MWArray rgbData)
    {
      byte[,,] array = (byte[,,]) ((MWNumericArray) rgbData).ToArray(MWArrayComponent.Real);
      int length1 = array.GetLength(1);
      int length2 = array.GetLength(2);
      Bitmap bitmap = new Bitmap(length2, length1, PixelFormat.Format24bppRgb);
      for (int y = 0; y < length1; ++y)
      {
        for (int x = 0; x < length2; ++x)
        {
          Color color = Color.FromArgb((int) array[0, y, x], (int) array[1, y, x], (int) array[2, y, x]);
          bitmap.SetPixel(x, y, color);
        }
      }
      return (Image) bitmap;
    }
  }
}
