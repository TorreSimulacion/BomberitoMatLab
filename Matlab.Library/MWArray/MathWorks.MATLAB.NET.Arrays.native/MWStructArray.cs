using System;
using System.Runtime.InteropServices;

namespace MathWorks.MATLAB.NET.Arrays.native
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MWStructArray : MWArray, ICloneable, IEquatable<MWStructArray>
	{
		private static readonly MWStructArray _Empty = new MWStructArray();

		private string[] fieldNames;

		public object this[string fieldName, params int[] indices]
		{
			get
			{
				if (indices.Length == 1)
				{
					return get(fieldName, indices[0]);
				}
				return get(fieldName, indices);
			}
			set
			{
				if (indices.Length == 1)
				{
					set(fieldName, indices[0], value);
				}
				else
				{
					set(fieldName, indices, value);
				}
			}
		}

		public object this[string fieldName]
		{
			get
			{
				return GetField(fieldName);
			}
			set
			{
				SetField(fieldName, value);
			}
		}

		public static MWStructArray Empty => (MWStructArray)_Empty.Clone();

		public string[] FieldNames => (string[])fieldNames.Clone();

		public int NumberOfFields => fieldNames.Length;

		public MWStructArray()
		{
			fieldNames = new string[0];
			array_Type = MWArrayType.Structure;
		}

		public MWStructArray(int rows, int cols, string[] fNames)
			: this(new int[2]
			{
				rows,
				cols
			}, fNames)
		{
		}

		public MWStructArray(int[] inDims, string[] fNames)
			: base(inDims)
		{
			fieldNames = (string[])fNames.Clone();
			initNativeArray(fieldNames.Length * base.NumberOfElements);
			array_Type = MWArrayType.Structure;
		}

		public MWStructArray(params object[] fieldDefs)
			: base(new int[2]
			{
				1,
				1
			})
		{
			int num = fieldDefs.Length;
			if (num % 2 != 0)
			{
				string @string = MWArray.resourceManager.GetString("MWErrorNotNameValuePair");
				throw new ArgumentException(@string, "fieldDefs");
			}
			num /= 2;
			fieldNames = new string[num];
			initNativeArray(fieldNames.Length);
			for (int i = 0; i < num; i++)
			{
				int num2 = i * 2;
				if (fieldDefs[num2].GetType() != typeof(string) || ((string)fieldDefs[num2]).Length == 0)
				{
					string string2 = MWArray.resourceManager.GetString("MWErrorFieldNotString");
					throw new ArgumentException(string2);
				}
				fieldNames[i] = (string)fieldDefs[num2];
				set(i + 1, fieldDefs[num2 + 1]);
			}
			array_Type = MWArrayType.Structure;
		}

		public override string ToString()
		{
			if (base.IsEmpty)
			{
				return "[]";
			}
			string str = base.ToString() + " with ";
			if (fieldNames.Length == 0)
			{
				str += "no fields.";
			}
			else
			{
				str += "fields:\n";
				string[] array = fieldNames;
				foreach (string text in array)
				{
					str = str + "\t" + text;
					if (!text.Equals(fieldNames[fieldNames.Length - 1]))
					{
						str += "\n";
					}
				}
			}
			return str;
		}

		public override object Clone()
		{
			MWStructArray mWStructArray = new MWStructArray(base.Dimensions, FieldNames);
			deepCopy(mWStructArray);
			return mWStructArray;
		}

		public override bool Equals(object obj)
		{
			if (obj is MWStructArray)
			{
				return Equals(obj as MWStructArray);
			}
			return false;
		}

		public bool Equals(MWStructArray obj)
		{
			if (base.Equals((object)obj))
			{
				return object.Equals(obj.fieldNames, fieldNames);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (fieldNames.Length > 0)
			{
				string[] array = fieldNames;
				foreach (string text in array)
				{
					num = num * 7 + text.GetHashCode();
				}
			}
			if (num >= 0)
			{
				return num;
			}
			return num * -1;
		}

		public object GetField(string fieldName, int index)
		{
			return get(fieldName, index);
		}

		public object GetField(string fieldName)
		{
			return GetField(fieldName, 1);
		}

		public void SetField(string fieldName, object fieldValue)
		{
			set(fieldName, 1, fieldValue);
		}

		public bool IsField(string fieldName)
		{
			if (fieldIndex(fieldName) != -1)
			{
				return true;
			}
			return false;
		}

		public MWStructArray RemoveField(string fName)
		{
			if (!IsField(fName))
			{
				string @string = MWArray.resourceManager.GetString("MWErrorFieldNotFound");
				throw new ArgumentException(@string, fName);
			}
			string[] array = new string[fieldNames.Length - 1];
			int i = 0;
			int num = 0;
			for (; i < fieldNames.Length; i++)
			{
				if (!fieldNames[i].Equals(fName))
				{
					array[num] = fieldNames[i];
					num++;
				}
			}
			object[] array2 = new object[(NumberOfFields - 1) * base.NumberOfElements];
			int num2 = 0;
			for (int j = 0; j < base.NumberOfElements; j++)
			{
				string[] array3 = fieldNames;
				foreach (string text in array3)
				{
					if (!text.Equals(fName))
					{
						array2[num2] = this[text, new int[1]
						{
							j + 1
						}];
						num2++;
					}
				}
			}
			setFlatArr(array2);
			fieldNames = array;
			return this;
		}

		private object get(string fName, int index)
		{
			int index2 = structIndexToOneBasedIndex(fName, index);
			return get(index2);
		}

		private object get(string fName, int[] idxArr)
		{
			int oneBasedIndexForArray = getOneBasedIndexForArray(idxArr);
			return get(fName, oneBasedIndexForArray);
		}

		private void set(string fName, int index, object val)
		{
			if (isValidOneBasedIndex(index) && !IsField(fName))
			{
				string[] array = new string[NumberOfFields + 1];
				for (int i = 0; i < NumberOfFields; i++)
				{
					array[i] = fieldNames[i];
				}
				array[NumberOfFields] = fName;
				object[] array2 = new object[array.Length * base.NumberOfElements];
				int j = 0;
				int num = 0;
				for (; j < base.NumberOfElements; j++)
				{
					string[] array3 = array;
					foreach (string text in array3)
					{
						if (!text.Equals(fName))
						{
							array2[num] = this[text, new int[1]
							{
								j + 1
							}];
						}
						num++;
					}
				}
				setFlatArr(array2);
				fieldNames = array;
			}
			int index2 = structIndexToOneBasedIndex(fName, index);
			set(index2, val);
		}

		private void set(string fName, int[] idxArr, object val)
		{
			int oneBasedIndexForArray = getOneBasedIndexForArray(idxArr);
			set(fName, oneBasedIndexForArray, val);
		}

		private int structIndexToOneBasedIndex(string fName, int index)
		{
			string @string = MWArray.resourceManager.GetString("MWErrorInvalidDimensions");
			if (!isValidOneBasedIndex(index))
			{
				throw new ArgumentException(@string);
			}
			int num = fieldIndex(fName);
			if (num == -1)
			{
				@string = MWArray.resourceManager.GetString("MWErrorBadFieldName");
				throw new ArgumentException(@string);
			}
			return (index - 1) * NumberOfFields + (num + 1);
		}

		private int fieldIndex(string fieldName)
		{
			int result = -1;
			for (int i = 0; i < fieldNames.Length; i++)
			{
				if (fieldNames[i].Equals(fieldName))
				{
					result = i;
					break;
				}
			}
			return result;
		}

		private bool isValidOneBasedIndex(int index)
		{
			if (index >= 1 && index <= base.NumberOfElements)
			{
				return true;
			}
			return false;
		}
	}
}
