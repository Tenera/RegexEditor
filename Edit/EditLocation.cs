//////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
//  Copyright Syncfusion Inc. 2001 - 2003. All rights reserved. Use of this code is subject to the terms of our 
//  license. A copy of the current license can be obtained at any time by e-mailing licensing@syncfusion.com. 
//  Re-distribution in any form is strictly prohibited. Any infringement will be prosecuted under applicable laws. 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
 
/*******************************************************************************
*                    Essential Edit - A syntax coloring edit                   *
*                                Author: B. Wu                                 *
********************************************************************************/

using System;
using System.ComponentModel;
using System.Globalization;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditLocation class encapsulates a location (as line and  
	/// char/column indexes) in a text buffer. The two data members 
	/// are L (line index) and C (char/column index). L and C are one
	/// based, i.e., starting from 1.
	/// </summary>
	public class EditLocation : IComparable
	{
		#region Data Members

		/// <summary>
		/// The line index.
		/// </summary>
		public int L;
		/// <summary>
		/// The char/column index.
		/// </summary>
		public int C;

		/// <summary>
		/// A read-only field representing an invalid location.
		/// </summary>
		public static readonly EditLocation None = new EditLocation (0, 0);
		/// <summary>
		/// A read-only field representing an infinite large location.
		/// </summary>
		public static readonly EditLocation Infinite = new EditLocation(-1, -1);

		#endregion

		#region Methods

		/// <summary>
		/// Default constructor. Creates a new EditLocatioin object with  
		/// L = 1 and C = 1.
		/// </summary>
		public EditLocation()
		{
			L = 1;
			C = 1;
		}

		/// <summary>
		/// Overloaded constructor. Creates a new EditLocation object 
		/// and copies the L and C values from the existing EditLocation 
		/// object.
		/// </summary>
		/// <param name="lc">The EditLocation object from which the C 
		/// and L values will be copied.</param>
		public EditLocation(EditLocation lc)
		{
			this.L = lc.L;
			this.C = lc.C;
		}

		/// <summary>
		/// Overloaded constructor. Creates a new EditLocation object 
		/// with specified L and C values.
		/// </summary>
		/// <param name="L">Initial L (line index) value.</param>
		/// <param name="C">Initial C (char/column index) value.</param>
		public EditLocation(int L, int C)
		{
			this.L = L;
			this.C = C;
		}

		/// <summary>
		/// Returns a string that represents the current EditLocation 
		/// object.
		/// </summary>
		/// <returns>A string that contains the line and char/column 
		/// indexes of the current EditLocation object.</returns>
		public override string ToString()
		{
			return ("L: " + this.L + ", " + "C: " + this.C);
		}

		/// <summary>
		/// Determines whether two EditLocation objects are equal.
		/// </summary>
		/// <param name="o">The EditLocation object that is compared to 
		/// the current EditLocation object.</param>
		/// <returns>true if L and C of the specified EditLocation 
		/// object match those of the current EditLocation object; 
		/// otherwise, false.</returns>
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			return ((this.L == ((EditLocation)o).L) 
				&& (this.C == ((EditLocation)o).C));
		}

		/// <summary>
		/// Overridden. Returns the hash code for the current EditLocation 
		/// object.
		/// </summary>
		/// <returns>A hash code for the current EditLocation object.</returns>
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		/// <summary>
		/// Overloaded. Compares the current EditLocation object with 
		/// the specified location.
		/// </summary>
		/// <param name="L">L of the specified location.</param>
		/// <param name="C">C of the specified location.</param>
		/// <returns>signed number indicating the relative values of the 
		/// current EditLocation object and specified EditLocation object:  
		/// -1 - the current EditLocation object is smaller than the 
		/// specified EditLocation object;
		/// 0 - the current EditLocation object is equal to the 
		/// specified EditLocation object;
		/// 1 - the current EditLocation object is greater than the 
		/// specified EditLocation object.
		/// </returns>
		public int CompareTo(int L, int C)
		{
			if ((this.L == -1) && (this.C == -1) && (L == -1) && (C == -1))
			{
				return 0;
			}
			if ((this.L == -1) && (this.C == -1))
			{
				return 1;
			}
			if ((L == -1) && (C == -1))
			{
				return -1;
			}
			if (this.L > L)
			{
				return 1;
			}
			else if (this.L < L)
			{
				return -1;
			}
			else if (this.L == L)
			{
				if (this.C > C)
				{
					return 1;
				}
				else if (this.C < C)
				{
					return -1;
				}
				else
				{
					return 0;
				}
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Overloaded. Compares the current EditLocation object with the 
		/// specified EditLocation object.
		/// </summary>
		/// <param name="o">An EditLocation object to compare.</param>
		/// <returns>signed number indicating the relative values of the 
		/// current EditLocation object and specified EditLocation object:  
		/// -1 - the current EditLocation object is smaller than the 
		/// specified EditLocation object;
		/// 0 - the current EditLocation object is equal to the 
		/// specified EditLocation object;
		/// 1 - the current EditLocation object is greater than the 
		/// specified EditLocation object.
		/// </returns>
		int IComparable.CompareTo(object o)
		{
			return CompareTo(((EditLocation)o).L, ((EditLocation)o).C);
		}

		/// <summary>
		/// Tests whether two EditLocation objects are equal.
		/// </summary>
		/// <param name="lc1">The first EditLocation to test.</param>
		/// <param name="lc2">The second EditLocation to test.</param>
		/// <returns>true if lc1 and lc2 have equal L and C; 
		/// otherwise, false.</returns>
		public static bool operator== (EditLocation lc1, EditLocation lc2)
		{
			if (((object)lc1 == null) && ((object)lc2 == null))
			{
				return true;
			}
			else if (((object)lc1 == null) || ((object)lc2 == null))
			{
				return false;
			}
			else
			{
				return lc1.Equals(lc2);
			}
		}

		/// <summary>
		/// Tests whether two EditLocation objects are different.
		/// </summary>
		/// <param name="lc1">The first EditLocation to test.</param>
		/// <param name="lc2">The second EditLocation to test.</param>
		/// <returns>true if lc1 and lc2 differ either in L or C; 
		/// false if lc1 and lc2 are equal.</returns>
		public static bool operator!= (EditLocation lc1, EditLocation lc2)
		{
			return !(lc1 == lc2);
		}

		/// <summary>
		/// Tests whether one EditLocation object is smaller than another 
		/// EditLocation object.
		/// </summary>
		/// <param name="lc1">The first EditLocation to test.</param>
		/// <param name="lc2">The second EditLocation to test.</param>
		/// <returns>true if lc1 is smaller than lc2; otherwise, 
		/// false.</returns>
		public static bool operator< (EditLocation lc1, EditLocation lc2)
		{
			return (((IComparable)lc1).CompareTo(lc2) < 0);
		}

		/// <summary>
		/// Tests whether one EditLocation object is smaller than or equal 
		/// to another EditLocation object.
		/// </summary>
		/// <param name="lc1">The first EditLocation to test.</param>
		/// <param name="lc2">The second EditLocation to test.</param>
		/// <returns>true if lc1 is smaller than or equal to lc2; 
		/// otherwise, false.</returns>
		public static bool operator<= (EditLocation lc1, EditLocation lc2)
		{
			return (((IComparable)lc1).CompareTo(lc2) <= 0);
		}

		/// <summary>
		/// Tests whether one EditLocation object is greater than another 
		/// EditLocation object.
		/// </summary>
		/// <param name="lc1">The first EditLocation to test.</param>
		/// <param name="lc2">The second EditLocation to test.</param>
		/// <returns>true if lc1 is greater than lc2; otherwise, false.</returns>
		public static bool operator> (EditLocation lc1, EditLocation lc2)
		{
			return (((IComparable)lc1).CompareTo(lc2) > 0);
		}

		/// <summary>
		/// Tests whether one EditLocation object is greater than or equal to 
		/// another EditLocation object.
		/// </summary>
		/// <param name="lc1">The first EditLocation to test.</param>
		/// <param name="lc2">The second EditLocation to test.</param>
		/// <returns>true if lc1 is greater than or equal to lc2; 
		/// otherwise, false.</returns>
		public static bool operator>= (EditLocation lc1, EditLocation lc2)
		{
			return (((IComparable)lc1).CompareTo(lc2) >= 0);
		}

		/// <summary>
		/// Tests whether the current EditLocation object is equal to 
		/// the specified location.
		/// </summary>
		/// <param name="L">L of the specified location.</param>
		/// <param name="C">C of the specified location.</param>
		/// <returns>true if the current EditLocation object is equal to 
		/// the specified location; otherwise, false.</returns>
		public bool EqualTo(int L, int C)
		{
			return (CompareTo(L, C) == 0);
		}

		/// <summary>
		/// Tests whether the current EditLocation object is not equal to 
		/// the specified location.
		/// </summary>
		/// <param name="L">L of the specified location.</param>
		/// <param name="C">C of the specified location.</param>
		/// <returns>true if the current EditLocation object is not equal 
		/// to the specified location; otherwise, false.</returns>
		public bool NotEqualTo(int L, int C)
		{
			return (CompareTo(L, C) != 0);
		}

		/// <summary>
		/// Tests whether the current EditLocation object is less than 
		/// the specified location.
		/// </summary>
		/// <param name="L">L of the specified location.</param>
		/// <param name="C">C of the specified location.</param>
		/// <returns>true if the current EditLocation object is less than 
		/// the specified location; otherwise, false.</returns>
		public bool LessThan(int L, int C)
		{
			return (CompareTo(L, C) < 0);
		}

		/// <summary>
		/// Tests whether the current EditLocation object is less than 
		/// or equal to the specified location.
		/// </summary>
		/// <param name="L">L of the specified location.</param>
		/// <param name="C">C of the specified location.</param>
		/// <returns>true if the current EditLocation object is less 
		/// than or equal to the specified location; otherwise, false.
		/// </returns>
		public bool LessThanOrEqualTo(int L, int C)
		{
			return (CompareTo(L, C) <= 0);
		}

		/// <summary>
		/// Tests whether the current EditLocation object is greater than 
		/// the specified location.
		/// </summary>
		/// <param name="L">L of the specified location.</param>
		/// <param name="C">C of the specified location.</param>
		/// <returns>true if the current EditLocation object is greater 
		/// than the specified location; otherwise, false.</returns>
		public bool GreaterThan(int L, int C)
		{
			return (CompareTo(L, C) > 0);
		}

		/// <summary>
		/// Tests whether the current EditLocation object is greater 
		/// than or equal to the specified location.
		/// </summary>
		/// <param name="L">L of the specified location.</param>
		/// <param name="C">C of the specified location.</param>
		/// <returns>true if the current EditLocation object is greater 
		/// than or equal to the specified location; otherwise, false.
		/// </returns>
		public bool GreaterThanOrEqualTo(int L, int C)
		{
			return (CompareTo(L, C) >= 0);
		}

		#endregion
	}

	/// <summary>
	/// The EditLocationConverter class is a helper class for EditLocation.
	/// It converts between an EditLocation object and its string expression.
	/// </summary>
	internal class EditLocationConverter : ExpandableObjectConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
		{
			if (t == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, t);
		}

		public override object ConvertFrom(ITypeDescriptorContext context,
			CultureInfo info, object value)
		{
			if (value is string)
			{
				try 
				{
					string str = (string) value;
					// Parse "L: 1, C: 1"
					int lIndex = str.IndexOf('L');
					int commaIndex = str.IndexOf(',');
					int cIndex = str.IndexOf('C');
					if ((lIndex != -1) && (commaIndex != -1)
						&& (cIndex != -1))
					{
						int l = Int32.Parse(str.Substring(lIndex + 2, 
							commaIndex - lIndex - 2));
						int c = Int32.Parse(str.Substring(cIndex + 2, 
							str.Length - cIndex - 2));
						return new EditLocation(l, c);
					}
				}
				catch
				{
				}
				throw new ArgumentException("Can not convert '" 
					+ (string)value + "' to type EditLocation");
			}
			return base.ConvertFrom(context, info, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context,
			CultureInfo culture, object value, Type destType)
		{
			if (destType == typeof(string) && value is EditLocation)
			{
				return ((EditLocation)value).ToString();
			}
			return base.ConvertTo(context, culture, value, destType);
		}
	}
}
