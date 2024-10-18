using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Resources;

namespace MathWorks.MATLAB.NET.Utility
{
	internal class JaggedArray
	{
		private Array array_;

		internal static ResourceManager resourceManager;

		public Array ArrayData => array_;

		static JaggedArray()
		{
			resourceManager = MWResources.getResourceManager();
		}

		public JaggedArray(Array array)
		{
			if (IsJagged(array) && IsRectangular(array))
			{
				array_ = array;
				return;
			}
			throw new ArgumentException(resourceManager.GetString("MWErrorNotRectangularJaggedArray"));
		}

		private static bool IsRectangular(Array array)
		{
			return IsRectangular(array, GetRank(array));
		}

		private static bool IsRectangular(Array array, int rank)
		{
			if (rank > 1)
			{
				int length = array.Length;
				int length2 = ((Array)array.GetValue(0)).Length;
				for (int i = 1; i < length; i++)
				{
					if (((Array)array.GetValue(i)).Length != length2)
					{
						return false;
					}
				}
				for (int j = 0; j < length; j++)
				{
					if (!IsRectangular((Array)array.GetValue(j), rank - 1))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		public int GetRank()
		{
			return GetRank(array_);
		}

		private static int GetRank(Array array)
		{
			if (!array.GetType().GetElementType().IsArray)
			{
				return 1;
			}
			return 1 + GetRank((Array)array.GetValue(0));
		}

		public static bool IsJagged(Array array)
		{
			return array.GetType().GetElementType().IsArray;
		}

		public static int GetRank(Type t)
		{
			if (!t.IsArray)
			{
				return 0;
			}
			return 1 + GetRank(t.GetElementType());
		}

		public static bool IsJagged(Type t)
		{
			return t.GetElementType().IsArray;
		}

		public static Type GetElementType(Type t)
		{
			int rank = GetRank(t);
			for (int i = 0; i < rank; i++)
			{
				t = t.GetElementType();
			}
			return t;
		}

		private int GetDimension(int dim)
		{
			if (dim < 0 || dim >= GetRank())
			{
				throw new IndexOutOfRangeException("Dimension:" + dim + " does not exist in array.");
			}
			Array array = array_;
			for (int i = 0; i < dim; i++)
			{
				array = (Array)array.GetValue(0);
			}
			return array.Length;
		}

		public int[] GetDimensions()
		{
			int rank = GetRank();
			int[] array = new int[rank];
			for (int i = 0; i < rank; i++)
			{
				array[i] = GetDimension(i);
			}
			return array;
		}

		public Array GetFlatArray()
		{
			int rank = GetRank();
			GetElementType();
			switch (rank)
			{
			case 2:
				return GetFlatArrayFrom2D(this);
			case 3:
				return GetFlatArrayFrom3D(this);
			default:
				return GetFlatArrayFromND(this);
			}
		}

		private Array GetFlatArrayFromND(JaggedArray src)
		{
			int rank = src.GetRank();
			if (rank == 3)
			{
				return GetFlatArrayFrom3D(src);
			}
			int dimension = src.GetDimension(0);
			Array array = Array.CreateInstance(src.GetElementType().MakeArrayType(), src.ArrayData.Length);
			for (int i = 0; i < dimension; i++)
			{
				array.SetValue(GetFlatArrayFromND(new JaggedArray((Array)src.ArrayData.GetValue(i))), i);
			}
			return Stitch1DArrays(array);
		}

		private Array GetFlatArrayFrom2D(JaggedArray src)
		{
			int[] dimensions = new int[2]
			{
				src.GetDimension(0),
				src.GetDimension(1)
			};
			Type elementType = src.GetElementType();
			if (elementType == typeof(byte))
			{
				return Flatten2D((byte[][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(short))
			{
				return Flatten2D((short[][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(int))
			{
				return Flatten2D((int[][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(long))
			{
				return Flatten2D((long[][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(float))
			{
				return Flatten2D((float[][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(double))
			{
				return Flatten2D((double[][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(char))
			{
				return Flatten2D((char[][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(bool))
			{
				return Flatten2D((bool[][])src.ArrayData, dimensions);
			}
			throw new ArgumentException(resourceManager.GetString("MWErrorUnsupportedArrayType") + elementType.FullName);
		}

		private Array GetFlatArrayFrom3D(JaggedArray src)
		{
			int[] dimensions = new int[3]
			{
				src.GetDimension(0),
				src.GetDimension(1),
				src.GetDimension(2)
			};
			Type elementType = src.GetElementType();
			if (elementType == typeof(byte))
			{
				return Flatten3D((byte[][][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(short))
			{
				return Flatten3D((short[][][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(int))
			{
				return Flatten3D((int[][][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(long))
			{
				return Flatten3D((long[][][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(float))
			{
				return Flatten3D((float[][][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(double))
			{
				return Flatten3D((double[][][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(char))
			{
				return Flatten3D((char[][][])src.ArrayData, dimensions);
			}
			if (elementType == typeof(bool))
			{
				return Flatten3D((bool[][][])src.ArrayData, dimensions);
			}
			throw new ArgumentException(resourceManager.GetString("MWErrorUnsupportedArrayType") + elementType.FullName);
		}

		private T[] Flatten2D<T>(T[][] src, int[] dimensions)
		{
			T[] array = new T[dimensions[0] * dimensions[1]];
			int num = 0;
			for (int i = 0; i < dimensions[1]; i++)
			{
				for (int j = 0; j < dimensions[0]; j++)
				{
					array[num++] = src[j][i];
				}
			}
			return array;
		}

		private T[] Flatten3D<T>(T[][][] src, int[] dimensions)
		{
			T[] array = new T[dimensions[0] * dimensions[1] * dimensions[2]];
			int num = 0;
			for (int i = 0; i < dimensions[0]; i++)
			{
				for (int j = 0; j < dimensions[2]; j++)
				{
					for (int k = 0; k < dimensions[1]; k++)
					{
						array[num++] = src[i][k][j];
					}
				}
			}
			return array;
		}

		private static Array Stitch1DArrays(Array src)
		{
			Type type = src.GetType();
			if (type == typeof(byte[][]))
			{
				return Stitch((byte[][])src);
			}
			if (type == typeof(short[][]))
			{
				return Stitch((byte[][])src);
			}
			if (type == typeof(int[][]))
			{
				return Stitch((int[][])src);
			}
			if (type == typeof(long[][]))
			{
				return Stitch((long[][])src);
			}
			if (type == typeof(float))
			{
				return Stitch((float[][])src);
			}
			if (type == typeof(double))
			{
				return Stitch((double[][])src);
			}
			if (type == typeof(char))
			{
				return Stitch((char[][])src);
			}
			if (type == typeof(bool))
			{
				return Stitch((bool[][])src);
			}
			throw new ArgumentException(resourceManager.GetString("MWErrorUnsupportedArrayType") + type.FullName);
		}

		private static T[] Stitch<T>(T[][] src)
		{
			int num = 0;
			foreach (T[] array in src)
			{
				num += array.Length;
			}
			T[] array2 = new T[num];
			int num2 = 0;
			foreach (T[] array3 in src)
			{
				array3.CopyTo(array2, num2);
				num2 += array3.Length;
			}
			return array2;
		}

		public Type GetElementType()
		{
			Type type = array_.GetType();
			int rank = GetRank();
			for (int i = 0; i < rank; i++)
			{
				type = type.GetElementType();
			}
			return type;
		}

		public static Array GetJaggedArrayFromFlatArray(Array flatArray, int[] dimensions)
		{
			switch (dimensions.LongLength)
			{
			case 2L:
				return Get2DJaggedArray(flatArray, dimensions[0], dimensions[1]);
			case 3L:
				return Get3DJaggedArray(flatArray, dimensions[1], dimensions[2], dimensions[0], 0);
			default:
				return GetNDJaggedArray(flatArray, dimensions, 0, 0, dimensions[1] * dimensions[2] * dimensions[0]);
			}
		}

		private static Array GetNDJaggedArray(Array flatArray, int[] dimensions, int currentDim, int startingOffset, int pageStride)
		{
			int num = dimensions.Length - currentDim;
			if (num == 3)
			{
				return Get3DJaggedArray(flatArray, dimensions[currentDim + 1], dimensions[currentDim + 2], dimensions[currentDim], startingOffset);
			}
			Type elementType = MakeJaggedArrayType(flatArray.GetType().GetElementType(), num);
			int num2 = dimensions[currentDim];
			Array array = Array.CreateInstance(elementType, num2);
			for (int i = 0; i < num2; i++)
			{
				Array nDJaggedArray = GetNDJaggedArray(flatArray, dimensions, currentDim + 1, startingOffset + i * pageStride, pageStride);
				array.SetValue(nDJaggedArray, i);
			}
			return array;
		}

		private static Array Get2DJaggedArray(Array flatArray, int row, int col)
		{
			Type elementType = flatArray.GetType().GetElementType();
			if (elementType == typeof(byte))
			{
				return Unflatten2D((byte[])flatArray, row, col);
			}
			if (elementType == typeof(short))
			{
				return Unflatten2D((short[])flatArray, row, col);
			}
			if (elementType == typeof(int))
			{
				return Unflatten2D((int[])flatArray, row, col);
			}
			if (elementType == typeof(long))
			{
				return Unflatten2D((long[])flatArray, row, col);
			}
			if (elementType == typeof(float))
			{
				return Unflatten2D((float[])flatArray, row, col);
			}
			if (elementType == typeof(double))
			{
				return Unflatten2D((double[])flatArray, row, col);
			}
			if (elementType == typeof(char))
			{
				return Unflatten2D((char[])flatArray, row, col);
			}
			if (elementType == typeof(bool))
			{
				return Unflatten2D((bool[])flatArray, row, col);
			}
			throw new ArgumentException(resourceManager.GetString("MWErrorUnsupportedArrayType") + elementType.FullName);
		}

		private static T[][] Unflatten2D<T>(T[] flatArray, int row, int col)
		{
			T[][] array = new T[row][];
			for (int i = 0; i < row; i++)
			{
				array[i] = new T[col];
			}
			for (int j = 0; j < row; j++)
			{
				for (int k = 0; k < col; k++)
				{
					array[j][k] = flatArray[k * row + j];
				}
			}
			return array;
		}

		private static Array Get3DJaggedArray(Array flatArray, int row, int col, int page, int startingOffset)
		{
			Type elementType = flatArray.GetType().GetElementType();
			if (elementType == typeof(byte))
			{
				return Unflatten3D((byte[])flatArray, row, col, page, startingOffset);
			}
			if (elementType == typeof(short))
			{
				return Unflatten3D((short[])flatArray, row, col, page, startingOffset);
			}
			if (elementType == typeof(int))
			{
				return Unflatten3D((int[])flatArray, row, col, page, startingOffset);
			}
			if (elementType == typeof(long))
			{
				return Unflatten3D((long[])flatArray, row, col, page, startingOffset);
			}
			if (elementType == typeof(float))
			{
				return Unflatten3D((float[])flatArray, row, col, page, startingOffset);
			}
			if (elementType == typeof(double))
			{
				return Unflatten3D((double[])flatArray, row, col, page, startingOffset);
			}
			if (elementType == typeof(char))
			{
				return Unflatten3D((char[])flatArray, row, col, page, startingOffset);
			}
			if (elementType == typeof(bool))
			{
				return Unflatten3D((bool[])flatArray, row, col, page, startingOffset);
			}
			throw new ArgumentException(resourceManager.GetString("MWErrorUnsupportedArrayType") + elementType.FullName);
		}

		private static T[][][] Unflatten3D<T>(T[] flatArray, int row, int col, int page, int startingOffset)
		{
			T[][][] array = new T[page][][];
			for (int i = 0; i < page; i++)
			{
				array[i] = new T[row][];
				for (int j = 0; j < row; j++)
				{
					array[i][j] = new T[col];
				}
			}
			int num = row * col;
			int num2 = 0;
			int num3 = 0;
			while (num3 < page)
			{
				for (int k = 0; k < row; k++)
				{
					for (int l = 0; l < col; l++)
					{
						array[num3][k][l] = flatArray[l * row + k + num2 + startingOffset];
					}
				}
				num3++;
				num2 += num;
			}
			return array;
		}

		private static Array CreateJaggedArray(Type t, int[] dimensions, int currentDim)
		{
			int num = dimensions.Length;
			int num2 = dimensions[currentDim];
			if (num - currentDim > 1)
			{
				Array array = Array.CreateInstance(MakeJaggedArrayType(t, num - currentDim), num2);
				for (int i = 0; i < num2; i++)
				{
					array.SetValue(CreateJaggedArray(t, dimensions, currentDim + 1), i);
				}
				return array;
			}
			return Array.CreateInstance(t, num2);
		}

		private static Type MakeJaggedArrayType(Type t, int rank)
		{
			if (rank == 2)
			{
				return t.MakeArrayType();
			}
			return MakeJaggedArrayType(t.MakeArrayType(), rank - 1);
		}
	}
}
