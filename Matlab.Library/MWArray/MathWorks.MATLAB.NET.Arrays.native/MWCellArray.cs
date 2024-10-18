using System;
using System.Runtime.InteropServices;

namespace MathWorks.MATLAB.NET.Arrays.native
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MWCellArray : MWArray, ICloneable, IEquatable<MWCellArray>
	{
		private static readonly MWCellArray _Empty = new MWCellArray();

		public object this[params int[] indices]
		{
			get
			{
				return get(indices);
			}
			set
			{
				set(indices, value);
			}
		}

		public static MWCellArray Empty => (MWCellArray)_Empty.Clone();

		public MWCellArray()
		{
			array_Type = MWArrayType.Cell;
		}

		public MWCellArray(int rows, int columns)
			: base(rows, columns)
		{
			array_Type = MWArrayType.Cell;
		}

		public MWCellArray(params int[] dimensions)
			: base(dimensions)
		{
			array_Type = MWArrayType.Cell;
		}

		public override object Clone()
		{
			MWCellArray mWCellArray = (MWCellArray)MemberwiseClone();
			deepCopy(mWCellArray);
			return mWCellArray;
		}

		public override bool Equals(object obj)
		{
			if (obj is MWCellArray)
			{
				return Equals(obj as MWCellArray);
			}
			return false;
		}

		public bool Equals(MWCellArray other)
		{
			return base.Equals((object)other);
		}
	}
}
