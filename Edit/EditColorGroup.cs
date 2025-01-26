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
using System.Collections;
using System.Drawing;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditColorGroup class stores information for a color group.
	/// </summary>
	internal class EditColorGroup : IComparable
	{
		#region Data Members

		/// <summary>
		/// The name of the color group.
		/// </summary>
		internal string GroupName;
		/// <summary>
		/// The foreground color of the color group.
		/// </summary>
		internal Color ForeColor;
		/// <summary>
		/// The background color of the color group.
		/// </summary>
		internal Color BackColor;
		/// <summary>
		/// A value indicating whether to use the normal text forecolor.
		/// </summary>
		internal bool IsAutoForeColor;
		/// <summary>
		/// A value indicating whether to use the normal text backcolor.
		/// </summary>
		internal bool IsAutoBackColor;
		/// <summary>
		/// The type of the color group.
		/// </summary>
		internal EditColorGroupType GroupType;

		/// <summary>
		/// Helper static EditColorGroup objects.
		/// </summary>
		internal static readonly EditColorGroup Default = new 
			EditColorGroup("Default", SystemColors.WindowText, 
			SystemColors.Window, true, true, EditColorGroupType.RegularText);
		internal static readonly EditColorGroup Unknown = new 
			EditColorGroup("Unknown", Color.Black, Color.White, 
			false, false, EditColorGroupType.RegularText);

		#endregion

		#region Methods

		/// <summary>
		/// Creates an EditColorGroup object with the specified values for 
		/// data members.
		/// </summary>
		internal EditColorGroup(string groupName, Color foreColor, 
			Color backColor, bool isAutoForeColor, bool isAutoBackColor, 
			EditColorGroupType groupType)
		{
			this.GroupName = groupName;
			this.ForeColor = foreColor;
			this.BackColor = backColor;
			this.IsAutoForeColor = isAutoForeColor;
			this.IsAutoBackColor = isAutoBackColor;
			this.GroupType = groupType;
		}

		/// <summary>
		/// Overloaded. Compares the current EditColorGroup object with 
		/// the specified EditColorGroup object.
		/// </summary>
		/// <param name="o">An EditColorGroup object to compare.</param>
		/// <returns>A signed number indicating the relative values 
		/// (based on the names of color groups) of the current 
		/// EditColorGroup object and the specified EditColorGroup object: 
		/// -1 - the name of the current EditColorGroup object is less 
		/// than that of the specified EditColorGroup object;
		/// 0 - the name of the current EditColorGroup object is equal to 
		/// that of the specified EditColorGroup object;
		/// 1 - the name of the current EditColorGroup object is greater 
		/// than that of the specified EditColorGroup object.</returns>
		int IComparable.CompareTo(object o)
		{
			return this.GroupName.CompareTo(((EditColorGroup)o).GroupName);
		}

		#endregion
	}
}
