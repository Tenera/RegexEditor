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
	/// The EditColorGroupList class stores and manages EditColorGroup objects.
	/// </summary>
	internal class EditColorGroupList
	{
		#region Data Members

		/// <summary>
		/// The internal ArrayList for EditColorGroup objects.
		/// </summary>
		private ArrayList editColorGroupList = new ArrayList();
		/// <summary>
		/// A temperary variable to speed up the searching efficiency.
		/// </summary>
		private EditColorGroup editColorGroupTemp = new EditColorGroup("Dummy", 
			Color.Black, Color.White, true, true, EditColorGroupType.RegularText);

		#endregion

		#region Methods

		/// <summary>
		/// Adds an EditColorGroup object to the internal ArrayList.
		/// </summary>
		/// <param name="cg">The EditColorGroup object to be added.</param>
		/// <returns>true if the object is inserted; otherwise, false</returns>
		internal bool Add(EditColorGroup cg)
		{
			int index = editColorGroupList.BinarySearch(cg);
			if (index < 0)
			{
				index = ~index;
				editColorGroupList.Insert(index, cg);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Adds an EditColorGroup object with the specified values of data 
		/// members to the internal ArrayList.
		/// </summary>
		/// <param name="groupName"></param>
		/// <param name="foreColor"></param>
		/// <param name="backColor"></param>
		/// <param name="isAutoForeColor"></param>
		/// <param name="isAutoBackColor"></param>
		/// <param name="groupType"></param>
		/// <returns>true if the object is inserted; otherwise, false</returns>
		internal bool Add(string groupName, Color foreColor, 
			Color backColor, bool isAutoForeColor, bool isAutoBackColor, 
			EditColorGroupType groupType)
		{
			return Add(new EditColorGroup(groupName, foreColor, backColor, 
				isAutoForeColor, isAutoBackColor, groupType));
		}

		/// <summary>
		/// Removes the specified EditColorGroup object.
		/// </summary>
		/// <param name="cg">The EditColorGroup object to be removed.</param>
		internal void Remove(EditColorGroup cg)
		{
			editColorGroupList.Remove((EditColorGroup) cg);
		}

		/// <summary>
		/// Removes the EditColorGroup at the specified index.
		/// </summary>
		/// <param name="i">The index of the EditColorGroup object to be 
		/// removed.</param>
		internal void RemoveAt(int i)
		{
			editColorGroupList.RemoveAt(i);
		}

		/// <summary>
		/// Gets the EditColorGroup object from the specified color group name.
		/// </summary>
		/// <param name="groupName">The name of the desired EditColorGroup object.</param>
		/// <returns>The EditColorGroup object with the specified color group
		/// name.</returns>
		internal EditColorGroup GetColorGroup(string groupName)
		{
			int cgIndex = GetColorGroupIndex(groupName);
			if (cgIndex >= 0)
			{
				return (EditColorGroup)editColorGroupList[cgIndex];
			}
			return EditColorGroup.Unknown;
		}

		/// <summary>
		/// Gets the index of the EditColorGroup object from the specified 
		/// color group name.
		/// </summary>
		/// <param name="groupName">The name of the desired color group.</param>
		/// <returns>The index of the EditColorGroup object with the specified
		/// color group name.</returns>
		internal int GetColorGroupIndex(string groupName)
		{
			editColorGroupTemp.GroupName = groupName;
			return editColorGroupList.BinarySearch(editColorGroupTemp);
		}

		/// <summary>
		/// Sets information for a color group.
		/// </summary>
		/// <param name="groupName"></param>
		/// <param name="foreColor"></param>
		/// <param name="backColor"></param>
		/// <param name="isAutoForeColor"></param>
		/// <param name="isAutoBackColor"></param>
		/// <param name="groupType"></param>
		/// <returns></returns>
		internal bool SetGroup(string groupName, Color foreColor, 
			Color backColor, bool isAutoForeColor, bool isAutoBackColor, 
			EditColorGroupType groupType)
		{
			int cgIndex = GetColorGroupIndex(groupName);
			if (cgIndex >= 0)
			{
				((EditColorGroup)editColorGroupList[cgIndex]).ForeColor 
					= foreColor;
				((EditColorGroup)editColorGroupList[cgIndex]).BackColor 
					= backColor;
				((EditColorGroup)editColorGroupList[cgIndex]).IsAutoForeColor 
					= isAutoForeColor;
				((EditColorGroup)editColorGroupList[cgIndex]).IsAutoBackColor 
					= isAutoBackColor;
				((EditColorGroup)editColorGroupList[cgIndex]).GroupType 
					= groupType;
				return true;
			}
			// A new color group.
			Add(groupName, foreColor, backColor, isAutoForeColor, isAutoBackColor, groupType);
			return false;
		}

		/// <summary>
		/// Gets the foreground color of the specified color group.
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		internal Color GetForeColor(string groupName)
		{
			int cgIndex = GetColorGroupIndex(groupName);
			if (cgIndex >= 0)
			{
				return ((EditColorGroup)editColorGroupList[cgIndex]).ForeColor;
			}
			return Color.Black;
		}

		/// <summary>
		/// Gets the background color of the specified color group.
		/// </summary>
		/// <param name="groupName"></param>
		/// <returns></returns>
		internal Color GetBackColor(string groupName)
		{
			int cgIndex = GetColorGroupIndex(groupName);
			if (cgIndex >= 0)
			{
				return ((EditColorGroup)editColorGroupList[cgIndex]).BackColor;
			}
			return Color.White;
		}

		/// <summary>
		/// Sets the foreground color of the specified color group.
		/// </summary>
		/// <param name="groupName"></param>
		/// <param name="foreColor"></param>
		/// <returns></returns>
		internal bool SetForeColor(string groupName, Color foreColor)
		{
			int cgIndex = GetColorGroupIndex(groupName);
			if (cgIndex >= 0)
			{
				((EditColorGroup)editColorGroupList[cgIndex]).ForeColor = foreColor;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Sets the background color of the specified color group.
		/// </summary>
		/// <param name="groupName"></param>
		/// <param name="backColor"></param>
		/// <returns></returns>
		internal bool SetBackColor(string groupName, Color backColor)
		{
			int cgIndex = GetColorGroupIndex(groupName);
			if (cgIndex >= 0)
			{
				((EditColorGroup)editColorGroupList[cgIndex]).BackColor = backColor;
				return true;
			}
			return false;
		}

		#endregion

		#region internal Properties

		/// <summary>
		/// Returns the EditColorGroup at the specified index.
		/// </summary>
		internal EditColorGroup this[int i]
		{
			get
			{
				return (EditColorGroup)editColorGroupList[i];
			}
			set
			{
				editColorGroupList[i] = value;
			}
		}

		/// <summary>
		/// Gets the count of the EditColorGroup objects in the internal ArrayList.
		/// </summary>
		internal int Count
		{
			get
			{
				return editColorGroupList.Count;
			}
		}

		#endregion
	}
}
