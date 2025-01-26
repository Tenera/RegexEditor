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
	/// The EditColorInfo class represents information for a range of color 
	/// characters in a line.
	/// </summary>
	internal class EditColorInfo : IComparable
	{
		#region Data Members

		/// <summary>
		/// The index of the starting char.
		/// </summary>
		internal int StartChar;
		/// <summary>
		/// The index of the ending char.
		/// </summary>
		internal int EndChar;
		/// <summary>
		/// The index of the color group.
		/// </summary>
		internal int ColorGroupIndex;

		#endregion

		#region Methods

		/// <summary>
		/// Overloaded constructor. Creates a new EditColorInfo object 
		/// with the specified values of starting char, ending char and 
		/// color group indexes.
		/// </summary>
		/// <param name="startChar">Initial starting char index.</param>
		/// <param name="endChar">Initial ending char index.</param>
		/// <param name="colorGroupIndex">Initial color group index.</param>
		internal EditColorInfo(int startChar, int endChar, int colorGroupIndex)
		{
			this.StartChar = startChar;
			this.EndChar = endChar;
			this.ColorGroupIndex = colorGroupIndex;
		}

		/// <summary>
		/// Overloaded constructor. Creates a new EditColorInfo object 
		/// and copies the values of data members from the specified 
		/// EditColorInfo object.
		/// </summary>
		/// <param name="ci">The EditColorInfo object from which the 
		/// values of data members will be copied.</param>
		internal EditColorInfo(EditColorInfo ci)
		{
			this.StartChar = ci.StartChar;
			this.EndChar = ci.EndChar;
			this.ColorGroupIndex = ci.ColorGroupIndex;
		}

		/// <summary>
		/// Compares the current EditColorInfo object with the specified 
		/// EditColorInfo object.
		/// </summary>
		/// <param name="o">An EditColorInfo object to compare.</param>
		/// <returns>A signed number indicating the relative location 
		/// (based on the index of the starting char) of the current 
		/// EditColorInfo object and the specified EditColorInfo object: 
		/// -1 - the current EditColorInfo object locates before the 
		/// specified EditColorInfo object;
		/// 0 - the current EditColorInfo object locates at the same 
		/// place as the specified EditColorInfo object;
		/// 1 - the current EditColorInfo object locates after the 
		/// specified EditColorInfo object.</returns>
		int IComparable.CompareTo(object o)
		{
			if(this.StartChar > ((EditColorInfo)o).StartChar)
			{
				return 1;
			}
			else if(this.StartChar < ((EditColorInfo)o).StartChar)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}

		#endregion
	}
}
