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

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditLocationRange class represents a range of locations in a 
	/// text buffer.
	/// </summary>
	public class EditLocationRange : IComparable
	{
		#region Data Members

		/// <summary>
		/// The starting location.
		/// </summary>
		public EditLocation Start;
		/// <summary>
		/// The ending location.
		/// </summary>
		public EditLocation End;

		/// <summary>
		/// A read-only field that represents an empty EditLocationRange.
		/// </summary>
		public static readonly EditLocationRange Empty = 
			new EditLocationRange(-1, -1, -1, -1);

		#endregion

		#region Methods

		/// <summary>
		/// Default constructor. Creates an EditLocatioinPair object with 
		/// both Start and End equal to the default EditLocation.
		/// </summary>
		public EditLocationRange()
		{
			this.Start = new EditLocation();
			this.End = new EditLocation();
		}

		/// <summary>
		/// Overloaded constructor. Creates an EditLocationRange object 
		/// and copies the Start and End values from the specified 
		/// EditLocationRange object.
		/// </summary>
		/// <param name="lcr">The EditLocationRange object from which the 
		/// values of Start and End will be copied.</param>
		public EditLocationRange(EditLocationRange lcr)
		{
			this.Start = new EditLocation(lcr.Start);
			this.End = new EditLocation(lcr.End);
		}

		/// <summary>
		/// Overloaded constructor. Creates an EditLocationRange object 
		/// with the specified values for Start and End.
		/// </summary>
		/// <param name="start">Initial Start value.</param>
		/// <param name="end">Initial End value.</param>
		public EditLocationRange(EditLocation start, EditLocation end)
		{
			this.Start = new EditLocation(start);
			this.End = new EditLocation(end);
		}

		/// <summary>
		/// Overloaded constructor. Creates an EditLocationRange object with 
		/// the specified L and C values for Start and End.
		/// </summary>
		/// <param name="startL">The L (line index) value for Start.</param>
		/// <param name="startC">The C (char/column index) value for Start.</param>
		/// <param name="endL">The L (line index) value for End.</param>
		/// <param name="endC">The C (char/column index) value for End.</param>
		public EditLocationRange(int startL, int startC, int endL, int endC)
		{
			this.Start = new EditLocation(startL, startC);
			this.End = new EditLocation(endL, endC);
		}

		/// <summary>
		/// Tests if the specified location is contained within the current 
		/// EditLocationRange object.
		/// </summary>
		/// <param name="ln">The line of the location to test.</param>
		/// <param name="ch">The char of the location to test.</param>
		/// <returns>true if the specified location is contained within
		/// the current EditLocationRange object; otherwise, false.</returns>
		public bool Contains(int ln, int ch)
		{
			EditLocationRange lcTemp = this.Normalize();
			return (lcTemp.Start.LessThanOrEqualTo(ln, ch)) 
				&& (lcTemp.End.GreaterThanOrEqualTo(ln, ch));
		}

		/// <summary>
		/// Tests if the specified EditLocation is contained within the current 
		/// EditLocationRange object.
		/// </summary>
		/// <param name="lc">The EditLocation to test.</param>
		/// <returns>true if lc is contained within the current EditLocationRange 
		/// object; otherwise, false.</returns>
		public bool Contains(EditLocation lc)
		{
			EditLocationRange lcTemp = this.Normalize();
			return ((lc >= lcTemp.Start) && (lc <= lcTemp.End));
		}

		/// <summary>
		/// Tests if the specified EditLocationRange object is contained 
		/// within the current EditLocationRange object.
		/// </summary>
		/// <param name="lcr">The EditLocationRange object to test.</param>
		/// <returns>true if lcr is contained within the current EditLocationRange
		/// object; otherwise, false.</returns>
		public bool Contains(EditLocationRange lcr)
		{
			EditLocationRange lcrTemp = this.Normalize();
			EditLocationRange lcrTemp1 = lcr.Normalize();
			return ((lcrTemp1.Start >= lcrTemp.Start) 
				&& (lcrTemp1.End <= lcrTemp.End));
		}

		/// <summary>
		/// Tests if the specified EditLocationRange object intersects
		/// the current EditLocationRange object.
		/// </summary>
		/// <param name="lcr">The EditLocationRange object to test.</param>
		/// <returns>true if lcr intersects the current EditLocationRange
		/// object; otherwise, false.</returns>
		public bool Intersects(EditLocationRange lcr)
		{
			EditLocationRange lcrTemp = this.Normalize();
			EditLocationRange lcrTemp1 = lcr.Normalize();
			return ((lcrTemp.End > lcrTemp1.Start) && 
				(lcrTemp.Start < lcrTemp1.Start))
				|| ((lcrTemp1.End > lcrTemp.Start) && 
				(lcrTemp1.Start < lcrTemp.Start));
		}

		/// <summary>
		/// Returns a string that represents the current EditLocationRange object.
		/// </summary>
		/// <returns>A string that contains the line and char/column indexes 
		/// for Start and End of the current EditLocationRange object.</returns>
		public override string ToString()
		{
			return ("Start L: " + Start.L + "," + " Start C: " + Start.C + "; "
				+ "End L: " + End.L + "," + " End C: " + End.C);
		}

		/// <summary>
		/// Tests whether the specified EditLocationRange object is equal to 
		/// the current EditLocationRange object.
		/// </summary>
		/// <param name="o">The EditLocationRange object that is compared to 
		/// the current EditLocationRange object.</param>
		/// <returns>true if the Start and End of the specified EditLocationRange 
		/// object match those of the current EditLocationRange object; otherwise, 
		/// false.</returns>
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}

			EditLocationRange lcrTemp1 = this.Normalize();
			EditLocationRange lcrTemp2 = ((EditLocationRange)o).Normalize();
			return ((lcrTemp1.Start == lcrTemp2.Start) 
				&& (lcrTemp1.End == lcrTemp2.End));
		}

		/// <summary>
		/// Overridden. Returns the hash code for the current EditLocationRange 
		/// object.
		/// </summary>
		/// <returns>A hash code for the current EditLocationRange object.</returns>
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}

		/// <summary>
		/// Overloaded. Compares the current EditLocationRange object with the 
		/// specified EditLocationRange object.
		/// </summary>
		/// <param name="o">An EditLocationRange object to compare.</param>
		/// <returns>A signed number indicating the relative values (starting 
		/// locations) of the current EditLocationRange object and the specified 
		/// EditLocationRange object: 
		/// -1 - the current EditLocationRange object is smaller than (starts 
		/// before) the specified EditLocationRange object;
		/// 0 - the current EditLocationRange object is equal to (starts at 
		/// the same location with) the specified EditLocationRange object;
		/// 1 - the current EditLocationRange object is greater than (starts 
		/// after) the specified EditLocationRange object.
		/// </returns>
		int IComparable.CompareTo(object o)
		{
			EditLocation start1 = Start < End ? Start : End;
			EditLocation start2 = ((EditLocationRange)o).Start 
				< ((EditLocationRange)o).End ? 
				((EditLocationRange)o).Start : ((EditLocationRange)o).End;
			if (start1 > start2)
			{
				return 1;
			}
			else if (start1 < start2)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Returns a normalized EditLocationRange object, i.e., Start 
		/// precedes End.
		/// </summary>
		/// <returns>A normalized version of the current EditLocationRange 
		/// object.</returns>
		public EditLocationRange Normalize()
		{
			if (Start <= End)
			{
				return new EditLocationRange(Start, End);
			}
			else
			{
				return new EditLocationRange(End, Start);
			}
		}

		/// <summary>
		/// Tests whether the current EditLocationRange object is in the 
		/// normalized form, i.e., Start precedes End.
		/// </summary>
		/// <returns></returns>
		public bool IsNormalized()
		{
			return (Start <= End);
		}

		/// <summary>
		/// Tests whether the current EditLocationRange object is an empty 
		/// range, i.e., Start equals to End.
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty()
		{
			return (Start == End);
		}

		/// <summary>
		/// Tests whether two EditLocationRange objects are equal.
		/// </summary>
		/// <param name="lcr1">The first EditLocationRange object to test.</param>
		/// <param name="lcr2">The second EditLocationRange object to test.</param>
		/// <returns>true if lcr1 and lcr2 have both equal Start and equal End; 
		/// otherwise, false.</returns>
		public static bool operator== (EditLocationRange lcr1, EditLocationRange lcr2)
		{
			if (((object)lcr1 == null) && ((object)lcr2 == null))
			{
				return true;
			}
			else if (((object)lcr1 == null) || ((object)lcr2 == null))
			{
				return false;
			}
			else
			{
				return lcr1.Equals(lcr2);
			}
		}

		/// <summary>
		/// Tests whether two EditLocationRange objects are different.
		/// </summary>
		/// <param name="lcr1">The first EditLocationRange object to test.</param>
		/// <param name="lcr2">The second EditLocationRange object to test.</param>
		/// <returns>true if lcr1 and lcr2 differ either in Start or End; false 
		/// if lcr1 and lcr2 are equal.</returns>
		public static bool operator!= (EditLocationRange lcr1, EditLocationRange lcr2)
		{
			return !(lcr1 == lcr2);
		}

		/// <summary>
		/// Tests whether one EditLocationRange object is smaller than another 
		/// EditLocationRange object.
		/// </summary>
		/// <param name="lcr1">The first EditLocationRange object to test.</param>
		/// <param name="lcr2">The second EditLocationRange object to test.</param>
		/// <returns>true if lcr1 is smaller than lcr2; otherwise, false.</returns>
		public static bool operator< (EditLocationRange lcr1, EditLocationRange lcr2)
		{
			return (((IComparable)lcr1).CompareTo(lcr2) < 0);
		}

		/// <summary>
		/// Tests whether one EditLocationRange object is smaller than or equal 
		/// to another EditLocationRange object.
		/// </summary>
		/// <param name="lcr1">The first EditLocationRange object to test.</param>
		/// <param name="lcr2">The second EditLocationRange object to test.</param>
		/// <returns>true if lcr1 is smaller than or equal to lcr2; otherwise, 
		/// false.</returns>
		public static bool operator<= (EditLocationRange lcr1, EditLocationRange lcr2)
		{
			return (((IComparable)lcr1).CompareTo(lcr2) <= 0);
		}

		/// <summary>
		/// Tests whether one EditLocationRange object is greater than another 
		/// EditLocationRange object.
		/// </summary>
		/// <param name="lcr1">The first EditLocationRange object to test.</param>
		/// <param name="lcr2">The second EditLocationRange object to test.</param>
		/// <returns>true if lcr1 is greater than lcr2; otherwise, false.</returns>
		public static bool operator> (EditLocationRange lcr1, EditLocationRange lcr2)
		{
			return (((IComparable)lcr1).CompareTo(lcr2) > 0);
		}

		/// <summary>
		/// Tests whether one EditLocationRange object is greater than or equal 
		/// to another EditLocationRange object.
		/// </summary>
		/// <param name="lcr1">The first EditLocationRange object to test.</param>
		/// <param name="lcr2">The second EditLocationRange object to test.</param>
		/// <returns>true if lcr1 is greater than or equal to lcr2; otherwise, 
		/// false.</returns>
		public static bool operator>= (EditLocationRange lcr1, EditLocationRange lcr2)
		{
			return (((IComparable)lcr1).CompareTo(lcr2) >= 0);
		}

		#endregion
	}
}
