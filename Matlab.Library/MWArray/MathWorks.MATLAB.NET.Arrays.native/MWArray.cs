using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

namespace MathWorks.MATLAB.NET.Arrays.native
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MWArray : ICloneable
	{
		internal static ResourceManager resourceManager;

		private int[] dims;

		private int numOfElem;

		private int maxValidIndex;

		private object[] flatArray;

		internal MWArrayType array_Type;

		private int[] axisWtArr;

		public MWArrayType ArrayType => array_Type;

		public int[] Dimensions => (int[])dims.Clone();

		public bool IsCellArray => array_Type == MWArrayType.Cell;

		public bool IsCharArray => array_Type == MWArrayType.Character;

		public bool IsLogicalArray => array_Type == MWArrayType.Logical;

		public bool IsNumericArray => array_Type == MWArrayType.Numeric;

		public bool IsStructArray => array_Type == MWArrayType.Structure;

		public bool IsEmpty => NumberOfElements == 0;

		public int NumberofDimensions => dims.Length;

		public int NumberOfElements
		{
			get
			{
				return numOfElem;
			}
			protected set
			{
				numOfElem = value;
			}
		}

		private int MaxValidIndex
		{
			get
			{
				return maxValidIndex;
			}
			set
			{
				maxValidIndex = value;
			}
		}

		static MWArray()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
			string text = manifestResourceNames[0];
			text = text.TrimEnd("resources".ToCharArray());
			text = text.TrimEnd(".".ToCharArray());
			resourceManager = new ResourceManager(text, executingAssembly);
		}

		internal MWArray()
		{
			int[] array = dims = new int[2];
			flatArray = new object[0];
			axisWtArr = new int[0];
			numOfElem = 0;
			MaxValidIndex = 0;
		}

		internal MWArray(int rows, int cols)
			: this(new int[2]
			{
				rows,
				cols
			})
		{
		}

		internal MWArray(int[] inDims)
		{
			setDims(inDims);
			setNumOfElem(inDims);
			createAxisWeightArray();
			initNativeArray(numOfElem);
		}

		public override bool Equals(object obj)
		{
			MWArray mWArray = (MWArray)obj;
			if (object.Equals(mWArray.Dimensions, Dimensions))
			{
				return object.Equals(mWArray.flatArray, flatArray);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = 17;
			if (!IsEmpty)
			{
				int[] array = dims;
				foreach (int num2 in array)
				{
					num = num * 7 + num2;
				}
				if (flatArray.Length > 0)
				{
					object[] array2 = flatArray;
					foreach (object obj in array2)
					{
						num = ((obj == null) ? num : (num * 7 + obj.GetHashCode()));
					}
				}
			}
			if (num >= 0)
			{
				return num;
			}
			return num * -1;
		}

		public override string ToString()
		{
			if (IsEmpty)
			{
				return "[]";
			}
			string dimsStr = getDimsStr();
			string str = "";
			switch (array_Type)
			{
			case MWArrayType.Structure:
				str = " struct";
				break;
			case MWArrayType.Cell:
				str = " cell";
				break;
			}
			return dimsStr + str + " array";
		}

		public virtual object Clone()
		{
			MWArray mWArray = (MWArray)MemberwiseClone();
			deepCopy(mWArray);
			return mWArray;
		}

		protected void setDims(int[] inDims)
		{
			string @string = resourceManager.GetString("MWErrorOneBasedIndexing");
			if (inDims.Length > 1)
			{
				dims = new int[inDims.Length];
				int num = 0;
				while (true)
				{
					if (num < inDims.Length)
					{
						if (inDims[num] < 0)
						{
							break;
						}
						dims[num] = inDims[num];
						num++;
						continue;
					}
					return;
				}
				throw new ArgumentException(@string);
			}
			if (inDims.Length == 1)
			{
				if (inDims[0] < 0)
				{
					throw new ArgumentException(@string);
				}
				dims = new int[2]
				{
					1,
					inDims[0]
				};
			}
			else
			{
				int[] array = dims = new int[2];
			}
		}

		internal object get(int index)
		{
			string @string = resourceManager.GetString("MWErrorInvalidDimensions");
			if (index < 1 || index > MaxValidIndex)
			{
				throw new ArgumentException(@string);
			}
			return flatArray[index - 1];
		}

		protected object get(int[] index)
		{
			if (index.Length == 1)
			{
				return get(index[0]);
			}
			int oneBasedIndexForArray = getOneBasedIndexForArray(index);
			return get(oneBasedIndexForArray);
		}

		protected void set(int index, object val)
		{
			string @string = resourceManager.GetString("MWErrorInvalidDimensions");
			if (index < 1 || index > MaxValidIndex)
			{
				throw new ArgumentException(@string);
			}
			if (val is ICloneable)
			{
				flatArray[index - 1] = ((ICloneable)val).Clone();
			}
			else
			{
				flatArray[index - 1] = val;
			}
		}

		protected void set(int[] index, object val)
		{
			if (index.Length == 1)
			{
				set(index[0], val);
				return;
			}
			int oneBasedIndexForArray = getOneBasedIndexForArray(index);
			set(oneBasedIndexForArray, val);
		}

		protected void setNumOfElem(int[] inDims)
		{
			if (inDims.Length == 0)
			{
				numOfElem = 0;
				return;
			}
			int num = 1;
			foreach (int num2 in inDims)
			{
				num *= num2;
			}
			numOfElem = num;
		}

		protected void createAxisWeightArray()
		{
			axisWtArr = new int[dims.Length];
			if (dims.Length >= 2)
			{
				axisWtArr[0] = dims[1];
				axisWtArr[1] = dims[0];
			}
			if (dims.Length >= 3)
			{
				axisWtArr[2] = axisWtArr[0] * axisWtArr[1];
			}
			for (int i = 3; i < dims.Length; i++)
			{
				axisWtArr[i] = axisWtArr[i - 1] * dims[i - 1];
			}
		}

		protected void initNativeArray(int num)
		{
			flatArray = new object[num];
			MaxValidIndex = num;
		}

		protected int getOneBasedIndexForArray(int[] index)
		{
			string @string = resourceManager.GetString("MWErrorInvalidDimensions");
			if (index.Length != dims.Length)
			{
				throw new ArgumentException(@string);
			}
			for (int i = 0; i < index.Length; i++)
			{
				if (index[i] < 1 || index[i] > dims[i])
				{
					throw new ArgumentException(@string);
				}
			}
			int num = getPageBasedIndex(index[0], index[1]);
			if (index.Length > 2)
			{
				for (int num2 = index.Length - 1; num2 > 1; num2--)
				{
					num += (index[num2] - 1) * axisWtArr[num2];
				}
			}
			return num;
		}

		protected void setFlatArr(object[] newArr)
		{
			flatArray = newArr;
			MaxValidIndex = flatArray.Length;
		}

		protected string getDimsStr()
		{
			string text = "";
			for (int i = 0; i < dims.Length; i++)
			{
				text += dims[i];
				if (i != dims.Length - 1)
				{
					text += "x";
				}
			}
			return text;
		}

		protected void deepCopy(MWArray target)
		{
			target.dims = (int[])dims.Clone();
			target.axisWtArr = (int[])axisWtArr.Clone();
			target.flatArray = (object[])flatArray.Clone();
		}

		protected void resizeFlatArray(int newSize)
		{
			Array.Resize(ref flatArray, newSize);
		}

		private int getPageBasedIndex(int r, int c)
		{
			r--;
			c--;
			return c * dims[0] + r + 1;
		}
	}
}
